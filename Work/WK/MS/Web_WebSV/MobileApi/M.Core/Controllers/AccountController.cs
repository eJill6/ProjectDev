using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Account;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ReturnModel;
using M.Core.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers
{
    public class AccountController : BaseAuthApiController
    {
        private static readonly string s_appTeamDepositUrl = "/Recharge";

        private readonly Lazy<IAccountControllerService> _accountControllerService;

        public AccountController()
        {
            _accountControllerService = ResolveService<IAccountControllerService>();
        }

        /// <summary> 登入 </summary>
        [HttpPost]
        [AllowAnonymous]
        public AppResponseModel<LogonResult> LogOn(MobileApiLogonParam logonParam)
        {
            var validateLogonParam = logonParam.CastByJson<ValidateLogonParam>();
            validateLogonParam.UserKeyExpiredMinutes = EnvLoginUser.Application.UserKeyExpiredMinutes;
            validateLogonParam.IsSlidingExpiration = EnvLoginUser.Application.IsSlidingUserKeyCache;
            validateLogonParam.DepositUrl = s_appTeamDepositUrl;

            LogonResult logonResult = _accountControllerService.Value.LogOn(validateLogonParam);

            return new AppResponseModel<LogonResult>
            {
                Success = true,
                Data = logonResult
            };
        }
    }
}