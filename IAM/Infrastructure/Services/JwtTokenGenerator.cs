using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pathly_backend.IAM.Domain.Entities;

namespace pathly_backend.IAM.Infrastructure.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int    _expiresMinutes;

    public JwtTokenGenerator(IConfiguration cfg)
    {
        _key           = cfg["Jwt:Key"]                     ?? throw new InvalidOperationException("Jwt:Key missing");
        _issuer        = cfg["Jwt:Issuer"]                  ?? "Pathly";
        _audience      = cfg["Jwt:Audience"]                ?? _issuer;
        _expiresMinutes = int.TryParse(cfg["Jwt:ExpiresMinutes"], out var m) ? m : 15;
    }

    public string GenerateToken(User user)
    {
        var creds  = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer:     _issuer,
            audience:   _audience,
            claims:     claims,
            expires:    DateTime.UtcNow.AddMinutes(_expiresMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}