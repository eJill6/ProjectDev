using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.CommissionTest
{
    [TestClass]
    public class LCTest : MainTest
    {
        private readonly PlatformProduct _product = PlatformProduct.LC;

        [TestMethod]
        public void TestGenerateProfitLoss()
        {
            DateTime startDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            DateTime endDate = DateTime.Now.AddDays(-1);
            UserInfo userInfo = UserInfoRelatedService.GetUserInfo("jackson");
            GenerateProfitLoss(_product, userInfo, startDate, endDate, 100, 200, true);
            GenerateDailyProfitLoss(_product, startDate, endDate);
        }

        
    }
}