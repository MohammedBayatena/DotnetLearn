using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthLearn.Entities;
using AuthLearn.Options;
using AuthLearn.Repositories;
using AuthLearn.Resources;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace AuthLearn.JWTAuthentication;

public interface IJwtTokenService
{
    Task<TokenInfoResource> GenerateTokenAsync(User user);
    Task<TokenInfoResource?> ValidateRefreshTokenAsync(User user, string token);
    Task InvalidateRefreshTokenAsync(string token);
    Task InvalidateTokenAsync(string token);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IOptions<JwtSettingsConfig> _jwtConfig;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IBlacklistedTokensRepository _blackListedTokensRepository;
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);

    public JwtTokenService(
        IOptions<JwtSettingsConfig> jwtConfig,
        IRefreshTokensRepository refreshTokensRepository,
        IBlacklistedTokensRepository blackListedTokensRepository)
    {
        _jwtConfig = jwtConfig;
        _refreshTokensRepository = refreshTokensRepository;
        _blackListedTokensRepository = blackListedTokensRepository;
    }

    public async Task<TokenInfoResource> GenerateTokenAsync(User user)
    {
        var tokenSecret = _jwtConfig.Value.Key;
        var issuer = _jwtConfig.Value.Issuer;
        var audience = _jwtConfig.Value.Audience;
        var expiryTimeStamp = DateTime.UtcNow.Add(TokenLifetime);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(tokenSecret);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Name),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("Admin", user.IsAdmin ? "true" : "false"),
            new("userId", user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        //Generate and Save Refresh Token to Secure Storage
        var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

        return new TokenInfoResource()
        {
            AccessToken = jwtToken,
            Expires = expiryTimeStamp,
            RefreshToken = refreshToken
        };
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshTokenValidityDays = _jwtConfig.Value.RefreshTokenValidityDays;
        var refreshToken = new RefreshTokenInfoEntity()
        {
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.UtcNow.AddDays(refreshTokenValidityDays),
            UserId = user.Id.ToString(),
            Valid = true
        };

        await _refreshTokensRepository.SaveRefreshTokenAsync(refreshToken);
        return refreshToken.Token;
    }

    public async Task<TokenInfoResource?> ValidateRefreshTokenAsync(User user, string token)
    {
        var refreshToken = await _refreshTokensRepository.GetRefreshTokenAsync(token);
        if (refreshToken is not { Valid: true } || refreshToken.Expires < DateTime.UtcNow)
        {
            return null;
        }

        //Invalidate Old Token
        await InvalidateRefreshTokenAsync(token);

        //Create new JWT Token for the User
        return await GenerateTokenAsync(user);
    }

    public async Task InvalidateRefreshTokenAsync(string token)
    {
        await _refreshTokensRepository.InvalidateRefreshTokenAsync(token);
    }

    public async Task InvalidateTokenAsync(string token)
    {
        await _blackListedTokensRepository.Blacklist(token);
    }
}