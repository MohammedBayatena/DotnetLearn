using AuthLearn.Database;
using AuthLearn.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthLearn.Repositories;

public interface IRefreshTokensRepository
{
    public Task<IEnumerable<RefreshTokenInfoEntity>> GetRefreshTokensAsync();
    public Task SaveRefreshTokenAsync(RefreshTokenInfoEntity refreshTokenInfo);

    public Task<RefreshTokenInfoEntity?> GetRefreshTokenAsync(string token);

    public Task InvalidateRefreshTokenAsync(string token);
}

public class RefreshRefreshTokensRepository : IRefreshTokensRepository
{
    private readonly AppDbContext _context;

    public RefreshRefreshTokensRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshTokenInfoEntity?> GetRefreshTokenAsync(string token)
    {
        return await _context.TokensInfo.AsNoTracking().SingleOrDefaultAsync(t => t.Token == token);
    }

    public async Task<IEnumerable<RefreshTokenInfoEntity>> GetRefreshTokensAsync()
    {
        return await _context.TokensInfo.AsNoTracking().ToListAsync();
    }

    public async Task InvalidateRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.TokensInfo.SingleOrDefaultAsync(t => t.Token == token);
        if (refreshToken == null) return;
        refreshToken.Valid = false;
        _context.TokensInfo.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task SaveRefreshTokenAsync(RefreshTokenInfoEntity refreshTokenInfo)
    {
        await _context.TokensInfo.AddAsync(refreshTokenInfo);
        await _context.SaveChangesAsync();
    }
}