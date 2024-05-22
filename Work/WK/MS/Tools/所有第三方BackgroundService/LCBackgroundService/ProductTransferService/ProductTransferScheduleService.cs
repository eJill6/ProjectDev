using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Old;
using ProductTransferService.LCDataBase.BLL;
using ProductTransferService.LCDataBase.DLL;
using ProductTransferService.LCDataBase.Enums;
using ProductTransferService.LCDataBase.Merchant;
using ProductTransferService.LCDataBase.Model;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : OldBaseTransferScheduleServiceV2
    {
        private string _currency;

        private string _serviceUrlWithRecord;

        private string _md5Key;

        private string _desKey;

        private string _linecode;

        private int _perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        private readonly LCProfitLossInfo _lcProfitLossInfo;

        private readonly Lazy<ILCConfigService> _lcConfigService;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override PlatformProduct Product => PlatformProduct.LC;

        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(LCBetDetailMSLService)).Keyed<ILCBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(LCProfitLossInfo)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(LCConfigService)).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType(typeof(LCOldSaveProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            _lcProfitLossInfo.Init();
        }

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            return true;
        }

        public ProductTransferScheduleService()
        {
            _transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
            _lcProfitLossInfo = new LCProfitLossInfo();
            _lcConfigService = DependencyUtil.ResolveService<ILCConfigService>();
        }

        /// <summary>
        /// 初始化設置資料
        /// </summary>
        /// <returns></returns>
        public bool InitLocalAppSettings()
        {
            bool isLoadingFinish = false;

            try
            {
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
                _serviceUrlWithRecord = configUtilService.Get("ServiceUrlWithRecord").Trim();
                _currency = configUtilService.Get("Currency").Trim();
                _md5Key = configUtilService.Get("MD5Key").Trim();
                _desKey = configUtilService.Get("DesKey").Trim();
                _linecode = configUtilService.Get("LCLinecode").Trim();
                _perOnceQueryMinutes = Convert.ToInt32(configUtilService.Get("PerOnceQueryMinutes").Trim());

                isLoadingFinish = true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("LCTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override void DoSaveRemoteBetLogToPlatformJob()
        {
            var param = new LCApiParamModel
            {
                ServiceUrl = _serviceUrlWithRecord,
                AgentID = _lcConfigService.Value.AgentID,
                ActType = ApiAction.PlayGameResult,
                Currency = _currency,
                DESKey = _desKey,
                MD5Key = _md5Key,
                Linecode = _linecode,
                PerOnceQueryMinutes = _perOnceQueryMinutes
            };

            _transactionLogs.SetBetInfo(param);
        }
    }
}