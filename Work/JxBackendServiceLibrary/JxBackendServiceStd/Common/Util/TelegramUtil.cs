using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace JxBackendService.Common.Util
{
    public class TelegramUtil
    {
        private static readonly int s_messageMaxLenght = 4000;

        private static readonly string s_apiUrlQueryMethodTemplate = "/bot{0}/{1}"; //token, methodName

        private static readonly List<string> s_networkErrorMessageKeywords = new List<string>()
        {
            "OperationCanceledException",
            "Cannot prepare for data connection",
            "WebException",
            "EndOfStreamException"
        };

        public static void SendMessageWithEnvInfoAsync(SendTelegramParam sendTelegramParam)
        {
            TelegramChatGroup platformTeamChatGroup = GetTelegramChatGroup(sendTelegramParam.EnvironmentUser.EnvironmentCode, sendTelegramParam.Message);

            SendMessageWithEnvInfoAsync(sendTelegramParam, platformTeamChatGroup);
        }

        public static void SendMessageWithEnvInfoAsync(SendTelegramParam sendTelegramParam, TelegramChatGroup telegramChatGroup)
        {
            sendTelegramParam.Message = ErrorMsgUtil.GetErrorMsgWithEnvironmentInfo(sendTelegramParam.Message, sendTelegramParam.EnvironmentUser);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //這地方不可呼叫JxTask, 會造成循環呼叫
            Task.Factory.StartNew(() =>
            {
                SendMessage(sendTelegramParam, telegramChatGroup);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public static TelegramSendMessageResponse SendMessage(SendTelegramParam sendTelegramParam)
        {
            TelegramChatGroup commonChatGroup = GetTelegramChatGroup(sendTelegramParam.EnvironmentUser.EnvironmentCode, sendTelegramParam.Message);

            return SendMessage(sendTelegramParam, commonChatGroup);
        }

        public static TelegramSendMessageResponse SendMessage(SendTelegramParam sendTelegramParam, TelegramChatGroup telegramChatGroup)
        {
            TelegramSendMessageResponse response = null;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string requestUrl = sendTelegramParam.ApiUrl.TrimEnd('/') + string.Format(s_apiUrlQueryMethodTemplate, telegramChatGroup.Token, "sendMessage");

                response = HttpWebRequestUtil.GetResponse(
                    nameof(SendMessageWithEnvInfoAsync),
                    HttpMethod.Post,
                    requestUrl,
                    new TelegramSendMessageRequest()
                    {
                        Chat_id = telegramChatGroup.ChatId,
                        Text = sendTelegramParam.Message.ToShortString(s_messageMaxLenght)
                    }.ToJsonString(isCamelCaseNaming: true))
                    .Deserialize<TelegramSendMessageResponse>();
            }
            catch (Exception ex)
            {
                var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                logUtilService.Error(ex);
            }

            return response;
        }

        private static TelegramChatGroup GetTelegramChatGroup(EnvironmentCode environmentCode, string message)
        {
            List<TelegramChatGroup> telegramChatGroups = TelegramChatGroup.GetAll()
                .Where(w => w.AllowEnvironmentCodes.Contains(environmentCode))
                .ToList();

            if (IsNetworkErrorMessage(message))
            {
                return telegramChatGroups.Where(w => w.TelegramChatType == TelegramChatTypes.Network).Single();
            }

            return telegramChatGroups.Where(w => w.TelegramChatType == TelegramChatTypes.Common).Single();
        }

        private static bool IsNetworkErrorMessage(string message)
        {
            return s_networkErrorMessageKeywords.Any(keyword => message.IndexOf(keyword) >= 0);
        }

        //public static async Task<string> SendMessageAsync(TelegramChatTypes chatType, string message)
        //{
        //    ITelegramBotClient telegramBotClient = CreateTelegramBotClient();
        //    try
        //    {
        //        ChatId chatId = new ChatId(TelegramChatTypes.GetChatId(chatType));
        //        await telegramBotClient.SendTextMessageAsync(chatId, message.ToShortString(_messageMaxLenght));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //}

        //private static ITelegramBotClient CreateTelegramBotClient(string token)
        //{
        //    return new TelegramBotClient(token);
        //}

        //private static ITelegramBotClient CreateTelegramBotClient()
        //{
        //    return CreateTelegramBotClient(_token);
        //}
    }
}