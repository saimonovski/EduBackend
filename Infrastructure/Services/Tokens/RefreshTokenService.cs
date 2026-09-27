using System.Security.Cryptography;
using System.Text;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Tokens;

public class RefreshTokenService(ApplicationDbContext dbContext)
{
    public async Task<(bool Found, RefreshToken? Token)> CheckRefreshTokenAsync(string refreshToken)
    {
        var hashed = HashToken(refreshToken);

       var token = await  dbContext.RefreshTokens
           .Include(t => t.User)
           .FirstOrDefaultAsync(t => t.TokenHash.Equals(hashed) && DateTime.UtcNow < t.ExpiresAt);

       return token == null ? (false, null) : (true, token);
    }
    private string GenerateRefreshToken()
    {
      var bytes =   RandomNumberGenerator.GetBytes(64);
      var token = Convert.ToBase64String(bytes);
      return token;
    }

    private void RemoveToken(RefreshToken token)
    {
        dbContext.RefreshTokens.Remove(token);
    }

    public async Task<string> RotateRefreshTokenAsync(RefreshToken token)
    {
        var newToken = CreateRefreshToken(token.User);
        RemoveToken(token);
        await dbContext.SaveChangesAsync();
        return newToken.RawToken;
    }
    
    private string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hashedBytes = SHA256.HashData(bytes);
        
        var hashedToken  = Convert.ToBase64String(hashedBytes);
        return hashedToken;
    }

    private (string RawToken, RefreshToken RefreshToken) CreateRefreshToken(User user)
    {
        var rawToken = GenerateRefreshToken();
        var refreshToken = new RefreshToken()
        {
            User = user,
            TokenHash = HashToken(rawToken),
        };
        dbContext.RefreshTokens.Add(refreshToken);
        return (rawToken,refreshToken);
    }

    public async Task<string>  LoginRefreshToken(User user)
    {
        var (rawToken, _) = CreateRefreshToken(user);
        await dbContext.SaveChangesAsync();
        return rawToken;
    }
}
