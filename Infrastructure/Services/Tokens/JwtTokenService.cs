using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Tokens;


public class JwtTokenService(IConfiguration configuration)
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="InvalidOperationException">When JWT key  or Audience or Issuiser is missing. Also when Email is missing</exception>
    public string GenerateToken(User user, IEnumerable<string> roles  )
    {
        if (user.Email is null)
        {
            throw new InvalidOperationException("Missing Email");
        }
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = configuration["Jwt:Key"];
        if (key == null)
        {
            throw new InvalidOperationException("Missing JWT key");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );
        var issuer = configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("Missing JWT issuer");

        var audience = configuration["Jwt:Audience"]
                       ?? throw new InvalidOperationException("Missing JWT audience");
        
      var securityToken =  new JwtSecurityToken(
            issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: credentials
            );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(securityToken);
            return token;
    }   
}