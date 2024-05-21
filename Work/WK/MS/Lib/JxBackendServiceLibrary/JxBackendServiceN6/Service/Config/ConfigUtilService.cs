using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JxBackendServiceN6.Service.Config
{
    public class ConfigUtilService : IConfigUtilService
    {
        private static readonly string s_defaultSection = "Default";

        private readonly Lazy<IConfiguration> _configuration;

        public ConfigUtilService()
        {
            _configuration = DependencyUtil.ResolveService<IConfiguration>();
        }

        public string Get(string key, string defaultValue)
        {
            string fullKey = $"{s_defaultSection}:{key}";
            string value = _configuration.Value.GetValue<string>(fullKey);

            if (value.IsNullOrEmpty())
            {
                return defaultValue;
            }

            return value;
        }

        public T Get<T>(string section)
        {
            return _configuration.Value.GetSection(section).Get<T>();
        }
    }
}