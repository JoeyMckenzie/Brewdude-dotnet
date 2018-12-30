using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Brewdude.Application.Security;
using Brewdude.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Brewdude.Jwt.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private const string Email = "email";

        public TokenService(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        /// <summary>
        /// Attempt to validate the user email passed contained in the token
        /// </summary>
        /// <param name="token">User entity</param>
        /// <returns>True if email is found and matches between user entity and token claim</returns>
        public bool ValidateUserEmail(string token, User user)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (jwtSecurityTokenHandler.CanReadToken(token) && jwtSecurityTokenHandler.CanValidateToken)
            {
                var securityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                foreach (var claim in securityToken.Claims)
                {
                    if (claim.Type == Email)
                    {
                        if (!string.IsNullOrWhiteSpace(claim.Value) && 
                                string.Equals(claim.Value, user.Email,
                                    StringComparison.CurrentCultureIgnoreCase))
                        {
                            return true;
                        }

                        break;
                    }
                }
            }

            return false;
        }

        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = BuildRoleBasedClaims(user),
                Issuer = "https://localhost:5001", // TODO: Modify once DNS is set
                Audience = "https://localhost:5001",
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string CreateAnonymousToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Anonymous, "anonymous"), 
                    new Claim(ClaimTypes.Name, "ANONYMOUS_USER"), 
                }),
                Issuer = "https://localhost:6001", // TODO: Modify once DNS is set
                Audience = "https://localhost:5001",
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private static ClaimsIdentity BuildRoleBasedClaims(User user)
        {
            if (user.Role == Role.Admin)
            {
                return new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.UserData, user.Username),
                    new Claim(ClaimTypes.UserData, user.FirstName),
                    new Claim(ClaimTypes.UserData, user.LastName),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("scopes", "read:beer"),
                    new Claim("scopes", "read:brewery"),
                    new Claim("scopes", "write:beer"),
                    new Claim("scopes", "write:brewery"),
                    new Claim("username", user.Username)
                });
            }
            
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Username),
                new Claim(ClaimTypes.UserData, user.FirstName),
                new Claim(ClaimTypes.UserData, user.LastName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("scopes", "read:beer"),
                new Claim("scopes", "read:brewery"),
                new Claim("username", user.Username)
            });   
        }
    }
}