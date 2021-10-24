using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Mail
{
    public class SendGridErrorResponse
    {
        public List<SendGridErrorInfo> Errors { get; set; }
    }

    public class SendGridErrorInfo
    {
        public string Message { get; set; }
        public string Field { get; set; }
        public object Help { get; set; }
    }

    public class SendGridSetting
    {
        public string ApiUrl { get; set; }
        
        public string Token { get; set; }
        
        public string FromMail { get; set; }
    }

    public class SendGridMailTo
    {
        public string Email { get; set; }
    }

    public class Personalization
    {
        public List<SendGridMailTo> To { get; set; }
    }

    public class SendGridMailFrom
    {
        public string Email { get; set; }
    }

    public class Content
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class SendGridRequestParam
    {
        public List<Personalization> Personalizations { get; set; }
        public SendGridMailFrom From { get; set; }
        public string Subject { get; set; }
        public List<Content> Content { get; set; }
    }
}
