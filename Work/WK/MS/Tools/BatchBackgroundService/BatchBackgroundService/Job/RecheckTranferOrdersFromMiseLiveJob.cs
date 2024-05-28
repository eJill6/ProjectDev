using BatchService.Job.Base;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Model.Enums;

namespace BatchService.Job
{
    public class RecheckTranferOrdersFromMiseLiveJob : BaseBatchServiceQuartzJob
    {
        private readonly Lazy<IRechargeService> _rechargeService;

        private readonly Lazy<IWithdrawService> _withdrawService;

        public RecheckTranferOrdersFromMiseLiveJob()
        {
            _rechargeService = DependencyUtil.ResolveJxBackendService<IRechargeService>(EnvUser, DbConnectionTypes.Master);
            _withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(EnvUser, DbConnectionTypes.Master);
        }

        public override void DoJob()
        {
            //充值订单确认
            _rechargeService.Value.RecheckOrdersFromMiseLive();

            //提现订单确认
            _withdrawService.Value.RecheckWithdrawOrdersFromMiseLive();
        }
    }
}