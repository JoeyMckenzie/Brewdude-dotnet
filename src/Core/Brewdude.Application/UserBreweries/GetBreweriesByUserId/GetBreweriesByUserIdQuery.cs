using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.UserBreweries.GetBreweriesByUserId
{
    public class GetBreweriesByUserIdQuery : IRequest<UserBreweriesViewModel>
    {
        public GetBreweriesByUserIdQuery(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; }
    }
}