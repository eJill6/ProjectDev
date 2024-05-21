using System;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Interface.Service.Config;

namespace JxBackendService.Model.MiseLive.Request
{
    public class MiseLiveAppSettingService : IMiseLiveAppSettingService
    {
        private readonly Lazy<IConfigUtilService> _configUtilService;

        public MiseLiveAppSettingService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }

        public string MSSealAddress => _configUtilService.Value.Get("MSSealAddress");

        public string MSSealSalt => _configUtilService.Value.Get("MSSealSalt");
    }
}