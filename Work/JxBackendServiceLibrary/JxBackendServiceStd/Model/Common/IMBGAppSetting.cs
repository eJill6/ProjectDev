namespace JxBackendService.Model.Common
{
    public interface IIMBGAppSetting
    {
        string MerchantCode { get; }

        string ServiceUrl { get; }

        string MD5Key { get; }

        string DESKey { get; }
    }

    public class IMBGAppSetting : SharedAppSettings, IIMBGAppSetting
    {
        public string MerchantCodeConfigKey { get; set; }

        public string ServiceUrlConfigKey { get; set; }

        public string MD5KeyConfigKey { get; set; }

        public string DESKeyConfigKey { get; set; }

        public string MerchantCode => Get(MerchantCodeConfigKey);

        public string ServiceUrl => Get(ServiceUrlConfigKey);

        public string MD5Key => Get(MD5KeyConfigKey);

        public string DESKey => Get(DESKeyConfigKey);
    }
}
