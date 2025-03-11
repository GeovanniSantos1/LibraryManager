using LibraryManager.API.Middlewares;
using LibraryManager.Application;
using LibraryManager.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
//app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseExceptionHandler();

app.Run();
