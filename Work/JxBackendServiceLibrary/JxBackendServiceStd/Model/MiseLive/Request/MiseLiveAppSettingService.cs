using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.MiseLive.Request
{
    public class MiseLiveAppSettingService : IMiseLiveAppSettingService
    {
        private readonly IConfigUtilService _configUtilService;

        public MiseLiveAppSettingService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }

        public string MSSealAddress => _configUtilService.Get("MSSealAddress");

        public string MSSealSalt => _configUtilService.Get("MSSealSalt");
    }
}