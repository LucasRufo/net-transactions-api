using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetTransactions.Api.Configuration.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApiVersioning(options => 
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.SubstituteApiVersionInUrl = true;
            options.GroupNameFormat = "V";
            options.SubstitutionFormat = "V";
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1);
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

        return services;
    }

    public static WebApplication UseSwaggerCustom(this WebApplication app, string pathBase)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in app.DescribeApiVersions())
                {
                    options.SwaggerEndpoint(
                        $"{pathBase}/swagger/{description.GroupName}/swagger.json",
                        description.GroupName);
                }
            });

        return app;
    }
}
