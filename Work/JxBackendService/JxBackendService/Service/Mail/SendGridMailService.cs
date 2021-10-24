using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param;
using JxBackendService.Model.Param.Mail;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace JxBackendService.Service.Mail
{
    public class SendGridMailService : IMailService
    {
        public BaseReturnModel SendMail(SendMailParam sendMailParam)
        {
            var sendGridRequestParam = new SendGridRequestParam()
            {
                Subject = sendMailParam.Subject,
                From = new SendGridMailFrom() { Email = SharedAppSettings.SendGridSetting.FromMail },
                Personalizations = new List<Personalization>()
                {
                    new Personalization() { To = new List<SendGridMailTo>()
                    {
                        new SendGridMailTo() { Email = sendMailParam.MailTo } }
                    }
                },
                Content = new List<Content>() { new Content() { Type = sendMailParam.ContentType.Value, Value = sendMailParam.Body } }
            };

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {SharedAppSettings.SendGridSetting.Token}" }
            };

            string responseText = HttpWebRequestUtil.GetResponse(new WebRequestParam()
            {
                Url = SharedAppSettings.SendGridSetting.ApiUrl,
                ContentType = HttpWebRequestContentType.Json,
                Method = HttpMethod.Post,
                Headers = headers,
                Body = sendGridRequestParam.ToJsonString(),
                IsResponseValidJson = false,
                Purpose = "SendGridMailService.SendMail"
            }, out HttpStatusCode httpStatusCode);

            if (responseText.IsNullOrEmpty() && httpStatusCode == HttpStatusCode.Accepted)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                if (StringUtil.IsValidJson(responseText))
                {
                    var errorResponse = responseText.Deserialize<SendGridErrorResponse>();
                    return new BaseReturnModel(errorResponse.Errors.ToJsonString());
                }

                return new BaseReturnModel($"HttpStatusCode={(int)HttpStatusCode.OK}");
            }
        }
    }
}
