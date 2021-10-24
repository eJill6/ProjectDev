using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    public class MobileApiDeviceService : IDeviceService
    {
        private readonly IOperationContextService _operationContextService;

        public MobileApiDeviceService(IOperationContextService operationContextService)
        {
            _operationContextService = DependencyUtil.ResolveService<IOperationContextService>();
        }

        public string GetDeviceName()
        {
            string deviceName = "Mobile";
            string version = _operationContextService.GetVersion();

            if (!version.IsNullOrEmpty())
            {
                deviceName = version;
            }

            return deviceName;
        }
    }
}
