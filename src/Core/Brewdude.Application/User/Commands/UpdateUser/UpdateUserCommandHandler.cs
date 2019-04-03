using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BrewdudeApiResponse>
    {
        private readonly UserManager<BrewdudeUser> _userManager;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(UserManager<BrewdudeUser> userManager, ILogger<UpdateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate the user exists
            var existingUser = await _userManager.FindByIdAsync(request.UserId);
            
            if (existingUser == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.UserNotFound, $"User with ID [{request.UserId}] was not found");
            
            // Update the existing user entity
            existingUser.Email = request.UpdatedEmail;
            existingUser.FirstName = request.UpdatedFirstName;
            existingUser.LastName = request.UpdatedLastName;

            await _userManager.UpdateAsync(existingUser);
            _logger.LogInformation($"User [{existingUser.UserName}] updated successfully");
            
            return new BrewdudeApiResponse((int)HttpStatusCode.OK, BrewdudeResponseMessage.Updated.GetDescription());
        }
    }
}