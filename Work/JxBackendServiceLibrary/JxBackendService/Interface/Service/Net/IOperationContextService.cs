﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Net
{
    public interface IOperationContextService
    {
        string GetDevice();

        string GetDeviceCode();

        string GetFirstNotPrivateIP();

        string GetIp();

        string GetLocalDomain();

        string GetOSName();

        string GetSysVer();

        string GetUserKey();

        string GetUserName();

        string GetVersion();

        string GetVersionCode();
    }
}