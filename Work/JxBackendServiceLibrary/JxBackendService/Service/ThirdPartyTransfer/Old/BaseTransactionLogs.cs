using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    public abstract class BaseTransactionLogs<ApiParamType, ReturnType> : BaseService
    {
        private readonly IBetDetailService<ApiParamType, ReturnType> _betDetailService;

        private readonly ITPGameAccountReadService _tpGameAccountReadService;

        protected ITPGameAccountReadService TPGameAccountReadService => _tpGameAccountReadService;

        protected IBetDetailService<ApiParamType, ReturnType> BetDetailService => _betDetailService;

        public BaseTransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(
                Merchant,
                EnvLoginUser,
                DbConnectionTypes.Slave);

            _betDetailService = ResolveBetDetailService(SharedAppSettings.PlatformMerchant);
        }

        protected abstract IBetDetailService<ApiParamType, ReturnType> ResolveBetDetailService(PlatformMerchant platformMerchant);
    }
}