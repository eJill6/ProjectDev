using JxBackendService.Model.Enums.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.Enums.User
{
    public class LogonMode : BaseIntValueModel<LogonMode>
    {
        public bool IsAllowFullScreen { get; set; }

        private LogonMode()
        { }

        public static LogonMode Native = new LogonMode()
        {
            Value = 0,
            IsAllowFullScreen = true,
        };

        public static LogonMode Lite = new LogonMode()
        {
            Value = 1,
            IsAllowFullScreen = false,
        };
    }
}