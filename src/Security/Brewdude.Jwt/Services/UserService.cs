using System;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Security;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using UserViewModel = Brewdude.Application.User.Commands.Models.UserViewModel;

namespace Brewdude.Jwt.Services
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly BrewdudeDbContext _context;

        public UserService(IMediator mediator, IMapper mapper, BrewdudeDbContext context, ILogger<UserService> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public Task<UserViewModel> LoginUserByUsername(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("User attempting to login contains invalid model state");
            
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Helper method to create stored passwords using HMAC SHA512, and validates the password before salting and hashing
        /// </summary>
        /// <param name="password">Raw text password</param>
        /// <param name="passwordHash">Returned password hash, if successful</param>
        /// <param name="passwordSalt">Returned password salt, if successful</param>
        /// <exception cref="ArgumentNullException">Thrown for null input password</exception>
        /// <exception cref="ArgumentException">Thrown if password is empty</exception>
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) 
                throw new ArgumentNullException(nameof(password), "Password cannot be null");
            
            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string.");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
        /// <summary>
        /// Helper to verify password of user by validating the salt and hash stored in user table
        /// </summary>
        /// <param name="password">Raw text password</param>
        /// <param name="storedHash">Returned password hash, if successful</param>
        /// <param name="storedSalt">Returned password salt, if successful</param>
        /// <returns>Boolean status of verification attempt</returns>
        /// <exception cref="ArgumentNullException">Throw for null input password</exception>
        /// <exception cref="ArgumentException">Throw if password is empty</exception>
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) 
                throw new ArgumentNullException("Password is null");
            
            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string");
            
            if (storedHash.Length != 64) 
                throw new ArgumentException($"Invalid length of password hash (64 bytes expected): {storedHash}");
            
            if (storedSalt.Length != 128) 
                throw new ArgumentException($"Invalid length of password salt (128 bytes expected: {storedSalt}");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
 
            return true;
        }
    }
}