using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Brewdude.Application.Security;
using Brewdude.Common;
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
        /// <param name="token">Generated token from register/login</param>
        /// <param name="user">BrewdudeUser entity</param>
        /// <returns>True if email is found and matches between user entity and token claim</returns>
        public bool ValidateUserEmail(string token, BrewdudeUser user)
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

        public string CreateToken(BrewdudeUser user, Role role)
        {
            // Instantiate scopes to be added to the token
            var brewdudeScopes = new BrewdudeScopes();
            HashSet<string> scopes;
            switch (role)
            {
                case Role.SuperUser:
                    scopes = brewdudeScopes.GetAllScopes().ToHashSet();
                    break;
                case Role.Admin:
                    scopes = brewdudeScopes.GetAdminUserScopes().ToHashSet();
                    break;
                case Role.User:
                    scopes = brewdudeScopes.GetUserScopes().ToHashSet();
                    break;
                default:
                    scopes = brewdudeScopes.GetAllScopes().ToHashSet();
                    break;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = BuildRoleBasedClaims(user, role, scopes),
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

        private static ClaimsIdentity BuildRoleBasedClaims(BrewdudeUser user, Role role, IEnumerable<string> scopes)
        {
            var scopeClaims = new HashSet<Claim>();
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.UserName),
                new Claim(ClaimTypes.UserData, user.FirstName),
                new Claim(ClaimTypes.UserData, user.LastName),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("username", user.UserName),
            };

            foreach (var scope in scopes)
                scopeClaims.Add(new Claim("scopes", scope));
            
            return new ClaimsIdentity(claims.Concat(scopeClaims));
        }
    }
}