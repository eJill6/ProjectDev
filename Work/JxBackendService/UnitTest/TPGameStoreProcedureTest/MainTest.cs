using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JxBackendService;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Repository.StoredProcedure;
using JxBackendService.Service.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Base;

namespace UnitTest.TPGameStoreProcedureTest
{
    [TestClass]
    public class MainTest : BaseTest
    {
        private readonly TPGameStoredProcedureService _tpGameStoredProcedureService;
        private readonly IMSGStoredProcedureRep _storedProcedureRep;
        private readonly GameCommissionRuleInfoService _gameCommissionRuleInfoService;
        private readonly UserCommissionService _userCommissionService;
        private readonly ITPGameStoredProcedureRep _tpGameStoredProcedureRep;

        public MainTest()
        {
            _tpGameStoredProcedureService = new TPGameStoredProcedureService(EnvLoginUser, DbConnectionTypes.Master);
            _storedProcedureRep = new IMSGStoredProcedureRep(EnvLoginUser, DbConnectionTypes.Slave);
            _gameCommissionRuleInfoService = new GameCommissionRuleInfoService(EnvLoginUser, DbConnectionTypes.Master);
            _userCommissionService = new UserCommissionService(EnvLoginUser, DbConnectionTypes.Slave);

            _tpGameStoredProcedureRep = DependencyUtil.ResolveJxBackendService<ITPGameStoredProcedureRep>(
                PlatformProduct.ABEB, 
                SharedAppSettings.PlatformMerchant, 
                EnvLoginUser, 
                DbConnectionTypes.Master);

        }


        [TestMethod]
        public void TestQueryMoneyIn()
        {
            var pagedResult = _storedProcedureRep.GetMoneyInInfoList(new SearchTPGameMoneyInfoParam()
            {
                //UserName = "rg001",
                OrderStatus = 2,
                PageNo = 2,
                PageSize = 5,
                SearchOrderStartDate = DateTime.Parse("2020-06-15"),
                SearchOrderEndDate = DateTime.Now,
                SortModels = new List<JxBackendService.Model.Paging.SortModel>() {
                    new JxBackendService.Model.Paging.SortModel()
                    {
                        ColumnName="OrderTime",
                        Sort= System.Data.SqlClient.SortOrder.Descending
                    }
                }
            });

            File.WriteAllText("pageResult.json", pagedResult.ToJsonString());
        }

        [TestMethod]
        public void TestGetPlayInfoList()
        {
            var pagedResult = _tpGameStoredProcedureService.GetPlayInfoList(new SearchTPGamePlayInfoParam()
            {
                ProductCode = PlatformProduct.PT.Value,
                UserName = "jackson",
                IsWins = new List<int> { 1, -1 },
                PageNo = 1,
                PageSize = 5,
                StartTime = DateTime.Parse("2021-02-01"),
                EndTime = DateTime.Parse("2021-02-03").ToQuerySmallEqualThanTime(DatePeriods.Day),
            });

            File.WriteAllText("pageResult.json", pagedResult.ToJsonString());
        }

        [TestMethod]
        public void TestGetProfitlossList()
        {
            var pagedResult = _tpGameStoredProcedureService.GetUserProfitLossDetails(new SearchTPGameProfitLossParam()
            {
                ProductCode = PlatformProduct.AG.Value,
                UserID = 69861,
                PageNo = 1,
                PageSize = 5,
                StartTime = DateTime.Parse("2021-1-01"),
                EndTime = DateTime.Now.ToQuerySmallEqualThanTime(DatePeriods.Day),
                IsCalculateStat = true,
            });

            File.WriteAllText("pageResult.json", pagedResult.ToJsonString());
        }



        [TestMethod]
        public void TestPager()
        {
            var pagedParam = new BasePagedParamsModel
            {
                PageNo = 1
            };

            Assert.IsTrue(pagedParam.PageIndex == 0);
        }

        [TestMethod]
        public void TestGetCommissionRuleInfo()
        {
            List<GameCommissionRuleInfo> ruleInfo = _gameCommissionRuleInfoService.GetGameCommissionRuleInfos(CommissionGroupType.PlatformLottery, 881108);
            File.WriteAllText("pageResult.json", ruleInfo.ToJsonString());
        }

        [TestMethod]
        public void TestSaveCommissionRuleInfo()
        {
            List<SaveCommissionRuleInfo> saveCommissionRuleInfos = new List<SaveCommissionRuleInfo>();

            for (int i = 0; i <= 5; i++)
            {
                var ruleInfos = new SaveCommissionRuleInfo()
                {
                    UserID = 1000,
                    UserName = "nizhuna",
                    MinProfitLossRange = i * 1000,
                    MaxProfitLossRange = i * 1000 + 500,
                    Visible = true,
                    CommissionPercent = 0.01 * i
                };

                saveCommissionRuleInfos.Add(ruleInfos);
            }


            BaseReturnModel ruleInfo = _gameCommissionRuleInfoService.SaveRuleInfoForPeriodCommission(CommissionGroupType.OtherLottery, saveCommissionRuleInfos);

        }

        [TestMethod]
        public void TestGetUserCommissionForBackSide()
        {
            var result = _userCommissionService.GetUserCommissionForBackSide("tom8", DateTime.Parse("2021-01-01"));
            string json = result.ToJsonString();
            Console.WriteLine(json);
        }

        [TestMethod]
        public void TestGetPlatformUserProfitLosses()
        {
            var result = _tpGameStoredProcedureService.GetPlatformUserProfitLosses(new SearchPlatformUserProfitLossParam()
            {
                PageNo = 1,
                PageSize = 30,
                IsCalculateStat = false,
                ProductCode = PlatformProduct.IMSG.Value,
                ProfitLossType=ProfitLossTypeName.KY.Value,
                StartTime = DateTime.Parse("2021-01-01"),
                EndTime = DateTime.Parse("2021-02-26"),
            });
            string json = result.ToJsonString();
            Console.WriteLine(json);
        }

        [TestMethod]
        public void TestTransferIn()
        {
            //_tpGameStoredProcedureRep.CreateMoneyInOrder(69778, 10);
            _tpGameStoredProcedureRep.DoTransferRollback(true, "5524784GOGDD00000004", "test");
        }


        //[TestMethod]
        //public void TestCompareCommissionRuleInfo()
        //{
        //    var parentRules = new List<SaveCommissionRuleInfo>
        //    {
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 0, MaxProfitLossRange = 20000, CommissionPercent = 0.11 },
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 20000, MaxProfitLossRange = 40000, CommissionPercent = 0.12 },
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 40000, MaxProfitLossRange = 9999999999, CommissionPercent = 0.14 }
        //    };

        //    var childRules = new List<SaveCommissionRuleInfo>
        //    {
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 0, MaxProfitLossRange = 10000, CommissionPercent = 0.09 },
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 10000, MaxProfitLossRange = 20000, CommissionPercent = 0.11 },
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 20000, MaxProfitLossRange = 30000, CommissionPercent = 0.12 },
        //        new SaveCommissionRuleInfo() { MinProfitLossRange = 30000, MaxProfitLossRange = 9999999999, CommissionPercent = 0.13 }
        //    };

        //    Assert.IsFalse(IsParentRulesGreaterEqualThanChild(parentRules, childRules));
        //}


    }
}
