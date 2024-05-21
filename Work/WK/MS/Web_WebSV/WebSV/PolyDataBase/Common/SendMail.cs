using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SLPolyGame.Web.Common
{
    public static class SendMails
    {
        public static bool SendMail(string strSmtpServer, string strForm, string strFormPass, string strto, string strSubject, string strBody)
        {
            System.Net.Mail.SmtpClient clint = new SmtpClient(strSmtpServer);
            //gmail需要开启安全连接
            clint.EnableSsl = true;
            clint.UseDefaultCredentials = false;
            clint.Credentials = new System.Net.NetworkCredential(strForm, strFormPass);
            clint.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.Mail.MailMessage message = new MailMessage(strForm, strto, strSubject, strBody);
            System.Text.Encoding.GetEncoding("gb2312");
            message.SubjectEncoding = Encoding.GetEncoding("gb2312");
            message.BodyEncoding = Encoding.GetEncoding("gb2312");
            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;
            clint.Send(message);
            return true;
        }
    }
}
