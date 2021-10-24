using System.Web.Mvc;
using JxBackendService.Common.Util.Route;

namespace JxBackendService.Common.Util
{
    public class RazorEngineUtil
    {
        public static void Init()
        {
            ViewEngines.Engines.Clear();
            var razorViewEngine = new RazorViewEngine();
            razorViewEngine.ViewLocationFormats = RouteUtil.ReplaceMerchantFormatPaths(razorViewEngine.ViewLocationFormats);
            razorViewEngine.PartialViewLocationFormats = RouteUtil.ReplaceMerchantFormatPaths(razorViewEngine.PartialViewLocationFormats);
            ViewEngines.Engines.Add(razorViewEngine);
        }
    }
}
