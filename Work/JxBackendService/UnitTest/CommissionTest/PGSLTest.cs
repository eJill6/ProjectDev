using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.CommissionTest
{
    [TestClass]
    public class PGSLTest : MainTest
    {
        private readonly PlatformProduct _product = PlatformProduct.PGSL;

        [TestMethod]
        public void TestMonthlyCommissionyQA1()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointa",
                ChildUserName = "jacksonpointa1",
                StartDate = DateTime.Parse($"2021/03/01"),
                EndDate = DateTime.Parse($"2021/03/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = 1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA2()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointb",
                ChildUserName = "jacksonpointb1",
                StartDate = DateTime.Parse($"2021/03/01"),
                EndDate = DateTime.Parse($"2021/03/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = -1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA3()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointc",
                ChildUserName = "jacksonpointc1",
                StartDate = DateTime.Parse($"2021/03/01"),
                EndDate = DateTime.Parse($"2021/03/01"),
                LastMonthContribute = 0,
                BetMoney = 11111.1111m,
                WinMoney = -11111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA4()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointx",
                ChildUserName = "jacksonpointx1",
                StartDate = DateTime.Parse($"2021/03/01"),
                EndDate = DateTime.Parse($"2021/03/01"),
                LastMonthContribute = -10000,
                BetMoney = 15000m,
                WinMoney = -15000m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);
        }

        [TestMethod]
        public void TestInsertDepositData()
        {
            DateTime startDate = DateTime.Parse($"2021/03/01");
            DateTime endDate = DateTime.Parse($"2021/03/01");

            var insertDepositParam1 = new InsertDepositParam()
            {
                UserName = "jacksonpoint",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 4000,
            };

            var insertDepositParam2 = new InsertDepositParam()
            {
                UserName = "jacksonpointa",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 1000,
            };

            var insertDepositParam3 = new InsertDepositParam()
            {
                UserName = "jacksonpointb",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 2000,
            };

            var insertDepositParam4 = new InsertDepositParam()
            {
                UserName = "jacksonpointc",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 3000,
            };

            InsertDepositParam[] insertDepositParams = { insertDepositParam1, insertDepositParam2, insertDepositParam3, insertDepositParam4 };

            foreach(var insertDepositParam in insertDepositParams)
            {
                UserInfo user = UserInfoRelatedService.GetUserInfo(insertDepositParam.UserName);
                DeleteTeamProfitLoss(user, insertDepositParam.StartDate, insertDepositParam.EndDate, ProfitLossTypeName.CZ);
            }

            foreach (var insertDepositParam in insertDepositParams)
            {
                InsertDepositData(insertDepositParam);
            }

            GenerateDailyProfitLoss(PlatformProduct.Lottery, startDate, endDate);

            //計算每月分紅
            //ExecUserCommission(startDate, endDate, true);
            ExecUserCommission(startDate, endDate, false);
        }

        [TestMethod]
        public void TestThisMonthFrontSideCommission()
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1);
            DateTime endDate = DateTime.Now;
            ExecUserCommission(startDate, endDate, true);
        }
    }
}