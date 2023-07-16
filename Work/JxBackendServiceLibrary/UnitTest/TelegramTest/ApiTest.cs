using JxBackendService.Common.Util;
using JxBackendService.Model;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnitTest.Base;

namespace UnitTest.TelegramTest
{
    [TestClass]
    public class ApiTest : BaseTest
    {
        [TestMethod]
        public void SendMessageTest()
        {
            foreach (TelegramChatGroup telegramChatGroup in TelegramChatGroup.GetAll())
            {
                TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    EnvironmentUser = new EnvironmentUser()
                    {
                        Application = JxApplication.FrontSideWeb,
                        LoginUser = EnvLoginUser.LoginUser,
                    },
                    Message = $"測試 {telegramChatGroup.Value} 發送訊息" + DateTime.Now.ToString()
                }, telegramChatGroup);

                Task.Delay(2000).Wait();
            }
        }

        [TestMethod]
        public void ErrorHandleTest()
        {
            Exception ex = new Exception("test exception");
            ErrorMsgUtil.ErrorHandle(ex, new EnvironmentUser()
            {
                Application = JxApplication.FrontSideWeb,
                LoginUser = EnvLoginUser.LoginUser,
            });

            System.Threading.Thread.Sleep(2000);
        }
    }
}