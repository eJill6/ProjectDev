using System;
using System.Collections.Generic;
using Autofac;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer.WLBG;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceNF.Common.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductTransferService;
using JxBackendServiceNF.DependencyInjection;

namespace UnitTestProject
{
    [TestClass]
    public class PlatformServiceTest
    {
        private readonly ITPGameApiService _tpGameApiService;

        private readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly ITPGameWLBGApiService _tpGameWLBGApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.WLBG;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.WLBGTransferService,
            LoginUser = new BasicUserInfo
            {
                UserId = 588,
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

            _tpGameWLBGApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameWLBGApiService>(
                  _environmentUser,
                  DbConnectionTypes.Slave);
        }

        /// <summary> 創建第三方帳號 </summary>
        [TestMethod]
        public void TestCreateAccountMethod()
        {
            BaseReturnModel returnModel = _tpGameApiService.CheckOrCreateAccount(_environmentUser.LoginUser.UserId);
        }

        [TestMethod]
        public void TestsigntMethod()
        {
            String src = "name=Alice&text=Hello";
            //byte[] aesKey = "".getBytes(StandardCharsets.UTF_8);
            //Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
            //cipher.init(Cipher.ENCRYPT_MODE, new SecretKeySpec(aesKey, "AES"));
            //byte[] encrypted = cipher.doFinal(src.getBytes(StandardCharsets.UTF_8));
            //String p = Base64.getEncoder().encodeToString(encrypted);
            DateTime dt2 = DateTime.ParseExact("20230127143524", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            string param1 = AESTool.AESEncrypt(src, "W@th=w$BEht3r1aU");
            string param2 = AESTool.Encrypt(src, "W@th=w$BEht3r1aU");

            string parambase64 = param2.ToBase64String();
            string unixTime = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString();
            string sign = MD5Tool.ToMD5($"{param2}{unixTime}{WLBGSharedAppSetting.RequestSignKey}", false);
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
                Amount = 50,
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
                Amount = 100,
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
                IsMobile = false
            };

            BaseReturnDataModel<TPGameOpenParam> returnModel = _tpGameApiService.GetForwardGameUrl(model);
        }

        /// <summary> 取得遊戲大廳遊戲網址(Mobile) </summary>
        [TestMethod]
        public void TestLaunchMobileGameMethod()
        {
            var model = new ForwardGameUrlParam
            {
                LoginUser = _environmentUser.LoginUser,
                IsMobile = true,
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
                BaseReturnDataModel<RequestAndResponse> returnModel = _tpGameApiService.GetRemoteBetLog("2023-02-01 00:20:36.000");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestTransferMockService()
        {
            ReflectUtilNF.RunInteractive(new WLBGTransferScheduleMockService());
        }

        [TestMethod]
        public void TestGetApiGameList()
        {
            Dictionary<string, string> gameDictionary = _tpGameWLBGApiReadService.GetApiGameListResult();
        }
    }
}