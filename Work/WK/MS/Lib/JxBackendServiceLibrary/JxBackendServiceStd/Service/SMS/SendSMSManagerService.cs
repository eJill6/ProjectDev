using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.SMS
{
    public class SendSMSManagerService : BaseEnvLoginUserService, ISendSMSManagerService
    {
        private static SMSServiceProvider _lastSuccessProvider;

        public SendSMSManagerService(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        public BaseReturnDataModel<ServiceProviderInfo> SendSMS(SendUserSMSParam sendUserSMSParam)
        {
            BaseReturnModel finalSendResult = null;

            //每次選用不同的provider, 失敗後找下一家,直到成功或者全部都試過
            List<SMSServiceProvider> allProviders = SMSServiceProvider.GetAll();

            if (!allProviders.Any())
            {
                return new BaseReturnDataModel<ServiceProviderInfo>($"{nameof(SMSServiceProvider)} is empty");
            }

            if (_lastSuccessProvider == null)
            {
                _lastSuccessProvider = allProviders.Last();
            }

            //[1,2,3,4,5] 若last為3，下次執行順序為4,5,1,2,3
            var serviceProviderQueue = new Queue<SMSServiceProvider>();
            allProviders.Where(w => w.Sort > _lastSuccessProvider.Sort).ToList().ForEach(p => serviceProviderQueue.Enqueue(p));
            allProviders.Where(w => w.Sort <= _lastSuccessProvider.Sort).ToList().ForEach(p => serviceProviderQueue.Enqueue(p));

            while (serviceProviderQueue.Any())
            {
                SMSServiceProvider smsServiceProvider = serviceProviderQueue.Dequeue();
                finalSendResult = DoSend(smsServiceProvider, sendUserSMSParam);

                if (finalSendResult.IsSuccess)
                {
                    if (_lastSuccessProvider.Value != smsServiceProvider.Value)
                    {
                        _lastSuccessProvider = smsServiceProvider;
                    }

                    break;
                }
            }

            if (finalSendResult != null && finalSendResult.IsSuccess)
            {
                var serviceProviderInfo = new ServiceProviderInfo()
                {
                    SendServiceName = nameof(SendSMSManagerService),
                    ProviderID = _lastSuccessProvider.Value,
                };

                return new BaseReturnDataModel<ServiceProviderInfo>(ReturnCode.Success, serviceProviderInfo);
            }

            //不回傳實際錯誤訊息
            return new BaseReturnDataModel<ServiceProviderInfo>(MessageElement.OperationFail);
        }

        protected virtual BaseReturnModel DoSend(SMSServiceProvider smsServiceProvider, SendUserSMSParam sendUserSMSParam)
        {
            ISMSService smsService = DependencyUtil.ResolveKeyed<ISMSService>(smsServiceProvider).Value;

            BaseReturnModel returnModel = null;
            string requestContent = $"簡訊商:{smsServiceProvider}, Param:{sendUserSMSParam.ToJsonString()}";

            try
            {
                returnModel = smsService.SendSMS(sendUserSMSParam);
            }
            catch (Exception ex)
            {
                //ignore;
                returnModel = new BaseReturnModel(ex.Message);
            }
            finally
            {
                if (!returnModel.IsSuccess)
                {
                    ErrorMsgUtil.ErrorHandle(
                        $"{requestContent}, ErrorMessage:{returnModel.Message}",
                        EnvLoginUser,
                        isSendMessageToTelegram: true);
                }
            }

            return returnModel;
        }
    }

    [MockService]
    public class SendSMSManagerMockService : SendSMSManagerService
    {
        private static readonly string _mockPhoneNoSuffix = "99999";

        public SendSMSManagerMockService(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override BaseReturnModel DoSend(SMSServiceProvider smsServiceProvider, SendUserSMSParam sendUserSMSParam)
        {
            if (sendUserSMSParam.PhoneNo.ToTrimString().EndsWith(_mockPhoneNoSuffix))
            {
                return SendToMockPhone(sendUserSMSParam);
            }

            return base.DoSend(smsServiceProvider, sendUserSMSParam);
        }

        private BaseReturnModel SendToMockPhone(SendUserSMSParam sendUserSMSParam)
        {
            var sendTelegramParam = new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = EnvLoginUser,
                Message = sendUserSMSParam.ToJsonString()
            };

            LogUtilService.ForcedDebug($"{new { sendTelegramParam.EnvironmentUser }.ToJsonString()}, {sendTelegramParam.Message}");

            //發送Telegram訊息，不管有無發送成功，都要回Success
            TelegramUtil.SendMessageWithEnvInfoAsync(sendTelegramParam);

            return new BaseReturnModel(ReturnCode.Success);
        }
    }
}