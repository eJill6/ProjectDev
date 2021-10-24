using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

namespace JxBackendService.Service.Net
{
    public class OperationContextService : IOperationContextService
    {
        private static readonly string _userNameHeadName = "p1";
        private static readonly string _userKeyHeadName = "p2";
        private static readonly string _osNameHeadName = "osName";
        private static readonly string _versionHeadName = "Version";
        private static readonly string _deviceHeadName = "device";
        private static readonly string _sysVerHeadName = "sysVer";
        private static readonly string _ipHeadName = "ip";
        private static readonly string _firstNotPrivateIpHeaderName = "firstNotPrivateIP";
        private static readonly string _localDomainNameHeaderName = "localDomainName";

        public string GetUserName()
        {
            return TryGetHeader(_userNameHeadName);
        }

        public string GetUserKey()
        {
            return TryGetHeader(_userKeyHeadName);
        }

        public string GetOSName()
        {
            return TryGetHeader(_osNameHeadName);
        }

        public string GetVersion()
        {
            return TryGetHeader(_versionHeadName);
        }

        public string GetDevice()
        {
            return TryGetHeader(_deviceHeadName);
        }

        public string GetSysVer()
        {
            return TryGetHeader(_sysVerHeadName);
        }

        public string GetIp()
        {
            return TryGetHeader(_ipHeadName);
        }

        public string GetFirstNotPrivateIP()
        {
            return TryGetHeader(_firstNotPrivateIpHeaderName);
        }

        public string GetLocalDomain()
        {
            return TryGetHeader(_localDomainNameHeaderName);
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
