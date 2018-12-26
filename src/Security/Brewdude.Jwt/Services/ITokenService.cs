using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Brewdude.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Brewdude.Jwt.Services
{
    public interface ITokenService
    {
        bool ValidateUserEmail(string token, User user);
        string CreateToken(User user);
        string CreateAnonymousToken();
    }
}