using LibraryManager.API.Middlewares;
using LibraryManager.Application;
using LibraryManager.Infrastructure;
using Microsoft.OpenApi.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using LibraryManager.Infrastructure.Settings;
using Amazon.Extensions.NETCore.Setup;
using Amazon;
using LibraryManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Em desenvolvimento, usamos o User Secrets
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Configurar AWS Systems Manager (Parameter Store)
builder.Configuration.AddSystemsManager(configureSource: options =>
{
    options.Path = "/LibraryManager/Production";
    options.Optional = true; // Torna opcional para funcionar em desenvolvimento
    options.ReloadAfter = TimeSpan.FromMinutes(5);
    options.AwsOptions = new AWSOptions
    {
        Region = RegionEndpoint.GetBySystemName(
            builder.Configuration["AWS:Credentials:Region"] ?? "us-east-2"),
        Credentials = new BasicAWSCredentials(
            builder.Configuration["AWS:Credentials:AccessKey"],
            builder.Configuration["AWS:Credentials:SecretKey"])
    };
});

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LibraryManager API",
        Version = "v1",
        Description = "Documentação da API usando RapiDoc"
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddMemoryCache();

// Configuração do DynamoDB usando as configurações do User Secrets
var awsOptions = new AWSDynamoDBSettings
{
    AccessKey = builder.Configuration["AWS:DynamoDB:AccessKey"],
    SecretKey = builder.Configuration["AWS:DynamoDB:SecretKey"],
    Region = builder.Configuration["AWS:DynamoDB:Region"] ?? "us-east-2"
};

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var config = new AmazonDynamoDBConfig
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region),
        Timeout = TimeSpan.FromSeconds(30)
    };
    return new AmazonDynamoDBClient(
        new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey), 
        config);
});

builder.Services.AddScoped<IDynamoDBContext>(sp =>
{
    var client = sp.GetRequiredService<IAmazonDynamoDB>();
    return new DynamoDBContext(client, new DynamoDBContextConfig
    {
        ConsistentRead = true,
        Conversion = DynamoDBEntryConversion.V2
    });
});

// Configuração do AWS S3 usando as configurações do User Secrets
var s3Settings = new AWSS3Settings
{
    BucketName = builder.Configuration["AWS:S3:BucketName"],
    Region = builder.Configuration["AWS:S3:Region"],
    AccessKey = builder.Configuration["AWS:S3:AccessKey"],
    SecretKey = builder.Configuration["AWS:S3:SecretKey"]
};

builder.Services.AddSingleton(s3Settings);
builder.Services.AddScoped<IStorageService, S3StorageService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManager API v1");
    });

    app.UseRapiDoc(c =>
    {
        c.SpecUrl = "/swagger/v1/swagger.json"; 
        c.DocumentTitle = "LibraryManager API - RapiDoc";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseExceptionHandler();

app.Run();
