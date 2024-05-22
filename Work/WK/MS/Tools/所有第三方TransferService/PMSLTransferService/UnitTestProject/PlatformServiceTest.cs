using System.Collections.Generic;
using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductTransferService;

namespace UnitTestProject
{
    [TestClass]
    public class PlatformServiceTest
    {
        private readonly ITPGameApiService _tpGameApiService;

        private readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.PMSL;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.PMSLTransferService,
            LoginUser = new BasicUserInfo
            {
                UserId = 588,
            }
        };

        public PlatformServiceTest()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
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
                Amount = 200,
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
        public void TestCheckUserScoreMethod()
        {
            IInvocationUserParam invocationUserParam = new InvocationUserParam();
            invocationUserParam.UserID = _environmentUser.LoginUser.UserId;
            BaseReturnDataModel<UserScore> returnModel = _tpGameApiService.GetRemoteUserScore(invocationUserParam, false);
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
        public void TestLaunchGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = false,
                LoginInfo = new LoginInfo
                {
                    GameCode = "32"
                }
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        [TestMethod]
        public void TestLaunchMoblieGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = true,
                LoginInfo = new LoginInfo
                {
                    GameCode = "66"
                }
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        [TestMethod]
        public void TestBetLogMethod()
        {
            BaseReturnDataModel<RequestAndResponse> returnModel = _tpGameApiService.GetRemoteBetLog("1680655239000");
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
            ReflectUtilNF.RunInteractive(new PMSLTransferScheduleMockService());
        }
    }
}