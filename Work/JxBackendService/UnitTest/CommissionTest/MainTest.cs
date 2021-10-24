using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using JxBackendService.Service.Game;
using JxBackendService.Service.ThirdPartyTransfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTest.Base;

namespace UnitTest.CommissionTest
{
    public abstract class MainTest : BaseTest
    {
        private readonly IGameCommissionRuleInfoService _gameCommissionRuleInfoService;
        private readonly IAppSettingService _appSettingService;
        private readonly IPlatformProductService _platformProductService;
        private readonly IUserCommissionService _userCommissionService;
        private readonly IUserDailyReportCommissionInfoService _userDailyReportCommissionInfoService;
        protected DbHelperSQL MasterDbHelper { get; private set; }
        private readonly DbHelperSQL _slaveDbHelper;
        private readonly string _testMemo = "測試假資料";
        //protected abstract PlatformProduct Product { get; }
        protected IUserInfoRelatedService UserInfoRelatedService { get; private set; }

        public MainTest()
        {
            _gameCommissionRuleInfoService = DependencyUtil.ResolveJxBackendService<IGameCommissionRuleInfoService>(EnvLoginUser, DbConnectionTypes.Slave);
            UserInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Slave);
            _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(EnvLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _userCommissionService = DependencyUtil.ResolveJxBackendService<IUserCommissionService>(EnvLoginUser, DbConnectionTypes.Slave);
            _userDailyReportCommissionInfoService = DependencyUtil.ResolveJxBackendService<IUserDailyReportCommissionInfoService>(EnvLoginUser, DbConnectionTypes.Slave);

            MasterDbHelper = new DbHelperSQL(_appSettingService.GetConnectionString(DbConnectionTypes.Master));
            _slaveDbHelper = new DbHelperSQL(_appSettingService.GetConnectionString(DbConnectionTypes.Slave));
        }

        /// <summary>
        /// 測試每月分紅數據
        /// </summary>
        public void TestCommissionResult(PlatformProduct product)
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = product,
                UserName = "jacksonimsg",
                ChildUserName = "jacksonimsgb1",
                StartDate = DateTime.Parse($"2020/11/01"),
                LastMonthContribute = 50000,
                BetMoney = 10000,
                WinMoney = -10000,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = false
            };

