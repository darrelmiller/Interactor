using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XBrowser.zFakeStuff
{
    public class FakeServer : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            
            switch (request.RequestUri.OriginalString)
            {
                case "http://myapi.com/plaintext":
                    return CreatePlainTextResponse();
                case "http://myapi.com/html":
                    return CreateHtmlResponse();
                case "http://myapi.com/hal":
                    return CreateHalResponse();
                case "http://myapi.com/hal_stylesheet":
                    return CreateHalStylesheetResponse();

            }

            return httpResponseMessage;
        }

        private HttpResponseMessage CreateHalStylesheetResponse()
        {
            var content = new EmbeddedContent(typeof(FakeServer).GetTypeInfo().Assembly, "Interactor.zFakeStuff.Stylesheet1.xaml");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/xaml+xml");
            return new HttpResponseMessage()
            {
                Content = content
            };
            
        }

        private HttpResponseMessage CreateHalResponse()
        {
            var content = new EmbeddedContent(typeof(FakeServer).GetTypeInfo().Assembly, "Interactor.zFakeStuff.HalDocument.xml");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/hal+xml");
            
            return new HttpResponseMessage()
            {
                Content = content
            };
        }

        private HttpResponseMessage CreatePlainTextResponse()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("Hello World")
            };
        }

        private HttpResponseMessage CreateHtmlResponse()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("<html><h1>This is a html page</h1></html>",Encoding.UTF8,"text/html")
            };
        }
    }


    public class EmbeddedContent : HttpContent
    {

        private readonly Stream _Stream;

        public bool IsEmpty { get; set; }

        public EmbeddedContent(Assembly assembly, string filename)
        {
            _Stream = assembly.GetManifestResourceStream(filename);
            IsEmpty = _Stream == null;
        }


        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {

            _Stream.CopyTo(stream);
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _Stream.Dispose();
            base.Dispose(disposing);
        }
    }
}
