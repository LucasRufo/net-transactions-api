using Microsoft.EntityFrameworkCore;
using NetTransactions.Api.Infrastructure;

namespace NetTransactions.Api.Configuration.DatabaseContext;

public static class DatabaseContextConfiguration
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration) 
        => services.AddDbContext<TransactionsDbContext>(builder =>
        {
            builder.UseNpgsql(configuration["DefaultConnectionString"], options => options.CommandTimeout(30).EnableRetryOnFailure());
        });
}
