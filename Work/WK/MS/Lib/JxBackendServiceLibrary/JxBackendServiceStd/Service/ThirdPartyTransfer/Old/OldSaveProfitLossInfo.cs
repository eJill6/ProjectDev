using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    public abstract class BaseOldSaveProfitLossInfo<T> : BaseEnvLoginUserService, IOldSaveProfitLossInfo<T> where T : BaseRemoteBetLog
    {
        private readonly Lazy<ITransferSqlLiteBackupRepository> _transferSqlLiteBackupRepository;

        private readonly Lazy<ITPGameApiService> _tpGameApiService;

        private readonly Lazy<ITPGameApiReadService> _tpGameApiReadService;

        private readonly Lazy<ITPGameAccountReadService> _tpGameAccountReadService;

        private readonly Lazy<IMerchantSettingService> _merchantSettingService;

        protected ITPGameApiReadService TPGameApiReadService => _tpGameApiReadService.Value;

        protected ITPGameAccountReadService TPGameAccountReadService => _tpGameAccountReadService.Value;

        protected abstract PlatformProduct Product { get; }

        protected bool IsComputeAdmissionBetMoney => _merchantSettingService.Value.IsComputeAdmissionBetMoney;

        protected BaseOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
            _transferSqlLiteBackupRepository = DependencyUtil.ResolveService<ITransferSqlLiteBackupRepository>();
            _merchantSettingService = ResolveJxBackendService<IMerchantSettingService>(DbConnectionTypes.Slave);

            _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                Product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DbConnectionTypes.Master);

            _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(
                Product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DbConnectionTypes.Slave);

            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(
               SharedAppSettings.PlatformMerchant,
               EnvLoginUser,
               DbConnectionTypes.Slave);
        }

        protected abstract List<InsertTPGameProfitlossParam> ConvertFilterBetLogToTPGameProfitloss(
            Dictionary<string, int> accountMap,
            List<T> betLogs);

        public void SaveDataToTarget(List<T> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();
            HashSet<string> tpGameAccounts = betLogs.Select(s => s.TPGameAccount).ConvertToHashSet();
            Dictionary<string, int> accountMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);
            betLogs.RemoveAll(r => !accountMap.ContainsKey(r.TPGameAccount));
            _transferSqlLiteBackupRepository.Value.BackupNewBetLogs(betLogs);

            List<InsertTPGameProfitlossParam> tpGameProfitlosses = ConvertFilterBetLogToTPGameProfitloss(accountMap, betLogs);

            _tpGameApiService.Value.SaveMultipleProfitlossToPlatform(
                new SaveProfitlossToPlatformParam()
                {
                    TPGameProfitlosses = tpGameProfitlosses,
                });
        }
    }
}