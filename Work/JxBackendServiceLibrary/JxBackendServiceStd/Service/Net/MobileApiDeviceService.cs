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

        /// <summary>
        /// 取得APP版本
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return _operationContextService.GetVersion();
        }

        /// <summary>
        /// 取得手机型号
        /// </summary>
        /// <returns></returns>
        public string GetDevice()
        {
            return _operationContextService.GetDevice();
        }

        /// <summary>
        /// 取得系统版本
        /// </summary>
        /// <returns></returns>
        public string GetSysVer()
        {
            return _operationContextService.GetSysVer();
        }
    }
}