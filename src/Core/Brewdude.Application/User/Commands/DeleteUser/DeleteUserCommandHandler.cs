namespace Brewdude.Application.User.Commands.DeleteUser
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Domain.Api;
    using Domain.Entities;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, BrewdudeApiResponse>
    {
        private readonly UserManager<BrewdudeUser> _userManager;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, UserManager<BrewdudeUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByIdAsync(request.UserId);

            if (existingUser == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.UserNotFound, $"Brewdude user with ID [{request.UserId}] does not exist");
            }

            await _userManager.DeleteAsync(existingUser);
            _logger.LogInformation($"User [{request.UserId}] successfully deleted");

            return new BrewdudeApiResponse((int)HttpStatusCode.OK, BrewdudeResponseMessage.Deleted.GetDescription());
        }
    }
}