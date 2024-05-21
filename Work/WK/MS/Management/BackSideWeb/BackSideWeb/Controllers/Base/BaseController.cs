using BackSideWeb.Models.Enums;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackSideWeb.Controllers.Base
{
    public class BaseController : Controller
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected JxApplication Application => s_environmentService.Value.Application;

        protected virtual EnvironmentUser EnvLoginUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo()
        };

        public BaseController()
        {
        }

        protected bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected void SetPageTitle(string title)
        {
            ViewBag.Title = title;
        }

        protected void SetPageActType(ActTypes actType)
        {
            ViewBag.ActType = actType;
        }

        protected void SetLayout(LayoutType layoutType)
        {
            ViewBag.DefaultLayout = layoutType.Value;
        }
    }
}