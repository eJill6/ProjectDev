using JxBackendService.Model;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Telegram;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace JxBackendService.Common.Util
{
    public class TelegramUtil
    {
        private static readonly string _token = "669615436:AAEBSaVKBBxDT6zrOUqaNOEM_WNQx8Fg3lE";
        private static readonly int _messageMaxLenght = 4000;
        private static readonly string _apiUrlQueryMethodTemplate = "/bot{0}/{1}"; //token, methodName

        public static void SendMessageWithEnvInfoAsync(SendTelegramParam sendTelegramParam)
        {
            sendTelegramParam.Message = ErrorMsgUtil.GetErrorMsgWithEnvironmentInfo(sendTelegramParam.Message, sendTelegramParam.EnvironmentUser);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            //這地方不可呼叫JxTask, 會造成循環呼叫
            Task.Factory.StartNew(() =>
            {
                SendMessage(sendTelegramParam);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public static TelegramSendMessageResponse SendMessage(SendTelegramParam sendTelegramParam)
        {
            TelegramSendMessageResponse response = null;

            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string requestUrl = sendTelegramParam.ApiUrl.TrimEnd('/') + string.Format(_apiUrlQueryMethodTemplate, _token, "sendMessage");
                response = HttpWebRequestUtil.GetResponse(
                    nameof(SendMessageWithEnvInfoAsync),
                    HttpMethod.Post,
                    requestUrl,
                    new
                    {
                        chat_id = GetTelegramChatGroup(sendTelegramParam.EnvironmentUser.EnvironmentCode).Value,
                        text = sendTelegramParam.Message.ToShortString(_messageMaxLenght)
                    }).Deserialize<TelegramSendMessageResponse>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }

            return response;
        }

        private static TelegramChatGroup GetTelegramChatGroup(EnvironmentCode environmentCode)
        {
            if (environmentCode == EnvironmentCode.Production)
            {
                return TelegramChatGroup.Production;
            }
            else
            {
                return TelegramChatGroup.Testing;
            }
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
