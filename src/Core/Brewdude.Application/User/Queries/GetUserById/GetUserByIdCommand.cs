namespace Brewdude.Application.User.Queries.GetUserById
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetUserByIdCommand : IRequest<BrewdudeApiResponse<UserViewModel>>
    {
        public GetUserByIdCommand(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}