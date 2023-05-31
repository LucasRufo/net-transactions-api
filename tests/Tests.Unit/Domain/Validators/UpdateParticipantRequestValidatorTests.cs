using FluentValidation.TestHelper;
using NetTransactions.Api.Domain.Validators;
using Tests.Common.Builders.Domain.Request;

namespace Tests.Unit.Domain.Validators;

public class UpdateParticipantRequestValidatorTests : TestsBase
{
    private readonly UpdateParticipantRequestValidator _validator = new();

    [Test]
    public void ShouldNotBeValidWhenNameLengthIsGreaterThanMaximum()
    {
        var invalidLength = 201;

        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .WithName(Faker.Random.String(invalidLength))
            .Generate();

        var validate = _validator.TestValidate(updateParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage($"The length of 'Name' must be 200 characters or fewer. You entered {invalidLength} characters.");
    }

    [Test]
    public void ShouldNotBeValidWhenEmailLengthIsGreaterThanMaximum()
    {
        var invalidLength = 201;

        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .WithEmail(Faker.Random.String(invalidLength))
            .Generate();

        var validate = _validator.TestValidate(updateParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage($"The length of 'Email' must be 200 characters or fewer. You entered {invalidLength} characters.");
    }

    [Test]
    public void ShouldNotBeValidWhenEmailIsNotAnEmailAdress()
    {
        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .WithEmail("notanemailadress.com")
            .Generate();

        var validate = _validator.TestValidate(updateParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("'Email' is not a valid email address.");
    }

    [Test]
    public void ShouldNotBeValidWhenAllFieldsAreEmpty()
    {
        var updateParticipantRequest = new UpdateParticipantRequestBuilder()
            .WithEmail("")
            .WithName("")
            .Generate();

        var validate = _validator.TestValidate(updateParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("At least one field must be filled.");
    }
}
