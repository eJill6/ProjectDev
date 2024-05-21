using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.GlobalSystem
{
    public class SendTelegramMessageService : BaseService, ISendTelegramMessageService
    {
        public SendTelegramMessageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        /// <summary>發送訊息給客服群</summary>
        public void SendToCustomerService(BasicUserInfo user, string message)
        {
            bool isTestingEnvironment = SharedAppSettings.GetEnvironmentCode().IsTestingEnvironment;
            TelegramChatGroup telegramChatGroup = TelegramChatGroup.CustomerServiceProduction;

            if (isTestingEnvironment)
            {
                telegramChatGroup = TelegramChatGroup.CustomerServiceTesting;
            }

            TelegramUtil.SendMessageWithEnvInfoAsync(
                new SendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    EnvironmentUser = new EnvironmentUser()
                    {
                        Application = EnvLoginUser.Application,
                        LoginUser = user
                    },
                    Message = message
                },
                telegramChatGroup);
        }
    }
}