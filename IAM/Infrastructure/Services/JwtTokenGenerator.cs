using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IAM.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IAM.Infrastructure.Services;

public class JwtTokenGenerator
{
    private readonly IConfiguration _cfg;

    public JwtTokenGenerator(IConfiguration cfg) => _cfg = cfg;

    public string Generate(User user)
    {
        var s = _cfg.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s["Key"]!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: s["Issuer"],
            audience: s["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(s["ExpiresMinutes"]!)),
            signingCredentials: cred);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}