using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace JxBackendService.Service.Net
{
    public class OperationContextService : IOperationContextService
    {
        private static readonly string s_userNameHeadName = "p1";

        private static readonly string s_userKeyHeadName = "p2";

        private static readonly string s_osNameHeadName = "osName";

        private static readonly string s_versionCode = "versionCode";

        private static readonly string s_versionHeadName = "Version";

        private static readonly string s_deviceHeadName = "device";

        private static readonly string s_sysVerHeadName = "sysVer";

        private static readonly string s_ipHeadName = "ip";

        private static readonly string s_firstNotPrivateIpHeaderName = "firstNotPrivateIP";

        private static readonly string s_localDomainNameHeaderName = "localDomainName";

        private static readonly string s_deviceCodeHeaderName = "DeviceCode";

        public string GetUserName()
        {
            return TryGetHeader(s_userNameHeadName);
        }

        public string GetUserKey()
        {
            return TryGetHeader(s_userKeyHeadName);
        }

        public string GetOSName()
        {
            return TryGetHeader(s_osNameHeadName);
        }

        public string GetVersionCode()
        {
            return TryGetHeader(s_versionCode);
        }

        public string GetVersion()
        {
            return TryGetHeader(s_versionHeadName);
        }

        public string GetDevice()
        {
            return TryGetHeader(s_deviceHeadName);
        }

        public string GetSysVer()
        {
            return TryGetHeader(s_sysVerHeadName);
        }

        public string GetIp()
        {
            return TryGetHeader(s_ipHeadName);
        }

        public string GetFirstNotPrivateIP()
        {
            return TryGetHeader(s_firstNotPrivateIpHeaderName);
        }

        public string GetLocalDomain()
        {
            return TryGetHeader(s_localDomainNameHeaderName);
        }

        public string GetDeviceCode()
        {
            return TryGetHeader(s_deviceCodeHeaderName);
        }

        private string TryGetHeader(string header)
        {
            if (OperationContext.Current == null)
            {
                return null;
            }

            MessageHeaders messageHeadersElement = OperationContext.Current.IncomingMessageHeaders;
            int id = messageHeadersElement.FindHeader(header, string.Empty);

            if (id > -1)
            {
                return HttpUtility.UrlDecode(messageHeadersElement.GetHeader<string>(header, string.Empty).ToTrimString());
            }

            return null;
        }
    }
}