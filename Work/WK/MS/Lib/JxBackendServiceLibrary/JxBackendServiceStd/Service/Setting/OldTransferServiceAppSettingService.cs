using JxBackendService.Interface.Service;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.Setting
{
    /// <summary>
    /// 給IMBG之前的第三方套用
    /// </summary>
    public class OldTransferServiceAppSettingService : BaseAppSettingService, IOldTransferServiceAppSettingService
    {
        protected override string MasterInloDbConnectionStringConfigKey => "ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => "Slave_InlodbBak_ConnectionString";

        public override string CommonDataHash => throw new NotImplementedException();

        public List<string> CopyBetLogToMerchantCodes => GetAppSettingValues("CopyBetLogToMerchantCodes");
    }

    public class OldTransferServiceGameAppSettingService : BaseGameAppSettingService
    {
        protected override string IMMerchantCodeConfigKey => "MerchantCode";

        protected override string IMServiceUrlConfigKey => "ServiceUrl";

        protected override string IMLanguageConfigKey => "Language";

        protected override string IMCurrencyConfigKey => "Currency";

        protected override string IMPPMerchantCodeConfigKey => "MerchantCode";

        protected override string IMPPServiceUrlConfigKey => "ServiceUrl";

        protected override string IMPPLanguageConfigKey => "Language";

        protected override string IMPPCurrencyConfigKey => "Currency";

        protected override string IMPTMerchantCodeConfigKey => "MerchantCode";

        protected override string IMPTServiceUrlConfigKey => "ServiceUrl";

        protected override string IMPTLanguageConfigKey => "Language";

        protected override string IMPTCurrencyConfigKey => "Currency";

        protected override string IMSportMerchantCodeConfigKey => "MerchantCode";

        protected override string IMSportServiceUrlConfigKey => "ServiceUrl";

        protected override string IMSportLanguageConfigKey => "Language";

        protected override string IMSportCurrencyConfigKey => "Currency";

        protected override string IMeBETMerchantCodeConfigKey => "MerchantCode";

        protected override string IMeBETServiceUrlConfigKey => "ServiceUrl";

        protected override string IMeBETLanguageConfigKey => "Language";

        protected override string IMeBETCurrencyConfigKey => "Currency";

        protected override string IMBGMerchantCodeConfigKey => "MerchantCode";

        protected override string IMBGServiceUrlConfigKey => "ServiceUrl";

        protected override string IMBGMD5KeyConfigKey => "MD5Key";

        protected override string IMBGDesKeyConfigKey => "DesKey";

        // 排成沒有此KEY
        protected override string AGLoginBaseUrlConfigKey => string.Empty;

        protected override string AGServiceBaseUrlConfigKey => "URL";

        protected override string AGVendorIDConfigKey => "Cagent";

        protected override string AGMD5KeyConfigKey => "Md5Key";

        protected override string AGDesKeyConfigKey => "DesKey";

        protected override string AGCurrencyConfigKey => "Cur";

        // 排成沒有此KEY
        protected override string AGOddsTypeConfigKey => string.Empty;

        protected override string AGAcTypeConfigKey => "Actype";

        // 排成沒有此KEY
        protected override string AGDMConfigKey => string.Empty;

        protected override string LCMerchantCodeConfigKey => "AgentID";

        protected override string LCServiceUrlConfigKey => "ServiceUrl";

        protected override string LCMD5KeyConfigKey => "MD5Key";

        protected override string LCDesKeyConfigKey => "DesKey";

        protected override string LCLinecodeConfigKey => "LCLinecode";

        protected override string SportMerchantCodeConfigKey => "VendorID";

        protected override string SportServiceUrlConfigKey => "URL";

        // 排成沒有此KEY
        protected override string SportLoginBaseUrlConfigKey => string.Empty;

        protected override string SportCurrencyConfigKey => "Currency";

        // 排成沒有此KEY
        protected override string SportOddsTypeConfigKey => string.Empty;
    }
}