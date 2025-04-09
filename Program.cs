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
using Amazon.Extensions.Configuration.SystemsManager;

var builder = WebApplication.CreateBuilder(args);

var credentials = new BasicAWSCredentials(
    "AKIAWQUOZT7J6LBMRTEP", 
    "BY5bcK97aSP9SI1scXYMv+Se9cA/zcHbP5liIWW4"
);

// Configurar para ler o parâmetro específico
builder.Configuration.AddSystemsManager(options =>
{
    options.Path = "/LibraryManager/Production";
    options.Optional = false;
    options.ReloadAfter = TimeSpan.FromMinutes(5);
    options.AwsOptions = new AWSOptions
    {
        Region = RegionEndpoint.USEast2,
        Credentials = credentials
    };
});

// Para debug - remova depois
var awsConfig = builder.Configuration.GetSection("AWS:DynamoDB").Get<AWSDynamoDBSettings>();
Console.WriteLine($"AWS Config: Region={awsConfig?.Region}, AccessKey={awsConfig?.AccessKey}");
Console.WriteLine($"ConnectionString: {builder.Configuration["ConnectionStrings:LibraryManagerCs"]}");
Console.WriteLine($"JWT Key: {builder.Configuration["Jwt:Key"]}");
Console.WriteLine($"AWS Region: {builder.Configuration["AWS:DynamoDB:Region"]}");

// Resto das configurações
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