using NetTransactions.Api.Domain.Entities;
using NetTransactions.Api.Domain.Response;
using Tests.Common.Builders.Domain.Request;
using Tests.Integration.Configuration.Extensions;

namespace Tests.Integration.ApiTests;

public class ParticipantEndpointTests : IntegrationTestsBase
{
    private HttpClient _httpClient;
    private const string _baseUri = "/api/v1/participant";

    [SetUp]
    public void SetUp()
    {
        _httpClient = TestApi.CreateClient();
    }

    [Test]
    public async Task CreateShouldReturnSuccessStatusCodeAndParticipant()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();

        var response = await _httpClient.PostAsync(_baseUri, createParticipantRequest.ToJsonContent());

        var participantFromDb = ContextForAsserts.Participant.First();

        response.Should().Be200Ok();
        response.Content.ShouldBeEquivalentTo<ParticipantResponse, Participant>(participantFromDb);
    }
}
