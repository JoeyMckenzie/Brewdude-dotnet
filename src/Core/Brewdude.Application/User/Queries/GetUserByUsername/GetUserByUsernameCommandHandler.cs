using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Application.Security;
using Brewdude.Application.User.Models;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.User.Queries.GetUserByUsername
{
    public class GetUserByUsernameCommandHandler : IRequestHandler<GetUserByUsernameCommand, UserViewModel>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<BrewdudeUser> _userManager;

        public GetUserByUsernameCommandHandler(IMapper mapper, ITokenService tokenService, UserManager<BrewdudeUser> userManager)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<UserViewModel> Handle(GetUserByUsernameCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                // Throw if user does not exist
                throw new UserNotFoundException($"User with username [{request.Username}] does not exist");
            }
            
            // Validate the request user's password against the stored hash and salt
            var isVerifiedPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isVerifiedPassword)
            {
                // Throw on bad password
                throw new ArgumentException($"Wrong password for user [{request.Username}]");
            }

            // Generate a token for immediate use
            var userRoles = await _userManager.GetRolesAsync(user);
            Role userRole;
            if (userRoles.Any())
            {
                var roleExists = Enum.TryParse(userRoles[0], out userRole);
                if (!roleExists)
                {
                    // Default to user role if no role is found for the retrieved user
                    userRole = Role.User;
                }
            }
            else
            {
                userRole = Role.User;
            }
            
            var token = _tokenService.CreateToken(user, userRole);
            if (string.IsNullOrWhiteSpace(token))
            {
                // Throw on token create error
                throw new UserCreationException("Token generation failed during user creation");
            }   
            
            // Map the entity user to view model
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Token = token;
            userViewModel.Role = userRole.ToString();
            
            return userViewModel;
        }
    }
}