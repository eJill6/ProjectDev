using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ErrorHandle;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnitTestN6;

namespace UnitTest.TelegramTest
{
    [TestClass]
    public class ApiTest : BaseUnitTest
    {
        [TestMethod]
        public void SendMessageTest()
        {
            foreach (TelegramChatGroup telegramChatGroup in TelegramChatGroup.GetAll().Where(w => w.TelegramChatType == TelegramChatTypes.PDTeam))
            {
                TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    EnvironmentUser = EnvironmentUser,
                    Message = $"測試 {telegramChatGroup.Value} 發送訊息" + DateTime.Now.ToString()
                }, telegramChatGroup);

                Task.Delay(2000).Wait();
            }
        }

        [TestMethod]
        public void ErrorHandleTest()
        {
            Exception ex = new Exception("http://192.168.104.70/mwt/4F5663F1BBD97745079FD3DE0FDD48D43A48E32DF3485E8853F1BF8ACFF68BB5CC3CFAB9B0D2B8E0750D01D8777BFFF4509C0D00D709C6E1ECF28CF471BE9055/MM/Index");
            ErrorMsgUtil.ErrorHandle(ex, EnvironmentUser);

            System.Threading.Thread.Sleep(2000);
        }

        [TestMethod]
        public void SendMessageRateTest()
        {
            TelegramChatGroup telegramChatGroup = TelegramChatGroup.Testing;
            int seq = 1;

            while (true)
            {
                TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    EnvironmentUser = EnvironmentUser,
                    Message = $"測試發送訊息 seq={seq}, time={DateTime.Now.ToFormatDateTimeMillisecondsString()}"
                }, telegramChatGroup);

                seq++;

                Task.Delay(500).Wait();
            }
        }
    }
}