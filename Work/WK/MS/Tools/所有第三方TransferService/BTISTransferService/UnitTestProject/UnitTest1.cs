using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Xml;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Ftp;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private readonly ITPGameApiService _tpGameApiService;

        private readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.BTIS;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.BTISTransferService,
            LoginUser = new BasicUserInfo
            {
                //UserId = 6251,
                UserId = 888
            }
        };

        public UnitTest1()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            //builder.RegisterType(typeof(HttpWebRequestUtilMockService)).AsImplementedInterfaces();
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
                    DbConnectionTypes.Master);
        }

        [TestMethod]
        public void TestCreateAccountMethod()
        {
            BaseReturnModel returnModel = _tpGameApiService.CheckOrCreateAccount(_environmentUser.LoginUser.UserId);
        }

        [TestMethod]
        public void TestCreateTransferInMethod()
        {
            var model = new TPGameTranfserParam
            {
                UserID = _environmentUser.LoginUser.UserId,
                Amount = 100,
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
        public void TestRecheckProcessingOrders()
        {
            var baseTPGameMoneyInfos = new List<BaseTPGameMoneyInfo>();

            // 正在處理轉入訂單
            baseTPGameMoneyInfos.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyInInfo());
            // 正在處理轉出訂單
            baseTPGameMoneyInfos.AddRange(_tpGameApiReadService.GetTPGameProcessingMoneyOutInfo());

            foreach (BaseTPGameMoneyInfo baseTPGameMoneyInfo in baseTPGameMoneyInfos)
            {
                _tpGameApiService.RecheckProcessingOrders(baseTPGameMoneyInfo);
            }
        }

        [TestMethod]
        public void TestCheckUserScoreMethod()
        {
            IInvocationUserParam invocationUserParam = new InvocationUserParam();
            invocationUserParam.UserID = _environmentUser.LoginUser.UserId;

            BaseReturnDataModel<UserScore> returnModel = _tpGameApiService.GetRemoteUserScore(invocationUserParam, false);
        }

        [TestMethod]
        public void TestLaunchGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = false
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        [TestMethod]
        public void TestLaunchMobileGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = true
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        [TestMethod]
        public void TestBetLogMethod()
        {
            BaseReturnDataModel<RequestAndResponse> returnModel = _tpGameApiService.GetRemoteBetLog("1680851706");
        }

        [TestMethod]
        public void TestCreateOrders()
        {
            var model = new TPGameTranfserParam
            {
                UserID = _environmentUser.LoginUser.UserId,
                Amount = 100
            };

            for (int i = 1; i <= 100; i++)
            {
                _tpGameApiService.CreateTransferInInfo(model);
            }

            _tpGameApiService.CreateTransferOutInfo(model, isTransferOutAll: false, out string moneyId);
        }

        [TestMethod]
        public void TestTransferMockService()
        {
            ReflectUtilNF.RunInteractive(new BTISTransferScheduleMockService());
        }

        [TestMethod]
        public void TestDataApiRate()
        {
            for (int i = 1; i <= 50; i++)
            {
                System.Diagnostics.Debug.WriteLine($"try GetRemoteBetLogApiResult: {i} ");
                var baseReturnDataModel = _tpGameApiService.GetRemoteBetLog(null);
                Assert.IsTrue(baseReturnDataModel.IsSuccess);

                var response = baseReturnDataModel.DataModel.ResponseContent.Deserialize<BTISBaseDataResponse>();

                Assert.IsTrue(response.IsSuccess);
            }
        }

        [TestMethod]
        public void TestDeserializeXml()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>" +
                @"<MerchantResponse xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns =""http://networkpot.com/"" >" +
                @"<ErrorCode>NoError</ErrorCode>
<CustomerID>33819693</CustomerID>
<AuthToken />
<Balance>1497.48</Balance>
<OpenBetsBalance>0</OpenBetsBalance>
<TransactionID>16279330932</TransactionID>
</MerchantResponse>";

            BTISWalletResponse response = XmlUtil.Deserialize<BTISWalletResponse>(xml);
            System.Diagnostics.Debug.WriteLine(response.ToJsonString());
        }

        [TestMethod]
        public void TestFileDir()
        {
            string remoteFilePath = "/sdgsdgsdg/dfhdfh/dfhfdh/dfh";

            string[] splitContents = remoteFilePath.Split('/').Where(w => !w.IsNullOrEmpty()).ToArray();
            int totalCount = splitContents.Length;

            if (remoteFilePath.IsNullOrEmpty())
            {
                //return null;
            }

            var filePathInfo = new FilePathInfo
            {
                FileDirectories = splitContents.Take(totalCount - 1).ToList(),
                FileName = splitContents.Last()
            };
        }

        [TestMethod]
        public void TestFtpUpload()
        {
            FtpUtil.UploadFile(FtpSharedSettings.FtpLoginParam, "C:\\Users\\jill.jhang\\Desktop\\新文字文件.txt", "AA/新文字文件.txt");
        }

        [TestMethod]
        public void TestFtpDownload()
        {
            FtpUtil.DownloadFile(FtpSharedSettings.FtpLoginParam, "C:\\Users\\jill.jhang\\Desktop\\AA.txt", "");
        }

        [TestMethod]
        public void TestFtpGetList()
        {
            FtpLoginParam ftpLoginParam = FtpSharedSettings.FtpLoginParam;
            //根目錄
            FtpUtil.GetFileList(ftpLoginParam, $"{FtpSharedSettings.FtpRemoteFilePath}/BTIS");
        }

        [TestMethod]
        public void TestDeleteFtpFile()
        {
            //根目錄
            FtpUtil.DeleteFtpFile(FtpSharedSettings.FtpLoginParam, "///");
        }

        [TestMethod]
        public void TestTransferReturnDataModel()
        {
            BaseReturnDataModel<RequestAndResponse> result = _tpGameApiService.GetRemoteBetLog(DateTime.Today.AddDays(-1).ToFormatDateString());

            Assert.IsTrue(result.IsSuccess);
            LogUtil.ForcedDebug(result.DataModel.ToJsonString());
        }
    }
}