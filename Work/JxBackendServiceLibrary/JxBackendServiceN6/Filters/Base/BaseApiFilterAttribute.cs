using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceN6.Filters.Base
{
    public abstract class BaseApiFilterAttribute : ActionFilterAttribute
    {
        /// <summary>EnvironmentUser</summary>
        protected abstract EnvironmentUser GetEnvironmentUser(HttpContext httpContext);
    }
}