using ErrorOr;
using NetTransactions.Api.Domain.Entities;
using NetTransactions.Api.Domain.Request;
using NetTransactions.Api.Infrastructure.DateTimeProvider;
using NetTransactions.Api.Infrastructure.Repositories;

namespace NetTransactions.Api.Domain.Services;

public class ParticipantService
{
    private readonly ParticipantRepository _participantRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ParticipantService(ParticipantRepository participantRepository, IDateTimeProvider dateTimeProvider)
    {
        _participantRepository = participantRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Participant>> Create(CreateParticipantRequest request)
    {
        var participant = new Participant()
        {
            Name = request.Name,
            Document = request.Document,
            Email = request.Email,
            CreatedAt = _dateTimeProvider.UtcNow
        };

        await _participantRepository.Create(participant);

        return participant;
    }
}
