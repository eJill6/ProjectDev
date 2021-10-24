using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace JxBackendService
{
    public class TPGameStoredProcedureService : BaseService, ITPGameStoredProcedureService
    {
        private readonly IUserInfoRelatedService _userInfoRelatedService;

        public TPGameStoredProcedureService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>();
        }

        public PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> GetTeamProfitloss(SearchProductProfitlossParam searchParam)
        {
            //後台不檢查查詢範圍
            if (EnvLoginUser.Application != JxApplication.BackSideWeb &&
                !CheckDateWithinRange(searchParam.QueryStartDate , searchParam.QueryEndDate))
            {
                throw new OverMaxSearchDaysException(GlobalVariables.QueryProfitLossAndPlayHistoryLimitDays);
            }

            if (searchParam.SortText.IsNullOrEmpty())
            {
                searchParam.SortText = "ZKYProfitLossMoney desc";
            }

            ITPGameStoredProcedureRep tpGameStoredProcedureRep = ResolveTPGameStoredProcedureRep(PlatformProduct.GetSingle(searchParam.ProductCode));

            return tpGameStoredProcedureRep.GetTeamProfitloss(new ProGetGameTeamUserTotalProfitLossParam()
            {
                LoginUserId = EnvLoginUser.LoginUser.UserId,
                PageNo = searchParam.PageNo,
                PageSize = searchParam.PageSize,
                QueryStartDate = searchParam.QueryStartDate,
                QueryEndDate = searchParam.QueryEndDate,
                SearchUserName = searchParam.SearchUserName,
                ExclusiveAfterSaveTime = searchParam.ExclusiveAfterSaveTime,
                SortModel = new SortModel(searchParam.SortText)
            });
        }

        public PlatformTotalProfitlossStat GetPlatformProfitLoss(SearchPlatformProfitLossParam searchParam)
        {
            ITPGameStoredProcedureRep tpGameStoredProcedureRep = ResolveTPGameStoredProcedureRep(PlatformProduct.GetSingle(searchParam.ProductCode));
            searchParam.EndDate = searchParam.EndDate.ToQuerySmallThanTime(DatePeriods.Second);
            return tpGameStoredProcedureRep.GetPlatformProfitLoss(searchParam);
        }

        /// <summary>站台共用的投注紀錄</summary>
        public PagedResultWithAdditionalData<TPGamePlayInfoRowModel, TPGamePlayInfoFooter> GetPlayInfoList(SearchTPGamePlayInfoParam searchParam)
        {
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                //判斷時間是否超出可查詢範圍(只能在35天內)
                if (!CheckDateWithinRange(searchParam.StartTime, searchParam.EndTime))
                {
                    throw new OverMaxSearchDaysException(GlobalVariables.QueryProfitLossAndPlayHistoryLimitDays);
                }

                //判斷時間是否超出可查詢範圍(前後只能在7天內)
                if (!CheckDateBetweenRange(searchParam.StartTime, searchParam.EndTime))
                {
                    throw new OverMaxSearchDaysException(GlobalVariables.QueryProfitLossAndPlayHistoryDetailLimitDays);
                }

                //判斷是否為下級
                if (searchParam.UserID.HasValue && !_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, searchParam.UserID.Value))
                {
                    return new PagedResultWithAdditionalData<TPGamePlayInfoRowModel, TPGamePlayInfoFooter>(searchParam);
                }
                else if (searchParam.UserName.IsNullOrEmpty() && !searchParam.UserID.HasValue)
                {
                    searchParam.UserID = EnvLoginUser.LoginUser.UserId;
                }
            }

            return ResolveTPGameStoredProcedureRep(searchParam.Product).GetPlayInfoList(searchParam);
        }

        public TPGamePlayInfoRowModel GetSinglePlayInfo(PlatformProduct product, int userId, string playInfoId)
        {
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                //判斷是否為下級
                if (!_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, userId))
                {
                    return null;
                }
            }

            return ResolveTPGameStoredProcedureRep(product).GetSinglePlayInfo(userId, playInfoId);
        }

        /// <summary>站台共用的盈虧紀錄</summary>
        public PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn> GetUserProfitLossDetails(SearchTPGameProfitLossParam searchParam)
        {
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                //判斷時間是否超出可查詢範圍(只能在35天內)
                if (!CheckDateWithinRange(searchParam.StartTime, searchParam.EndTime))
                {
                    throw new OverMaxSearchDaysException(GlobalVariables.QueryProfitLossAndPlayHistoryLimitDays);
                }

                //判斷時間是否超出可查詢範圍(前後只能在7天內)
                if (!CheckDateBetweenRange(searchParam.StartTime, searchParam.EndTime))
                {
                    throw new OverMaxSearchDaysException(GlobalVariables.QueryProfitLossAndPlayHistoryDetailLimitDays);
                }

                //判斷是否為下級
                if (!_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, searchParam.UserID))
                {
                    return new PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn>(searchParam);
                }
            }

            return ResolveTPGameStoredProcedureRep(searchParam.Product).GetUserProfitLossDetails(searchParam);
        }

        /// <summary>
        /// 後台平台盈虧的用戶明細
        /// </summary>
        /// <returns></returns>
        public PagedResultModel<PlatformUserProfitloss> GetPlatformUserProfitLosses(CommonSearchTPGameProfitLossParam searchParam)
        {
            if (searchParam.ProfitLossType.IsNullOrEmpty())
            {
                searchParam.ProfitLossType = ProfitLossTypeName.CZ.Value;
            }

            searchParam.SortModels = new List<SortModel>()
            {
                new SortModel(){ColumnName = nameof(TPGameProfitLossRowModel.ProfitLossTime), Sort= SortOrder.Descending}
            };

            var returnList = new List<PlatformUserProfitloss>();

            PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn> pagedResult = ResolveTPGameStoredProcedureRep(searchParam.Product)
                .GetUserProfitLossDetails(searchParam);

            foreach (TPGameProfitLossRowModel tpGameProfitLossRowModel in pagedResult.ResultList)
            {
                var platformUserProfitloss = new PlatformUserProfitloss()
                {
                    UserName = tpGameProfitLossRowModel.UserName,
                    DisplayProfitLossType = ProfitLossTypeName.GetName(tpGameProfitLossRowModel.ProfitLossType),
                    Memo = tpGameProfitLossRowModel.Memo,
                    DisplayProfitLossTime = tpGameProfitLossRowModel.ProfitLossTime.ToFormatDateTimeString()
                };

                var userProfitlossStat = new BasicUserProfitlossStat()
                {
                    ProfitLossType = tpGameProfitLossRowModel.ProfitLossType,
                    TotalProfitLossMoney = tpGameProfitLossRowModel.ProfitLossMoney,
                    TotalPrizeMoney = tpGameProfitLossRowModel.PrizeMoney,
                    TotalWinMoney = tpGameProfitLossRowModel.WinMoney
                };

                var profitlossStatColumn = new ProfitLossStatColumn();
                ResolveTPGameStoredProcedureRep(searchParam.Product).ConvertProfitlossToColumns(userProfitlossStat, profitlossStatColumn);

                if (searchParam.ProfitLossType == ProfitLossTypeName.CZ)
                {
                    platformUserProfitloss.DisplayMoney = profitlossStatColumn.CZProfitLossMoneyText;
                }
                else if (searchParam.ProfitLossType == ProfitLossTypeName.TX)
                {
                    platformUserProfitloss.DisplayMoney = profitlossStatColumn.TXProfitLossMoneyText;
                }
                else if (searchParam.ProfitLossType == ProfitLossTypeName.KY)
                {
                    platformUserProfitloss.DisplayMoney = profitlossStatColumn.TZProfitLossMoneyText;
                    platformUserProfitloss.DisplayProfitLossMoney = profitlossStatColumn.ZKYProfitLossMoneyText;
                }
                else if (searchParam.ProfitLossType == ProfitLossTypeName.FD)
                {
                    platformUserProfitloss.DisplayMoney = profitlossStatColumn.FDProfitLossMoneyText;
                }
                else
                {
                    throw new NotImplementedException();
                }

                returnList.Add(platformUserProfitloss);
            }

            return new PagedResultModel<PlatformUserProfitloss>(searchParam)
            {
                ResultList = returnList,
                TotalCount = pagedResult.TotalCount
            };
        }

        public TPGameSelfProfitLossSearchResult GetSelfReport(PlatformProduct product, TPGameProfitLossSearchParam searchParam)
        {
            return ResolveTPGameStoredProcedureRep(product).GetSelfReport(searchParam);
        }

        public TPGameTeamProfitLossSearchResult GetTeamReport(PlatformProduct product, TPGameTeamProfitLossSearchParam searchParam)
        {
            return ResolveTPGameStoredProcedureRep(product).GetTeamReport(searchParam);
        }

        public DateTime GetReportCenterLastModifiedTime(PlatformProduct product)
        {
            return ResolveTPGameStoredProcedureRep(product).GetReportCenterLastModifiedTime();
        }

        public PagedResultModel<TPGameMoneyInfoViewModel> GetMoneyInfoList(PlatformProduct product, SearchTransferType searchTransferType, SearchTPGameMoneyInfoParam param)
        {
            var returnModel = new PagedResultModel<TPGameMoneyInfoViewModel>(param)
            {
                ResultList = new List<TPGameMoneyInfoViewModel>()
            };

            ITPGameStoredProcedureRep rep = ResolveTPGameStoredProcedureRep(product);
            List<BaseTPGameMoneyInfo> baseMoneyInfos = null;

            if (searchTransferType == SearchTransferType.In)
            {
                PagedResultModel<TPGameMoneyInInfo> moneyInInfos = rep.GetMoneyInInfoList(param);
                baseMoneyInfos = moneyInInfos.ResultList.ToList<BaseTPGameMoneyInfo>();
                returnModel.TotalCount = moneyInInfos.TotalCount;
            }
            else if (searchTransferType == SearchTransferType.Out)
            {
                PagedResultModel<TPGameMoneyOutInfo> moneyOutInfos = rep.GetMoneyOutInfoList(param);
                baseMoneyInfos = moneyOutInfos.ResultList.ToList<BaseTPGameMoneyInfo>();
                returnModel.TotalCount = moneyOutInfos.TotalCount;
            }
            else
            {
                throw new ArgumentNullException();
            }

            returnModel.ResultList = new List<TPGameMoneyInfoViewModel>();

            baseMoneyInfos.ForEach(f =>
            {
                TPGameMoneyInfoViewModel viewModel = f.CastByJson<TPGameMoneyInfoViewModel>();
                viewModel.Id = f.GetMoneyID();
                viewModel.OrderTypeName = product.Name + searchTransferType.Name;

                if (searchTransferType == SearchTransferType.In)
                {
                    viewModel.StatusName = TPGameMoneyInOrderStatus.GetName(f.Status);
                }
                else if (searchTransferType == SearchTransferType.Out)
                {
                    viewModel.StatusName = TPGameMoneyOutOrderStatus.GetName(f.Status);
                }

                returnModel.ResultList.Add(viewModel);
            });

            return returnModel;
        }

        /// <summary>
        /// 查詢前台彩票/第三方盈虧團隊報表是否在限制天數內
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool CheckDateWithinRange(DateTime startDate, DateTime endDate)
        {
            int availableDays = GlobalVariables.QueryProfitLossAndPlayHistoryLimitDays;

            //起始日期超過今天以前的{availableDays}天 或 結束日期超過今天 則跳錯
            if (DateTime.Today.Subtract(startDate).TotalDays >= availableDays ||
                endDate >= DateTime.Today.AddDays(1))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 查詢前台彩票/第三方盈虧個人明細是否在限制天數內
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool CheckDateBetweenRange(DateTime startDate, DateTime endDate)
        {
            int availableDays = GlobalVariables.QueryProfitLossAndPlayHistoryDetailLimitDays;

            //起始日期和結束日期相差超過{availableDays} 則跳錯
            if (endDate.Subtract(startDate).TotalDays >= availableDays)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private ITPGameStoredProcedureRep ResolveTPGameStoredProcedureRep(PlatformProduct product)
        {
            return ResolveJxBackendService<ITPGameStoredProcedureRep>(product);
        }
    }
}
