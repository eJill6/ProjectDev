﻿using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.CommissionTest
{
    [TestClass]
    public class ABEBTest : MainTest
    {
        private readonly PlatformProduct _product = PlatformProduct.ABEB;

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

            GenerateSecondProductProfitLoss(new ValidCommissionParam()
            {
                Product= PlatformProduct.IMeBET,
                UserName=validCommissionParam.UserName,
                ChildUserName = validCommissionParam.ChildUserName,
                StartDate = validCommissionParam.StartDate,
                EndDate = validCommissionParam.EndDate,
                BetMoney=1111.1111m,
                WinMoney=3333.3333m                
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
                StartDate = DateTime.Parse($"2021/03/01"),
                EndDate = DateTime.Parse($"2021/03/01"),
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
                WinMoney = 3333.3333m
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
                StartDate = DateTime.Parse($"2021/03/01"),
                EndDate = DateTime.Parse($"2021/03/01"),
                LastMonthContribute = 0,
                BetMoney = 11111.1111m,
                WinMoney = -11111.1111m,
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
        public void TestInsertDepositData()
        {
            DateTime startDate = DateTime.Parse($"2021/03/01");
            DateTime endDate = DateTime.Parse($"2021/03/01");

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
            ExecUserCommission(startDate, endDate, true);
            ExecUserCommission(startDate, endDate, false);
        }
    }
}