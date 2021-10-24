using JxBackendService.Model.Entity.User.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Permission;
using JxBackendServiceNet45.Model.Enums;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;
using System;

namespace JxBackendServiceNet45.Interface.Service.Authenticator
{
    public interface IUserAuthenticatorValidReadService
    {
        UserAuthenticator GetUserAuthenticator(int userId);

        UserAuthenticator GetUserAuthenticator(string userName);

        BaseReturnDataModel<UserAuthenticatorInfo> GetUserAuthenticatorInfo(int userId);

        AuthenticatorPermission GetAuthenticatorPermission(int userId, UserAuthenticatorSettingTypes userAuthenticatorSettingType);

        string GetAuthenticatorExpiredWarningMsg(int userId);
        
        BaseReturnModel IsUserTwoFactorPinValid(int userId, UserAuthenticatorSettingTypes userAuthenticatorSettingType, string clientPin);
        
        BaseReturnModel IsUserTwoFactorPinValid(int userId, string clientPin);
        
        bool IsTwoFactorPinValid(AuthenticatorType authenticatorType, string accountSecretKey, string twoFactorCodeFromClient);
    }

    public interface IUserAuthenticatorValidService
    {
        QrCodeViewModel GetQrCode(CreateQrCodeViewModelParam createQrCodeViewModelParam);
        
        BaseReturnModel VerifyAuthenticator(VerifiedAuthenticatorParam param);

        BaseReturnModel ForceUnverifiedAuthenticator(string userName);        
    }
}
