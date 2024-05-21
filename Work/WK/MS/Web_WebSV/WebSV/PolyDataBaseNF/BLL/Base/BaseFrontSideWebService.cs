using System;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using PolyDataBase.Helpers;

namespace SLPolyGame.Web.BLL
{
    public class BaseFrontSideWebService : BaseApplicationService
    {
        private static readonly JxApplication _currentApplication = JxApplication.FrontSideWeb;

        private static Lazy<EnvironmentCode> _environmentCode = new Lazy<EnvironmentCode>(
            () =>
            {
                return SharedAppSettings.GetEnvironmentCode(JxApplication.FrontSideWeb);
            });

        public static EnvironmentCode GetEnvironmentCode() => _environmentCode.Value;

        private EnvironmentUser _environmentUser;

        public override EnvironmentUser EnvLoginUser
        {
            get
            {
                _environmentUser = AssignValueOnceUtil.GetAssignValueOnce(
                    _environmentUser,
                    () =>
                    {
                        var environmentUser = new EnvironmentUser()
                        {
                            Application = _currentApplication,
                            LoginUser = new BasicUserInfo()
                        };

                        Model.UserInfoToken userInfoToken = MessageContextHelper.GetUserInfoToken();

                        if (userInfoToken != null)
                        {
                            environmentUser.LoginUser.UserId = userInfoToken.UserId;
                            environmentUser.LoginUser.UserKey = userInfoToken.Key;
                        }

                        return environmentUser;
                    });

                return _environmentUser;
            }
        }
    }
}