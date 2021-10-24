using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    public class FrontSideDeviceService : IDeviceService
    {
        public string GetDeviceName() => "NewWeb";

        //public string GetDeviceName() => JxApplication.BackSideWeb.Value;
    }
}
