namespace JxBackendService.Model.Common
{
    public interface IAGAppSetting
    {
        string LoginBaseUrl { get; }

        string ServiceBaseUrl { get; }

        string VendorID { get; }

        string Currency { get; }

        string OddsType { get; }

        string AcType { get; }

        string DM { get; }

        string MD5Key { get; }

        string DesKey { get; }
    }

    public class AGAppSetting : SharedAppSettings, IAGAppSetting
    {
        public string LoginBaseUrlConfigKey { get; set; }

        public string ServiceBaseUrlConfigKey { get; set; }

        public string VendorIDConfigKey { get; set; }

        public string CurrencyConfigKey { get; set; }

        public string OddsTypeConfigKey { get; set; }

        public string AcTypeConfigKey { get; set; }

        public string DMConfigKey { get; set; }

        public string MD5KeyConfigKey { get; set; }

        public string DesKeyConfigKey { get; set; }

        public string LoginBaseUrl => Get(LoginBaseUrlConfigKey);

        public string ServiceBaseUrl => Get(ServiceBaseUrlConfigKey);

        public string VendorID => Get(VendorIDConfigKey);

        public string Currency => Get(CurrencyConfigKey);

        public string OddsType => Get(OddsTypeConfigKey);

        public string AcType => Get(AcTypeConfigKey);

        public string DM => Get(DMConfigKey);

        public string MD5Key => Get(MD5KeyConfigKey);

        public string DesKey => Get(DesKeyConfigKey);
    }
}