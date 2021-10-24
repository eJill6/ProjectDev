using JxBackendService.Model.Param;
using JxBackendService.Model.Param.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class MailTemplate : BaseIntValueModel<MailTemplate>
    {
        private MailTemplate() { }

        /// <summary> 主題 </summary>
        public string Subject { get; private set; }
        /// <summary> 內容 </summary>
        public string Body { get; private set; }

        public static MailTemplate ChangePassword = new MailTemplate()
        {
            Value = 1,
            Subject = "-系统密码更改提醒-",
            Body = @"尊敬的会员：<br />
                   您的{0}密码已经由系统自动更改，请用新密码登录，并且及时更改密码。<br />
                   新的密码为：{1}。<br />
                   系统发送，无需回复。",
        };

        public static MailTemplate RetrievePasswordWithValidCode = new MailTemplate()
        {
            Value = 2,
            Subject = "-系统密码更改提醒-",
            Body = @"尊敬的会员：<br />
                   您稍早于平台使用邮箱找回密码，您的验证码是{0}<br />
                   <br />
                   如非本人操作，请忽略此邮件<br />
                   系统发送，无需回复。",
        };

        public static MailTemplate ChangeMoneyPwd = new MailTemplate()
        {
            Value = 3,
            Subject = "-系统密码更改提醒-",
            Body = @"尊敬的会员：<br />
                   资金密码重置成功，新密码为{0}<br />
                   请尽快登入修改<br />
                   <br />
                   如非本人操作，请忽略此邮件<br />
                   系统发送，无需回复。",
        };



        /// <summary>
        /// 取得 ChangePassword 的資訊
        /// </summary>
        public static SendMailParam GetChangePasswordMailTempParam(string mailAddress, string passwordType, string newPassword)
        {
            return new SendMailParam()
            {
                MailTo = mailAddress,
                Subject = ChangePassword.Subject,
                Body = string.Format(ChangePassword.Body, passwordType, newPassword)
            };
        }

        /// <summary>
        /// 取得 RetrievePasswordWithValidCode 的資訊
        /// </summary>
        public static SendMailParam GetRetrievePasswordWithValidCodeMailTempParam(string mailAddress, string randomNumber)
        {
            return new SendMailParam()
            {
                MailTo = mailAddress,
                Subject = RetrievePasswordWithValidCode.Subject,
                Body = string.Format(RetrievePasswordWithValidCode.Body, randomNumber)
            };
        }

        /// <summary>
        /// 取得 GetChangeMoneyPwdMailTempParam 的資訊
        /// </summary>
        public static SendMailParam GetChangeMoneyPwdMailTempParam(string mailAddress, string randomNumber)
        {
            return new SendMailParam()
            {
                MailTo = mailAddress,
                Subject = ChangeMoneyPwd.Subject,
                Body = string.Format(ChangeMoneyPwd.Body, randomNumber)
            };
        }
    }
}
