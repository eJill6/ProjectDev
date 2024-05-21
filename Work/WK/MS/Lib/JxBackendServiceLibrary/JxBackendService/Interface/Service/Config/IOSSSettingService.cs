using JxBackendService.Model.Param.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Service.Config
{
    public interface IOSSSettingService
    {
        IOSSSetting GetOSSClientSetting();

        IOSSSetting GetCoreOSSClientSetting();
    }
}