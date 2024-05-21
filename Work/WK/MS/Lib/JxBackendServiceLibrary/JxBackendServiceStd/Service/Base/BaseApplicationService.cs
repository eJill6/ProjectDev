using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using System;

namespace JxBackendService.Service.Base
{
    public abstract class BaseApplicationService
    {
        protected static PlatformMerchant Merchant => SharedAppSettings.PlatformMerchant;

        public abstract EnvironmentUser EnvLoginUser { get; }

        protected Lazy<T> ResolveJxBackendService<T>(DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(EnvLoginUser, dbConnectionType);
        }

        protected Lazy<object> ResolveJxBackendService(Type type, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService(type, EnvLoginUser, dbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformMerchant merchant, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(merchant, EnvLoginUser, dbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformProduct product, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(product, SharedAppSettings.PlatformMerchant, EnvLoginUser, dbConnectionType);
        }

        //protected T ResolveJxBackendService<T>(PlatformProduct product, bool isMockService, DbConnectionTypes dbConnectionType)
        //{
        //    return DependencyUtil.ResolveJxBackendService<T>(product, isMockService, EnvLoginUser, dbConnectionType);
        //}

        protected Lazy<T> ResolveJxBackendService<T>(JxApplication keyModel, DbConnectionTypes dbConnectionType)
            => DependencyUtil.ResolveJxBackendService<T>(keyModel, SharedAppSettings.PlatformMerchant, EnvLoginUser, dbConnectionType);

        protected static Lazy<T> ResolveKeyed<T>(JxApplication keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, Merchant);
        }

        protected static Lazy<T> ResolveKeyed<T>(JxApplication keyModel, PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, platformMerchant);
        }

        //protected static Lazy<T> ResolveKeyedForModel<T>(JxApplication keyCtorModel)
        //{
        //    return DependencyUtil.ResolveKeyedForModel<T>(keyCtorModel, Merchant);
        //}

        //protected static Lazy<T> ResolveServiceForModel<T>(JxApplication ctorParam)
        //{
        //    return DependencyUtil.ResolveServiceForModel<T>(ctorParam);
        //}

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
                    environmentCode = SharedAppSettings.GetEnvironmentCode();
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
        protected string GetCommonStaticFileDomain()
        {
            string cdnSetting = SharedAppSettings.CommonStaticFileCDNString;

            if (!string.IsNullOrWhiteSpace(cdnSetting))
            {
                //使用CDN時，Online庫上對運維只有單一Git (JxCommonStaticFile) ，在更版時為了不影響其他專案的運作，將資料夾以 Application 拆開
                //未使用CDN時不必拆開，直接拿取Local端的相對路徑資源
                return string.Format("{0}/{1}/", cdnSetting.TrimEnd('/'), EnvLoginUser.Application.Value.ToLower());
            }
            return GetLocalDomain();
        }

        public static BaseReturnDataModel<BaseUserInfoToken> GetUserByUserKey(JxApplication jxApplication, string userKey, Func<string> getLogInfo)
        {
            string json = GetUserJsonByUserKey(jxApplication, userKey, isSlidingExpiration: true);
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;

            if (string.IsNullOrWhiteSpace(json))
            {
                string msg = $"回傳hash串不正確，用戶被強制下線";

                if (getLogInfo != null)
                {
                    msg += getLogInfo.Invoke();
                }

                logUtilService.Error(msg);

                return new BaseReturnDataModel<BaseUserInfoToken>(msg, null);
            }

            BaseUserInfoToken userInfoToken = null;

            try
            {
                userInfoToken = json.Deserialize<BaseUserInfoToken>();
            }
            catch (Exception ex)
            {
                var jxCacheService = DependencyUtil.ResolveService<IJxCacheService>().Value;

                CacheKey cacheKey = CacheKey.GetFrontSideUserInfoKey(userKey);
                jxCacheService.RemoveCache(cacheKey);
                string msg = $"反序列化BasicUserInfo異常，cacheJson={json}";

                if (getLogInfo != null)
                {
                    msg += getLogInfo.Invoke();
                }

                msg += $"，详细信息：{ex.Message},堆栈：{ex.StackTrace}";
                logUtilService.Error(msg);

                return new BaseReturnDataModel<BaseUserInfoToken>(msg, null);
            }

            if (userInfoToken == null)
            {
                string msg = "userinfo is null";
                logUtilService.Error(msg);

                return new BaseReturnDataModel<BaseUserInfoToken>(msg, null);
            }

            return new BaseReturnDataModel<BaseUserInfoToken>(ReturnCode.Success, userInfoToken);
        }

        public static string GetUserJsonByUserKey(JxApplication jxApplication, string userKey, bool isSlidingExpiration)
        {
            var jxCacheService = DependencyUtil.ResolveService<IJxCacheService>().Value;
            CacheKey cacheKey = CacheKey.GetFrontSideUserInfoKey(userKey);

            string json = jxCacheService.GetCache<string>(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = jxApplication.UserKeyExpiredMinutes * 60,
                IsForceRefresh = false,
                IsCloneInstance = false,
                IsSlidingExpiration = isSlidingExpiration
            }, null);

            return json;
        }
    }
}