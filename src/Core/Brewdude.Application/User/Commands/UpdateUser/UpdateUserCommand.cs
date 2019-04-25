namespace Brewdude.Application.User.Commands.UpdateUser
{
    using Domain.Api;
    using MediatR;

    public class UpdateUserCommand : IRequest<BrewdudeApiResponse>
    {
        public UpdateUserCommand(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set;  }

        public string UpdatedEmail { get; set; }

        public string UpdatedFirstName { get; set; }

        public string UpdatedLastName { get; set; }
    }
}