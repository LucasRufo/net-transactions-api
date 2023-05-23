using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NetTransactions.Api.Infrastructure;
using Npgsql;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Integration.ApiTests;

namespace Tests.Integration;

public class IntegrationTestsBase
{
    public Faker Faker;
    public IServiceProvider ServiceProvider;
    public TransactionsDbContext ContextForAsserts;
    public TransactionsDbContext Context;

    private IServiceScope _databaseScope;
    private IServiceScope _apiScope;

    protected TestApplication TestApi;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        TestApi = new TestApplication();
        ServiceProvider = TestApi.Services;
    }

    [SetUp]
    public async Task SetUpBase()
    {
        _apiScope = ServiceProvider.CreateScope();
        _databaseScope = ServiceProvider.CreateScope();
        Context = _apiScope.ServiceProvider.GetRequiredService<TransactionsDbContext>();
        ContextForAsserts = _databaseScope.ServiceProvider.GetService<TransactionsDbContext>()!;
        await DatabaseFixture.ResetDatabase();

        Faker = new Faker();
        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(100))).WhenTypeIs<DateTime>();
            options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(100))).WhenTypeIs<DateTimeOffset>();
            return options;
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Context?.Dispose();
        ContextForAsserts?.Dispose();
        TestApi?.Dispose();
        _databaseScope?.Dispose();
        _apiScope?.Dispose();
    }
}
