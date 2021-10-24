using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.Net
{
    public class IpUtilService : IIpUtilService
    {
        public JxIpInformation GetDoWorkIPInformation()
        {
            var ipInformation = IpUtil.GetDoWorkIPInformation();

            return ipInformation.ToJxIpInformation();
        }

        public string GetIPAddress()
        {
            return IpUtil.GetDoWorkIP();
        }
    }
}
