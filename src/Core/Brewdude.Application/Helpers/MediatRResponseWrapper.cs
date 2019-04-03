using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.Helpers
{
    public class MediatRResponseWrapper : IRequest<BrewdudeApiResponse>
    {
        
    }

    public class MediatRResponseWrapper<T> : IRequest<BrewdudeApiResponse<T>>
        where T : BaseViewModel
    {
        
    }
}