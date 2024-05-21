using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Account;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Service.Controller
{
    public class AccountControllerService : IAccountControllerService
    {
        private readonly Lazy<IUserService> _userService;

        public AccountControllerService()
        {
            _userService = DependencyUtil.ResolveService<IUserService>();
        }

        public LogonResult LogOn(ValidateLogonParam logonParam)
        {
            UserAuthInformation userAuthInformation = _userService.Value.ValidateLogin(new LoginRequestParam
            {
                UserId = logonParam.UserID.Value,
                UserName = logonParam.UserName,
                RoomNo = logonParam.RoomNo,
                GameID = logonParam.GameID,
                DepositUrl = logonParam.DepositUrl,
                LogonMode = logonParam.LogonMode,
                UserKeyExpiredMinutes = logonParam.UserKeyExpiredMinutes,
                IsSlidingExpiration = logonParam.IsSlidingExpiration,
            });

            var basicUserInfo = new BasicUserInfo()
            {
                UserId = userAuthInformation.UserId,
                UserKey = userAuthInformation.Key,
            };

            if (!logonParam.IsSlidingExpiration)
            {
                basicUserInfo.Ts = DateTime.UtcNow.ToUnixOfTime();
            }

            string token = AuthenticationUtil.CreateMiseWebToken(basicUserInfo);

            var logonResult = new LogonResult
            {
                Token = token,
                ExpiredTimestamp = userAuthInformation.ExpiredTimestamp
            };

            return logonResult;
        }
    }
}