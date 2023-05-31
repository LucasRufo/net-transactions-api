using FluentValidation;
using NetTransactions.Api.Domain.Services;
using NetTransactions.Api.Domain.Validators;
using NetTransactions.Api.Infrastructure.DateTimeProvider;
using NetTransactions.Api.Infrastructure.Repositories;

namespace NetTransactions.Api.Configuration.ApplicationServices;

public static class ApplicationServicesConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        => services.AddServices().AddRepositories().AddInfrastrucuture().AddValidators();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ParticipantService>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ParticipantRepository>();

        return services;
    }

    private static IServiceCollection AddInfrastrucuture(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateParticipantRequestValidator>();

        return services;
    }
}
