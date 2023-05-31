using NetTransactions.Api.Controllers.Shared;
using NetTransactions.Api.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using Tests.Common.Builders.Domain.Entities;
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
    public async Task GetShouldReturnSuccessAndParticipants()
    {
        new ParticipantBuilder().GenerateInDatabase(Context, 3);

        var response = await _httpClient.GetAsync(_baseUri);

        var expectedParticipants = ContextForAsserts.Participant.ToList();

        var participantsFromResponse = await response.Content.ReadFromJsonAsync<ICollection<Participant>>();

        response.Should().Be200Ok();
        participantsFromResponse?.Count.Should().NotBe(0);
        participantsFromResponse.Should().BeEquivalentTo(expectedParticipants);
    }

    [Test]
    public async Task GetByIdShouldReturnSuccessAndParticipant()
    {
        var participant = new ParticipantBuilder().GenerateInDatabase(Context);

        var urlWithId = $"{_baseUri}/{participant.Id}";

        var response = await _httpClient.GetAsync(urlWithId);

        var participantFromDb = ContextForAsserts.Participant.First();

        var participantFromResponse = await response.Content.ReadFromJsonAsync<Participant>();

        response.Should().Be200Ok();
        participantFromResponse.Should().BeEquivalentTo(participantFromDb);
    }

    [Test]
    public async Task CreateShouldReturnSuccessAndParticipant()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();

        var response = await _httpClient.PostAsync(_baseUri, createParticipantRequest.ToJsonContent());

        var participantFromDb = ContextForAsserts.Participant.First();

        var participantFromResponse = await response.Content.ReadFromJsonAsync<Participant>();

        response.Should().Be200Ok();
        participantFromResponse.Should().BeEquivalentTo(participantFromDb);
    }

    [Test]
    public async Task CreateShouldReturnBadRequestWhenRequestIsInvalid()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithName("")
            .Generate();

        var response = await _httpClient.PostAsync(_baseUri, createParticipantRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var customProblemDetailsExpected = new CustomProblemDetails(
            HttpStatusCode.BadRequest, 
            _baseUri, 
            new List<CustomProblemDetailsError>() 
            {
                new CustomProblemDetailsError("Name", "'Name' must not be empty.")
            });

        response.Should().Be400BadRequest();
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected);
    }

    [Test]
    public async Task UpdateShouldReturnSuccessAndParticipant()
    {
        var generatedParticipant = new ParticipantBuilder().GenerateInDatabase(Context);

        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .WithId(generatedParticipant.Id)
            .Generate();

        var urlWithId = $"{_baseUri}/{updateParticipantRequest.Id}";

        var response = await _httpClient.PutAsync(urlWithId, updateParticipantRequest.ToJsonContent());

        var participantFromDb = ContextForAsserts.Participant.First();

        var participantFromResponse = await response.Content.ReadFromJsonAsync<Participant>();

        response.Should().Be200Ok();
        participantFromResponse.Should().BeEquivalentTo(participantFromDb);
    }

    [Test]
    public async Task UpdateShouldReturnBadRequestWhenRequestIsInvalid()
    {
        var generatedParticipant = new ParticipantBuilder().GenerateInDatabase(Context);

        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .WithId(generatedParticipant.Id)
            .WithEmail("notanemailadress.com")
            .Generate();

        var urlWithId = $"{_baseUri}/{updateParticipantRequest.Id}";

        var response = await _httpClient.PutAsync(urlWithId, updateParticipantRequest.ToJsonContent());

        var problemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        var customProblemDetailsExpected = new CustomProblemDetails(
            HttpStatusCode.BadRequest,
            urlWithId,
            new List<CustomProblemDetailsError>()
            {
                new CustomProblemDetailsError("Email", "'Email' is not a valid email address.")
            });

        response.Should().Be400BadRequest();
        problemDetails.Should().BeEquivalentTo(customProblemDetailsExpected);
    }

    [Test]
    public async Task UpdateShouldReturnBadRequestWhenIdFromPathAndFromEntityDontMatch()
    {
        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .Generate();

        var urlWithId = $"{_baseUri}/{Guid.NewGuid()}";

        var response = await _httpClient.PutAsync(urlWithId, updateParticipantRequest.ToJsonContent());

        response.Should().Be400BadRequest();
    }

    [Test]
    public async Task UpdateShouldReturnNotFoundWhenParticipantIsNotFound()
    {
        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .Generate();

        var urlWithId = $"{_baseUri}/{updateParticipantRequest.Id}";

        var response = await _httpClient.PutAsync(urlWithId, updateParticipantRequest.ToJsonContent());

        response.Should().Be404NotFound();
    }
}
