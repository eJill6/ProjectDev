using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Old;
using ProductTransferService.IMBGDataBase.BLL;
using ProductTransferService.IMBGDataBase.Common;
using ProductTransferService.IMBGDataBase.DLL;
using ProductTransferService.IMBGDataBase.Enums;
using ProductTransferService.IMBGDataBase.Merchant;
using ProductTransferService.IMBGDataBase.Model;
using System.Data;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : OldBaseTransferScheduleServiceV2
    {
        private static string s_merchantCode;

        private static string s_language;

        private static string s_serviceUrl;

        private static string s_md5Key;

        private static string s_desKey;

        private static int s_perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        public ProductTransferScheduleService()
        {
            _transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        protected override PlatformProduct Product => PlatformProduct.IMBG;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMBGBetDetailMSLService)).Keyed<IIMBGBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMBGProfitLossInfo)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(IMBGOldSaveProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMBGProfitLossInfo.InIt();
        }

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            return true;
        }

        protected override void DoSaveRemoteBetLogToPlatformJob()
        {
            var param = new IMBGApiParamModel
            {
                ServiceUrl = s_serviceUrl,
                MerchantCode = s_merchantCode,
                ActType = ApiAction.PlayGameResult,
                Language = s_language,
                Page = 1,
                PerOnceQueryMinutes = s_perOnceQueryMinutes,
                MD5Key = s_md5Key,
                DesKey = s_desKey
            };

            _transactionLogs.SetBetInfo(param);
        }

        private bool InitLocalAppSettings()
        {
            bool isLoadingFinish = false;
            try
            {
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
                s_merchantCode = configUtilService.Get("MerchantCode", string.Empty).Trim();
                s_serviceUrl = configUtilService.Get("ServiceUrl", string.Empty).Trim();
                s_language = configUtilService.Get("Language", string.Empty).Trim();
                s_md5Key = configUtilService.Get("MD5Key", string.Empty).Trim();
                s_desKey = configUtilService.Get("DesKey", string.Empty).Trim();
                s_perOnceQueryMinutes = configUtilService.Get("PerOnceQueryMinutes", string.Empty).Trim().ToInt32();

                isLoadingFinish = true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("IMBGTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }
    }
}