using NetTransactions.Api.Domain.Entities;

namespace NetTransactions.Api.Infrastructure.Repositories;

public class ParticipantRepository
{
    private readonly TransactionsDbContext _dbContext;

    public ParticipantRepository(TransactionsDbContext context)
    {
        _dbContext = context;
    }

    public async Task Create(Participant participant)
    {
        await _dbContext.AddAsync(participant);
        await _dbContext.SaveChangesAsync();
    }
}
