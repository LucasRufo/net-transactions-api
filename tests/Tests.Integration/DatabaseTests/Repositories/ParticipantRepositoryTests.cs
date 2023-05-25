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
    public async Task ShouldCreateParticipant()
    {
        var expectedParticipant = new ParticipantBuilder().Generate();

        await _participantRepository.Create(expectedParticipant);

        var returnedParticipant = ContextForAsserts.Participant
                    .FirstOrDefault(x => x.Id == expectedParticipant.Id);

        returnedParticipant.Should().BeEquivalentTo(expectedParticipant);
    }

    [Test]
    public async Task ShouldGetParticipantByCPF()
    {
        var expectedParticipant = new ParticipantBuilder().GenerateInDatabase(Context);

        var participantFromDb = await _participantRepository.GetByCPF(expectedParticipant.CPF);

        participantFromDb.Should().BeEquivalentTo(expectedParticipant);
    }
}
