using Common.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

public class CL_JWTtokenGeneration
{
    private readonly IConfiguration _config;

    public CL_JWTtokenGeneration(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken()
    {
        var key = _config["SprintService:JWT-Key"];
        var partnerId = _config["SprintService:User-Agent"];

        // 🔴 Safety checks (VERY IMPORTANT)
        if (string.IsNullOrWhiteSpace(key))
            throw new AppException("JWT Key is missing in configuration");

        if (string.IsNullOrWhiteSpace(partnerId))
            throw new AppException("User-Agent (PartnerId) is missing in configuration");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var reqId = GenerateSecureRandomNumber();

        // ✅ Better way using claims (cleaner)
        var claims = new List<System.Security.Claims.Claim>
        {
            new("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()),
            new("partnerId", partnerId),
            new("reqid", reqId.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5), // ✅ expiry
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private int GenerateSecureRandomNumber()
    {
        var bytes = new byte[4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0) & int.MaxValue;
    }
}