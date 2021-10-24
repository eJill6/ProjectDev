using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param;
using JxBackendService.Model.Param.Mail;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace JxBackendService.Service.Mail
{
    public class AWSSendMailService : IMailService
    {
        private static readonly string _smtpUsername = SharedAppSettings.AWSSendMailSetting.Username;
        private static readonly string _smtpPassword = SharedAppSettings.AWSSendMailSetting.Password;
        private static readonly string _smtpHost = SharedAppSettings.AWSSendMailSetting.Host;
        private static readonly string _smtpPort = SharedAppSettings.AWSSendMailSetting.Port;
        private static readonly string _senderAddress = SharedAppSettings.AWSSendMailSetting.SenderAddress;
        private static readonly string _senderName = SharedAppSettings.AWSSendMailSetting.SenderName;

        public BaseReturnModel SendMail(SendMailParam sendMailParam)
        {
            // 先確定設定檔符合預期
            if (string.IsNullOrEmpty(_smtpUsername) || string.IsNullOrEmpty(_smtpPassword) ||   // 設定檔缺失
                string.IsNullOrEmpty(_smtpHost) || string.IsNullOrEmpty(_smtpPort) ||           // 設定檔缺失
                string.IsNullOrEmpty(_senderAddress) || string.IsNullOrEmpty(_senderName) ||    // 設定檔缺失
                !Int32.TryParse(_smtpPort, out int portNumber) ||                               // 設定檔 Port 不是數字
                portNumber < IPEndPoint.MinPort || portNumber > IPEndPoint.MaxPort)             // 設定檔 Port 值不合理
            {
                throw new Exception(MessageElement.ConfigInvalid);
            }

            // 建立和設定郵件內容
            MailMessage message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(_senderAddress, _senderName),
                Subject = sendMailParam.Subject,
                Body = sendMailParam.Body,
            };

            message.To.Add(new MailAddress(sendMailParam.MailTo));

            // 使用SMTP Client 發送郵件
            using (var smtpClient = new System.Net.Mail.SmtpClient(_smtpHost, portNumber))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

                smtpClient.EnableSsl = true;

                smtpClient.Send(message);

                // 因為Send不會回傳狀態, 無法得知結果
                // 如果發送過程有失敗, 會由外層的try catch去handle

                return new BaseReturnModel(ReturnCode.Success);
            }
        }
    }
}
