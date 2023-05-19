using Bogus;
using NetTransactions.Api.Domain.Entities;

namespace Tests.Common.Builders.Domain.Entities;

public class ParticipantBuilder : Faker<Participant>
{
	public ParticipantBuilder()
	{
        RuleFor(x => x.Id, faker => faker.Random.Guid());
        RuleFor(x => x.Name, faker => faker.Name.FullName());
        RuleFor(x => x.Document, faker => faker.Random.ReplaceNumbers("###########"));
        RuleFor(x => x.Email, faker => faker.Internet.Email());
        RuleFor(x => x.CreatedAt, faker => DateTime.UtcNow);
        RuleFor(x => x.UpdatedAt, faker => DateTime.UtcNow);
    }

    public ParticipantBuilder WithCreatedAt(DateTime? datetime)
    {
        RuleFor(x => x.UpdatedAt, faker => datetime);
        return this;
    }

    public ParticipantBuilder WithUpdateAt(DateTime? datetime)
    {
        RuleFor(x => x.UpdatedAt, faker => datetime);
        return this;
    }
}
