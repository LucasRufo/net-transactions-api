using NetTransactions.Api.Domain.Services;
using NetTransactions.Api.Infrastructure.DateTimeProvider;
using NetTransactions.Api.Infrastructure.Repositories;
using Tests.Common.Builders.Domain.Entities;
using Tests.Common.Builders.Domain.Request;
using Tests.Unit.Configuration;

namespace Tests.Unit.Domain.Service;

public class ParticipantServiceTests : TestsBase
{
    [SetUp]
    public void SetUp()
    {
        AutoFake.WithInMemoryContext();
        AutoFake.Provide(A.Fake<ParticipantRepository>());
        AutoFake.Provide(A.Fake<IDateTimeProvider>());
    }

    [Test]
    public async Task ShouldGetAllParticipants()
    {
        var expectedParticipants = new ParticipantBuilder().Generate(3);

        A.CallTo(() => AutoFake
            .Resolve<ParticipantRepository>().Get())
            .Returns(expectedParticipants);

        var participants = await AutoFake.Resolve<ParticipantService>()
            .Get();

        participants.Count.Should().NotBe(0);
        participants.Should().BeEquivalentTo(expectedParticipants);
    }

    [Test]
    public async Task ShouldGetParticipantById()
    {
        var expectedParticipant = new ParticipantBuilder().Generate();

        A.CallTo(() => AutoFake
            .Resolve<ParticipantRepository>().GetById(expectedParticipant.Id))
            .Returns(expectedParticipant);

        var participant = await AutoFake.Resolve<ParticipantService>()
            .GetById(expectedParticipant.Id);

        participant.Should().BeEquivalentTo(expectedParticipant);
    }

    [Test]
    public async Task ShouldCreateParticipant()
    {
        var createParticipantRequest = new ParticipantRequestBuilder().Generate();
        var createdAtFake = Faker.Date.Past();

        A.CallTo(() => AutoFake
            .Resolve<IDateTimeProvider>().UtcNow)
            .Returns(createdAtFake);

        var participantResult = await AutoFake.Resolve<ParticipantService>()
            .Create(createParticipantRequest);

        var expectedParticipant = new ParticipantBuilder()
            .WithId(participantResult.Value.Id)
            .WithName(createParticipantRequest.Name)
            .WithEmail(createParticipantRequest.Email)
            .WithCPF(createParticipantRequest.CPF)
            .WithCreatedAt(createdAtFake)
            .WithUpdateAt(null)
            .Generate();

        participantResult.IsError.Should().BeFalse();
        participantResult.Value.Should().BeEquivalentTo(expectedParticipant);
    }
}
