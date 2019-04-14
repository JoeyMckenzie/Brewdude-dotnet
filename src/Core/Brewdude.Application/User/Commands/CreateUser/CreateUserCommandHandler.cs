using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Security;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using Brewdude.Domain.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Brewdude.Application.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BrewdudeApiResponse<UserViewModel>>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<BrewdudeUser> _userManager;

        public CreateUserCommandHandler(IMapper mapper, ITokenService tokenService, UserManager<BrewdudeUser> userManager)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<BrewdudeApiResponse<UserViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                // Throw if user already exists
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"User with username [{request.Username}] already exists");
            }
            
            // Assign request fields to entity
            var brewdudeUser = new BrewdudeUser
            {
                Email = request.Email,
                UserName = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = false,
            };
            
            var result = await _userManager.CreateAsync(brewdudeUser);
            if (!result.Succeeded)
            {
                // Throw if there was an issue created the user
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"Unexpected error while creating user ${request.Username}");
            }
            
            // Add user password and role
            await _userManager.AddPasswordAsync(brewdudeUser, request.Password);
            await _userManager.AddToRoleAsync(brewdudeUser, request.Role.ToString());

            // Generate a token for immediate use
            var token = _tokenService.CreateToken(brewdudeUser, Role.User);
            if (string.IsNullOrWhiteSpace(token))
            {
                // Throw on token create error
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, "Token generation failed during user creation");
            }
            
            // Map the entity user to view model
            var userViewModel = _mapper.Map<UserViewModel>(brewdudeUser);
            userViewModel.Token = token;
            userViewModel.Role = request.Role.ToString();
            
            return new BrewdudeApiResponse<UserViewModel>((int)HttpStatusCode.OK, BrewdudeResponseMessage.Success.GetDescription(), userViewModel, 1);
        }
    }
}