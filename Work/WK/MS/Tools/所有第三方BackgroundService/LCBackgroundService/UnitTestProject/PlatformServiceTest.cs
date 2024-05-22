using Autofac;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductTransferService;
using ProductTransferService.Service;
using UnitTestN6;

namespace UnitTestProject
{
    [TestClass]
    public class PlatformServiceTest : BaseUnitTest
    {
        private readonly Lazy<ITPGameApiService> _tpGameApiService;

        private readonly Lazy<ITPGameApiReadService> _tpGameApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.LC;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EnvironmentService>().AsImplementedInterfaces();
        }

        public PlatformServiceTest()
        {
            _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                _product,
                SharedAppSettings.PlatformMerchant,
                EnvironmentUser,
                DbConnectionTypes.Master);

            _tpGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(
                _product,
                SharedAppSettings.PlatformMerchant,
                EnvironmentUser,
                DbConnectionTypes.Slave);
        }

        [TestMethod]
        public void TestCreateAccountMethod()
        {
            BaseReturnModel returnModel = _tpGameApiService.Value.CheckOrCreateAccount(EnvironmentUser.LoginUser.UserId);
        }

        [TestMethod]
        public void TestCreateTransferInMethod()
        {
            var model = new TPGameTranfserParam
            {
                UserID = EnvironmentUser.LoginUser.UserId,
                Amount = 200,
            };

            BaseReturnModel createTransferInResult = _tpGameApiService.Value.CreateTransferInInfo(model);
        }

        [TestMethod]
        public void TestCheckUserScoreMethod()
        {
            IInvocationUserParam invocationUserParam = new InvocationUserParam();
            invocationUserParam.UserID = EnvironmentUser.LoginUser.UserId;

            BaseReturnDataModel<UserScore> returnModel = _tpGameApiService.Value.GetRemoteUserScore(invocationUserParam, false);
        }

        [TestMethod]
        public void TestCreateTransferOutMethod()
        {
            var model = new TPGameTranfserOutParam
            {
                UserID = EnvironmentUser.LoginUser.UserId,
            };

            BaseReturnModel createTransferOutResult = _tpGameApiService.Value.CreateTransferOutInfo(model, isTransferOutAll: false, out string moneyId);
        }

        [TestMethod]
        public void TestRecheckProcessingOrders()
        {
            var baseTPGameMoneyInfos = new List<BaseTPGameMoneyInfo>();

            // 正在處理轉入訂單
            baseTPGameMoneyInfos.AddRange(_tpGameApiReadService.Value.GetTPGameProcessingMoneyInInfo());
            // 正在處理轉出訂單
            baseTPGameMoneyInfos.AddRange(_tpGameApiReadService.Value.GetTPGameProcessingMoneyOutInfo());

            foreach (BaseTPGameMoneyInfo baseTPGameMoneyInfo in baseTPGameMoneyInfos)
            {
                _tpGameApiService.Value.RecheckProcessingOrders(baseTPGameMoneyInfo);
            }
        }

        [TestMethod]
        public void TestLaunchGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = EnvironmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = false
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.Value.GetForwardGameUrl(model);
        }

        [TestMethod]
        public void TestLaunchMobileGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = EnvironmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = true
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.Value.GetForwardGameUrl(model);
        }

        //[TestMethod]
        //public void TestBetLogMethod()
        //{
        //    BaseReturnDataModel<RequestAndResponse> returnModel = _tpGameApiService.GetRemoteBetLog("1661937032000");
        //}

        [TestMethod]
        public void TestCreateOrders()
        {
            var tpGameTranfserOutParam = new TPGameTranfserOutParam
            {
                UserID = EnvironmentUser.LoginUser.UserId,
            };

            var tpGameTranfserParam = new TPGameTranfserParam
            {
                UserID = EnvironmentUser.LoginUser.UserId,
                Amount = 10
            };

            for (int i = 1; i <= 10; i++)
            {
                BaseReturnModel baseReturnModel = _tpGameApiService.Value.CreateTransferInInfo(tpGameTranfserParam);

                Assert.IsTrue(baseReturnModel.IsSuccess);
            }

            BaseReturnModel transferOutResult = _tpGameApiService.Value.CreateTransferOutInfo(tpGameTranfserOutParam, isTransferOutAll: false, out string moneyId);

            Assert.IsTrue(transferOutResult.IsSuccess);
        }

        [TestMethod]
        public async Task TestTransferMockServiceAsync()
        {
            await RunHostedServiceAsync<ProductTransferScheduleMockService>();
        }
    }
}