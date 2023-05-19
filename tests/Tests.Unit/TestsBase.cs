using Autofac.Extras.FakeItEasy;
using Bogus;
using FluentAssertions;

namespace Tests.Unit;

public class TestsBase
{
    protected Faker Faker { get; private set; }
    protected AutoFake AutoFake { get; set; }

    [SetUp]
    public void AllTestsSetUp()
    {
        Faker = new();
        AutoFake = new();

        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options
                .Using<DateTime>(ctx => ctx.Subject.ToLocalTime().Should().BeCloseTo(ctx.Expectation.ToLocalTime(), new TimeSpan(10000)))
                .WhenTypeIs<DateTime>();
            return options;
        });
    }
}
