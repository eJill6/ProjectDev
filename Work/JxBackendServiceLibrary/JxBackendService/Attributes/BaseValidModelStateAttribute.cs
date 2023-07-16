using System.Linq;
using System.Web.Mvc;
using System.Net;

namespace JxBackendService.Attributes
{
    public abstract class BaseValidModelStateAttribute : ActionFilterAttribute
    {
        private readonly int? _httpStatusCode;

        public BaseValidModelStateAttribute()
        {
        }

        public BaseValidModelStateAttribute(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = (int)httpStatusCode;
        }

        public int? StatusCode => _httpStatusCode;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                string errorMessage = filterContext.Controller.ViewData.ModelState.Values
                    .Where(w => w.Errors.Count > 0)
                    .Select(s => s.Errors[0].ErrorMessage).First();

                if (_httpStatusCode.HasValue)
                {
                    filterContext.HttpContext.Response.StatusCode = _httpStatusCode.Value;
                }

                filterContext.Result = GetInvalidActionResult(errorMessage);
            }
        }

        protected abstract ActionResult GetInvalidActionResult(string errorMessage);
    }
}