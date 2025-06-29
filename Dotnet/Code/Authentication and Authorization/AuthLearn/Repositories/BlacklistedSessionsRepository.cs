using AuthLearn.Database;
using AuthLearn.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthLearn.Repositories;

public interface IBlacklistedSessionsRepository
{
    public Task Blacklist(string sessionId);
    public Task<bool> ExistsAsync(string? sessionId);
}

public class BlacklistedSessionsRepository : IBlacklistedSessionsRepository
{
    private readonly AppDbContext _dbContext;

    public BlacklistedSessionsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync(string? sessionId)
    {
        if (sessionId == null)
            return false;

        var session = await _dbContext.CookiesBlackList.AsNoTracking()
            .FirstOrDefaultAsync(t => t.SessionId == sessionId);
        return session is not null;
    }


    public async Task Blacklist(string sessionId)
    {
        if (await ExistsAsync(sessionId))
            return;
        await _dbContext.CookiesBlackList.AddAsync(new SessionBlackListEntity
        {
            SessionId = sessionId
        });
        await _dbContext.SaveChangesAsync();
    }
}