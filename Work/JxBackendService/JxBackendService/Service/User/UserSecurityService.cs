using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.User
{
    public class UserSecurityService : IUserSecurityService
    {
        private readonly JxApplication _jxApplication;

        public UserSecurityService(JxApplication jxApplication)
        {
            _jxApplication = jxApplication;
        }

        public string CreateSecurityToken(int userId, TokenType tokenType, int stepId, string data)
        {
            IAppSettingService appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(_jxApplication, SharedAppSettings.PlatformMerchant);

            var securityToken = new SecurityToken()
            {
                TokenTypeId = (int)tokenType,
                UserId = userId,
                StepId = stepId,
                Data = data,
                HandleDate = DateTime.Now,
            };

            return securityToken.ToJsonString().ToEncryptedData(appSettingService.CommonDataHash);
        }

        public BaseReturnModel IsSecurityTokenValid(int userId, string accessToken, TokenType tokenType, int tokenStepId, int expiredSeconds, string data)
        {
            IAppSettingService appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(_jxApplication, SharedAppSettings.PlatformMerchant);
            string jsonString = null;

            try
            {
                if (accessToken.IsNullOrEmpty())
                {
                    return new BaseReturnModel(ReturnCode.EntranceIsTimeOut);
                }

                jsonString = accessToken.ToDescryptedData(appSettingService.CommonDataHash);
                SecurityToken securityToken = jsonString.Deserialize<SecurityToken>();

                if (securityToken.UserId != userId ||
                    securityToken.TokenTypeId != (int)tokenType ||
                    securityToken.StepId != tokenStepId ||
                    (!securityToken.Data.IsNullOrEmpty() && securityToken.Data != data) ||
                    securityToken.HandleDate.AddSeconds(expiredSeconds) < DateTime.Now)
                {
                    return new BaseReturnModel(ReturnCode.EntranceIsTimeOut);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new BaseReturnModel(MessageElement.OperationFail);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }
    }
}
