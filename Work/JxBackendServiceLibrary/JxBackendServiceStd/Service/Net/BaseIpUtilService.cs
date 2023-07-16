using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Service.Net
{
    public abstract class BaseIpUtilService : IIpUtilService
    {
        protected ILogUtilService LogUtilService { get; private set; }

        protected BaseIpUtilService()
        {
            LogUtilService = DependencyUtil.ResolveService<ILogUtilService>();
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