using Flurl;
using JxBackendService.Common;
using JxBackendService.Common.Extensions;
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
    public class MxtongInternationalSMSService : ISMSService
    {
        private static readonly string s_sendSMSSingle = "/api/send-sms-single";

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        public MxtongInternationalSMSService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public BaseReturnModel SendSMS(SendUserSMSParam sendUserSMSParam)
        {
            MxtongSMSSetting mxtongSMSSetting = _configUtilService.Value.Get<MxtongSMSSetting>("SMSSetting:Mxtong.International");

            var request = new MxtongInternationalRequest
            {
                Sp_id = mxtongSMSSetting.Account,
                Mobile = $"+{sendUserSMSParam.CountryCode}{sendUserSMSParam.PhoneNo}",
                Password = MD5Tool.ToMD5(mxtongSMSSetting.Password, isToUpper: false),
                Content = string.Format(mxtongSMSSetting.TemplateInfo, sendUserSMSParam.ContentParamInfo)
            };

            string fullUrl = Url.Combine(mxtongSMSSetting.ServiceUrl, s_sendSMSSingle);
            string response = DoRequest(fullUrl, request.ToKeyValueURL(isCamelCase: true));

            var smsResult = response.Deserialize<MxtongInternationalSendSMSResult>();

            if (!smsResult.IsSuccess)
            {
                string errorMsg = $"{GetType().Name}.SendSMS Error Msg = {smsResult.Msg}";

                _logUtilService.Value.Error(errorMsg);

                return new BaseReturnModel(errorMsg);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string DoRequest(string fullUrl, string requestBody)
        {
            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

            string response = httpWebRequestUtilService.GetResponse(new WebRequestParam()
            {
                Purpose = $"{GetType().Name}.SendSMS",
                Method = HttpMethod.Post,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                Url = fullUrl,
                Body = requestBody,
                IsResponseValidJson = true
            }, out HttpStatusCode httpStatusCode);

            return response;
        }
    }
}