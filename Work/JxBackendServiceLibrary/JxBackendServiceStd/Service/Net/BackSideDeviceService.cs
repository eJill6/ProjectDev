using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Service.Setting
{
    public class BackSideDeviceService : IDeviceService
    {
        public string GetDeviceName() => JxApplication.BackSideWeb.Value;

        public string GetDevice()
        {
            //不適用
            throw new NotImplementedException();
        }

        public string GetSysVer()
        {
            //不適用
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            //不適用
            throw new NotImplementedException();
        }
    }
}