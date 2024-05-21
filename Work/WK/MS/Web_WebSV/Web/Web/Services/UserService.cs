using JxBackendService.DependencyInjection;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace Web.Services
{
    public class UserService : IUserService
    {
        private readonly ISLPolyGameWebSVService _slPolyGameWebSVService = null;

        public UserService()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
        }

        public SysSettings GetSysSettings()
        {
            var sysSettings = _slPolyGameWebSVService.GetSysSettings();

            return sysSettings;
        }

        public UserInfo GetUserInfo()
        {
            return _slPolyGameWebSVService.GetUserInfo();
        }

        public UserInfo GetUserInfo(int userID)
        {
            var userInfo = _slPolyGameWebSVService.GetUserInfoByUserID(userID);

            return userInfo;
        }

        public UserAuthInformation ValidateLogin(LoginRequestParam param)
        {
            return _slPolyGameWebSVService.ValidateLogin(param).Data;
        }
    }
}