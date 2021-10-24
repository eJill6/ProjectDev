using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    public class BackSideDeviceService : IDeviceService
    {
        public string GetDeviceName() => JxApplication.BackSideWeb.Value;
    }
}
