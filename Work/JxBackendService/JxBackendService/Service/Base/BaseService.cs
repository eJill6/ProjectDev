using System;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.Param.Cache;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Cache;

namespace JxBackendService.Service.Base
{
    public class BaseService
    {
        private readonly DbConnectionTypes _dbConnectionType;
        protected readonly EnvironmentUser EnvLoginUser;

        protected static PlatformMerchant Merchant { get; } = SharedAppSettings.PlatformMerchant;

        public BaseService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            EnvLoginUser = envLoginUser;
            _dbConnectionType = dbConnectionType;
        }

        protected T ResolveJxBackendService<T>()
        {
            return DependencyUtil.ResolveJxBackendService<T>(EnvLoginUser, _dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(Type type)
        {
            return (T)DependencyUtil.ResolveJxBackendService(type, EnvLoginUser, _dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(PlatformMerchant keyModel)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, _dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(PlatformProduct keyModel)
        {
            return ResolveJxBackendService<T>(keyModel, _dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(PlatformProduct keyModel, DbConnectionTypes dbConnectionTypes)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, Merchant, EnvLoginUser, dbConnectionTypes);
        }

        protected T ResolveJxBackendService<T>(CommissionGroupType keyModel)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, _dbConnectionType);
        }

        protected T ResolveJxBackendService<T>(VIPBonusType keyModel)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, _dbConnectionType);
        }

        protected static T ResolveKeyed<T>(PlatformProduct keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, Merchant);
        }

        protected static T ResolveKeyed<T>(JxApplication keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, Merchant);
        }

        protected static T ResolveKeyedForModel<T>(JxApplication keyCtorModel)
        {
            return DependencyUtil.ResolveKeyedForModel<T>(keyCtorModel, Merchant);
        }

        protected static T ResolveServiceForModel<T>(JxApplication ctorParam)
        {
            return DependencyUtil.ResolveServiceForModel<T>(ctorParam);
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

        protected void DoIntervalWork(CacheKey cacheKey, int cacheSeconds, bool isSuspendWhenException, bool isDoErrorHandle, Action action)
        {
            IntervalJobUtil.DoIntervalWork(new IntervalJobParam()
            {
                EnvironmentUser = EnvLoginUser,
                CacheKey = cacheKey,
                CacheSeconds = cacheSeconds,
                IsDoErrorHandle = isDoErrorHandle,
                IsSuspendWhenException = isSuspendWhenException
            }, action);
        }

        protected T DoIntervalWork<T>(IntervalJobParam intervalJobParam, Func<T> getValueWork)
        {
            return IntervalJobUtil.DoIntervalWork(intervalJobParam, getValueWork);
        }

        protected T DoIntervalWork<T>(IntervalJobParam intervalJobParam, Func<T> getValueWork, Action afterSuspend)
        {
            return IntervalJobUtil.DoIntervalWork(intervalJobParam, getValueWork, afterSuspend);
        }

        protected T DoWorkWithErrorHandle<T>(Func<T> serviceWork, T exceptionDefaultResult = default(T))
        {
            return ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, serviceWork, false, exceptionDefaultResult);
        }

        protected void DoWorkWithErrorHandle(Action serviceWork)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, serviceWork);
        }

        protected bool IsValidRequired(params object[] list)
        {
            bool isValid = true;

            if (list != null)
            {
                foreach (object obj in list)
                {
                    if (obj is string)
                    {
                        if (string.IsNullOrEmpty(obj.ToTrimString()))
                        {
                            isValid = false;
                            break;
                        }
                    }
                    else
                    {
                        if (obj == null)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }

            return isValid;
        }

        ///// <summary>
        ///// 是否要寫操作記錄
        ///// </summary>
        //protected bool IsWriteOperationLog => EnvLoginUser.Application == JxApplication.BackSideWeb;
    }
}
