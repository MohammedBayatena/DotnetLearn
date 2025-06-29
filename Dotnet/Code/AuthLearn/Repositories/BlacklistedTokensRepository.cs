using AuthLearn.Database;
using AuthLearn.Entities;
using AuthLearn.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AuthLearn.Repositories;

public interface IBlacklistedTokensRepository
{
    public Task Blacklist(string token);
    public Task<bool> ExistsAsync(string token);
}

public class BlacklistedTokensRepository : IBlacklistedTokensRepository
{
    private readonly AppDbContext _dbContext;

    public BlacklistedTokensRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Blacklist(string token)
    {
        if (await ExistsAsync(token))
        {
            return;
        }

        await _dbContext.TokensBlackList.AddAsync(new TokenBlackListEntity()
        {
            TokenHash = TokenHasher.HashToken(token)
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string token)
    {
        var tokenEntity = await _dbContext.TokensBlackList
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TokenHash == TokenHasher.HashToken(token));
        return tokenEntity is not null;
    }
}