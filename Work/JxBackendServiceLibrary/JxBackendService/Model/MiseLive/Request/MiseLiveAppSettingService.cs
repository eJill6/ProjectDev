using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.MiseLive.Request;
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
        public string MSSealAddress => ConfigUtil.Get("MSSealAddress");

        public string MSSealSalt => ConfigUtil.Get("MSSealSalt");
    }
}