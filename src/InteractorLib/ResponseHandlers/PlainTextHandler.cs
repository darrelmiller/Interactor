using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InteractorLib.Interfaces;

namespace InteractorLib.ResponseHandlers
{
    public class PlainTextHandler : IHttpResponseHandler
    {
        private readonly IShell _shell;

        public PlainTextHandler(IShell shell)
        {
            _shell = shell;
        }

        public async Task<HttpResponseMessage> HandleAsync(Link link, HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.OK &&
                responseMessage.Content.Headers.ContentType.MediaType == "text/plain")
            {
                var message = await responseMessage.Content.ReadAsStringAsync();
                _shell.Show(message);
                return null;
            }
            return responseMessage;
        }

    
    }
}
