using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Repository.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.CommissionTest
{
    [TestClass]
    public class EVEBTest : MainTest
    {
        private readonly PlatformProduct _product = PlatformProduct.EVEB;

        public EVEBTest()
        {

        }

        [TestMethod]
        public void TestMonthlyCommissionyAll()
        {
            DeleteUserCommissionInfo();
            TestMonthlyCommissionyQA1();
            TestMonthlyCommissionyQA2();
            TestMonthlyCommissionyQA3();
            TestMonthlyCommissionyQA4();
            TestMonthlyCommissionyQA5();
            TestMonthlyCommissionyQA6();
            TestMonthlyCommissionyQA7();
            TestInsertDepositData();
            TestExecCurrentMonthFrontSideCommissionData();
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA1()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointa",
                ChildUserName = "jacksonpointa1",
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = 1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product = PlatformProduct.IMeBET,
                UserName = validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney = 1111.1111m,
                WinMoney = 3333.3333m
            });
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
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = 1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product = PlatformProduct.IMeBET,
                UserName = validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney = 3333.3333m,
                WinMoney = -3333.3333m
            });
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
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = 1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product = PlatformProduct.IMeBET,
                UserName = validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney = 12222.2222m,
                WinMoney = -12222.2222m
            });
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA4()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointd",
                ChildUserName = "jacksonpointd1",
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = -1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product = PlatformProduct.IMeBET,
                UserName = validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney = 1111.1111m,
                WinMoney = 3333.3333m,
            });
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA5()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointe",
                ChildUserName = "jacksonpointe1",
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = -1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product = PlatformProduct.IMeBET,
                UserName = validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney = 3333.3333m,
                WinMoney = -3333.3333m,
            });
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA6()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointf",
                ChildUserName = "jacksonpointf1",
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = 0,
                BetMoney = 1111.1111m,
                WinMoney = -1111.1111m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product = PlatformProduct.IMeBET,
                UserName = validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney = 9999.9999m,
                WinMoney = -9999.9999m,
            });
        }

        [TestMethod]
        public void TestMonthlyCommissionyQA7()
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = _product,
                UserName = "jacksonpointx",
                ChildUserName = "jacksonpointx1",
                StartDate = DateTime.Parse($"2021/04/01"),
                EndDate = DateTime.Parse($"2021/04/01"),
                LastMonthContribute = -10000,
                BetMoney = 15000m,
                WinMoney = -15000m,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            ValidCommissionOfUsers(validCommissionParam);

            //沒有次要產品資料
        }

        [TestMethod]
        public void TestInsertDepositData()
        {
            DateTime startDate = DateTime.Parse($"2021/04/01");
            DateTime endDate = DateTime.Parse($"2021/04/01");

            InsertDepositData(new InsertDepositParam()
            {
                UserName = "jacksonpoint",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 4000,
            });

            InsertDepositData(new InsertDepositParam()
            {
                UserName = "jacksonpointa",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 1000,
            });

            InsertDepositData(new InsertDepositParam()
            {
                UserName = "jacksonpointb",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 2000,
            });

            InsertDepositData(new InsertDepositParam()
            {
                UserName = "jacksonpointc",
                StartDate = startDate,
                EndDate = endDate,
                Amount = 3000,
            });

            GenerateDailyProfitLoss(PlatformProduct.Lottery, startDate, endDate);

            //計算每月分紅
            //ExecUserCommission(startDate, endDate, true);
            ExecUserCommission(startDate, startDate.ToQuerySmallEqualThanTime(DatePeriods.Month), false);
        }

        [TestMethod]
        public void TestExecCurrentMonthFrontSideCommissionData()
        {
            ExecUserCommission(DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1), DateTime.Now, true);
        }

        private void DeleteUserCommissionInfo()
        {
            string[] userNames = new string[] { "jacksonpoint", "jacksonpointa", "jacksonpointa1", "jacksonpointb", "jacksonpointb1", "jacksonpointc",
                "jacksonpointc1", "jacksonpointd", "jacksonpointd1", "jacksonpointe", "jacksonpointe1",
                "jacksonpointf", "jacksonpointf1", "jacksonpointx", "jacksonpointx1" };

            string sql = @"DELETE FROM UserCommissionInfo WHERE UserName IN @userNames;
                           DELETE FROM UserCommissionList WHERE UserName IN @userNames;";

            MasterDbHelper.Execute(sql, new { userNames = userNames.Select(s => s.ToNVarchar(50)) });
        }
    }
}