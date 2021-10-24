using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.CommissionTest
{
    [TestClass]
    public class GenerateAllBetDataTest : MainTest
    {
        [TestMethod]
        public void TestGenerateProfitLoss()
        {
            foreach (PlatformProduct product in PlatformProduct.GetAll().Where(w => !w.IsSelfProduct))
            {
                DateTime startDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
                DateTime endDate = DateTime.Now;
                UserInfo userInfo = UserInfoRelatedService.GetUserInfo("jackson");
                GenerateProfitLoss(product, userInfo, startDate, endDate, 100, 200, true);
                GenerateDailyProfitLoss(product, startDate, endDate);
            }
        }
    }
}