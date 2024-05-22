using System.Collections.Generic;
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
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private readonly ITPGameApiService _tpGameApiService;

        private readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.OBFI;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.OBFITransferService,
            LoginUser = new BasicUserInfo
            {
                UserId = 888,
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
                Amount = 400,
                IsSynchronizing = true
            };

            BaseReturnModel createTransferInResult = _tpGameApiService.CreateTransferInInfo(model);
        }

        [TestMethod]
        public void TestCheckUserScoreMethod()
        {
            IInvocationUserParam invocationUserParam = new InvocationUserParam();
            invocationUserParam.UserID = _environmentUser.LoginUser.UserId;
            BaseReturnDataModel<UserScore> returnModel = _tpGameApiService.GetRemoteUserScore(invocationUserParam, false);
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
        public void TestLaunchGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = false,
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
                IsMobile = true,
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        [TestMethod]
        public void TestTransferMockService()
        {
            ReflectUtilNF.RunInteractive(new OBFITransferScheduleMockService());
        }

        [TestMethod]
        public void TestTransferReturnDataModel()
        {
            var result = _tpGameApiService.GetRemoteBetLog("1614322800000");
        }
    }
}