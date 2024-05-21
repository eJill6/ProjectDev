using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Model.Param.Cache;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Cache;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace JxBackendService.Attributes
{
    /// <summary>
    /// 檢查用戶是否需要被登出
    /// </summary>
    public abstract class BaseFrontSideUserAllowedLoginAttribute : ActionFilterAttribute
    {
        private readonly int _checkRulesSeconds;

        protected abstract string GetUserKey();

        protected abstract EnvironmentUser EnvLoginUser { get; }

        protected abstract Func<bool> CheckUserAllowedLogin { get; }

        protected abstract Action<ActionExecutingContext> DoLogout { get; }

        public BaseFrontSideUserAllowedLoginAttribute(int checkRulesSeconds = 5 * 60)
        {
            _checkRulesSeconds = checkRulesSeconds;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (GetUserKey().IsNullOrEmpty() || EnvLoginUser.LoginUser == null)
            {
                return;
            }

            var intervalJobParam = new IntervalJobParam()
            {
                CacheKey = CacheKey.CheckUserAllowedLogin(EnvLoginUser.LoginUser.UserKey),
                CacheSeconds = _checkRulesSeconds,
                MaxNormalTryCount = 1,
                MaxExceptionTryCount = 5,
                EnvironmentUser = EnvLoginUser
            };

            Action action = () =>
            {
                bool isAllowedLogin = CheckUserAllowedLogin();

                if (!isAllowedLogin)
                {
                    DoLogout.Invoke(filterContext);
                }
            };

            IntervalJobUtil.DoIntervalWork(intervalJobParam, action);
        }
    }
}