using Bogus;
using NetTransactions.Api.Infrastructure;

namespace Tests.Integration.Configuration.Extensions;

public static class FakerExtensions
{
    public static T GenerateInDatabase<T>(this Faker<T> objFake, TransactionsDbContext context)
        where T : class
    {
        var obj = objFake.Generate();

        context.Add(obj);
        context.SaveChanges();
        return obj;
    }

    public static List<T> GenerateInDatabase<T>(this Faker<T> objFake, TransactionsDbContext context, int count)
        where T : class
    {
        var objList = objFake.Generate(count);

        context.AddRange(objList);
        context.SaveChanges();
        return objList;
    }
}
