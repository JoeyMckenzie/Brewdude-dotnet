namespace Brewdude.Application.User.Commands.DeleteUser
{
    using Domain.Api;
    using MediatR;

    public class DeleteUserCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteUserCommand(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}