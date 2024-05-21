using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Service.Config
{
    public class WebSVServiceSettingService : IWebSVServiceSettingService
    {
        public virtual int WebRequestWebSVWaitMilliSeconds => 5 * 1000;
    }

    [MockService]
    public class WebSVServiceSettingMockService : WebSVServiceSettingService
    {
        public override int WebRequestWebSVWaitMilliSeconds => 100 * 1000;
    }
}