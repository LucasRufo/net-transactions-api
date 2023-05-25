using Microsoft.EntityFrameworkCore;
using NetTransactions.Api.Domain.Entities;

namespace NetTransactions.Api.Infrastructure.Repositories;

public class ParticipantRepository
{
    private readonly TransactionsDbContext _dbContext;

    public ParticipantRepository(TransactionsDbContext context)
    {
        _dbContext = context;
    }

    public virtual async Task<Participant?> GetByCPF(string cpf)
        => await _dbContext.Participant.FirstOrDefaultAsync(x => x.CPF == cpf);

    public async Task Create(Participant participant)
    {
        await _dbContext.AddAsync(participant);
        await _dbContext.SaveChangesAsync();
    }
}
