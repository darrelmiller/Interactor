using System;
using System.Net.Http;

namespace InteractorLib.Interfaces
{
    public class Link
    {
        public string Relation { get; set; }
        public Uri Target { get; set; }
        public string Title { get; set; }

        public HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage()
            {
                RequestUri = Target
            };
        }
    }
}