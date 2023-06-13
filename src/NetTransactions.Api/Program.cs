using NetTransactions.Api.Configuration.ApplicationServices;
using NetTransactions.Api.Configuration.DatabaseContext;
using NetTransactions.Api.Configuration.HealthCheck;
using NetTransactions.Api.Configuration.Swagger;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices();

var app = builder.Build();

ConfigureApp();

app.Run();

void ConfigureServices()
{
    builder.Services.AddControllers();
    builder.Services.AddSwagger();
    builder.Services.AddDatabaseContext(builder.Configuration);
    builder.Services.AddHealthCheck(builder.Configuration);
    builder.Services.AddApplicationServices();
}

void ConfigureApp()
{
    var pathbase = builder.Configuration["PathBase"];
    app.UsePathBase(pathbase);
    app.UseSwaggerCustom(pathbase ?? "");
    app.UseHealthCheck();
    app.MapControllers();
}

public partial class Program { }
