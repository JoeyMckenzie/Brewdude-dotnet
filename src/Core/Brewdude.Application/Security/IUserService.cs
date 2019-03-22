using System.Threading.Tasks;
using Brewdude.Domain.ViewModels;

namespace Brewdude.Application.Security
{
    public interface IUserService
    {
        Task<UserViewModel> LoginUserByUsername(string username, string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}