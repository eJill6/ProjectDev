using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.DownloadFile
{
    /// <summary>
    /// 用於定義下載檔案路徑組成
    /// 路徑開頭使用content是因為不破壞原來的靜態資源結構
    /// </summary>
    public class DownloadFilePathTypes : BaseIntValueModel<DownloadFilePathTypes>
    {
        private DownloadFilePathTypes() { }

        public string FilePath { get; set; }

        public string ExtensionType { get; set; }

        public static DownloadFilePathTypes GameMenuEntranceItemLogo = new DownloadFilePathTypes()
        {
            Value = 0,
            FilePath = "content/images/gamemenus/entranceitem/logo/{0}/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static DownloadFilePathTypes GameMenuEntranceItemAdvertising = new DownloadFilePathTypes()
        {
            Value = 1,
            FilePath = "content/images/gamemenus/entranceitem/advertising/{0}/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static DownloadFilePathTypes GameMenuEntranceItemLoading = new DownloadFilePathTypes()
        {
            Value = 2,
            FilePath = "content/images/gamemenus/entranceitem/loading/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static DownloadFilePathTypes GameMenuRecommendItem = new DownloadFilePathTypes()
        {
            Value = 3,
            FilePath = "content/images/gamemenus/recommenditem/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static DownloadFilePathTypes GoogleBindTutorialHtml = new DownloadFilePathTypes()
        {
            Value = 4,
            FilePath = "content/googleAuth/GoogleBindTutorial.html",
            ExtensionType = HtmlExtensionTypes.HTML
        };

        public static DownloadFilePathTypes LotteryCenterThirdPartyLogo = new DownloadFilePathTypes()
        {
            Value = 5,
            FilePath = "content/images/lotterycenter/thirdparty/logo/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static DownloadFilePathTypes LotteryCenterLogo = new DownloadFilePathTypes()
        {
            Value = 6,
            FilePath = "content/images/lotterycenter/lottery/logo/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static DownloadFilePathTypes TransferInfoLogo = new DownloadFilePathTypes()
        {
            Value = 7,
            FilePath = "content/images/transferinfo/logo/",
            ExtensionType = ImageExtensionTypes.PNG
        };

        public static string GetBaseImagePath(DownloadFilePathTypes imagePath, string imageName)
        {
            return string.Format("{0}{1}{2}", imagePath.FilePath, imageName, imagePath.ExtensionType);
        }

        public static string GetControlSizeImagePath(DownloadFilePathTypes imagePath, string imageName, string imageSizeTypeValue)
        {
            string fullPath = string.Format(imagePath.FilePath, imageSizeTypeValue);

            return string.Format("{0}{1}{2}", fullPath, imageName, imagePath.ExtensionType);
        }
    }   

    public static class ImageExtensionTypes
    {
        public static string PNG = ".png";
        public static string JPG = ".jpg";
        public static string JPEG = ".jpeg";
        public static string SVG = ".svg";
        public static string GIF = ".gif";
    }

    public static class HtmlExtensionTypes
    {
        public static string HTML = ".html";
        public static string HTM = ".htm";
    }

    public static class ImageSizeTypes
    {
        public static string Small = "s";
        //public static string Medium = "m";
        public static string Large = "l";
    }
}
