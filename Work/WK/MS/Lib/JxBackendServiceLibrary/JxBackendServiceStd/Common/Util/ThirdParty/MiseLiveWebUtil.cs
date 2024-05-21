using System.Web;

namespace JxBackendService.Common.Util.ThirdParty
{
    public static class MiseLiveWebUtil
    {
        private static readonly string s_fullScreenUrlTemplate = "seal://common/webview?recovery=true&fullScreen={0}&type=url&title={1}&initUrl={2}";

        public static string ConvertToFullScreenUrl(this string url, bool isHideHeaderWithFullScreen, string title)
        {
            return string.Format(s_fullScreenUrlTemplate,
                isHideHeaderWithFullScreen.ToString().ToLower(),
                HttpUtility.UrlEncode(title),
                HttpUtility.UrlEncode(url));
        }
    }
}