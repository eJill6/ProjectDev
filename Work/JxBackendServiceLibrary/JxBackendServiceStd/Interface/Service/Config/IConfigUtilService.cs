﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Service.Config
{
    public interface IConfigUtilService
    {
        string Get(string key, string defaultValue = null);

        T Get<T>(string section);
    }
}