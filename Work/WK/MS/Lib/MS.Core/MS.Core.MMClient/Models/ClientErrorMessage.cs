using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace MS.Core.MMClient.Models
{
    public class ClientErrorMessage
    {
        public string Url { get; set; }
        public string TraceId { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public IList<Parameter> RequestHeaders { get; set; }
    }
}