namespace Brewdude.Application.User.Queries.GetUserByUsername
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Extensions;
    using Domain.Api;
    using Domain.Entities;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Security;

    public class GetUserByUsernameCommandHandler : IRequestHandler<GetUserByUsernameCommand, BrewdudeApiResponse<UserViewModel>>
    {
        private readonly UserManager<BrewdudeUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetUserByUsernameCommandHandler(IMapper mapper, ITokenService tokenService, UserManager<BrewdudeUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<UserViewModel>> Handle(GetUserByUsernameCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                // Throw if user does not exist
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.UserNotFound, $"No user with username [{request.Username}] was found");
            }

            // Validate the request user's password against the stored hash and salt
            var isVerifiedPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isVerifiedPassword)
            {
                // Throw on bad password
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"Wrong password for user [{request.Username}]");
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
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, "Token generation failed during user creation");
            }

            // Map the entity user to view model
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Token = token;
            userViewModel.Role = userRole.ToString();

            return new BrewdudeApiResponse<UserViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                userViewModel,
                1);
        }
    }
}