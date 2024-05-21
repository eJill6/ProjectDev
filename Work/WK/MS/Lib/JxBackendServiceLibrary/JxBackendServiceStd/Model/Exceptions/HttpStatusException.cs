using JxBackendService.Model.Attributes;
using System;
using System.Net;

namespace JxBackendService.Model.Exceptions
{
    public class HttpStatusException : FlowControlException
    {
        public HttpStatusCode HttpStatusCode { get; private set; }

        public HttpStatusException(HttpStatusCode httpStatusCode) : this(httpStatusCode, message: httpStatusCode.ToString())
        {
        }

        public HttpStatusException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}