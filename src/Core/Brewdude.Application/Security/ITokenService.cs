namespace Brewdude.Application.Security
{
    public interface ITokenService
    {
        bool ValidateUserEmail(string token, Domain.Entities.User user);
        string CreateToken(Domain.Entities.User user);
        string CreateAnonymousToken();
    }
}