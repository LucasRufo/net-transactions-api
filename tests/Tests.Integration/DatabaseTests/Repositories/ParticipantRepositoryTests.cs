using Microsoft.Extensions.DependencyInjection;
using NetTransactions.Api.Infrastructure.Repositories;
using Tests.Common.Builders.Domain.Entities;
using Tests.Integration.Configuration.Extensions;

namespace Tests.Integration.DatabaseTests.Repositories;

public class ParticipantRepositoryTests : IntegrationTestsBase
{
    private ParticipantRepository _participantRepository;

    [SetUp]
    public void SetUp() => _participantRepository = ServiceProvider.GetRequiredService<ParticipantRepository>();

    [Test]
    public async Task ShouldGetAllParticipants()
    {
        var expectedParticipants = new ParticipantBuilder().GenerateInDatabase(Context, 5);

        var participants = await _participantRepository.Get();

        participants.Should().BeEquivalentTo(expectedParticipants);
    }

    [Test]
    public async Task ShouldGetParticipantById()
    {
        var expectedParticipant = new ParticipantBuilder().GenerateInDatabase(Context);

        var participant = await _participantRepository.GetById(expectedParticipant.Id);

        participant.Should().BeEquivalentTo(expectedParticipant);
    }

    [Test]
    public async Task ShouldGetParticipantByCPF()
    {
        var expectedParticipant = new ParticipantBuilder().GenerateInDatabase(Context);

        var participant = await _participantRepository.GetByCPF(expectedParticipant.CPF);

        participant.Should().BeEquivalentTo(expectedParticipant);
    }

    [Test]
    public async Task ShouldCreateParticipant()
    {
        var expectedParticipant = new ParticipantBuilder().Generate();

        await _participantRepository.Create(expectedParticipant);

        var participant = ContextForAsserts.Participant
                    .FirstOrDefault(x => x.Id == expectedParticipant.Id);

        participant.Should().BeEquivalentTo(expectedParticipant);
    }
}
