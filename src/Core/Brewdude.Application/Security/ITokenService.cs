using Brewdude.Domain.Entities;

namespace Brewdude.Application.Security
{
    public interface ITokenService
    {
        bool ValidateUserEmail(string token, BrewdudeUser user);
        string CreateToken(BrewdudeUser user, Role role);
        string CreateAnonymousToken();
    }
}