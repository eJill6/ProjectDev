using SLPolyGame.Web.Model;

namespace ControllerShareLib.Interfaces.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        UserInfo GetUserInfo();

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        UserInfo GetUserInfo(int userID);

        /// <summary>
        /// 系统配置信息获取
        /// </summary>
        SysSettings GetSysSettings();

        UserAuthInformation ValidateLogin(LoginRequestParam param);

        Task<UserInfo> GetUserInfoWithoutAvailable(int userId);
    }
}