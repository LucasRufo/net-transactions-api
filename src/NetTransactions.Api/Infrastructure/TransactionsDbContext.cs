using Microsoft.EntityFrameworkCore;
using NetTransactions.Api.Domain.Entities;
using System.Reflection;

namespace NetTransactions.Api.Infrastructure;

public class TransactionsDbContext : DbContext
{
    public TransactionsDbContext()
    {
    }

    public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();

    public DbSet<Participant> Participant { get; set; }
}
