using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Model.MiseLive.Request
{
    public interface IMiseLiveAppSettingService
    {
        string MSSealAddress { get; }

        string MSSealSalt { get; }
    }
}