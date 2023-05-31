using Bogus;
using Bogus.Extensions.Brazil;
using NetTransactions.Api.Domain.Request;

namespace Tests.Common.Builders.Domain.Request;

public class CreateParticipantRequestBuilder : Faker<CreateParticipantRequest>
{
	public CreateParticipantRequestBuilder()
	{
		RuleFor(x => x.Name, faker => faker.Name.FullName());
		RuleFor(x => x.CPF, faker => faker.Person.Cpf(false));
		RuleFor(x => x.Email, faker => faker.Internet.Email());
	}

    public CreateParticipantRequestBuilder WithName(string name)
    {
        RuleFor(x => x.Name, faker => name);
        return this;
    }

    public CreateParticipantRequestBuilder WithCPF(string cpf)
    {
        RuleFor(x => x.CPF, faker => cpf);
        return this;
    }

    public CreateParticipantRequestBuilder WithEmail(string email)
    {
        RuleFor(x => x.Email, faker => email);
        return this;
    }
}
