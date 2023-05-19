using Autofac.Extras.FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NetTransactions.Api.Infrastructure;

namespace Tests.Unit.Configuration;

public static class InMemoryContextExtension
{
    public static AutoFake WithInMemoryContext(this AutoFake autoFake)
    {
        var options = new DbContextOptionsBuilder<TransactionsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        autoFake.Provide(new TransactionsDbContext(options));

        return autoFake;
    }
}
