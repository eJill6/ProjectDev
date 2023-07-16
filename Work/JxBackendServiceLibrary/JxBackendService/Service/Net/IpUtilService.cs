using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.Net
{
    public class IpUtilService : IIpUtilService
    {
        public JxIpInformation GetDoWorkIPInformation()
        {
            IPToolModel.IPInformation ipInformation = IpUtil.GetDoWorkIPInformation();

            return ipInformation.ToJxIpInformation();
        }

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
        public JxIpInformation GetDoWorkIPInformation()
        {
            return new JxIpInformation()
            {
                DestinationIP = "61.220.213.94",
                DestinationIPVersion = IPToolModel.Enums.IPVersion_Enum.IPVersion.Version4,
                DestinationIPNumber = 1037882718,
                SourceIP = "61.220.213.94"
            };
        }

        public string GetIPAddress()
        {
            return "61.220.213.94";
        }
    }
}