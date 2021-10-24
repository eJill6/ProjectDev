using System.ServiceModel;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Permission;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;

namespace JxBackendServiceNet45.Interface.Service.Authenticator
{
    [ServiceContract]
    public interface IUserAuthApiService
    {
        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<UserAuthenticatorInfo> GetUserAuthenticatorInfo();

        BaseReturnDataModel<UserAuthenticatorInfo> GetUserAuthenticatorInfo(int userId);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<bool> IsMoneyPasswordExpired();

        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<bool> HasGoogleBound();

        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<UserAuthInfo> GetUserAuthInfo();

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel UserAuthBeforeCheck(int userAuthenticatorSettingType);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel VerifyAuthenticator(string moneyPasswordHash, string clientPin, bool isVerified);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel TransferToChild(BasicTransferToChildParam basicTransferToChildParam);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel UpdateUSDTWallet(BasicBlockChainInfoParam basicBlockChainInfoParam);

        [OperationContract, CommonMOperationBehavior]
        BaseAuthenticatorPermission GetAuthenticatorPermission(int userAuthenticatorSettingType);

        [OperationContract, CommonMOperationBehavior]
        string CreateSecurityToken(TokenType tokenType, int stepId, string data);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel IsSecurityTokenValid(string accessToken, TokenType tokenType, int tokenStepId, int expiredSeconds, string data);

        [OperationContract, CommonMOperationBehavior]
        string GetUserNickname();

        [OperationContract, CommonMOperationBehavior]
        SecurityNextStep SaveUserNickname(string nickname);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel IsUserTwoFactorPinValid(int userAuthenticatorSettingTypeValue, string clientPin);        

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel SaveUserSecurityInfo(SaveSecuritySetting securitySetting);

        [OperationContract, CommonMOperationBehavior]
        bool HasUserActiveBankInfo();

        [OperationContract, CommonMOperationBehavior]
        bool HasUserActiveUsdtAccount();

        [OperationContract, CommonMOperationBehavior]
        bool HasUserInitializationComplete(int userId);

        [OperationContract, CommonMOperationBehavior]
        string GetGoogleBindTutorialUrl();

        [OperationContract, CommonMOperationBehavior]
        bool CheckUserAllowedLogin();

        [OperationContract, CommonMOperationBehavior]
        string GetWebForwardUrl(int clientWebPageValue);
    }
}
