using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Old;
using ProductTransferService.SportDataBase.BLL;
using ProductTransferService.SportDataBase.Common;
using ProductTransferService.SportDataBase.DLL;
using ProductTransferService.SportDataBase.Merchant;
using ProductTransferService.SportDataBase.Model;
using System.Data;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : OldBaseTransferScheduleServiceV2
    {
        private readonly TransactionLogs _transactionLogs;

        public ProductTransferScheduleService()
        {
            _transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        protected virtual bool IsWorkRefreshSportAvailableScores => true;
        
        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override PlatformProduct Product => PlatformProduct.Sport;

        protected override void DoInitSqlLite()
        {
            ProductTransferService.SportDataBase.DLL.SportProfitLossInfo.InIt();
        }

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            return true;
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(SportBetDetailMSLService)).Keyed<ISportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(ApiClient)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(SportProfitLossInfo)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(SportOldSaveProfitLossInfo)).AsImplementedInterfaces();
        }

        public bool InitLocalAppSettings()
        {
            try
            {
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

                return true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("SportTransferService异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return false;
        }

        protected override void DoSaveRemoteBetLogToPlatformJob()
        {
            _transactionLogs.SetBetInfo();
        }
    }
}