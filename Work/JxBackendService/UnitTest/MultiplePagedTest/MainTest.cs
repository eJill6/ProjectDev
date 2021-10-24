using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JxBackendService;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Resource.Element;
using JxBackendService.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Base;

namespace UnitTest.MultiplePagedTest
{
    [TestClass]
    public class MainTest : BaseTest
    {

        public MainTest()
        {

        }


        //[TestMethod]
        //public void TestAccountChangesLog()
        //{
        //    var dbHelper = new DbHelperSQL(InlodbBakConnectionString);
            
        //    var param = new MultiplePagedSqlQueryParam()
        //    {
        //        PageNo = 3,
        //        PageSize = 20,
        //        Parameters = new
        //        {
        //            userId = 71156,
        //            ChangesTimeStart = DateTime.Parse("2020-04-09 15:03:32.567"),
        //            ChangesTimeEnd = DateTime.Now,
        //        }
        //    };

        //    param.SingleTableQueryParams.Add(new SingleTableQueryParam()
        //    {
        //        SelectColumnNames = @"ID, BetType, OldAvailableScores, OldFreezeScores, NewAvailableScores, NewFreezeScores, 
        //                              ChangesTime, ChangesAMoney, ChangesFMoney, UserID, Handle, Memo, TypeName, UserName",
        //        FullTableName = $"{InlodbType.InlodbBak}.dbo.Bet_Logs",
        //        Filters = "USERID = @userId AND ChangesTime >= @ChangesTimeStart AND ChangesTime < @ChangesTimeEnd ",
        //        OrderBy = "ORDER BY ChangesTime"
        //    });

        //    param.SingleTableQueryParams.Add(new SingleTableQueryParam()
        //    {
        //        SelectColumnNames = @"ID, LotteryType, OldAvailableScores, OldFreezeScores, NewAvailableScores, NewFreezeScores, 
        //                              ChangesTime, ChangesAMoney, ChangesFMoney, UserID, Handle, Memo, TypeName, UserName",
        //        FullTableName = $"{InlodbType.InlodbBak}.dbo.Lottery_Logs",
        //        Filters = "USERID = @userId AND ChangesTime >= @ChangesTimeStart AND ChangesTime < @ChangesTimeEnd ",
        //        OrderBy = "ORDER BY ChangesTime"
        //    });

        //    PagedResultModel<object> pagedResult =  dbHelper.MultiplePagedSqlQuery<object>(param);
        //    File.WriteAllText("pageResult.json", pagedResult.ToJsonString());
        //}
    }
}