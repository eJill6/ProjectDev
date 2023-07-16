using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class TPGameRemoteLoginParam
    {
        public CreateRemoteAccountParam CreateRemoteAccountParam { get; set; }

        public string IpAddress { get; set; }

        public bool IsMobile { get; set; }

        public LoginInfo LoginInfo { get; set; }
    }
}