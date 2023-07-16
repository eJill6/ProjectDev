using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Attributes;
using JxBackendServiceNF.Common.Util;

namespace JxBackendServiceNF.Service.Net
{
    public class IpUtilService : IIpUtilService
    {
        public string GetIPAddress()
        {
            string ipAddress = IpUtil.GetDoWorkIpAddressFromHeader();

            if (ipAddress.IsNullOrEmpty())
            {
                ipAddress = IpUtil.GetDoWorkIP();
            }

            return ipAddress;
        }
    }

    [MockService]
    public class IpUtilMockService : IIpUtilService
    {
        public string GetIPAddress()
        {
            return "61.220.213.94";
        }
    }
}