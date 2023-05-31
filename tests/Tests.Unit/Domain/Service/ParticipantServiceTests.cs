using ErrorOr;
using NetTransactions.Api.Domain.Entities;
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
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();
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

    [Test]
    public async Task ShouldUpdateParticipant()
    {
        var updateParticipantRequest = new UpdateParticipantRequestBuilder().Generate();
        var participantFake = new ParticipantBuilder()
            .WithId(updateParticipantRequest.Id)
            .Generate();
        var updatedAtFake = Faker.Date.Past();

        A.CallTo(() => AutoFake
            .Resolve<IDateTimeProvider>().UtcNow)
            .Returns(updatedAtFake);

        A.CallTo(() => AutoFake
            .Resolve<ParticipantRepository>().GetById(updateParticipantRequest.Id))
            .Returns(participantFake);

        var participantResult = await AutoFake.Resolve<ParticipantService>()
            .Update(updateParticipantRequest);

        var expectedParticipant = new ParticipantBuilder()
            .WithId(updateParticipantRequest.Id)
            .WithName(updateParticipantRequest.Name)
            .WithCPF(participantFake.CPF)
            .WithEmail(updateParticipantRequest.Email)
            .WithCreatedAt(participantFake.CreatedAt)
            .WithUpdateAt(updatedAtFake)
            .Generate();

        participantResult.IsError.Should().BeFalse();
        participantResult.Value.Should().BeEquivalentTo(expectedParticipant);
    }

    [Test]
    public async Task UpdateShouldReturnErrorWhenParticipantIsNotFound()
    {
        var updateParticipantRequest = new UpdateParticipantRequestBuilder().Generate();

        A.CallTo(() => AutoFake
            .Resolve<ParticipantRepository>().GetById(updateParticipantRequest.Id))
            .Returns((Participant?)null);

        var participantResult = await AutoFake.Resolve<ParticipantService>()
            .Update(updateParticipantRequest);

        participantResult.IsError.Should().BeTrue();
        participantResult.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}
