using System;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Util;

namespace JxBackendService.Service.Net
{
    public abstract class BaseIpUtilService : IIpUtilService
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        protected BaseIpUtilService()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public string GetIPAddress()
        {
            string ipAddress = GetIPFromSVHeader();

            if (ipAddress.IsNullOrEmpty())
            {
                ipAddress = GetIPFromWebHeader();
            }

            return ipAddress;
        }

        protected abstract string GetIPFromWebHeader();

        protected abstract string GetIPFromSVHeader();
    }
}