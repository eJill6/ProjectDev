using System;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.Base
{
    public abstract class BaseApplicationService
    {
        private static readonly PlatformMerchant _platformMerchant = SharedAppSettings.PlatformMerchant;

        public abstract EnvironmentUser EnvLoginUser { get; }

        protected T ResolveJxBackendService<T>(DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(EnvLoginUser, dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(Type type, DbConnectionTypes dbConnectionType)
        {
            return (T)DependencyUtil.ResolveJxBackendService(type, EnvLoginUser, dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(PlatformMerchant merchant, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(merchant, EnvLoginUser, dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(PlatformProduct product, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(product, SharedAppSettings.PlatformMerchant, EnvLoginUser, dbConnectionType);
        }

        //protected T ResolveJxBackendService<T>(PlatformProduct product, bool isMockService, DbConnectionTypes dbConnectionType)
        //{
        //    return DependencyUtil.ResolveJxBackendService<T>(product, isMockService, EnvLoginUser, dbConnectionType);
        //}

        protected T ResolveJxBackendService<T>(CommissionGroupType keyModel, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(VIPBonusType keyModel, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(VIPEventType keyModel, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, dbConnectionType);
        }

        protected static T ResolveKeyed<T>(JxApplication keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, _platformMerchant);
        }

        protected static T ResolveKeyed<T>(JxApplication keyModel, PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, platformMerchant);
        }

        protected static T ResolveKeyedForModel<T>(JxApplication keyCtorModel)
        {
            return DependencyUtil.ResolveKeyedForModel<T>(keyCtorModel, _platformMerchant);
        }

        protected static T ResolveServiceForModel<T>(JxApplication ctorParam)
        {
            return DependencyUtil.ResolveServiceForModel<T>(ctorParam);
        }

        public virtual T DoWorkWithErrorHandle<T>(Func<T> serviceWork, T exceptionDefaultResult = default(T))
        {
            return ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, serviceWork, false, exceptionDefaultResult);
        }

        public virtual void DoWorkWithErrorHandle(Action serviceWork)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, serviceWork);
        }

        private EnvironmentCode environmentCode = null;
        /// <summary>
        /// 環境變數,為了讓命名分開,故使用簡寫
        /// </summary>
        protected EnvironmentCode EnvCode
        {
            get
            {
                if (environmentCode == null)
                {
                    environmentCode = SharedAppSettings.GetEnvironmentCode(EnvLoginUser.Application);
                }

                return environmentCode;
            }
        }

        protected virtual string GetLocalDomain()
        {
            return "/";
        }

        /// <summary>
        /// 取得靜態資源域名
        /// </summary>
        /// <returns></returns>
        public string GetCommonStaticFileDomain()
        {
            string cdnSetting = SharedAppSettings.CommonStaticFileCDNString;

            if (!string.IsNullOrWhiteSpace(cdnSetting))
            {
                //使用CDN時，Online庫上對運維只有單一Git (JxCommonStaticFile) ，在更版時為了不影響其他專案的運作，將資料夾以 Application 拆開
                //未使用CDN時不必拆開，直接拿取Local端的相對路徑資源
                return string.Format("{0}{1}/", cdnSetting, EnvLoginUser.Application.Value.ToLower());
            }
            return GetLocalDomain();
        }

        public static BaseReturnDataModel<BaseUserInfoToken> GetUserByUserKey(JxApplication jxApplication, string userKey, Func<string> getLogInfo)
        {
            var jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(jxApplication);
            CacheKey cacheKey = CacheKey.GetFrontSideUserInfoKey(userKey);

            string json = jxCacheService.GetCache<string>(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = jxApplication.UserKeyExpiredMinutes * 60,
                IsForceRefresh = false,
                IsCloneInstance = false,
                IsSlidingExpiration = true
            }, null);


            if (string.IsNullOrWhiteSpace(json))
            {
                string msg = $"回傳hash串不正確，用戶被強制下線";

                if (getLogInfo != null)
                {
                    msg += getLogInfo.Invoke();
                }

                LogUtil.Error(msg);
                return new BaseReturnDataModel<BaseUserInfoToken>(msg, null);
            }

            BaseUserInfoToken userInfoToken = null;
            try
            {
                userInfoToken = json.Deserialize<BaseUserInfoToken>();
            }
            catch (Exception ex)
            {
                jxCacheService.RemoveCache(cacheKey);
                string msg = $"反序列化BasicUserInfo異常，cacheJson={json}";

                if (getLogInfo != null)
                {
                    msg += getLogInfo.Invoke();
                }

                msg += $"，详细信息：{ex.Message},堆栈：{ex.StackTrace}";
                LogUtil.Error(msg);
                return new BaseReturnDataModel<BaseUserInfoToken>(msg, null);
            }

            if (userInfoToken == null)
            {
                string msg = "userinfo is null";
                LogUtil.Error(msg);
                return new BaseReturnDataModel<BaseUserInfoToken>(msg, null);
            }

            return new BaseReturnDataModel<BaseUserInfoToken>(ReturnCode.Success, userInfoToken);
        }
    }

    public abstract class BaseSingleConnApplicationService : BaseApplicationService
    {
        protected T ResolveJxBackendService<T>()
        {
            return ResolveJxBackendService<T>(DbConnectionTypes.Master);
        }

        protected T ResolveJxBackendService<T>(PlatformMerchant merchant)
        {
            return ResolveJxBackendService<T>(merchant, DbConnectionTypes.Master);
        }
    }
}
