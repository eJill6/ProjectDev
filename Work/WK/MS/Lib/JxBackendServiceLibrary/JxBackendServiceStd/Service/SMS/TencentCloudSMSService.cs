using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.Param.SMS.ThirdParty;
using JxBackendService.Model.ReturnModel;
using System;
using System.Linq;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;

namespace JxBackendService.Service.SMS
{
    public class TencentCloudSMSService : ISMSService
    {
        private static readonly string s_apiSuccessCode = "Ok";

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        public TencentCloudSMSService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public BaseReturnModel SendSMS(SendUserSMSParam sendUserSMSParam)
        {
            var tercentCloudSetting = _configUtilService.Value.Get<TercentCloudSetting>("SMSSetting:TercentCloud");

            Credential credential = new Credential()
            {
                SecretId = tercentCloudSetting.SecretId,
                SecretKey = tercentCloudSetting.SecretKey
            };

            /* 非必要步骤:
             * 实例化一个客户端配置对象，可以指定超时时间等配置 */
            var clientProfile = new ClientProfile();
            clientProfile.HttpProfile.Endpoint = tercentCloudSetting.Endpoint;

            var smsClient = new SmsClient(credential, region: "ap-guangzhou", clientProfile);

            /* 实例化一个请求对象，根据调用的接口和实际情况，可以进一步设置请求参数
             * 您可以直接查询SDK源码确定SendSmsRequest有哪些属性可以设置
             * 属性可能是基本类型，也可能引用了另一个数据结构
             * 推荐使用IDE进行开发，可以方便的跳转查阅各个接口和数据结构的文档说明 */
            var sendSmsRequest = new SendSmsRequest()
            {
                SmsSdkAppId = tercentCloudSetting.AppID,
                SignName = tercentCloudSetting.SignName,
                PhoneNumberSet = new string[] { $"+{sendUserSMSParam.CountryCode}{sendUserSMSParam.PhoneNo}" },
            };

            SetRequestContent(sendUserSMSParam, tercentCloudSetting, sendSmsRequest);

            SendSmsResponse sendSmsResponse = smsClient.SendSmsSync(sendSmsRequest);

            if (!sendSmsResponse.SendStatusSet.AnyAndNotNull())
            {
                return new BaseReturnModel("SendStatusSet not AnyAndNotNull");
            }

            //單獨發送1筆，應該只會有一個結果回傳
            SendStatus sendStatus = sendSmsResponse.SendStatusSet.Single();

            if (sendStatus.Code != s_apiSuccessCode)
            {
                _logUtilService.Value.Error(sendStatus.ToJsonString());

                return new BaseReturnModel($"{sendStatus.Code}:{sendStatus.Message}");
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private void SetRequestContent(SendUserSMSParam sendUserSMSParam, TercentCloudSetting tercentCloudSetting, SendSmsRequest sendSmsRequest)
        {
            switch (sendUserSMSParam.Usage)
            {
                case SMSUsages.ValidateCode:
                    sendSmsRequest.TemplateParamSet = new string[] { sendUserSMSParam.ContentParamInfo };

                    if (sendUserSMSParam.CountryCode == CountryCode.China)
                    {
                        sendSmsRequest.TemplateId = tercentCloudSetting.ValidateCodeTemplate.ChinaTemplateInfo;
                    }
                    else
                    {
                        sendSmsRequest.TemplateId = tercentCloudSetting.ValidateCodeTemplate.InternationalTemplateInfo;
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}