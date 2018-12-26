using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Application.Security;
using Brewdude.Application.User.Commands.Models;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(BrewdudeDbContext context, IUserService userService, IMapper mapper, ITokenService tokenService)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
            
            if (existingUser != null)
                throw new UserCreationException($"User with username [{request.Username}] already exists");
            
            _userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            if (passwordHash == null || passwordHash.Length != 64 &&
                    passwordSalt == null || passwordSalt.Length != 128)
            {
                throw new UserCreationException("Error attempting to create user password hash");
            }

            var user = new Domain.Entities.User
            {
                Email = request.Email,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            
            // Generate a token for immediate use
            var token = _tokenService.CreateToken(user);
            if (string.IsNullOrWhiteSpace(token))
                throw new UserCreationException("Token generation failed during user creation");
            
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Token = token;

            return userViewModel;
        }
    }
}