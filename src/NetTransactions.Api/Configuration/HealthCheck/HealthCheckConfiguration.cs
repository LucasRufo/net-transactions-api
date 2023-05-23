namespace NetTransactions.Api.Configuration.HealthCheck;

public static class HealthCheckConfiguration
{
    public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        => services.AddHealthChecks().AddNpgSql(configuration["DefaultConnectionString"]!);

    public static void UseHealthCheck(this WebApplication app)
        => app.MapHealthChecks("/health");
}
