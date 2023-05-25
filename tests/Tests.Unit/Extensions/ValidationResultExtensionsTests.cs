using FluentValidation.Results;
using NetTransactions.Api.Controllers.Shared;
using NetTransactions.Api.Extensions;

namespace Tests.Unit.Extensions;

public class ValidationResultExtensionsTests : TestsBase
{
    [Test]
    public void ShouldConvertValidationResultToCustomProblemDetailsError()
    {
        var property = "Property";
        var error = "Error";

        var validationResult = new ValidationResult(new List<ValidationFailure>() 
        {
            new ValidationFailure(property, error)
        });

        var expectedCustomProblemDetailsError = new List<CustomProblemDetailsError>()
        {
            new CustomProblemDetailsError(property, error)
        };

        var customProblemDetailsError = validationResult.ToCustomProblemDetailsError();

        customProblemDetailsError.Should().BeEquivalentTo(expectedCustomProblemDetailsError);
    }
}
