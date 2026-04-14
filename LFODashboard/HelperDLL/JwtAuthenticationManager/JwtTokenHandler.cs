using JwtAuthenticationManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        private string JWT_SECURITY_KEY;
        private int JWT_TOKEN_VALIDITY_MINS;

        public JwtTokenHandler(IConfiguration config)
        {
            JWT_SECURITY_KEY = config["Jwt:SecurityKey"];
            JWT_TOKEN_VALIDITY_MINS = int.Parse(config["Jwt:TokenValidityInMinutes"]);
        }

        public AuthenticationResponse GenrateJwtToken(AuthenticationRequest authenticationRequest)
        {
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenkey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim(ClaimTypes.NameIdentifier, authenticationRequest.LoginId.ToString()),
                new Claim(ClaimTypes.Name, authenticationRequest.MobileNo),
                new Claim(ClaimTypes.Role, authenticationRequest.AccessType)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenkey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            // Generate Refresh Token

            var refreshToken = GenerateRefreshTokenJwt(authenticationRequest.MobileNo);

            return new AuthenticationResponse
            {
                UserName = authenticationRequest.MobileNo,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token,
                RefreshToken = refreshToken
            };
        }

        public AuthenticationResponse GetTokenByRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userName = principal.Claims .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;

                if (string.IsNullOrEmpty(userName))
                    return null;

                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);

                var claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, userName)
                });

                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature);

                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = tokenExpiryTimeStamp,
                    SigningCredentials = signingCredentials
                };

                var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
                var newJwtToken = tokenHandler.WriteToken(securityToken);

                return new AuthenticationResponse
                {
                    UserName = userName,
                    JwtToken = newJwtToken,
                    RefreshToken = refreshToken, // returning same refresh token
                    ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
                };
            }
            catch
            {
                return null;
            }
        }

        private string GenerateRefreshTokenJwt(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            var tokenExpiryTimeStamp = DateTime.Now.AddDays(7); // longer expiry

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim("token_type", "refresh") // important to differentiate
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
