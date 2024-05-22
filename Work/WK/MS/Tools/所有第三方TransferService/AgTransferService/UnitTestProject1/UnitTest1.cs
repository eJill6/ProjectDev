using AgDataBase.DLL;
using AgDataBase.Model;
using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using UnitTestProject;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private static FileSystemWatcher configWatcher = new FileSystemWatcher()
        {
            Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            NotifyFilter = NotifyFilters.LastWrite
        };

        private readonly ITPGameApiService _tpGameApiService;

        private readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.AG;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.AGTransferService,
            LoginUser = new BasicUserInfo
            {
                UserId = 888,
            }
        };

        public UnitTest1()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            // 加上autofac
            ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            builder.RegisterType(typeof(TPGameAGApiMSLMockService))
                .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(PlatformProduct.AG.Value, PlatformMerchant.MiseLiveStream.Value));

            DependencyUtil.SetContainer(builder.Build());

            _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                _product,
                SharedAppSettings.PlatformMerchant,
                _environmentUser,
                DbConnectionTypes.Master);

            _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(
                _product,
                SharedAppSettings.PlatformMerchant,
                _environmentUser,
                DbConnectionTypes.Slave);
        }

        [TestMethod]
        public void TestCreateAccount()
        {
            BaseReturnModel returnModel = _tpGameApiService.CheckOrCreateAccount(_environmentUser.LoginUser.UserId);
        }

        [TestMethod]
        public void TestHSRPrivousDecimalParseFail()
        {
            List<AgFishInfo> agFishInfoes = new List<AgFishInfo>();
            string filename = "201907010410.xml";
            int errorCount = 0;
            decimal totalPreviousAmount = 0M;

            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                if (sr != null)
                {
                    string line = string.Empty;

                    while (false == string.IsNullOrEmpty((line = sr.ReadLine())))
                    {
                        var dataType = Regex.Match(line, "(?<=dataType=\")[\\s\\S]*?(?=\")").ToString();

                        if (dataType == "HSR")
                        {
                            var flag = Regex.Match(line, "(?<=flag=\")[\\s\\S]*?(?=\")").ToString();
                            var type = Regex.Match(line, "(?<=type=\")[\\s\\S]*?(?=\")").ToString();

                            if (flag == "0" && (type == "1" || type == "2" || type == "7"))
                            {
                                var ProfitLossID = Regex.Match(line, "(?<=ID=\")[\\s\\S]*?(?=\")").ToString();
                                var tradeNo = Regex.Match(line, "(?<=tradeNo=\")[\\s\\S]*?(?=\")").ToString();
                                var platformType = Regex.Match(line, "(?<=platformType=\")[\\s\\S]*?(?=\")").ToString();
                                var sceneId = Regex.Match(line, "(?<=sceneId=\")[\\s\\S]*?(?=\")").ToString();
                                var playerName = Regex.Match(line, "(?<=playerName=\")[\\s\\S]*?(?=\")").ToString();
                                var SceneStartTime = Regex.Match(line, "(?<=SceneStartTime=\")[\\s\\S]*?(?=\")").ToString();
                                var SceneEndTime = Regex.Match(line, "(?<=SceneEndTime=\")[\\s\\S]*?(?=\")").ToString();
                                var Roomid = Regex.Match(line, "(?<=Roomid=\")[\\s\\S]*?(?=\")").ToString();
                                var Roombet = Regex.Match(line, "(?<=Roombet=\")[\\s\\S]*?(?=\")").ToString();
                                var Cost = Regex.Match(line, "(?<=Cost=\")[\\s\\S]*?(?=\")").ToString();
                                var Earn = Regex.Match(line, "(?<=Earn=\")[\\s\\S]*?(?=\")").ToString();
                                var Jackpotcomm = Regex.Match(line, "(?<=Jackpotcomm=\")[\\s\\S]*?(?=\")").ToString();
                                var transferAmount = Regex.Match(line, "(?<=transferAmount=\")[\\s\\S]*?(?=\")").ToString();
                                var previousAmount = Regex.Match(line, "(?<=previousAmount=\")[\\s\\S]*?(?=\")").ToString();
                                var currentAmount = Regex.Match(line, "(?<=currentAmount=\")[\\s\\S]*?(?=\")").ToString();
                                var currency = Regex.Match(line, "(?<=currency=\")[\\s\\S]*?(?=\")").ToString();
                                var exchangeRate = Regex.Match(line, "(?<=exchangeRate=\")[\\s\\S]*?(?=\")").ToString();
                                var IP = Regex.Match(line, "(?<=IP=\")[\\s\\S]*?(?=\")").ToString();
                                var creationTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                                var gameCode = Regex.Match(line, "(?<=gameCode=\")[\\s\\S]*?(?=\")").ToString();

                                try
                                {
                                    decimal parsePreviousAmount = 0M;
                                    decimal.TryParse(previousAmount, out parsePreviousAmount);
                                    totalPreviousAmount += parsePreviousAmount;
                                }
                                catch (Exception)
                                {
                                    errorCount++;
                                }
                            }
                        }
                    }
                }
            }

            Assert.AreEqual(0, errorCount);
            Assert.AreEqual(100.25M, totalPreviousAmount);
        }

        [TestMethod]
        public void TestFile()
        {
            IAGProfitLossInfo agProfitLossInfo = new AGProfitLossInfo();

            agProfitLossInfo.TransferRemoteXMLData(AGGameType.AGIN);
            agProfitLossInfo.TransferRemoteXMLData(AGGameType.AGIN);
            agProfitLossInfo.TransferRemoteXMLData(AGGameType.AGIN);

            //for (int i = 0; i < 5; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //    }
            //    else
            //    {
            //        //AGProfitLossInfo.TransferRemoteXMLData("HUNTER");
            //    }
            //}

            //Task.Run(() => );
            //Task.Run(() => AGProfitLossInfo.TransferRemoteXMLData("XIN"));
            //Task.Run(() => AGProfitLossInfo.TransferRemoteXMLData("HUNTER"));
            //Task.Run(() => AGProfitLossInfo.TransferRemoteXMLData("YOPLAY"));
        }

        [TestMethod]
        public void TestReadWriteSQLite()
        {
            AGProfitLossInfo.ExistsOrder("1");
            //AGProfitLossInfo.SaveDataToLocal("1", "1", "1", "1", "1", "1", "1", "1",
            //     "1", "1", "1", "1", "1", "1", "1", "1", "1", "1",
            //     "1", "1", "1", "1", "1", "1");
        }

        [TestMethod]
        public void TestCreateTransferInMethod()
        {
            var model = new TPGameTranfserParam
            {
                UserID = _environmentUser.LoginUser.UserId,
                Amount = 20,
                IsSynchronizing = true
            };

            BaseReturnModel createTransferInResult = _tpGameApiService.CreateTransferInInfo(model);
        }

        [TestMethod]
        public void TestCreateTransferOutMethod()
        {
            var model = new TPGameTranfserParam
            {
                UserID = _environmentUser.LoginUser.UserId,
                Amount = 10,
                IsSynchronizing = true
            };

            BaseReturnModel createTransferOutResult = _tpGameApiService.CreateTransferOutInfo(model, isTransferOutAll: false, out string moneyId);
        }

        [TestMethod]
        public void TestTransferMockService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AgTransferMockService()
            };

            ReflectUtilNF.RunInteractive(ServicesToRun);
        }

        [TestMethod]
        public void TestClearExpiredDataMethod()
        {
            new AGProfitLossInfo().ClearExpiredData();
        }
    }
}