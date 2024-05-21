using JxBackendService.Interface.Service.Config;
using JxBackendServiceNF.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceNF.Service.Config
{
    public class ConfigUtilService : IConfigUtilService
    {
        public string Get(string key, string defaultValue)
        {
            return ConfigUtil.Get(key, defaultValue);
        }

        public T Get<T>(string section)
        {
            throw new NotSupportedException();
        }
    }
}