            validCommissionParam.EndDate = validCommissionParam.StartDate.AddMonths(1).AddSeconds(-1).ToQuerySmallEqualThanTime(DatePeriods.Second);
            ValidCommissionOfUsers(validCommissionParam);
        }

        protected void ValidCommissionOfUsers(ValidCommissionParam validCommissionParam)
        {
            ExpectedCommissionResult expectedCommissionResult = GetExpectedCommissionAmount(validCommissionParam);

            //For DB同步
            Thread.Sleep(3000);

            ValidUserCommission(expectedCommissionResult.CommissionGroupType,
                validCommissionParam.Product,
                expectedCommissionResult.SelfUser,
                validCommissionParam.StartDate,
                expectedCommissionResult.UserCommissionAmount);

            ValidUserCommission(expectedCommissionResult.CommissionGroupType,
                validCommissionParam.Product,
                expectedCommissionResult.ChildUser,
                validCommissionParam.StartDate,
                expectedCommissionResult.ChildUserCommissionAmount);
        }

        /// <summary>
        /// 測試前台顯示分紅數據
        /// </summary>        
        protected void TestDailyReportCommission(PlatformProduct product)
        {
            //定義測試參數
            var validCommissionParam = new ValidCommissionParam()
            {
                Product = product,
                UserName = "jacksonimsg",
                ChildUserName = "jacksonimsgb1",
                StartDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1),
                EndDate = DateTime.Now,
                LastMonthContribute = 50000,
                BetMoney = 10000,
                WinMoney = -5000,
                IsReCalculate = true,
                IsDailyUserCommissionByReport = true,
            };

            ExpectedCommissionResult expectedCommissionResult = GetExpectedCommissionAmount(validCommissionParam);
            ValidDailyReportCommission(expectedCommissionResult.SelfUser.UserID, expectedCommissionResult.UserCommissionAmount);
            ValidDailyReportCommission(expectedCommissionResult.ChildUser.UserID, expectedCommissionResult.ChildUserCommissionAmount);
        }

        protected ExpectedCommissionResult GetExpectedCommissionAmount(ValidCommissionParam validCommissionParam)
        {
            CommissionGroupType commissionGroupType = CommissionGroupType.GetAll()
                .Where(w => w.CalculateProductTypes.Contains(validCommissionParam.Product.ProductType))
                .Single();

            UserInfo user = UserInfoRelatedService.GetUserInfo(validCommissionParam.UserName);
            UserInfo childUser = UserInfoRelatedService.GetUserInfo(validCommissionParam.ChildUserName);
            List<GameCommissionRuleInfo> userRules = GetCommissionRules(commissionGroupType, user.UserID);
            List<GameCommissionRuleInfo> childUserRules = GetCommissionRules(commissionGroupType, childUser.UserID);

            if (validCommissionParam.IsReCalculate)
            {
                //清除團隊盈虧資料,分紅資料, 因為UserDailyReport_CommissionInfo會計算所有資料加總,故所有產品都須清除避免計算誤判
                int thisYearMonth = validCommissionParam.StartDate.ToFormatYearMonthValue().ToInt32();
                DeleteUserCommission(user, thisYearMonth);
                DeleteUserCommission(childUser, thisYearMonth);

                //清除上月分紅
                int lastYearMonth = validCommissionParam.StartDate.AddMonths(-1).ToFormatYearMonthValue().ToInt32();
                DeleteUserCommission(user, lastYearMonth);
                DeleteUserCommission(childUser, lastYearMonth);
                //填入上月分紅資料
                GenerateUserCommission(user, validCommissionParam.Product, lastYearMonth, validCommissionParam.LastMonthContribute);
                GenerateUserCommission(childUser, validCommissionParam.Product, lastYearMonth, validCommissionParam.LastMonthContribute);

                DeleteTeamProfitLoss(user, validCommissionParam.StartDate, validCommissionParam.EndDate, null);

                GenerateProfitLoss(validCommissionParam.Product, user, validCommissionParam.StartDate, validCommissionParam.EndDate,
                    validCommissionParam.BetMoney, validCommissionParam.WinMoney, false);

                GenerateProfitLoss(validCommissionParam.Product, childUser, validCommissionParam.StartDate, validCommissionParam.EndDate,
                    validCommissionParam.BetMoney, validCommissionParam.WinMoney, false);

                //For DB同步
                Thread.Sleep(3000);
                GenerateDailyProfitLoss(validCommissionParam.Product, validCommissionParam.StartDate, validCommissionParam.EndDate);

                //計算每月分紅
                ExecUserCommission(validCommissionParam.StartDate, validCommissionParam.EndDate, validCommissionParam.IsDailyUserCommissionByReport);
            }

            //驗證資料正確性
            //計算天數
            int calcDays = (int)validCommissionParam.EndDate.Date.Subtract(validCommissionParam.StartDate.Date).TotalDays + 1;

            //預期貢獻值
            decimal thisMonthContribute = calcDays * validCommissionParam.WinMoney * -1;
            decimal userContribute = thisMonthContribute * 2;//因為預設只有一個下級

            if (validCommissionParam.LastMonthContribute < 0)
            {
                userContribute += validCommissionParam.LastMonthContribute;
            }

            decimal childUserContribute = thisMonthContribute;

            if (validCommissionParam.LastMonthContribute < 0)
            {
                childUserContribute += validCommissionParam.LastMonthContribute;
            }

            //預期上級分紅金額
            decimal userCommissionAmount = (decimal)GetMatchCommissionPercent(userRules, userContribute).GetValueOrDefault() * userContribute;
            userCommissionAmount = DecimalUtil.Floor(userCommissionAmount, 4); //四位以下無條件捨去
            //預期下級分紅金額
            decimal childUserCommissionAmount = (decimal)GetMatchCommissionPercent(childUserRules, childUserContribute).GetValueOrDefault() * childUserContribute;
            childUserCommissionAmount = DecimalUtil.Floor(childUserCommissionAmount, 4); //四位以下無條件捨去

            return new ExpectedCommissionResult()
            {
                SelfUser = user,
                ChildUser = childUser,
                UserCommissionAmount = userCommissionAmount,
                ChildUserCommissionAmount = childUserCommissionAmount,
                CommissionGroupType = commissionGroupType
            };
        }

        protected void InsertDepositData(InsertDepositParam insertDepositParam)
        {
            UserInfo userInfo = UserInfoRelatedService.GetUserInfo(insertDepositParam.UserName);

            //刪除區段內所有充值資料
            string sql = @"
                DELETE FROM {0}.dbo.ProfitLoss 
                WHERE 
                    UserID = @UserID
                    AND ProfitLossType = @ProfitLossType 
                    AND ProfitLossTime >= @StartDate 
                    AND ProfitLossTime <= @EndDate ";

            object deleteParam = new
            {
                userInfo.UserID,
                insertDepositParam.UserName,
                insertDepositParam.StartDate,
                insertDepositParam.EndDate,
                ProfitLossType = ProfitLossTypeName.CZ.Value.ToNVarchar(50)
            };

            MasterDbHelper.Execute(string.Format(sql, InlodbType.Inlodb.Value), deleteParam);
            _slaveDbHelper.Execute(string.Format(sql, InlodbType.InlodbBak.Value), deleteParam);

            GenerateDepositData(userInfo, insertDepositParam.StartDate, insertDepositParam.EndDate, insertDepositParam.Amount);
        }

        private double? GetMatchCommissionPercent(List<GameCommissionRuleInfo> userRules, decimal userContribute)
        {
            GameCommissionRuleInfo userRule = userRules.SingleOrDefault(s => userContribute >= s.MinProfitLossRange && userContribute < s.MaxProfitLossRange);

            if (userRule != null)
            {
                return userRules.SingleOrDefault(s => userContribute >= s.MinProfitLossRange && userContribute < s.MaxProfitLossRange).CommissionPercent;
            }

            return 0;
        }

        private void ValidUserCommission(CommissionGroupType group, PlatformProduct product, UserInfo user, DateTime startDate, decimal userCommissionAmount)
        {
            decimal actualGroupCommissionAmount = 0;
            decimal actualProductCommissionAmount = 0;

            //取得db資料
            BaseReturnDataModel<UserCommissionBackSideViewModel> userReturnData = _userCommissionService.GetUserCommissionForBackSide(user.UserName, startDate);

            SpUserCommissionSelBackendResult userCommissionGroupResult = userReturnData.DataModel.UserCommissions
                .Where(w => w.UserID == user.UserID && w.CommissionType == group.Value).SingleOrDefault();

            SpUserCommissionSelBackendResult userCommissionProductResult = userReturnData.DataModel.UserCommissions
                .Where(w => w.UserID == user.UserID && w.CommissionType == product.Value).SingleOrDefault();

            if (userCommissionGroupResult != null)
            {
                actualGroupCommissionAmount = userCommissionGroupResult.CommissionAmount;
            }

            if (userCommissionProductResult != null)
            {
                actualProductCommissionAmount = userCommissionProductResult.CommissionAmount;
            }

            Assert.AreEqual(userCommissionAmount, actualGroupCommissionAmount);
            Assert.AreEqual(userCommissionAmount, actualProductCommissionAmount);
        }

        private void ValidDailyReportCommission(int userId, decimal userCommissionAmount)
        {
            //For DB同步
            Thread.Sleep(3000);

            UserDailyReport_CommissionInfo userDailyCommission = _userDailyReportCommissionInfoService.GetSingle(userId);
            Assert.AreEqual(userCommissionAmount, userDailyCommission.CommissionAmount);
        }

        /// <summary>
        /// 取得用戶分紅比例
        /// </summary>
        private List<GameCommissionRuleInfo> GetCommissionRules(CommissionGroupType commissionGroupType, int userId)
        {
            List<GameCommissionRuleInfo> rules = _gameCommissionRuleInfoService.GetGameCommissionRuleInfos(commissionGroupType, userId);
            return rules;
        }

        /// <summary>
        /// 刪除分紅資料
        /// </summary>
        private void DeleteUserCommission(UserInfo user, int processMonth)
        {
            string sql = "DELETE FROM UserCommissionInfo WHERE UserID = @userId AND ProcessMonth = @processMonth;" +
                "DELETE FROM UserCommissionList WHERE UserID = @UserID AND ProcessMonth = @processMonth";
            MasterDbHelper.Execute(sql, new { user.UserID, processMonth });
        }
        /// <summary>
        /// 清除團隊盈虧資料
        /// </summary>        
        protected void DeleteTeamProfitLoss(UserInfo user, DateTime startDate, DateTime endDate, ProfitLossTypeName profitLossTypeName)
        {
            //取得對應的產品
            List<PlatformProduct> products = _platformProductService.GetAll();

            foreach (PlatformProduct product in products)
            {
                string profitlossTableName = GetProfitlossTableName(product);

                string userIdWhereString = $@"
                    AND UserID IN (
                        SELECT UserID FROM {InlodbType.Inlodb}.dbo.UserInfo WITH(NOLOCK) 
                        WHERE UserID = @userId OR UserPaths LIKE @likeUserPath
                    )";

                string profitLossTimeColumnName = GetProfitLossTimeColumnName(product);

                string deleteWhereString = @"
                    WHERE 
                        {0} >= @startDate AND {0} <= @endDate 
                        " + userIdWhereString;

                string deleteProfitLossSql = "DELETE FROM {0}.dbo." + profitlossTableName + string.Format(deleteWhereString, "ProfitLossTime");
                string profitLossType = null;

                if (profitLossTypeName != null)
                {
                    deleteProfitLossSql += " AND ProfitLossType = @profitLossType ";
                    profitLossType = profitLossTypeName.Value;
                }

                object param = new
                {
                    startDate,
                    endDate,
                    user.UserID,
                    likeUserPath = $"{user.UserPaths}/{user.UserID}/%",
                    profitLossType
                };

                MasterDbHelper.Execute(string.Format(deleteProfitLossSql, InlodbType.Inlodb), param);
                _slaveDbHelper.Execute(string.Format(deleteProfitLossSql, InlodbType.InlodbBak), param);

                string playInfoTableName = GetPlayInfoTableName(product);
                string deletePlayInfoSql = "DELETE FROM {0}.dbo." + playInfoTableName + string.Format(deleteWhereString, profitLossTimeColumnName);
                MasterDbHelper.Execute(string.Format(deletePlayInfoSql, InlodbType.Inlodb), param);
                _slaveDbHelper.Execute(string.Format(deletePlayInfoSql, InlodbType.InlodbBak), param);

                string dwDailyProfitLossTableName = GetDWDailyProfitLossTableName(product);
                string deleteDailyProfitLossSql = $@"DELETE FROM {InlodbType.InlodbBak}.dbo.{dwDailyProfitLossTableName}
                                  WHERE RecordDate >= @startDate AND RecordDate <= @endDate {userIdWhereString}";

                _slaveDbHelper.Execute(deleteDailyProfitLossSql, param);
            }
        }

        private void GenerateUserCommission(UserInfo user, PlatformProduct product, int processMonth, decimal contribute)
        {
            string sql = @"INSERT INTO UserCommissionList (UserID, UserName, ParentID, ParentName, CommissionType, 
ProfitLossMoney, PrizeMoney, DownlineWinMoney, Contribute, TotalContribute, CommissionPercent, CommissionAmount, 
downlineCommissionAmount, SelfCommissionAmount, AuditStatus, ProcessMonth, ProcessTime, CreateTime)
VALUES(@UserID, @UserName, @ParentID, @ParentName, @CommissionType, @ProfitLossMoney, @PrizeMoney, 
@DownlineWinMoney, @Contribute, @TotalContribute, @CommissionPercent, @CommissionAmount, 
@downlineCommissionAmount, @SelfCommissionAmount, @AuditStatus, @ProcessMonth, @ProcessTime, @CreateTime) ";

            List<string> allCommissionTypes = new List<string>();
            allCommissionTypes.Add(product.Value);
            allCommissionTypes.Add(CommissionGroupType.GetAll().Where(w => w.CalculateProductTypes.Contains(product.ProductType))
                .Select(s => s.Value).Single());
            DateTime? processTime = null;

            foreach (string commissionType in allCommissionTypes.Distinct())
            {
                MasterDbHelper.Execute(sql, new
                {
                    user.UserID,
                    user.UserName,
                    user.ParentID,
                    ParentName = string.Empty,
                    commissionType,
                    ProfitLossMoney = 0,
                    PrizeMoney = 0,
                    DownlineWinMoney = 0,
                    contribute,
                    TotalContribute = contribute,
                    CommissionPercent = 0,
                    CommissionAmount = 0,
                    downlineCommissionAmount = 0,
                    SelfCommissionAmount = 0,
                    AuditStatus = 0,
                    processMonth,
                    ProcessTime = processTime,
                    CreateTime = DateTime.Now
                });
            }
        }

        protected void GenerateProfitLoss(PlatformProduct product, UserInfo user,
            DateTime startDate, DateTime endDate,
            decimal betMoney, decimal winMoney, bool isDeleteTestingData)
        {
            DateTime profitLossTime = startDate;
            string profitlossTableName = GetProfitlossTableName(product);
            string playInfoTableName = GetPlayInfoTableName(product);

            //這邊單純測試用,所以不做抽象化,早期的資料表有些許欄位會不同,用if/else處理掉             
            List<string> inloProfitLossColumns = new List<string>()
            {
                "ProfitLossID", "UserID", "UserName", "ParentID", "ProfitLossTime", "ProfitLossType", "ProfitLossMoney", "WinMoney",
                "PrizeMoney", "IsWin", "Memo", "PalyID", "GameType", "RefID", "BetTime", "AllBetMoney", "SaveTime"
            };

            List<string> inloBakProfitLossColumns = inloProfitLossColumns.Select(s => s).ToList();

            List<string> inloPlayInfoColumns = new List<string>()
            {
                "PalyInfoID", "UserID", "UserName", "ParentID", "PalyID", "BetTime", "ProfitLossTime", "BetMoney",
                "WinMoney", "GameType", "Memo", "RefID", "SaveTime", "IsWin", "AllBetMoney"
            };

            if (product == PlatformProduct.AG || product == PlatformProduct.Sport || product == PlatformProduct.PT)
            {
                inloProfitLossColumns.Remove("ProfitLossID");
                inloProfitLossColumns.Remove("AllBetMoney");
                inloBakProfitLossColumns.Remove("AllBetMoney");

                inloPlayInfoColumns.Remove("AllBetMoney");

                if (product == PlatformProduct.PT)
                {
                    inloProfitLossColumns[inloProfitLossColumns.IndexOf("BetTime")] = "NoteTime";
                    inloBakProfitLossColumns[inloBakProfitLossColumns.IndexOf("BetTime")] = "NoteTime";
                    inloProfitLossColumns.Add("ValidMoney");
                    inloBakProfitLossColumns.Add("ValidMoney");

                    inloPlayInfoColumns.Remove("RefID");
                    inloPlayInfoColumns.Remove("PalyInfoID");
                    inloPlayInfoColumns[inloPlayInfoColumns.IndexOf("BetTime")] = "NoteTime";
                    inloPlayInfoColumns[inloPlayInfoColumns.IndexOf("ProfitLossTime")] = "LotteryTime";
                    inloPlayInfoColumns.Add("ValidMoney");
                    inloPlayInfoColumns.Add("IsFactionAward");
                    inloPlayInfoColumns[inloPlayInfoColumns.IndexOf("BetMoney")] = "NoteMoney";
                }
            }
            else if (product == PlatformProduct.IMSport ||
                     product == PlatformProduct.IMeBET ||
                     product == PlatformProduct.RG ||
                     product == PlatformProduct.IM ||
                     product == PlatformProduct.IMPP ||
                     product == PlatformProduct.IMPT ||
                     product == PlatformProduct.IMeBET ||
                     product == PlatformProduct.IMBG ||
                     product == PlatformProduct.IMSport ||
                     product == PlatformProduct.IM)
            {
                inloPlayInfoColumns.Remove("AllBetMoney");
            }


            string insertProfitLossSql = "INSERT INTO {0}.dbo." + profitlossTableName + $@" ({string.Join(",", inloProfitLossColumns)})
                               VALUES({string.Join(",", inloProfitLossColumns.Select(s => "@" + s)) });";

            string insertPlayInfoSql = @"
                INSERT INTO {0}.dbo." + playInfoTableName + $@" ({string.Join(",", inloPlayInfoColumns)})
                VALUES({string.Join(",", inloPlayInfoColumns.Select(s => "@" + s))});";

            string insertBakProfitLossSql = "INSERT INTO {0}.dbo." + profitlossTableName + $@" ({string.Join(",", inloBakProfitLossColumns)})
                               VALUES({string.Join(",", inloBakProfitLossColumns.Select(s => "@" + s)) });";

            if (!inloProfitLossColumns.Contains("ProfitLossID"))
            {
                insertProfitLossSql += "SELECT @@IDENTITY;";
                insertPlayInfoSql += "SELECT @@IDENTITY;";
            }

            if (isDeleteTestingData)
            {
                object deleteParam = new
                {
                    user.UserID,
                    memo = _testMemo
                };

                MasterDbHelper.Execute(
                    $"DELETE FROM {InlodbType.Inlodb}.dbo.{profitlossTableName} WHERE UserID = @UserID AND memo = @memo ",
                    deleteParam);

                _slaveDbHelper.Execute(
                    $"DELETE FROM {InlodbType.InlodbBak}.dbo.{profitlossTableName} WHERE UserID = @UserID AND memo = @memo ",
                    deleteParam);

                MasterDbHelper.Execute(
                    $"DELETE FROM {InlodbType.Inlodb}.dbo.{playInfoTableName} WHERE UserID = @UserID AND memo = @memo ",
                    deleteParam);

                _slaveDbHelper.Execute(
                    $"DELETE FROM {InlodbType.InlodbBak}.dbo.{playInfoTableName} WHERE UserID = @UserID AND memo = @memo ",
                    deleteParam);
            }

            int serialNo = 1;

            while (profitLossTime <= endDate)
            {
                string profitLossID = null;

                if (inloProfitLossColumns.Contains("ProfitLossID"))
                {
                    if (product == PlatformProduct.LC ||
                        product == PlatformProduct.IM ||
                        product == PlatformProduct.RG ||
                        product == PlatformProduct.IMPT ||
                        product == PlatformProduct.IMPP ||
                        product == PlatformProduct.IMSport ||
                        product == PlatformProduct.IMeBET ||
                        product == PlatformProduct.IMBG)
                    {
                        profitLossID = GetProfitLossSeqBySeqName($"SEQ_{product.Value}ProfitLoss_ProfitLossID");
                    }
                    else
                    {
                        profitLossID = GetProfitLossSeqByTableName(profitlossTableName);
                    }
                }

                int userID = user.UserID;
                string userName = user.UserName;
                int parentId = user.ParentID.Value;
                string profitLossType = ProfitLossTypeName.KY.Value;
                decimal prizeMoney = betMoney + winMoney;
                int isWin = 0;
                string memo = _testMemo;
                string palyID = $"{product.Value}{profitLossID}";

                if (profitLossID.IsNullOrEmpty())
                {
                    palyID = $"{product.Value}{DateTime.Now.ToString("yyyyMMddHHmm")}{serialNo.ToString().PadLeft(3, '0')}";
                }

                string gameType = _testMemo;
                string refID = null;
                DateTime betTime = profitLossTime;
                decimal allBetMoney = betMoney;
                DateTime saveTime = DateTime.Now;

                var profitLossParam = new ProfitLossParam()
                {
                    ProfitLossID = profitLossID,
                    UserID = userID,
                    UserName = userName,
                    ParentId = parentId,
                    ProfitLossTime = profitLossTime,
                    ProfitLossType = profitLossType,
                    ProfitLossMoney = betMoney,
                    WinMoney = winMoney,
                    PrizeMoney = prizeMoney,
                    IsWin = isWin,
                    Memo = memo,
                    PalyID = palyID,
                    GameType = gameType,
                    RefID = refID,
                    BetTime = betTime,
                    AllBetMoney = allBetMoney,
                    SaveTime = saveTime
                };

                profitLossID = MasterDbHelper.ExecuteScalar<string>(string.Format(insertProfitLossSql, InlodbType.Inlodb), profitLossParam);

                if (!profitLossID.IsNullOrEmpty())
                {
                    profitLossParam.ProfitLossID = profitLossID;
                }

                _slaveDbHelper.Execute(string.Format(insertBakProfitLossSql, InlodbType.InlodbBak), profitLossParam);

                var playInfoParam = new PlayInfoParam()
                {
                    PalyInfoID = profitLossParam.ProfitLossID,
                    UserID = userID,
                    UserName = userName,
                    ParentId = parentId,
                    PalyID = palyID,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    BetMoney = allBetMoney,
                    WinMoney = winMoney,
                    GameType = gameType,
                    Memo = memo,
                    RefID = refID,
                    SaveTime = saveTime,
                    IsWin = isWin,
                    AllBetMoney = allBetMoney
                };

                string playInfoID = MasterDbHelper.ExecuteScalar<string>(string.Format(insertPlayInfoSql, InlodbType.Inlodb), playInfoParam);

                if (product == PlatformProduct.PT)
                {
                    inloPlayInfoColumns.Add("ID");
                    string insertBakPlayInfoSql = @"
                        INSERT INTO {0}.dbo." + playInfoTableName + $@" ({string.Join(",", inloPlayInfoColumns)})
                        VALUES({string.Join(",", inloPlayInfoColumns.Select(s => "@" + s))});";
                    playInfoParam.PalyInfoID = playInfoID;
                    inloPlayInfoColumns.Remove("ID");
                    _slaveDbHelper.Execute(string.Format(insertBakPlayInfoSql, InlodbType.InlodbBak), playInfoParam);
                }
                else
                {
                    _slaveDbHelper.Execute(string.Format(insertPlayInfoSql, InlodbType.InlodbBak), playInfoParam);
                }

                profitLossTime = profitLossTime.AddDays(1);
                serialNo++;
            }

        }


        private void GenerateDepositData(UserInfo user, DateTime startDate, DateTime endDate, decimal amount)
        {
            DateTime profitLossTime = startDate;
            string insertProfitLossSql = @"
                INSERT INTO {0}.dbo.ProfitLoss (ProfitLossTime, Memo, ProfitLossType, ProfitLossMoney, PalyID, UserID, UserPaths)
                VALUES(@ProfitLossTime, @Memo, @ProfitLossType, @ProfitLossMoney, @PalyID, @UserID, @UserPaths);
                SELECT @@IDENTITY";

            while (profitLossTime <= endDate)
            {
                string memo = "測試";
                string profitLossType = ProfitLossTypeName.CZ.Value;
                decimal profitLossMoney = amount;
                string palyID = profitLossTime.ToUnixOfTime().ToString().Substring(5);
                int userID = user.UserID;
                string userPaths = user.UserPaths;

                object profitLossParam = new
                {
                    profitLossTime,
                    memo,
                    profitLossType,
                    profitLossMoney,
                    palyID,
                    userID,
                    userPaths
                };

                int profitlossID = MasterDbHelper.ExecuteScalar<int>(string.Format(insertProfitLossSql, InlodbType.Inlodb), profitLossParam);

                //四天前的資料才要塞BAK
                if (profitLossTime < DateTime.Now.AddDays(-4))
                {
                    Thread.Sleep(1000);//for db sync
                    _slaveDbHelper.Execute(@"
INSERT INTO Inlodb_bak.dbo.ProfitLoss(ProfitLossID, ProfitLossTime,Memo,ProfitLossType,ProfitLossMoney,PalyID,UserID,UserPaths)   
SELECT ProfitLossID, ProfitLossTime,Memo,ProfitLossType,ProfitLossMoney,PalyID,UserID,UserPaths
FROM inlodb.dbo.ProfitLoss WITH(NOLOCK) where ProfitLossID = @profitlossID ", new { profitlossID });
                }

                profitLossTime = profitLossTime.AddDays(1);
            }
        }


        protected void GenerateDailyProfitLoss(PlatformProduct product, DateTime startDate, DateTime endDate)
        {
            DateTime processData = startDate;
            string profitlossTableName = GetProfitlossTableName(product);
            string spSuffix = null;

            if (product == PlatformProduct.LC ||
               product == PlatformProduct.IM ||
               product == PlatformProduct.RG)
            {
                spSuffix = "_SI";
            }
            else if (product == PlatformProduct.PT)
            {
                profitlossTableName = profitlossTableName.TrimEnd("_HS".ToArray());
            }


            while (processData <= endDate)
            {
                string sql = $"EXEC {InlodbType.InlodbBak}.dbo.Pro_Job_DW_Daily_{profitlossTableName}{spSuffix} '{processData.AddDays(4).ToFormatDateTimeString()}'";
                //_slaveDbHelper.Execute(sql, new { processData = processData.AddDays(4) }, CommandType.StoredProcedure);
                _slaveDbHelper.Execute(sql, null);
                processData = processData.AddDays(1);
            }
        }

        protected void ExecUserCommission(DateTime startDate, DateTime endDate, bool isDailyUserCommissionByReport)
        {
            object autoExecParam = new { startDate, endDate };
            object processDateParam = new { processDate = startDate };

            if (isDailyUserCommissionByReport)
            {
                _slaveDbHelper.Execute($"{InlodbType.InlodbBak}.dbo.Pro_JOB_DW_Daily_UserCommissionByReport", autoExecParam, CommandType.StoredProcedure);
            }
            else
            {
                _slaveDbHelper.Execute($"{InlodbType.InlodbBak}.dbo.Pro_UserCommission_AutoExec_V3", autoExecParam, CommandType.StoredProcedure);
                _slaveDbHelper.Execute($"{InlodbType.InlodbBak}.dbo.Pro_UserCommissionMoveDataToInlodb_SID", processDateParam, CommandType.StoredProcedure);
                MasterDbHelper.Execute($"{InlodbType.Inlodb}.dbo.Pro_UserCommission_InsTransferData", processDateParam, CommandType.StoredProcedure);
            }
        }

        protected void GenerateSecondProductProfitLoss(ValidCommissionParam validCommissionParam)
        {
            UserInfo userInfo = UserInfoRelatedService.GetUserInfo(validCommissionParam.UserName);
            UserInfo childUserInfo = UserInfoRelatedService.GetUserInfo(validCommissionParam.ChildUserName);

            bool IsDeleteTestingData = true;
            GenerateProfitLoss(
                validCommissionParam.Product,
                userInfo, validCommissionParam.StartDate,
                validCommissionParam.EndDate,
                validCommissionParam.BetMoney,
                validCommissionParam.WinMoney,
                IsDeleteTestingData);

            GenerateProfitLoss(
                validCommissionParam.Product,
                childUserInfo,
                validCommissionParam.StartDate,
                validCommissionParam.EndDate,
                validCommissionParam.BetMoney,
                validCommissionParam.WinMoney,
                IsDeleteTestingData);

            GenerateDailyProfitLoss(validCommissionParam.Product, validCommissionParam.StartDate, validCommissionParam.EndDate);

            //計算每月分紅
            ExecUserCommission(validCommissionParam.StartDate, validCommissionParam.EndDate, true);
            ExecUserCommission(validCommissionParam.StartDate, validCommissionParam.EndDate, false);
        }


        private string GetProfitLossSeqByTableName(string tableName)
        {
            string sql = $@"DECLARE @SeqID_KY VARCHAR(32)
                           EXEC {InlodbType.Inlodb}.dbo.Pro_GetTableSequence 
	                           @TableName = @tableName,
	                           @SeqResult = @SeqID_KY OUTPUT
                           SELECT @SeqID_KY";
            return MasterDbHelper.ExecuteScalar<string>(sql, new { tableName });
        }

        private string GetProfitLossSeqBySeqName(string sequenceName)
        {
            string sql = $@"
                DECLARE @SeqResult VARCHAR(32)
                EXEC[dbo].[Pro_GetSequenceIdentity]
                    @SequenceName = @sequenceName,
                    @SEQID = @SeqResult OUTPUT
                SELECT @SeqResult";
            return MasterDbHelper.ExecuteScalar<string>(sql, new { sequenceName });
        }

        private string GetProfitlossTableName(PlatformProduct product)
        {
            return GetTPGameStoredProcedureRepPropertyAbstractValue(product, "ProfitlossTableName");
        }

        private string GetDWDailyProfitLossTableName(PlatformProduct product)
        {
            return GetTPGameStoredProcedureRepPropertyAbstractValue(product, "DWDailyProfitLossTableName");
        }

        private string GetPlayInfoTableName(PlatformProduct product)
        {
            return GetTPGameStoredProcedureRepPropertyAbstractValue(product, "PlayInfoTableName");
        }

        private string GetProfitLossTimeColumnName(PlatformProduct product)
        {
            var tpGameStoredProcedureRep = DependencyUtil.ResolveJxBackendService<ITPGameStoredProcedureRep>(
                product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DbConnectionTypes.Master);

            var list = tpGameStoredProcedureRep.GetType()
               .GetProperty("PlayInfoSelectColumnInfos", BindingFlags.NonPublic | BindingFlags.Instance)
               .GetValue(tpGameStoredProcedureRep) as List<SqlSelectColumnInfo>;

            return list.Where(w => w.AliasName == nameof(TPGamePlayInfoRowModel.ProfitLossTime)).Single().ColumnName;
        }

        private string GetTPGameStoredProcedureRepPropertyAbstractValue(PlatformProduct product, string propertyName)
        {
            var tpGameStoredProcedureRep = DependencyUtil.ResolveJxBackendService<ITPGameStoredProcedureRep>(
                product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DbConnectionTypes.Master);

            return tpGameStoredProcedureRep.GetType()
                .GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(tpGameStoredProcedureRep).ToString();
        }
    }

    public class ValidCommissionParam
    {
        public PlatformProduct Product { get; set; }

        public string UserName { get; set; }

        public string ChildUserName { get; set; }

        public bool IsReCalculate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal LastMonthContribute { get; set; }

        public bool IsDailyUserCommissionByReport { get; set; }

        public decimal BetMoney { get; set; }
        public decimal WinMoney { get; set; }
    }

    public class ExpectedCommissionResult
    {
        public UserInfo SelfUser { get; set; }

        public UserInfo ChildUser { get; set; }

        public decimal UserCommissionAmount { get; set; }

        public decimal ChildUserCommissionAmount { get; set; }

        public CommissionGroupType CommissionGroupType { get; set; }
    }

    public class InsertDepositParam
    {
        public string UserName { get; set; }

        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class ProfitLossParam
    {
        public string ProfitLossID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int ParentId { get; set; }
        public DateTime ProfitLossTime { get; set; }
        public string ProfitLossType { get; set; }
        public decimal? ProfitLossMoney { get; set; }
        public decimal? WinMoney { get; set; }
        public decimal? PrizeMoney { get; set; }
        public int? IsWin { get; set; }
        public string Memo { get; set; }
        public string PalyID { get; set; }
        public string GameType { get; set; }
        public string RefID { get; set; }
        public DateTime BetTime { get; set; }
        public decimal? AllBetMoney { get; set; }
        public DateTime SaveTime { get; set; }


        public DateTime NoteTime => BetTime;
        public decimal? ValidMoney => ProfitLossMoney;
    }

    public class PlayInfoParam
    {
        public string PalyInfoID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int ParentId { get; set; }
        public string PalyID { get; set; }
        public DateTime BetTime { get; set; }
        public DateTime ProfitLossTime { get; set; }
        public decimal? BetMoney { get; set; }
        public decimal? WinMoney { get; set; }
        public string GameType { get; set; }
        public string Memo { get; set; }
        public string RefID { get; set; }
        public DateTime SaveTime { get; set; }
        public int? IsWin { get; set; }
        public decimal? AllBetMoney { get; set; }

        public DateTime NoteTime => BetTime;
        public DateTime LotteryTime => ProfitLossTime;
        public decimal? NoteMoney => BetMoney;
        public decimal? ValidMoney => BetMoney;
        public int IsFactionAward => 1;
        public string ID => PalyInfoID;
    }
}