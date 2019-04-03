using System;
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
using Microsoft.EntityFrameworkCore.Internal;

namespace Brewdude.Application.User.Queries.GetUserById
{
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, BrewdudeApiResponse<UserViewModel>>
    {
        private readonly UserManager<BrewdudeUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetUserByIdCommandHandler(ITokenService tokenService, IMapper mapper, UserManager<BrewdudeUser> userManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<UserViewModel>> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.UserNotFound, $"User [{request.UserId}] does not exist");
            
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
            
            // Generate a token for immediate use
            var token = _tokenService.CreateToken(user, userRole);
            if (string.IsNullOrWhiteSpace(token))
            {
                // Throw on token create error
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, "Token generation failed during user login");
            }
            
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Token = token;

            return new BrewdudeApiResponse<UserViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                userViewModel,
                1);
        }
    }
}