using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Model;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using JxBackendService.Service.Finance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BatchService.Job
{
    public class RecheckTranferOrdersFromMiseLiveJob : BaseBatchServiceQuartzJob
    {
        private readonly IRechargeService _rechargeService;

        private readonly IWithdrawService _withdrawService;

        public RecheckTranferOrdersFromMiseLiveJob()
        {
            _rechargeService = DependencyUtil.ResolveJxBackendService<IRechargeService>(EnvUser, DbConnectionTypes.Master);
            _withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(EnvUser, DbConnectionTypes.Master);
        }

        public override void DoJob()
        {
            //充值订单确认
            _rechargeService.RecheckOrdersFromMiseLive();

            //提现订单确认
            _withdrawService.RecheckWithdrawOrdersFromMiseLive();
        }
    }
}