using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InteractorLib.Interfaces;
using InteractorLib.ViewModels;

namespace InteractorLib.ResponseHandlers
{
    public class HalHandler : IHttpResponseHandler
    {
        private readonly IShell _shell;
        private readonly HttpClient _httpClient;

        public HalHandler(IShell shell, HttpClient httpClient)
        {
            _shell = shell;
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> HandleAsync(Link link, HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.OK &&
                 responseMessage.Content.Headers.ContentType.MediaType == "application/hal+xml")
            {
                var model = new HalViewModel(_httpClient);
                await model.Load(await responseMessage.Content.ReadAsStreamAsync());

                _shell.ShowHal(model);
                return null;
            }
            return responseMessage;
        }
    }
}
