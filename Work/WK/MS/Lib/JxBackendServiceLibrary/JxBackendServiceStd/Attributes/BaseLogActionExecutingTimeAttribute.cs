using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Attribute;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JxBackendService.Attributes
{
    public abstract class BaseLogActionExecutingTimeAttribute : ActionFilterAttribute
    {
        private ILogActionExecutingTimeService _logActionExecutingTimeService;

        private readonly double? _warningMilliseconds;

        protected abstract EnvironmentUser EnvLoginUser { get; }

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
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            //不可放建構子,網站啟動初始化會拿不到UrlHelper造成從Route取得前台User資料時會發生錯誤
            _logActionExecutingTimeService = DependencyUtil.ResolveJxBackendService<ILogActionExecutingTimeService>(EnvLoginUser, DbConnectionTypes.Slave).Value;
            _logActionExecutingTimeService.Init(_warningMilliseconds);
            _logActionExecutingTimeService.ActionExecuting(actionExecutingContext);
        }

        /// <summary>
        /// 回傳Result後
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            _logActionExecutingTimeService.ActionExecuting(actionExecutedContext);
        }
    }
}