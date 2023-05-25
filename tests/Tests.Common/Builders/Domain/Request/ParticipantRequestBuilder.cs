using Bogus;
using Bogus.Extensions.Brazil;
using NetTransactions.Api.Domain.Request;

namespace Tests.Common.Builders.Domain.Request;

public class ParticipantRequestBuilder : Faker<ParticipantRequest>
{
	public ParticipantRequestBuilder()
	{
		RuleFor(x => x.Name, faker => faker.Name.FullName());
		RuleFor(x => x.CPF, faker => faker.Person.Cpf(false));
		RuleFor(x => x.Email, faker => faker.Internet.Email());
	}

    public ParticipantRequestBuilder WithName(string name)
    {
        RuleFor(x => x.Name, faker => name);
        return this;
    }

    public ParticipantRequestBuilder WithCPF(string cpf)
    {
        RuleFor(x => x.CPF, faker => cpf);
        return this;
    }

    public ParticipantRequestBuilder WithEmail(string email)
    {
        RuleFor(x => x.Email, faker => email);
        return this;
    }
}
