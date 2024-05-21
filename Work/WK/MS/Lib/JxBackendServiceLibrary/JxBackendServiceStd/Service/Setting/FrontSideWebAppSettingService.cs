using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.Setting
{
    /// <summary>
    /// 平台彩票前台設定服務
    /// </summary>
    ///
    public class FrontSideWebAppSettingService : BaseAppSettingService
    {
        protected override string MasterInloDbConnectionStringConfigKey => "ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => "ConnectionString_Bak";

        public override string CommonDataHash => GetAppSettingValue("CommonHash");

        public override int? MinWorkerThreads => 400;
    }

    /// <summary>
    /// 平台彩票MobileApi設定服務
    /// </summary>
    ///
    public class MobileApiAppSettingService : BaseAppSettingService
    {
        public override int? MinWorkerThreads => 600;

        public override string CommonDataHash => throw new System.NotImplementedException();

        protected override string MasterInloDbConnectionStringConfigKey => throw new System.NotImplementedException();

        protected override string SlaveInloDbBakConnectionStringConfigKey => throw new System.NotImplementedException();
    }

    public class BackSideWebAppSettingService : BaseAppSettingService
    {
        public override int? MinWorkerThreads => 250;

        protected override string MasterInloDbConnectionStringConfigKey => "Master_Inlodb_ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => "Slave_InlodbBak_ConnectionString";

        public override string CommonDataHash => GetAppSettingValue("CommonHash");
    }

    public class FrontSideWebGameAppSettingService : BaseGameAppSettingService
    {
        protected override string IMMerchantCodeConfigKey => "IMMerchantCode";

        protected override string IMServiceUrlConfigKey => "IMServiceBaseUrl";

        protected override string IMLanguageConfigKey => "IMLanguage";

        protected override string IMCurrencyConfigKey => "IMCurrency";

        protected override string IMPPMerchantCodeConfigKey => "IMPPMerchantCode";

        protected override string IMPPServiceUrlConfigKey => "IMPPServiceBaseUrl";

        protected override string IMPPLanguageConfigKey => "IMPPLanguage";

        protected override string IMPPCurrencyConfigKey => "IMPPCurrency";

        protected override string IMPTMerchantCodeConfigKey => "IMPTMerchantCode";

        protected override string IMPTServiceUrlConfigKey => "IMPTServiceBaseUrl";

        protected override string IMPTLanguageConfigKey => "IMPTLanguage";

        protected override string IMPTCurrencyConfigKey => "IMPTCurrency";

        protected override string IMSportMerchantCodeConfigKey => "IMSportsbookMerchantCode";

        protected override string IMSportServiceUrlConfigKey => "IMSportsbookServiceBaseUrl";

        protected override string IMSportLanguageConfigKey => "IMSportsbookLanguage";

        protected override string IMSportCurrencyConfigKey => "IMSportsbookCurrency";

        protected override string IMeBETMerchantCodeConfigKey => "IMeBETMerchantCode";

        protected override string IMeBETServiceUrlConfigKey => "IMeBETServiceBaseUrl";

        protected override string IMeBETLanguageConfigKey => "IMeBETLanguage";

        protected override string IMeBETCurrencyConfigKey => "IMeBETCurrency";

        protected override string IMBGMerchantCodeConfigKey => "IMBGMerchantCode";

        protected override string IMBGServiceUrlConfigKey => "IMBGServiceBaseUrl";

        protected override string IMBGMD5KeyConfigKey => "IMBGMD5Key";

        protected override string IMBGDesKeyConfigKey => "IMBGDesKey";

        protected override string AGLoginBaseUrlConfigKey => "AGLoginBaseUrl";

        protected override string AGServiceBaseUrlConfigKey => "AGServiceBaseUrl";

        protected override string AGVendorIDConfigKey => "AGVendorID";

        protected override string AGMD5KeyConfigKey => "AGMD5Key";

        protected override string AGDesKeyConfigKey => "AGDesKey";

        protected override string AGCurrencyConfigKey => "AGCur";

        protected override string AGOddsTypeConfigKey => "AGOddsType";

        protected override string AGAcTypeConfigKey => "AGActype";

        protected override string AGDMConfigKey => "AGDM";

        protected override string LCMerchantCodeConfigKey => "LCAgentID";

        protected override string LCServiceUrlConfigKey => "LCServiceBaseUrl";

        protected override string LCMD5KeyConfigKey => "LCMD5Key";

        protected override string LCDesKeyConfigKey => "LCDesKey";

        protected override string LCLinecodeConfigKey => "LCLinecode";

        protected override string SportMerchantCodeConfigKey => "SportVendorID";

        protected override string SportServiceUrlConfigKey => "SportServiceBaseUrl";

        protected override string SportLoginBaseUrlConfigKey => "SportLoginUrl";

        protected override string SportCurrencyConfigKey => "SportCur";

        protected override string SportOddsTypeConfigKey => "SportOddsType";
    }

    public class MobileApiGameAppSettingService : IGameAppSettingService
    {
        public IAGAppSetting GetAGAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public IIMOneAppSetting GetIMAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public IIMBGAppSetting GetIMBGAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public IIMOneAppSetting GetIMeBETAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public IIMOneAppSetting GetIMPPAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public IIMOneAppSetting GetIMPTAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public IIMOneAppSetting GetIMSportAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public ILCAppSetting GetLCAppSetting()
        {
            throw new System.NotImplementedException();
        }

        public ISportAppSetting GetSportAppSetting()
        {
            throw new System.NotImplementedException();
        }
    }
}