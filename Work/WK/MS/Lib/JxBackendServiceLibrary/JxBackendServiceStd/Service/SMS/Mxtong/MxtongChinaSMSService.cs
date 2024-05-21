using Flurl;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.Param.SMS.ThirdParty.Mxtong;
using JxBackendService.Model.ReturnModel;
using System;
using System.Net;

namespace JxBackendService.Service.SMS.Mxtong
{
    public class MxtongChinaSMSService : ISMSService
    {
        private static readonly string s_batchSendSMS = "/msg/HttpBatchSendSM";

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        public MxtongChinaSMSService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public BaseReturnModel SendSMS(SendUserSMSParam sendUserSMSParam)
        {
            MxtongSMSSetting mxtongSMSSetting = _configUtilService.Value.Get<MxtongSMSSetting>("SMSSetting:Mxtong.China");

            string requestContent = GetRequestContent(sendUserSMSParam, mxtongSMSSetting);
            string requestUrl = Url.Combine(mxtongSMSSetting.ServiceUrl, s_batchSendSMS);
            string response = DoRequest(requestUrl, requestContent);

            var sendSMSResult = response.Deserialize<MxtongChinaSendSMSResult>();

            if (!sendSMSResult.IsSuccess)
            {
                string errorMsg = $"{GetType().Name}.SendSMS Error result = {sendSMSResult.Result}";
                errorMsg = GerErrorDescription(errorMsg, sendSMSResult.Result);

                _logUtilService.Value.Error(errorMsg);

                return new BaseReturnModel(errorMsg);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string GerErrorDescription(string errorMsg, int? responseCode)
        {
            if (responseCode.HasValue)
            {
                string errorDescription = MxtongSMSErrorCode.GetName(responseCode.Value);

                if (!errorDescription.IsNullOrEmpty())
                {
                    errorMsg += $"({errorDescription})";
                }
            }

            return errorMsg;
        }

        protected virtual string DoRequest(string requestUrl, string requestContent)
        {
            string fullUrl = Url.Combine(requestUrl, requestContent);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();

            string response = httpWebRequestUtilService.Value.GetResponse(new WebRequestParam()
            {
                Purpose = $"{GetType().Name}.SendSMS",
                Method = HttpMethod.Get,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                Url = fullUrl,
                IsResponseValidJson = true
            }, out HttpStatusCode httpStatusCode);

            return response;
        }

        private string GetMD5Password(string timestamp, MxtongSMSSetting mxtongSMSSetting)
        {
            string sign = $"{mxtongSMSSetting.Account}{mxtongSMSSetting.Password}{timestamp}";
            string encryptPassword = MD5Tool.ToMD5(sign, isToUpper: true);

            return encryptPassword;
        }

        private string GetRequestContent(SendUserSMSParam sendUserSMSParam, MxtongSMSSetting mxtongSMSSetting)
        {
            string fullPhoneNo = string.Empty;

            if (sendUserSMSParam.Usage != SMSUsages.ValidateCode)
            {
                throw new NotImplementedException();
            }
            else
            {
                fullPhoneNo = sendUserSMSParam.PhoneNo;
            }

            string sendMessage = $"{string.Format(mxtongSMSSetting.TemplateInfo, sendUserSMSParam.ContentParamInfo)}";

            string timestamp = DateTime.Now.ToFormatDateTimeStringWithoutSymbol();
            string queryString = $"?account={mxtongSMSSetting.Account}&ts={timestamp}&pswd={GetMD5Password(timestamp, mxtongSMSSetting)}" +
                $"&mobile={fullPhoneNo}&msg={sendMessage}&needstatus=true&resptype=json";

            return queryString;
        }
    }
}