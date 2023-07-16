using JxBackendService.Interface.Service;
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
}