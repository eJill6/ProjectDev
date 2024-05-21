using System.Globalization;

namespace JxBackendService.Model.Enums
{
    public class PlatformCulture : BaseStringValueModel<PlatformCulture>
    {
        public CultureInfo Culture { get; private set; }

        private PlatformCulture() { }

        public static readonly PlatformCulture China = new PlatformCulture()
        {
            Value = "zh-CN",
            Culture = new CultureInfo("zh-CN")
        };

        public static readonly PlatformCulture Vietnam = new PlatformCulture()
        {
            Value = "vi-VN",
            Culture = new CultureInfo("vi-VN")
        };

        public static readonly PlatformCulture US = new PlatformCulture()
        {
            Value = "en-US",
            Culture = new CultureInfo("en-US")
        };
    }
}
