using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Cache;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Cache;
using System;

namespace JxBackendService.Service.Base
{
    public class BaseService : BaseEnvLoginUserService
    {
        protected readonly DbConnectionTypes DbConnectionType;

        public BaseService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser)
        {
            DbConnectionType = dbConnectionType;
        }

        protected Lazy<T> ResolveJxBackendService<T>()
        {
            return ResolveJxBackendService<T>(DbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformMerchant keyModel)
        {
            return ResolveJxBackendService<T>(keyModel, DbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformMerchant keyModel, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, EnvLoginUser, dbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformProduct keyModel)
        {
            return ResolveJxBackendService<T>(keyModel, DbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformProduct keyModel, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, Merchant, EnvLoginUser, dbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(JxApplication keyModel)
        {
            return ResolveJxBackendService<T>(keyModel, DbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(JxApplication keyModel, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(keyModel, Merchant, EnvLoginUser, dbConnectionType);
        }

        protected static Lazy<T> ResolveKeyed<T>(PlatformProduct keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, Merchant);
        }

        protected static Lazy<T> ResolveKeyed<T>(JxApplication keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, Merchant);
        }

        //protected static Lazy<T> ResolveKeyedForModel<T>(JxApplication keyCtorModel)
        //{
        //    return DependencyUtil.ResolveKeyedForModel<T>(keyCtorModel, Merchant);
        //}

        //protected static Lazy<T> ResolveServiceForModel<T>(JxApplication ctorParam)
        //{
        //    return DependencyUtil.ResolveServiceForModel<T>(ctorParam);
        //}

        protected static Lazy<T> ResolveServiceForModel<T>(IOSSSetting ctorParam)
        {
            return DependencyUtil.ResolveServiceForModel<T>(ctorParam);
        }

        protected void DoIntervalWork(CacheKey cacheKey, double cacheSeconds, bool isSuspendWhenException, bool isDoErrorHandle, Action action)
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
            return RequiredUtil.IsValidRequired(list);
        }

        protected BaseReturnModel ValidateParamThenDoJob(Func<BaseReturnModel> validateParamJob, Func<BaseReturnModel> doAfterSuccess)
        {
            BaseReturnModel validateParamResult = validateParamJob.Invoke();

            if (!validateParamResult.IsSuccess)
            {
                return validateParamResult;
            }

            return doAfterSuccess.Invoke();
        }

        protected BaseReturnModel GetParamThenDoJob<T>(Func<BaseReturnDataModel<T>> getParamJob, Func<T, BaseReturnModel> doAfterSuccess)
        {
            return GetParamThenDoJob<T, BaseReturnModel>(getParamJob, doAfterSuccess);
        }

        protected BaseReturnDataModel<T> GetDataModel<T>(Func<BaseReturnDataModel<T>> getParamJob)
        {
            return GetParamThenDoJob<T, BaseReturnDataModel<T>>(getParamJob,
                doAfterSuccess: (dataModel) => new BaseReturnDataModel<T>(ReturnCode.Success, dataModel));
        }

        protected BaseReturnDataModel<T> GetParamThenDoJob<T>(Func<BaseReturnDataModel<T>> getParamJob, Func<T, BaseReturnDataModel<T>> doAfterSuccess)
        {
            return GetParamThenDoJob<T, BaseReturnDataModel<T>>(getParamJob, doAfterSuccess);
        }

        private TResult GetParamThenDoJob<T, TResult>(Func<BaseReturnDataModel<T>> getParamJob, Func<T, TResult> doAfterSuccess) where TResult : new()
        {
            BaseReturnDataModel<T> getParamResult = getParamJob.Invoke();

            if (!getParamResult.IsSuccess)
            {
                return getParamResult.CastByJson<TResult>();
            }

            return doAfterSuccess.Invoke(getParamResult.DataModel);
        }
    }
}