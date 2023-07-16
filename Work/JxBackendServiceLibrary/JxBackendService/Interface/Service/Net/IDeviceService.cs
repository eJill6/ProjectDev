using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Net
{
    public interface IDeviceService
    {
        string GetDevice();

        string GetDeviceName();

        string GetSysVer();

        string GetVersion();
    }
}