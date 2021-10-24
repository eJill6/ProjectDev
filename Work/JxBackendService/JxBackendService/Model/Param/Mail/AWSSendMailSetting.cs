using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace JxBackendService.Model.Param.Mail
{
    public class AWSSendMailSetting
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string SenderAddress { get; set; }

        public string SenderName { get; set; }
    }
}
