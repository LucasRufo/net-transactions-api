using Microsoft.EntityFrameworkCore;
using NetTransactions.Api.Domain.Entities;
using NetTransactions.Api.Extensions;

namespace NetTransactions.Api.Infrastructure.Repositories;

public class ParticipantRepository
{
    private readonly TransactionsDbContext _dbContext;

    public ParticipantRepository(TransactionsDbContext context)
    {
        _dbContext = context;
    }

    public virtual async Task<ICollection<Participant>> Get()
        => await _dbContext.Participant.OrderBy(x => x.CreatedAt).ToListAsync();

    public virtual async Task<Participant?> GetById(Guid id)
        => await _dbContext.Participant.FirstOrDefaultAsync(x => x.Id == id);

    public virtual async Task<Participant?> GetByCPF(string cpf)
        => await _dbContext.Participant.FirstOrDefaultAsync(x => x.CPF == cpf);

    public virtual async Task Save(Participant participant)
    {
        if (!_dbContext.ExistsInTracker(participant))
            await _dbContext.AddAsync(participant);

        await _dbContext.SaveChangesAsync();
    }
}
