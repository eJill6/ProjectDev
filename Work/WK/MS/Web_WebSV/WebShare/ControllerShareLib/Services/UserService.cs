using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Services
{
    public class UserService : IUserService
    {
        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        public UserService()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
        }

        public SysSettings GetSysSettings()
        {
            var sysSettings = _slPolyGameWebSVService.Value.GetSysSettings().GetAwaiterAndResult();

            return sysSettings;
        }

        public UserInfo GetUserInfo()
        {
            return _slPolyGameWebSVService.Value.GetUserInfo().GetAwaiterAndResult();
        }

        public UserInfo GetUserInfo(int userID)
        {
            var userInfo = _slPolyGameWebSVService.Value.GetUserInfoByUserID(userID).GetAwaiterAndResult();

            return userInfo;
        }

        public UserAuthInformation ValidateLogin(LoginRequestParam param)
        {
            return _slPolyGameWebSVService.Value.ValidateLogin(param).GetAwaiterAndResult().Data;
        }

        public async Task<UserInfo> GetUserInfoWithoutAvailable(int userId)
        {
            var userInfo = await _slPolyGameWebSVService.Value.GetUserInfoWithoutAvailable(userId);

            return userInfo;
        }
    }
}