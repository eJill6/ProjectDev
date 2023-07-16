using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System.Web.Mvc;

namespace JxBackendService.Attributes
{
    public abstract class BaseLogActionExecutingTimeAttribute : ActionFilterAttribute
    {
        private ILogExecutingTimeService _logExecutingTimeService;

        private readonly double? _warningMilliseconds;

        protected abstract EnvironmentUser EnvLoginUser { get; }

        public ILogExecutingTimeService LogExecutingTimeService => _logExecutingTimeService;

        public BaseLogActionExecutingTimeAttribute()
        {
        }

        public BaseLogActionExecutingTimeAttribute(double? warningMilliseconds) : base()
        {
            _warningMilliseconds = warningMilliseconds;
        }

        /// <summary>
        /// 進去Action前
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logExecutingTimeService = DependencyUtil.ResolveJxBackendService<ILogExecutingTimeService>(EnvLoginUser, DbConnectionTypes.Slave);
            _logExecutingTimeService.Start();
        }

        /// <summary>
        /// 回傳Result後
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            _logExecutingTimeService.Stop(
                (elapsedTotalMilliseconds) => $"URL:{actionExecutedContext.HttpContext.Request.Url} 花費:{elapsedTotalMilliseconds} 毫秒"
                , _warningMilliseconds);
        }
    }
}