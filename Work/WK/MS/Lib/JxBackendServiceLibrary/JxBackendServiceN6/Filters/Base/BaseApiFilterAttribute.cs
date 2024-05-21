using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JxBackendServiceN6.Filters.Base
{
    public abstract class BaseApiFilterAttribute : ActionFilterAttribute
    {
        /// <summary>EnvironmentUser</summary>
        protected abstract EnvironmentUser GetEnvironmentUser(HttpContext httpContext);
    }
}