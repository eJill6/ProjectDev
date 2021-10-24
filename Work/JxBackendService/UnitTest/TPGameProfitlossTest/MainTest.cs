using JxBackendService;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Service.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnitTest.Base;

namespace UnitTest.TPGameProfitlossTest
{
    [TestClass]
    public class MainTest : BaseTest
    {
        private readonly TPGameStoredProcedureService _service;

        public MainTest()
        {
            _service = new TPGameStoredProcedureService(EnvLoginUser, DbConnectionTypes.Slave);
        }

        private PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> GetTeamProfitloss(PlatformProduct platformProduct)
        {
            SearchProductProfitlossParam param = new SearchProductProfitlossParam()
            {
                SearchUserName = "jackson",
                QueryStartDate = DateTime.Parse("2021-01-01"),
                QueryEndDate = DateTime.Now,
                PageNo = 1,
                PageSize = 100
            };

            return _service.GetTeamProfitloss(param);
        }

        [TestMethod]
        public void TestIMTeamUserTotalProfitloss()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> pageResult = GetTeamProfitloss(PlatformProduct.IM);
            stopWatch.Stop();
            File.WriteAllText("pageResult.json", pageResult.ToJsonString());
            Console.WriteLine("TotalSeconds=" + stopWatch.Elapsed.TotalSeconds);
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 10);
        }

        [TestMethod]
        public void TestLCTeamUserTotalProfitloss()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> pageResult = GetTeamProfitloss(PlatformProduct.LC);
            stopWatch.Stop();
            File.WriteAllText("pageResult.json", pageResult.ToJsonString());
            Console.WriteLine("TotalSeconds=" + stopWatch.Elapsed.TotalSeconds);
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 10);
        }

        [TestMethod]
        public void TestPTTeamUserTotalProfitloss()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> pageResult = GetTeamProfitloss(PlatformProduct.PT);
            stopWatch.Stop();
            File.WriteAllText("pageResult.json", pageResult.ToJsonString());
            Console.WriteLine("TotalSeconds=" + stopWatch.Elapsed.TotalSeconds);
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 10);
        }

        [TestMethod]
        public void TestSportTeamUserTotalProfitloss()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> pageResult = GetTeamProfitloss(PlatformProduct.Sport);
            stopWatch.Stop();
            File.WriteAllText("pageResult.json", pageResult.ToJsonString());
            Console.WriteLine("TotalSeconds=" + stopWatch.Elapsed.TotalSeconds);
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 10);
        }

        [TestMethod]
        public void TestAGTeamUserTotalProfitloss()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> pageResult = GetTeamProfitloss(PlatformProduct.AG);
            stopWatch.Stop();
            File.WriteAllText("pageResult.json", pageResult.ToJsonString());
            Console.WriteLine("TotalSeconds=" + stopWatch.Elapsed.TotalSeconds);
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 10);
        }

        [TestMethod]
        public void TestLotteryTeamUserTotalProfitloss()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> pageResult = GetTeamProfitloss(PlatformProduct.Lottery);
            stopWatch.Stop();
            File.WriteAllText("pageResult.json", pageResult.ToJsonString());
            Console.WriteLine("TotalSeconds=" + stopWatch.Elapsed.TotalSeconds);
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 20);
        }

        [TestMethod]
        public void TestLastMonthDateTime()
        {
            DateTime dateTime = DateTime.Now.AddDays(-DateTime.Now.Day);
            Assert.IsTrue(dateTime.AddDays(1).Month == DateTime.Now.Month);
        }

        [TestMethod]
        public void TestUserContributeDetailForFrontSide()
        {
            var userCommissionService = DependencyUtil.ResolveJxBackendService<IUserCommissionService>(EnvLoginUser, DbConnectionTypes.Master);
            var list = userCommissionService.UserContributeDetailForFrontSide(69778, DateTime.Now.AddMonths(-3), DateTime.Now);
            System.Diagnostics.Debug.WriteLine(list.ToJsonString());
        }

        //[TestMethod]
        //public void TestGetProductInloTotalProfitLoss()
        //{
        //    var teamUserViewModel = _tpGameProfitlossRep.GetTeamUsers(1, "jackson");
        //    string rootFullUserPath = teamUserViewModel.Level12Users.Where(w => w.DataType == (int)ProfitlossReportDataTypes.Self).Single().FullUserPaths;

        //    var resultList = _tpGameProfitlossRep.GetProductInloTotalProfitLoss(PlatformProduct.Lottery,
        //        rootFullUserPath,
        //         DateTime.Now.AddDays(-5),
        //         DateTime.Now,
        //         null);
        //    //DateTime dateTime = DateTime.Now.AddDays(-DateTime.Now.Day);
        //    //Assert.IsTrue(dateTime.AddDays(1).Month == DateTime.Now.Month);
        //}
    }
}