using FluentValidation;
using NetTransactions.Api.Domain.Request;

namespace NetTransactions.Api.Domain.Validators;

public class UpdateParticipantRequestValidator : AbstractValidator<UpdateParticipantRequest>
{
    public UpdateParticipantRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .MaximumLength(200)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x).Must(x =>
        {
            return !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.Email);
        }).WithMessage("At least one field must be filled."); ;
    }
}
