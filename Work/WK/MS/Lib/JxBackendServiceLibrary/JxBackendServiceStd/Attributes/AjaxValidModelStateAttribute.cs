using JxBackendService.Common.Util;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JxBackendService.Attributes
{
    public class AjaxValidModelStateAttribute : BaseValidModelStateAttribute
    {
        private readonly HttpStatusCode _httpStatusCode = HttpStatusCode.BadRequest;

        public AjaxValidModelStateAttribute()
        {
        }

        public AjaxValidModelStateAttribute(HttpStatusCode httpStatusCode) : base(httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        protected override IActionResult GetInvalidActionResult(string errorMessage)
        {
            var contentResult = new ContentResult
            {
                ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8",
                Content = new BaseReturnModel(errorMessage).ToJsonString(isCamelCaseNaming: true),
                StatusCode = (int)_httpStatusCode
            };

            return contentResult;
        }
    }
}