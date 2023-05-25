using FluentValidation;
using NetTransactions.Api.Domain.Request;
using NetTransactions.Api.Infrastructure.Repositories;

namespace NetTransactions.Api.Domain.Validators;

public class ParticipantRequestValidator : AbstractValidator<ParticipantRequest>
{
    public ParticipantRequestValidator(ParticipantRepository participantRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(200)
            .EmailAddress();

        RuleFor(x => x.CPF)
            .NotEmpty()
            .IsValidCPF().WithMessage("The CPF is invalid.")
            .MustAsync(async (cpf, cancellation) =>
            {
                var existingParticipant = await participantRepository.GetByCPF(cpf);
                return existingParticipant is null;
            }).WithMessage("A participant with this CPF already exists.");
    }
}
