using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.UserBreweries.GetBreweriesByUserId
{
    public class GetBreweriesByUserIdQuery : IRequest<UserBreweriesViewModel>
    {
        public GetBreweriesByUserIdQuery(int userId)
        {
            UserId = userId;
        }
        
        public int UserId { get; }
    }
}