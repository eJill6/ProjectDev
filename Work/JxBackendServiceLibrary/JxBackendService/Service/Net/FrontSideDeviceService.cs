using JxBackendService.Interface.Service.Net;
using System;

namespace JxBackendService.Service.Setting
{
    public class FrontSideDeviceService : IDeviceService
    {
        public string GetDeviceName() => "NewWeb";

        //public string GetDeviceName() => JxApplication.BackSideWeb.Value;
        public string GetDevice()
        {
            //不適用
            throw new PlatformNotSupportedException();
        }

        public string GetSysVer()
        {
            //不適用
            throw new PlatformNotSupportedException();
        }

        public string GetVersion()
        {
            //不適用
            throw new PlatformNotSupportedException();
        }
    }
}