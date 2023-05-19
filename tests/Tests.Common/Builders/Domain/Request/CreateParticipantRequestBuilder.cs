using Bogus;
using NetTransactions.Api.Domain.Request;

namespace Tests.Common.Builders.Domain.Request;

public class CreateParticipantRequestBuilder : Faker<CreateParticipantRequest>
{
	public CreateParticipantRequestBuilder()
	{
		RuleFor(x => x.Name, faker => faker.Name.FullName());
		RuleFor(x => x.Document, faker => faker.Random.ReplaceNumbers("###########"));
		RuleFor(x => x.Email, faker => faker.Internet.Email());
	}
}
