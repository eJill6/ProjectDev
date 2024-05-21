using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System;

namespace JxBackendService.Service.Base
{
    public class BaseEnvLoginUserService
    {
        private EnvironmentCode environmentCode = null;

        protected static PlatformMerchant Merchant => SharedAppSettings.PlatformMerchant;

        public EnvironmentUser EnvLoginUser { get; private set; }

        private readonly Lazy<ILogUtilService> _logUtilService;

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        public BaseEnvLoginUserService(EnvironmentUser envLoginUser)
        {
            EnvLoginUser = envLoginUser;
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        protected EnvironmentCode EnvCode
        {
            get
            {
                environmentCode = AssignValueOnceUtil.GetAssignValueOnce(
                    environmentCode,
                    () =>
                    {
                        return DependencyUtil.ResolveKeyed<IAppSettingService>(EnvLoginUser.Application, Merchant).Value.GetEnvironmentCode();
                    });

                return environmentCode;
            }
        }

        protected Lazy<T> ResolveEnvLoginUserService<T>() where T : IEnvLoginUserService
        {
            return DependencyUtil.ResolveEnvLoginUserService<T>(EnvLoginUser);
        }

        protected Lazy<T> ResolveJxBackendService<T>(DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(EnvLoginUser, dbConnectionType);
        }
    }
}