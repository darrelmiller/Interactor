using System.Net.Http;
using System.Threading.Tasks;

namespace InteractorLib.Interfaces
{
    public interface IHttpResponseHandler
    {
        Task<HttpResponseMessage> HandleAsync(Link link, HttpResponseMessage responseMessage);
    }
}
