using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InteractorLib.Interfaces;

namespace InteractorLib.ResponseHandlers
{
    public class HtmlHandler : IHttpResponseHandler
    {
        private readonly IShell _shell;

        public HtmlHandler(IShell shell)
        {
            _shell = shell;
        }

        public async Task<HttpResponseMessage> HandleAsync(Link link, HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.OK &&
                responseMessage.Content.Headers.ContentType.MediaType == "text/html")
            {
                var page = await responseMessage.Content.ReadAsStringAsync();
                _shell.ShowHtml(page);
                return null;
            }
            return responseMessage;
        }

    
    }
}
