namespace Brewdude.Application.Security
{
    using Domain.Entities;

    public interface ITokenService
    {
        string CreateToken(BrewdudeUser user, Role role);

        string CreateAnonymousToken();
    }
}