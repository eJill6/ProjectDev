using JxBackendService.Attributes;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using JxBackendServiceNF.Attributes;
using System;
using System.Net;
using System.Web.Mvc;
using Web.Models.Results;

namespace Web.Infrastructure.Filters
{
    public class WebValidModelStateAttribute : BaseValidModelStateNFAttribute
    {
        public WebValidModelStateAttribute(HttpStatusCode httpStatusCode) : base(httpStatusCode)
        {
        }

        protected override ActionResult GetInvalidActionResult(string errorMessage)
        {
            var data = new MiseLiveResponse<object>()
            {
                Success = false,
                Error = errorMessage
            };

            return new JsonCamelCaseResult(data);
        }
    }
}