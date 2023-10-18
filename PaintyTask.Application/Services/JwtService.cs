using Microsoft.IdentityModel.Tokens;
using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Domain.Exceptions;
using PaintyTask.Domain.Models.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaintyTask.Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtConfig _jwtConfig;

    public JwtService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string GenerateJwt(string email, int userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email),
            new Claim("UserId", userId.ToString())
        };

        var claimsIdentity = new ClaimsIdentity
        (
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken
        (
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            notBefore: now,
            claims: claimsIdentity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(_jwtConfig.LifeTime)),
            signingCredentials: credentials
        );

        var encodedJwt = new JwtSecurityTokenHandler()
            .WriteToken(jwt);

        return encodedJwt;
    }

    public int GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(token) ?? throw new InvalidJwtTokenException("Invalid JWT token");

        var userId = int.Parse(decodedToken.Claims
            .FirstOrDefault(c => c.Type == "UserId")!.Value);

        return userId;
    }
}