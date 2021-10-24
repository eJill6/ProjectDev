using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.ServiceProcess;
using UnitTest.Base;

namespace UnitTest.TransferTest
{
    [TestClass]
    public class MainTest : BaseTest
    {
        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.FrontSideWeb,
            LoginUser = new BasicUserInfo
            {
                UserId = 69778,
                UserName = "jackson"
            }
        };

        public MainTest()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            DependencyUtil.SetContainer(builder.Build());
        }

        [TestMethod]
        public void TestIMSGTransferScheduleService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new IMSGTransferScheduleMockService()
            };
            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
        }

        [TestMethod]
        public void TestIMVRTransferScheduleService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new IMVRTransferScheduleMockService()
            };
            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
        }

        [TestMethod]
        public void TestABEBTransferScheduleService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ABEBTransferScheduleMockService()
            };
            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
        }

        [TestMethod]
        public void TestSqliteInsertBetLog()
        {
            var sqliteRep = DependencyUtil.ResolveKeyed<ITransferSqlLiteRepository>(PlatformProduct.IMSG, SharedAppSettings.PlatformMerchant);
            sqliteRep.TryCreateDataBase();
            sqliteRep.TryCreateTableProfitLoss();
            sqliteRep.TryCreateTableLastSearchToken();

            string apiResult = @"{""Code"": ""0"",""Message"": ""Successful."",""Result"": [{""Provider"":""VR_LOTTERY"",""GameId"":""imlotto20001"",""GameName"":""Venus 1.5 Lottery"",""ChineseGameName"":""VR 金星 1.5 分彩"",
""GameNo"":""20210112264"",""GameNoId"":""1"",""PlayerId"":""GOGSadmintest"",""ProviderPlayerId"":""IM0TSGOGSadmintest"",""Currency"":""CNY"",""Tray"":""1980"",
""BetId"":""10112153436908480451"",""BetOn"":""五星总和大小单双"",""BetType"":""Position=万,千,百,十,个;Number=总和大;"",
""BetDetails"":""WinningNumber=8,0,5,6,6;Award=五星总和大小单双;PrizeNumber=总和大;Unit=1.00000;Multiple=1;Count=1;"",
""Odds"":""1.97"",""BetAmount"":1,""ValidBet"":1,""WinLoss"":0.97,""PlayerWinLoss"":0.97,""LossPrize"":0,""Tips"":0,
""CommissionRate"":0,""Commission"":0,""Status"":""Settled"",""Platform"":""NA"",""BetDate"":""2021-01-12 15:34:36 +08:00"",
""ResultDate"":""2021-01-12 15:36:43 +08:00"",""SettlementDate"":""2021-01-12 15:36:43 +08:00"",""ReportingDate"":""2021-01-12 15:36:43 +08:00"",
""DateCreated"":""2021-01-12 15:36:15 +08:00"",""LastUpdatedDate"":""2021-01-12 15:42:10 +08:00""}]}";

            var responseModel = apiResult.Deserialize<IMLotteryBetLogResponseModel>();
            sqliteRep.SaveProfitloss(responseModel.Result);
            var list = sqliteRep.GetBatchProfitlossNotSavedToRemote<IMLotteryBetLog>();
            sqliteRep.SaveProfitlossToPlatformFail(list.First().KeyId);
            sqliteRep.SaveProfitlossToPlatformSuccess(list.First().KeyId);
        }


        [TestMethod]
        public void TestMQ()
        {
            IMessageQueueService messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.PGSLTransferService);
            messageQueueService.SendTransferMessage(69778, 1, "test");
        }

        [TestMethod]
        public void TestLocalizationUtil()
        {
            Type type = typeof(DBContentElement);
            var cc = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == typeof(DBContentElement).FullName).FirstOrDefault();
            var dd = AppDomain.CurrentDomain.GetAssemblies().Where(w => w.ManifestModule.Name == type.Module.Name).SelectMany(s => s.GetTypes());
            var ee = dd.Where(w => w.FullName == type.FullName).First();

            var qq = AppDomain.CurrentDomain.GetAssemblies()
                .Where(w => w.ManifestModule.Name == type.Module.Name && w.GetTypes().Where(ww => ww.FullName == type.FullName).Any())
                .GetType();

            //string content = @"{""LocalizationSentences"":[{""ResourcePropertyName"":""VIPLevelDownLogMemo"",""Args"":[""VIP9"",""VIP8""]}]}";

            string testContent = @"
{
  ""SplitOperator"": ""||"",
  ""LocalizationSentences"": [
	{
	  ""ResourceName"": ""AA"",
	  ""ResourcePropertyName"": ""TheUserHasUnbindedAuthenticator"",
	  ""Args"": [
		""1"",
	  ]
	},
	{
	  ""ResourceName"": ""JxBackendService.Resource.Element.SecurityElement"",
	  ""ResourcePropertyName"": ""this is a key"",
	  ""Args"": null
	}
  ]
}";

            string result = LocalizationUtil.ToLocalizationContent(testContent);

            ResourceManager resourceManager = new ResourceManager(typeof(DBContentElement).FullName, typeof(DBContentElement).Assembly);
            string tt = resourceManager.GetString("VIPLevelDownLogMemo");

        }
    }
}