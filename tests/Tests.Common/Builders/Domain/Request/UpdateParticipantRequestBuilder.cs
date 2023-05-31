using Bogus;
using NetTransactions.Api.Domain.Request;

namespace Tests.Common.Builders.Domain.Request;

public class UpdateParticipantRequestBuilder : Faker<UpdateParticipantRequest>
{
    public UpdateParticipantRequestBuilder()
    {
        RuleFor(x => x.Id, faker => faker.Random.Guid());
        RuleFor(x => x.Name, faker => faker.Name.FullName());
        RuleFor(x => x.Email, faker => faker.Internet.Email());
    }

    public UpdateParticipantRequestBuilder WithId(Guid id)
    {
        RuleFor(x => x.Id, faker => id);
        return this;
    }

    public UpdateParticipantRequestBuilder WithName(string name)
    {
        RuleFor(x => x.Name, faker => name);
        return this;
    }

    public UpdateParticipantRequestBuilder WithEmail(string email)
    {
        RuleFor(x => x.Email, faker => email);
        return this;
    }
}

