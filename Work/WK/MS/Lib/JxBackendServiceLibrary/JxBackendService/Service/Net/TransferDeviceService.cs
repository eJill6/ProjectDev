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
    public class TransferDeviceService : IDeviceService
    {
        public string GetDeviceName() => "TransferService";

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