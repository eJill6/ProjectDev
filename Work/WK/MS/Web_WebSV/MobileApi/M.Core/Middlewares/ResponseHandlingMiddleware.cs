using JxBackendService.Middlewares;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using System.Net;

namespace M.Core.Middlewares
{
    public class ResponseHandlingMiddleware : BaseResponseHandlingMiddleware<AppResponseModel>
    {
        public ResponseHandlingMiddleware(RequestDelegate next) : base(next)
        {
        }

        protected override AppResponseModel ConvertToResponseModel(string errorMsg) => new AppResponseModel(errorMsg);

        protected override HttpStatusCode ConvertHttpStatusCode(HttpStatusCode httpStatusCode) => HttpStatusCode.OK;

        protected override Endpoint GetEndpoint(HttpContext httpContext) => httpContext.GetEndpoint();
    }
}