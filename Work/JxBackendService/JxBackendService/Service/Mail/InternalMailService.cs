using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param;
using JxBackendService.Model.Param.Mail;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace JxBackendService.Service.Mail

{
    public class InternalMailService : IMailService
    {
        private static readonly int _apiTypeParam = 2;
        private static readonly string _mailServerApiRule = "{0}?address={1}&message={2}&title={3}&type={4}";

        public BaseReturnModel SendMail(SendMailParam sendMailParam)
        {
            try
            {
                string requestUrl = string.Format(_mailServerApiRule,
                    SharedAppSettings.InternalMailApi,
                    sendMailParam.MailTo,
                    HttpUtility.UrlEncode(sendMailParam.Body),
                    HttpUtility.UrlEncode(sendMailParam.Subject),
                    _apiTypeParam);

                InternalMailApiResponse response = HttpWebRequestUtil.GetResponse(nameof(SendMail), HttpMethod.Get, requestUrl).Deserialize<InternalMailApiResponse>();

                if (response.IsSuccess)
                {
                    return new BaseReturnModel(ReturnCode.Success);                    
                }

                LogUtil.ForcedDebug($"SendInterMail Response:{response.ToJsonString()}");
                return new BaseReturnModel(response.Message);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new BaseReturnModel(ex.Message);
            }
        }
    }
}
