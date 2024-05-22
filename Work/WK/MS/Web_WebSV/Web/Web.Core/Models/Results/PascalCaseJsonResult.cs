namespace Web.Core.Models.Results
{
    using JxBackendService.Common.Util;
    using Microsoft.AspNetCore.Mvc;

    public class PascalCaseJsonResult : JsonResult
    {
        public PascalCaseJsonResult(object? value) : base(value)
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
            string json = Value.ToJsonString();
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.WriteAsync(json);
        }
    }
}