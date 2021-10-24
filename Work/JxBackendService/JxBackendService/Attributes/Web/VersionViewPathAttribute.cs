using JxBackendService.Common.Exceptions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using System.Linq;
using System.Web.Mvc;

namespace JxBackendService.Attributes.Web
{
    public class VersionViewPathAttribute : ActionFilterAttribute, IResultFilter
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is ViewResult viewResult)
            {
                RazorViewEngine razorEngine = viewResult.ViewEngineCollection.OfType<RazorViewEngine>().Single();
                string viewName = viewResult.ViewName.IsNullOrEmpty()
                    ? RouteUtil.GetActionName(filterContext.RouteData)
                    : RouteUtil.ReplaceMerchantFormatPath(viewResult.ViewName);

                RazorView razorView = razorEngine.FindView(filterContext.Controller.ControllerContext, viewName, viewResult.MasterName, false).View as RazorView;
                if (razorView == null)
                {
                    throw new ViewNotFoundException(viewName);
                }

                viewResult.ViewName = razorView.ViewPath;
            }

            base.OnResultExecuting(filterContext);
        }
    }
}
