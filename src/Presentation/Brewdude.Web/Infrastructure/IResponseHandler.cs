using System.Threading.Tasks;
using MediatR;

namespace Brewdude.Web.Infrastructure
{
    public interface IResponseHandler<out T>
    {
        T GetResponseEntity();
        Task HandleRequest(IRequest request);
    }
}