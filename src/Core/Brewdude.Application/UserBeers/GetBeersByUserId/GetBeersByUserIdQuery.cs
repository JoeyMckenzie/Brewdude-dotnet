using MediatR;

namespace Brewdude.Application.UserBeers.GetBeersByUserId
{
    public class GetBeersByUserIdQuery : IRequest<UserBeersViewModel>
    {
        public GetBeersByUserIdQuery(int userId)
        {
            UserId = userId;
        }
        
        public int UserId { get; }
    }
}