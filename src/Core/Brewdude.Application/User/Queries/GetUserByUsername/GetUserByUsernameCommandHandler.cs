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

namespace Brewdude.Application.User.Queries.GetUserByUsername
{
    public class GetUserByUsernameCommandHandler : IRequestHandler<GetUserByUsernameCommand, UserViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetUserByUsernameCommandHandler(IMapper mapper, BrewdudeDbContext context, IUserService userService, ITokenService tokenService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<UserViewModel> Handle(GetUserByUsernameCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null)
                throw new UserNotFoundException($"User [{request.Username}] does not exists");

            // Validate the request user's password against the stored hash and salt
            var isVerifiedPassword =
                _userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            
            if (!isVerifiedPassword)
                throw new ArgumentException($"Wrong password for user [{request.Username}]");

            // Generate a token for immediate use
            var token = _tokenService.CreateToken(user);
            if (string.IsNullOrWhiteSpace(token))
                throw new UserCreationException("Token generation failed during username retrieval");
            
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Token = token;
            
            return userViewModel;
        }
    }
}