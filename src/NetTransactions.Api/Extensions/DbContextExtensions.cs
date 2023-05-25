using Microsoft.EntityFrameworkCore;

namespace NetTransactions.Api.Extensions;

public static class DbContextExtensions
{
    public static bool ExistsInTracker<TContext, TEntity>(this TContext context, TEntity entity)
        where TContext : DbContext
        where TEntity : class
    {
        return context.Set<TEntity>().Local.Any(e => e == entity);
    }
}
