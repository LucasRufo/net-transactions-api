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
}

void ConfigureApp()
{
    app.UseSwaggerCustom();
    app.MapControllers();
}
