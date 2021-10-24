using System;
using System.ServiceModel;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Merchant;
using JxBackendService.Model.ViewModel.Permission;
using JxBackendServiceNet45.Model.Param;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;

namespace JxBackendServiceNet45.Interface.Service.Public
{
    [ServiceContract]
    public interface IPublicApiService
    {
        [OperationContract]
        bool BlockWebActionClient(string ipAddress, int webActionTypeValue);

        [OperationContract]
        BaseReturnModel UpdatePasswordHashByAuthenticator(SaveFindPasswordHashParam param);

        [OperationContract]
        BaseReturnDataModel<string> GetValidateCodeImage(int actionTypeValue, string userIdentityKey, bool isForceRefresh = false);

        [OperationContract]
        void LogOff();

        [OperationContract]
        MerchantSettingModel GetMerchantSetting();

        #region 相容舊版Remote CacheBase 而開的Service
        [OperationContract]
        string GetRemoteCacheForOldService(string key);

        [OperationContract]
        void RemoveRemoteCacheForOldService(string key);

        [OperationContract]
        void SetRemoteCacheForOldService(string key, int cacheSeconds, bool isSlidingExpiration, string value);
        #endregion

        [OperationContract]
        BaseReturnDataModel<BaseUserInfoToken> GetUserByToken(string encryptedToken);
    }
}
