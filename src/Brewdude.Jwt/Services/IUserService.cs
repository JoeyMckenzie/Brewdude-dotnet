using System.Threading.Tasks;
using Brewdude.Jwt.Models;

namespace Brewdude.Jwt.Services
{
    public interface IUserService
    {
        Task<UserViewModel> LoginUserByUsername(string username, string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}