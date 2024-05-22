using Autofac;
using IMBGDataBase.Merchant;
using IMSportDataBase.BLL;
using IMSportDataBase.DLL;
using IMSportDataBase.Enums;
using IMSportDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IMSport;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Old;

namespace ProductTransferService
{
    public class ProductTransferScheduleService : OldBaseTransferScheduleServiceV2
    {
        private static string s_merchantCode;

        private static string s_currency;

        private static string s_language;

        private static string s_serviceUrl;

        private static string s_productWallet;

        private static int s_perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override PlatformProduct Product => PlatformProduct.IMSport;

        public ProductTransferScheduleService()
        {
            _transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMSportBetDetailMSLService)).Keyed<IIMSportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMSportProfitLossInfo)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(IMSportOldSaveProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMSportProfitLossInfo.InIt();
        }

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            return true;
        }

        public bool InitLocalAppSettings()
        {
            bool isLoadingFinish = false;
            try
            {
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;

                s_serviceUrl = configUtilService.Get("ServiceUrl", string.Empty).Trim();
                s_merchantCode = configUtilService.Get("MerchantCode", string.Empty).Trim();
                s_currency = configUtilService.Get("Currency", string.Empty).Trim();
                s_language = configUtilService.Get("Language", string.Empty).Trim();
                s_productWallet = configUtilService.Get("ProductWallet", string.Empty).Trim();
                s_perOnceQueryMinutes = configUtilService.Get("PerOnceQueryMinutes", string.Empty).Trim().ToInt32();

                isLoadingFinish = true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("IMSportsbookTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override void DoSaveRemoteBetLogToPlatformJob()
        {
            var param = new IMSportApiParamModel
            {
                ServiceUrl = s_serviceUrl,
                MerchantCode = s_merchantCode,
                ActType = ApiAction.PlayGameResult,
                ProductWallet = s_productWallet,
                Currency = s_currency,
                Language = s_language,
                Page = 1,
                PerOnceQueryMinutes = s_perOnceQueryMinutes
            };

            _transactionLogs.SaveBetInfo(param);

            //目前ProductWallet只有一個, 先把這段註解
            //若未來改成多個, 請參考ProductCode的方式在下一層做foreach
            //foreach (var product in _productWallet.Split(';'))
            //{
            //    param.ProductWallet = product;
            //    _transactionLogs.GetBetInfo(param);
            //}
        }
    }
}

namespace WagerTypeUtilService
{
    public static class WagerTypeUtil
    {
        public static WagerType ConvertToWagerType(string imsportWagerType)
        {
            IMSportWagerType imSportWagerType = IMSportWagerType.GetSingle(imsportWagerType);

            if (imSportWagerType != null && imSportWagerType.WagerType != null)
            {
                return imSportWagerType.WagerType;
            }

            return WagerType.Single;
        }
    }
}