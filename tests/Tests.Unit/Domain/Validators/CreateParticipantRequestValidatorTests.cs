using Bogus.Extensions.Brazil;
using FluentValidation.TestHelper;
using NetTransactions.Api.Domain.Validators;
using NetTransactions.Api.Infrastructure.Repositories;
using Tests.Common.Builders.Domain.Entities;
using Tests.Common.Builders.Domain.Request;

namespace Tests.Unit.Domain.Validators;

public class CreateParticipantRequestValidatorTests : TestsBase
{
    [SetUp]
    public void SetUp() => AutoFake.Provide(A.Fake<ParticipantRepository>());

    [Test]
    public async Task ShouldNotBeValidWhenNameIsEmpty()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithName("")
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenNameLengthIsGreaterThanMaximum()
    {
        var invalidLength = 201;

        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithName(Faker.Random.String(invalidLength))
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage($"The length of 'Name' must be 200 characters or fewer. You entered {invalidLength} characters.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenEmailIsEmpty()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithEmail("")
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("'Email' must not be empty.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenEmailLengthIsGreaterThanMaximum()
    {
        var invalidLength = 201;

        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithEmail(Faker.Random.String(invalidLength))
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage($"The length of 'Email' must be 200 characters or fewer. You entered {invalidLength} characters.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenEmailIsNotAnEmailAdress()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithEmail("notanemailadress.com")
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("'Email' is not a valid email address.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenCPFIsEmpty()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithCPF("")
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.CPF)
            .WithErrorMessage("'CPF' must not be empty.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenCPFIsInvalid()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithCPF("12345678910")
            .Generate();

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.CPF)
            .WithErrorMessage("The CPF is invalid.");
    }

    [Test]
    public async Task ShouldNotBeValidWhenCPFIsAlreadyOnDatabase()
    {
        var cpf = Faker.Person.Cpf();

        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithCPF(cpf)
            .Generate();

        A.CallTo(() => AutoFake.Resolve<ParticipantRepository>()
            .GetByCPF(cpf))
            .Returns(new ParticipantBuilder().WithCPF(cpf).Generate());

        var validate = await AutoFake.Resolve<CreateParticipantRequestValidator>()
            .TestValidateAsync(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.CPF)
            .WithErrorMessage("A participant with this CPF already exists.");
    }
}
