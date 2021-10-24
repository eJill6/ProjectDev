using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Mail
{
    public class SendMailParam
    {
        /// <summary> 要寄送到的Email </summary>
        public string MailTo { get; set; }

        /// <summary> 主題 </summary>
        public string Subject { get; set; }

        /// <summary> 內容 </summary>        
        public string Body { get; set; }

        public MailContentType ContentType { get; set; } = MailContentType.Html;
    }

    public class MailContentType : BaseStringValueModel<MailContentType>
    {
        private MailContentType() { }

        public static MailContentType Text = new MailContentType() { Value = "text/plain" };
        public static MailContentType Html = new MailContentType() { Value = "text/html" };
    }
}
