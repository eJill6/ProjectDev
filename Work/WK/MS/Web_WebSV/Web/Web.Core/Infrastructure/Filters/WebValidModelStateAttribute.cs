using JxBackendService.Attributes;
using JxBackendService.Common.Util;
using JxBackendService.Model.MiseLive.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web.Infrastructure.Filters
{
    public class WebValidModelStateAttribute : BaseValidModelStateAttribute
    {
        private readonly HttpStatusCode _httpStatusCode = HttpStatusCode.BadRequest;

        public WebValidModelStateAttribute()
        {
        }

        public WebValidModelStateAttribute(HttpStatusCode httpStatusCode) : base(httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        protected override IActionResult GetInvalidActionResult(string errorMessage)
        {
            var miseLiveResponse = new BaseMiseLiveResponse()
            {
                Success = false,
                Error = errorMessage
            };

            var contentResult = new ContentResult
            {
                ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8",
                Content = miseLiveResponse.ToJsonString(isCamelCaseNaming: true),
                StatusCode = (int)_httpStatusCode,
            };

            return contentResult;
        }
    }
}