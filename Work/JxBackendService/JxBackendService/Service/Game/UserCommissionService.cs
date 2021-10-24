using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.StoredProcedure;
using JxBackendService.Model.Param.Commission;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Repository.Game;
using JxBackendService.Repository.User;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendService.Service.User;
using OfficeOpenXml;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class UserCommissionService : BaseService, IUserCommissionService
    {
        private readonly static ConcurrentDictionary<string, CommissionTypeSortInfo> _commissionTypeSortInfoMap = new ConcurrentDictionary<string, CommissionTypeSortInfo>();
        private readonly IUserCommissionStoredProcedureRep _userCommissionStoredProcedureRep;
        private readonly IUserInfoRep _userInfoRep;
        private readonly IUserCommissionListRep _userCommissionListRep;
        private readonly IUserCommissionInfoRep _userCommissionInfoRep;

        public UserCommissionService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userCommissionStoredProcedureRep = ResolveJxBackendService<IUserCommissionStoredProcedureRep>();
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _userCommissionListRep = ResolveJxBackendService<IUserCommissionListRep>();
            _userCommissionInfoRep = ResolveJxBackendService<IUserCommissionInfoRep>();
        }

        public BaseReturnDataModel<UserCommissionBackSideViewModel> GetUserCommissionForBackSide(string userName, DateTime startDate)
        {
            int? userId = null;

            if (!userName.IsNullOrEmpty())
            {
                userId = _userInfoRep.GetFrontSideUserId(userName);
            }
            else
            {
                userId = EnvLoginUser.LoginUser.UserId;
            }

            int yearMonth = startDate.ToFormatYearMonthValue().ToInt32();
            BaseReturnDataModel<double> systemCommissionData = _userCommissionStoredProcedureRep.GetCommissionPayBySystem(yearMonth);

            if (!systemCommissionData.IsSuccess)
            {
                return new BaseReturnDataModel<UserCommissionBackSideViewModel>(systemCommissionData.Message, null);
            }

            List<SpUserCommissionSelBackendResult> userCommissionBackSideViewModels = _userCommissionStoredProcedureRep.GetUserCommissionForBackSide(userId, startDate);

            //並加入產品名稱與排序
            foreach (SpUserCommissionSelBackendResult userCommission in userCommissionBackSideViewModels)
            {
                BaseSpUserCommissionResult baseSpUserCommissionResult = GetBaseSpUserCommissionResult(userCommission.CommissionType);
                string productCode = TryConvertToProductCode(userCommission.CommissionType);
                userCommission.ProductCode = baseSpUserCommissionResult.ProductCode;
                userCommission.ProductType = baseSpUserCommissionResult.ProductType;
                userCommission.DisplayName = baseSpUserCommissionResult.DisplayName;
                userCommission.GroupSort = baseSpUserCommissionResult.GroupSort;
                userCommission.ProductSort = baseSpUserCommissionResult.ProductSort;
                userCommission.IsGroup = baseSpUserCommissionResult.IsGroup;
                userCommission.CommissionReportDataType = (int)CommissionReportDataTypes.List;
            }

            List<UserCommissionInfo> commissionInfos = _userCommissionInfoRep.GetByProcessMonth(yearMonth)
                .Where(w => userCommissionBackSideViewModels.Any(a => a.UserID == w.UserID)).ToList();

            userCommissionBackSideViewModels.AddRange(commissionInfos.Select(s => new SpUserCommissionSelBackendResult
            {
                DisplayName = CommissionElement.Total,
                UserID = s.UserID,
                UserName = s.UserName,
                CommissionType = s.CommissionType,
                Contribute = s.Contribute.GetValueOrDefault() * (-1),
                TotalContribute = s.TotalContribute.GetValueOrDefault(),
                DownlineWinMoney = s.DownlineWinMoney.GetValueOrDefault() * (-1),
                CommissionAmount = s.CommissionAmount.GetValueOrDefault(),
                DownlineCommissionAmount = s.DownlineCommissionAmount.GetValueOrDefault(),
                SelfCommissionAmount = s.SelfCommissionAmount.GetValueOrDefault(),
                ProcessMonth = s.ProcessMonth,
                DepositFee = s.DepositFee.GetValueOrDefault(),
                AuditStatus = s.AuditStatus,
                CommissionReportDataType = (int)CommissionReportDataTypes.Stat,
                ProductType = ProductTypes.Lottery, // 預設一個產品類別，不然前端轉換會發生異常                
            }).ToList());

            List<SpUserCommissionSelBackendResult> sortBackendResults = userCommissionBackSideViewModels
                .OrderBy(o => o.UserID)
                .ThenBy(o => o.CommissionReportDataType)
                .ThenBy(o => o.GroupSort)
                .ThenBy(t => t.ProductSort).ToList();

            foreach (SpUserCommissionSelBackendResult result in sortBackendResults.Where(w => w.IsGroup))
            {
                if (result.CommissionReportDataType == (int)CommissionReportDataTypes.List)
                {
                    result.DisplayName = $"【{result.DisplayName}】";
                }
            }

            return new BaseReturnDataModel<UserCommissionBackSideViewModel>(ReturnCode.Success,
                new UserCommissionBackSideViewModel()
                {
                    UserCommissions = sortBackendResults,
                    SumCommissionAmount = systemCommissionData.DataModel
                });
        }

        public byte[] GetExportUserCommissionBytes(DateTime queryDate)
        {
            List<UserCommissionExportViewModel> exportUserCommissions = GetExportUserCommissions(queryDate);

            if (!exportUserCommissions.AnyAndNotNull())
            {
                return null;
            }

            var stream = new MemoryStream();

            using (var excelPackage = new ExcelPackage(stream))
            {
                var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                int cellIndex = 1;
                sheet.Cells[1, cellIndex].Value = CommonElement.UserName;
                sheet.Cells[1, ++cellIndex].Value = CommonElement.ParentUserName;// "上線名";
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.CommissionType;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.BetMoney;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.PrizeMoney;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.DownlineWinMoney;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.Contribute;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.TotalContribute;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.CommissionPercent;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.CommissionAmount;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.DownlineCommissionAmount;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.SelfCommissionAmount;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.DepositFee;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.FinalSelfCommissionAmount;
                sheet.Cells[1, ++cellIndex].Value = CommissionElement.AuditStatus;

                int rowIndex = 2;

                foreach (UserCommissionExportViewModel exportUserCommission in exportUserCommissions)
                {
                    cellIndex = 1;
                    sheet.Cells[rowIndex, cellIndex].Value = exportUserCommission.UserName;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.ParentName;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.DisplayName;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.ProfitLossMoney;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.PrizeMoney;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.DownlineWinMoney;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.Contribute;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.TotalContribute;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.CommissionPercent;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.CommissionAmount;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.DownlineCommissionAmount;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.SelfCommissionAmount;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.DepositFee;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.FinalSelfCommissionAmount;
                    sheet.Cells[rowIndex, ++cellIndex].Value = exportUserCommission.AuditStatusText;

                    rowIndex++;
                }

                sheet.Cells[1, 1, 1, cellIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[1, 1, 1, cellIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                sheet.DefaultColWidth = 20;
                sheet.View.FreezePanes(2, 1);

                excelPackage.Save();
            }

            return stream.ToArray();
        }

        public BaseReturnDataModel<UserSelfCommissionApiResult> GetUserSelfCommissionForApi(CommissionSearchParam commissionSearchParam)
        {
            CommissionTypes commissionType = CommissionTypes.GetSingle(commissionSearchParam.CommissionType);

            List<SpUserCommissionSelFrontSideResult> userCommissionFrontSideViewModels = _userCommissionStoredProcedureRep.GetUserCommissionForFrontSide(
                commissionSearchParam.UserId,
                commissionType.QueryCommissionType,
                commissionSearchParam.StartDate,
                commissionSearchParam.EndDate,
                commissionSearchParam.ReportType);

            UserSelfCommissionApiResult resultData = new UserSelfCommissionApiResult();

            if (userCommissionFrontSideViewModels.AnyAndNotNull())
            {
                //因為是Self，所以只篩選自己UserId的資料
                List<SpUserCommissionSelFrontSideResult> selfResults = userCommissionFrontSideViewModels.Where(c => c.UserID == commissionSearchParam.UserId &&
                                                                                                    c.CommissionType == commissionType.QueryCommissionType).ToList();
                //如果有超過一筆，寫Log
                if (selfResults.Count() > 1)
                {
                    LogUtil.Error($"More then one row is returned for self bonus {commissionSearchParam.UserId} {commissionSearchParam.CommissionType}");
                }

                SpUserCommissionSelFrontSideResult selfData = selfResults.FirstOrDefault();

                decimal teamCommissionAmount = 0;

                //有資料，計算合計
                if (selfData != null)
                {
                    //原邏輯，拿除了自己UserId以外的資料且 AuditStatus ==2 的資料算 CommissionAmount 的總和
                    teamCommissionAmount = userCommissionFrontSideViewModels.Where(
                                                        c => c.UserID != commissionSearchParam.UserId &&
                                                        c.AuditStatus == CommissionAuditStatus.Debt.Value).Sum(c => c.CommissionAmount);
                }

                resultData = new UserSelfCommissionApiResult()
                {
                    UserId = selfData.UserID,
                    UserName = selfData.UserName,
                    CommissionType = selfData.CommissionType,
                    ProcessMonth = selfData.ProcessMonth.ToString(),
                    CommissionPercent = selfData.CommissionPercent,
                    CommissionAmount = selfData.CommissionAmount,
                    AuditStatus = selfData.AuditStatus,
                    ProfitLossMoney = selfData.ProfitLossMoney,
                    DownlineWinMoney = selfData.DownlineWinMoney,
                    DownlineCommissionAmount = selfData.DownlineCommissionAmount,
                    SelfCommissionAmount = selfData.SelfCommissionAmount,
                    DebtAmount = teamCommissionAmount,
                    Contribute = selfData.Contribute,
                    TotalContribute = selfData.TotalContribute,   
                    DepositFee = selfData.DepositFee
                };

                return new BaseReturnDataModel<UserSelfCommissionApiResult>(ReturnCode.Success, resultData);
            }
            else
            {
                return new BaseReturnDataModel<UserSelfCommissionApiResult>(ReturnCode.SystemError, resultData);
            }
        }

        public BaseReturnDataModel<List<UserTeamCommissionApiResult>> GetUserTeamCommissionForApi(CommissionSearchParam commissionSearchParam)
        {
            CommissionTypes commissionType = CommissionTypes.GetSingle(commissionSearchParam.CommissionType);

            List<SpUserCommissionSelFrontSideResult> userCommissionFrontSideViewModels = _userCommissionStoredProcedureRep.GetUserCommissionForFrontSide(
                commissionSearchParam.UserId,
                commissionType.QueryCommissionType,
                commissionSearchParam.StartDate,
                commissionSearchParam.EndDate,
                commissionSearchParam.ReportType);

            List<UserTeamCommissionApiResult> resultData = new List<UserTeamCommissionApiResult>();

            if (userCommissionFrontSideViewModels.AnyAndNotNull())
            {
                //Team不顯示自己的資料，所以篩選掉自己UserId的資料
                List<SpUserCommissionSelFrontSideResult> teamResults = userCommissionFrontSideViewModels.Where(c => c.UserID != commissionSearchParam.UserId &&
                                                                                                    c.CommissionType == commissionType.QueryCommissionType).ToList();

                resultData = teamResults.Select(c => new UserTeamCommissionApiResult()
                {
                    UserId = c.UserID,
                    UserName = c.UserName,
                    CommissionType = c.CommissionType,
                    ProcessMonth = c.ProcessMonth.ToString(),
                    CommissionPercent = c.CommissionPercent,
                    CommissionAmount = c.CommissionAmount,
                    AuditStatus = c.AuditStatus,
                    ProfitLossMoney = c.ProfitLossMoney,
                    DownlineWinMoney = c.DownlineWinMoney,
                    Contribute = c.Contribute
                }).ToList();

                return new BaseReturnDataModel<List<UserTeamCommissionApiResult>>(ReturnCode.Success, resultData);
            }
            else
            {
                return new BaseReturnDataModel<List<UserTeamCommissionApiResult>>(ReturnCode.SystemError, resultData);
            }
        }

        /// <summary>
        /// 前台貢獻明細
        /// </summary>
        public List<ContributeDetailViewModel> UserContributeDetailForFrontSide(int userId, DateTime startDate, DateTime endDate)
        {
            List<SpUserCommissionSelFrontSideResult> spResults = _userCommissionStoredProcedureRep.GetUserCommissionForFrontSide(
                userId,
                string.Empty,
                startDate,
                endDate,
                (int)ProUserCommissionSelCommissionListTypes.ContributeDetail);

            spResults = spResults.Where(w => w.UserID == userId).ToList();

            bool hasThousandComma = true;
            var contributeDetailViewModels = new List<ContributeDetailViewModel>();
            decimal totalBetAmount = 0;
            decimal totalPrizeAmount = 0;
            decimal totalProfitLossAmount = 0;
            decimal totalContribute = 0;

            //並加入產品名稱與排序
            foreach (SpUserCommissionSelFrontSideResult spResult in spResults)
            {
                var contributeDetailViewModel = new ContributeDetailViewModel()
                {
                    BetAmountText = spResult.ProfitLossMoney.ToCurrency(hasThousandComma),
                    PrizeAmountText = spResult.PrizeMoney.ToCurrency(hasThousandComma),
                    ProfitLossAmountText = (-spResult.Contribute).ToCurrency(hasThousandComma),
                    ContributeText = spResult.Contribute.ToCurrency(hasThousandComma)
                };

                BaseSpUserCommissionResult baseSpUserCommissionResult = GetBaseSpUserCommissionResult(spResult.CommissionType);
                contributeDetailViewModel.ProductType = (int)baseSpUserCommissionResult.ProductType;
                contributeDetailViewModel.DisplayName = baseSpUserCommissionResult.DisplayName;
                contributeDetailViewModel.GroupSort = baseSpUserCommissionResult.GroupSort;
                contributeDetailViewModel.ProductSort = baseSpUserCommissionResult.ProductSort;
                contributeDetailViewModel.IsGroup = baseSpUserCommissionResult.IsGroup;

                if (baseSpUserCommissionResult.IsGroup)
                {
                    totalBetAmount += spResult.ProfitLossMoney;
                    totalPrizeAmount += spResult.PrizeMoney;
                    totalProfitLossAmount += -spResult.Contribute;
                    totalContribute += spResult.Contribute;
                }

                contributeDetailViewModels.Add(contributeDetailViewModel);
            }

            foreach (ContributeDetailViewModel groupContributeDetail in contributeDetailViewModels.Where(s => s.IsGroup))
            {
                if (contributeDetailViewModels.Any(a => !a.IsGroup && a.ProductType == groupContributeDetail.ProductType))
                {
                    groupContributeDetail.IsCollapseVisible = true;
                }
            }

            var sortedList = contributeDetailViewModels
                .OrderBy(o => o.GroupSort)
                .ThenBy(t => t.ProductSort).ToList();

            sortedList.Add(new ContributeDetailViewModel()
            {
                DisplayName = CommonElement.Total,
                BetAmountText = totalBetAmount.ToCurrency(hasThousandComma),
                PrizeAmountText = totalPrizeAmount.ToCurrency(hasThousandComma),
                ProfitLossAmountText = totalProfitLossAmount.ToCurrency(hasThousandComma),
                ContributeText = totalContribute.ToCurrency(hasThousandComma)
            });           

            return sortedList;
        }

        private BaseSpUserCommissionResult GetBaseSpUserCommissionResult(string commissionType)
        {
            string productCode = TryConvertToProductCode(commissionType);

            if (productCode.IsNullOrEmpty())
            {
                return null;
            }

            CommissionTypeSortInfo commissionTypeSortInfo = GetDisplayNameAndSort(commissionType);

            if (commissionTypeSortInfo == null)
            {
                return null;
            }

            var baseSpUserCommissionResult = new BaseSpUserCommissionResult
            {
                ProductCode = productCode,
                ProductType = PlatformProduct.GetSingle(productCode).ProductType,
                DisplayName = commissionTypeSortInfo.DisplayName,
                GroupSort = commissionTypeSortInfo.GroupSort,
                ProductSort = commissionTypeSortInfo.ProductSort,
                IsGroup = commissionTypeSortInfo.IsGroup,
            };

            return baseSpUserCommissionResult;
        }

        private List<UserCommissionExportViewModel> GetExportUserCommissions(DateTime queryDate)
        {
            int yearMonth = queryDate.ToFormatYearMonthValue().ToInt32();
            var returnResult = _userCommissionListRep
                .GetByProcessMonth(yearMonth)
                .CastByJson<List<UserCommissionExportViewModel>>();

            //並加入產品名稱與排序
            foreach (UserCommissionExportViewModel userCommission in returnResult)
            {
                BaseSpUserCommissionResult baseSpUserCommissionResult = GetBaseSpUserCommissionResult(userCommission.CommissionType);

                userCommission.DisplayName = baseSpUserCommissionResult.DisplayName;
                userCommission.GroupSort = baseSpUserCommissionResult.GroupSort;
                userCommission.ProductSort = baseSpUserCommissionResult.ProductSort;
                userCommission.CommissionReportDataType = (int)CommissionReportDataTypes.List;
            }

            List<UserCommissionInfo> infoResult = _userCommissionInfoRep.GetByProcessMonth(yearMonth)
                .Where(w => returnResult.Any(a => a.UserID == w.UserID)).ToList();

            returnResult.AddRange(infoResult.Select(s => new UserCommissionExportViewModel
            {
                DisplayName = CommissionElement.Total,
                UserID = s.UserID,
                UserName = s.UserName,
                ParentID = s.ParentID,
                ParentName = s.ParentName,
                CommissionType = s.CommissionType,
                Contribute = s.Contribute.GetValueOrDefault() * (-1),
                TotalContribute = s.TotalContribute.GetValueOrDefault(),
                DownlineWinMoney = s.DownlineWinMoney.GetValueOrDefault() * (-1),
                CommissionAmount = s.CommissionAmount.GetValueOrDefault(),
                DownlineCommissionAmount = s.DownlineCommissionAmount.GetValueOrDefault(),
                SelfCommissionAmount = s.SelfCommissionAmount.GetValueOrDefault(),
                ProcessMonth = s.ProcessMonth,
                DepositFee = s.DepositFee.GetValueOrDefault(),
                CommissionReportDataType = (int)CommissionReportDataTypes.Stat,
                CommissionPercent = 0,
                PrizeMoney = s.PrizeMoney,
                ProfitLossMoney = s.ProfitLossMoney,
                AuditStatus = s.AuditStatus
            }).ToList());

            return returnResult
                .OrderBy(o => o.UserName)
                .ThenBy(o => o.CommissionReportDataType)
                .ThenBy(o => o.GroupSort)
                .ThenBy(t => t.ProductSort).ToList();
        }

        private CommissionTypeSortInfo GetDisplayNameAndSort(string commissionType)
        {
            _commissionTypeSortInfoMap.TryGetValue(commissionType, out CommissionTypeSortInfo value);

            if (value != null)
            {
                return value;
            }

            value = new CommissionTypeSortInfo();

            CommissionGroupType commissionGroupType = CommissionGroupType.GetSingle(commissionType);
            string displayName = string.Empty;
            int groupSort = 0;
            int productSort = 0;
            bool isGroup = false;

            if (commissionGroupType != null)
            {
                groupSort = commissionGroupType.Sort;
                displayName = commissionGroupType.Name;
                isGroup = true;
            }
            else
            {
                PlatformProduct product = PlatformProduct.GetSingle(commissionType);

                if (product != null)
                {
                    productSort = product.Sort;
                    displayName = product.Name;

                    CommissionTypes commissionTypeModel = CommissionTypes.GetAll().Where(w => w.Product == product).SingleOrDefault();


                    if (commissionTypeModel != null)
                    {
                        groupSort = commissionTypeModel.CommissionGroupType.Sort;
                    }
                }
            }

            value = new CommissionTypeSortInfo()
            {
                DisplayName = displayName,
                GroupSort = groupSort,
                ProductSort = productSort,
                IsGroup = isGroup
            };

            _commissionTypeSortInfoMap[commissionType] = value;

            return value;
        }

        private string TryConvertToProductCode(string commissionType)
        {
            CommissionGroupType commissionGroupType = CommissionGroupType.GetSingle(commissionType);

            //找不到就當作產品代碼
            if (commissionGroupType == null)
            {
                return commissionType;
            }

            //取得該group下的第一個產品代碼
            string productCode = CommissionTypes.GetProductValueByCommissionGroupType(commissionGroupType);

            return productCode;
        }
    }
}
