using JxBackendService.Interface.Service;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.Setting
{
    /// <summary>
    /// 給IMSG以後的排程套用
    /// </summary>
    public class NewTransferServiceAppSettingService : BaseAppSettingService, INewTransferServiceAppSettingService
    {
        protected override string MasterInloDbConnectionStringConfigKey => "Master_Inlodb_ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => "Slave_InlodbBak_ConnectionString";

        public override string CommonDataHash => GetAppSettingValue("CommonHash");

        public List<string> CopyBetLogToMerchantCodes => GetAppSettingValues("CopyBetLogToMerchantCodes");
    }

    public class NewTransferServiceGameAppSettingService : BaseGameAppSettingService
    {
        protected override string IMMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string IMServiceUrlConfigKey => throw new NotImplementedException();

        protected override string IMLanguageConfigKey => throw new NotImplementedException();

        protected override string IMCurrencyConfigKey => throw new NotImplementedException();

        protected override string IMPPMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string IMPPServiceUrlConfigKey => throw new NotImplementedException();

        protected override string IMPPLanguageConfigKey => throw new NotImplementedException();

        protected override string IMPPCurrencyConfigKey => throw new NotImplementedException();

        protected override string IMPTMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string IMPTServiceUrlConfigKey => throw new NotImplementedException();

        protected override string IMPTLanguageConfigKey => throw new NotImplementedException();

        protected override string IMPTCurrencyConfigKey => throw new NotImplementedException();

        protected override string IMSportMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string IMSportServiceUrlConfigKey => throw new NotImplementedException();

        protected override string IMSportLanguageConfigKey => throw new NotImplementedException();

        protected override string IMSportCurrencyConfigKey => throw new NotImplementedException();

        protected override string IMeBETMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string IMeBETServiceUrlConfigKey => throw new NotImplementedException();

        protected override string IMeBETLanguageConfigKey => throw new NotImplementedException();

        protected override string IMeBETCurrencyConfigKey => throw new NotImplementedException();

        protected override string IMBGMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string IMBGServiceUrlConfigKey => throw new NotImplementedException();

        protected override string IMBGMD5KeyConfigKey => throw new NotImplementedException();

        protected override string IMBGDesKeyConfigKey => throw new NotImplementedException();

        protected override string AGLoginBaseUrlConfigKey => throw new NotImplementedException();

        protected override string AGServiceBaseUrlConfigKey => throw new NotImplementedException();

        protected override string AGVendorIDConfigKey => throw new NotImplementedException();

        protected override string AGMD5KeyConfigKey => throw new NotImplementedException();

        protected override string AGDesKeyConfigKey => throw new NotImplementedException();

        protected override string AGCurrencyConfigKey => throw new NotImplementedException();

        protected override string AGOddsTypeConfigKey => throw new NotImplementedException();

        protected override string AGAcTypeConfigKey => throw new NotImplementedException();

        protected override string AGDMConfigKey => throw new NotImplementedException();

        protected override string LCMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string LCServiceUrlConfigKey => throw new NotImplementedException();

        protected override string LCMD5KeyConfigKey => throw new NotImplementedException();

        protected override string LCDesKeyConfigKey => throw new NotImplementedException();

        protected override string LCLinecodeConfigKey => throw new NotImplementedException();

        protected override string SportMerchantCodeConfigKey => throw new NotImplementedException();

        protected override string SportServiceUrlConfigKey => throw new NotImplementedException();

        protected override string SportLoginBaseUrlConfigKey => throw new NotImplementedException();

        protected override string SportCurrencyConfigKey => throw new NotImplementedException();

        protected override string SportOddsTypeConfigKey => throw new NotImplementedException();
    }
}