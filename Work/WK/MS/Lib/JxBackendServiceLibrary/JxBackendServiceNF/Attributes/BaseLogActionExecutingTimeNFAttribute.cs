using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Attribute;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System.Web.Mvc;

namespace JxBackendService.Attributes
{
    public abstract class BaseLogActionExecutingTimeNFAttribute : ActionFilterAttribute
    {
        private ILogActionExecutingTimeService _logActionExecutingTimeService;

        private readonly double? _warningMilliseconds;

        protected abstract EnvironmentUser EnvLoginUser { get; }

        public BaseLogActionExecutingTimeNFAttribute()
        {
        }

        public BaseLogActionExecutingTimeNFAttribute(double? warningMilliseconds) : base()
        {
            _warningMilliseconds = warningMilliseconds;
        }

        /// <summary>
        /// 進去Action前
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            //必須在runtime時取得envLoginUser
            _logActionExecutingTimeService = DependencyUtil.ResolveJxBackendService<ILogActionExecutingTimeService>(EnvLoginUser, DbConnectionTypes.Slave);
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