using System;
using System.Collections.Generic;
using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
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

        private readonly PlatformProduct _product = PlatformProduct.AWCSP;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.AWCSPTransferService,
            LoginUser = new BasicUserInfo
            {
                //UserId = 6251,
                UserId = 888,
            }
        };

        public PlatformServiceTest()
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + "\\";
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

        /// <summary> 創建第三方帳號 </summary>
        [TestMethod]
        public void TestCreateAccountMethod()
        {
            BaseReturnModel returnModel = _tpGameApiService.CheckOrCreateAccount(_environmentUser.LoginUser.UserId);
        }

        /// <summary> 用戶在第三方餘額 </summary>
        [TestMethod]
        public void TestCheckUserScoreMethod()
        {
            IInvocationUserParam invocationUserParam = new InvocationUserParam();
            invocationUserParam.UserID = _environmentUser.LoginUser.UserId;

            BaseReturnDataModel<UserScore> returnModel = _tpGameApiService.GetRemoteUserScore(invocationUserParam, false);
        }

        /// <summary> 主錢包轉到第三方帳戶 </summary>
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

        /// <summary> 第三方帳戶轉回主錢包 </summary>
        [TestMethod]
        public void TestCreateTransferOutMethod()
        {
            var model = new TPGameTranfserParam
            {
                UserID = _environmentUser.LoginUser.UserId,
                Amount = 5,
                IsSynchronizing = true
            };

            BaseReturnModel createTransferOutResult = _tpGameApiService.CreateTransferOutInfo(model, isTransferOutAll: false, out string moneyId);
        }

        /// <summary> 取得遊戲大廳遊戲網址(WEB) </summary>
        [TestMethod]
        public void TestLaunchGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = false,
                LoginInfo = new LoginInfo()
                {
                    GameCode = ThirdPartySubGameCodes.AWCHB.Value,
                    RemoteCode = ""
                }
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        /// <summary> 取得遊戲大廳遊戲網址(Mobile) </summary>
        [TestMethod]
        public void TestLaunchMoblieGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IpAddress = "127.0.0.1",
                IsMobile = true,
                LoginInfo = new LoginInfo()
                {
                    GameCode = ThirdPartySubGameCodes.AWCHB.Value,
                    RemoteCode = ""
                }
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        /// <summary> 取得正在處理轉入轉出單，訪問第三方遠端狀態 </summary>
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

        /// <summary> 取得投注資料 </summary>
        [TestMethod]
        public void TestBetLogMethod()
        {
            try
            {
                BaseReturnDataModel<RequestAndResponse> returnModel = _tpGameApiService.GetRemoteBetLog("2023-03-06 11:30:00.000");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestTransferMockService()
        {
            ReflectUtilNF.RunInteractive(new AWCSPTransferScheduleMockService());
        }
    }
}