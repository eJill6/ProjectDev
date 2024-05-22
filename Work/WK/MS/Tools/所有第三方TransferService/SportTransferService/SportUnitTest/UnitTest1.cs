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
using Maticsoft.DBUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportDataBase.BLL;
using SportDataBase.DLL;
using SportDataBase.Model;
using SportUnitTest;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace TestSportTransferService
{
    [TestClass]
    public class UnitTest1
    {
        private readonly ITPGameApiService _tpGameApiService;

        private readonly ITPGameApiReadService _tpGameApiReadService;

        private readonly PlatformProduct _product = PlatformProduct.Sport;

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.SportTransferService,
            LoginUser = new BasicUserInfo
            {
                UserId = 888
            }
        };

        private readonly string dbFullName;

        public UnitTest1()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            // 加上autofac
            ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            DependencyUtil.SetContainer(builder.Build());

            SportProfitLossInfo.InIt();
            dbFullName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data.db");

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
        public void TestSqliteTableCreateDailySequenceIsExist()
        {
            var result = !SQLiteDBHelper.TableIsExist(dbFullName, "DailySequence");
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CreateDailySequence()
        {
            string sql = @"CREATE TABLE DailySequence (
    DailyDate TEXT (8) PRIMARY KEY
                       NOT NULL,
    SeqNumber INTEGER  NOT NULL
                       DEFAULT (0)
);
";
            SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, null);

            sql = @"INSERT INTO DailySequence(DailyDate,SeqNumber)
VALUES(strftime('%Y%m%d', 'now'),0)";

            int result = SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, null);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void UpdateAndGetSequenceNumber()
        {
            string sql = @"
";
            SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, null);

            sql = @"UPDATE DailySequence SET SeqNumber = SeqNumber + 1 WHERE DailyDate = (SELECT strftime('%Y%m%d', 'now'));
SELECT SeqNumber FROM DailySequence WHERE DailyDate = (SELECT strftime('%Y%m%d', 'now'))";

            var result = SQLiteDBHelper.ExecuteScalar(dbFullName, sql, null);

            Assert.AreEqual(1, result);
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
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SportTransferMockService()
            };

            ReflectUtilNF.RunInteractive(ServicesToRun);
        }

        [TestMethod]
        public void TestReadWriteSQLite()
        {
            SportProfitLossInfo.ExistsOrder("1");
            SportProfitLossInfo.SaveDataToLocal(new BetDetails(), new BetDetailName());
        }

        [TestMethod]
        public void TestSetLocalScores()
        {
            int userId = 69778;
            decimal availableScores = 0, freezeScores = 0;
            var transfer = new Transfer(new EnvironmentUser() { Application = JxApplication.SportTransferService, LoginUser = new BasicUserInfo() }, DbConnectionTypes.Master);
            transfer.SetLocalUserScores(userId, ref availableScores, ref freezeScores);
        }
    }
}