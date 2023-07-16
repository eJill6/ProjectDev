using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Common.Util
{
    public class ConfigUtil
    {
        public static string Get(string name, string defaultValue = null)
        {
            if (name.IsNullOrEmpty())
            {
                return string.Empty;
            }

            string value = ConfigurationManager.AppSettings[name].ToTrimString();

            if (value.IsNullOrEmpty() && !defaultValue.IsNullOrEmpty())
            {
                value = defaultValue;
            }

            return value;
        }
    }
}