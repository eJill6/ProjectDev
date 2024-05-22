﻿using IMBGDataBase.Merchant;
using IMSportDataBase.Common;
using IMSportDataBase.DLL;
using IMSportDataBase.Enums;
using IMSportDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ThirdParty.IMSport;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Text;

namespace IMSportDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<IMSportApiParamModel, BetLogResult<List<BetResult>>>
    {
        private readonly int _maxDetailMemoContentCount = 8;

        private readonly IMSportProfitLossInfo _profitLossInfo;

        private readonly Lazy<IIMSportOldSaveProfitLossInfo> _imsportOldSaveProfitLossInfo;

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossInfo = new IMSportProfitLossInfo();
            _imsportOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IIMSportOldSaveProfitLossInfo>(envLoginUser);
        }

        protected override IBetDetailService<IMSportApiParamModel, BetLogResult<List<BetResult>>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<IIMSportBetDetailService>(platformMerchant).Value;
        }

        public void SaveBetInfo(IMSportApiParamModel model)
        {
            string lastSearchToken = _profitLossInfo.SelectLastSearchTime();
            model.LastSearchToken = lastSearchToken;

            if (string.IsNullOrWhiteSpace(lastSearchToken))
            {
                LogUtilService.Info("获取上次查询投注时间区间失败");

                return;
            }

            DateTime currentTime = DateTime.Now;
            DateTime nearTime = lastSearchToken.ToInt64().ToDateTime();

            if ((currentTime - nearTime).TotalMinutes > model.PerOnceQueryMinutes)
            {
                model.StartTime = nearTime;
                model.EndTime = nearTime.AddMinutes(model.PerOnceQueryMinutes);
            }
            else
            {
                model.StartTime = currentTime.AddMinutes(-model.PerOnceQueryMinutes);
                model.EndTime = currentTime;
            }

            //取得第三方搜尋條件注單
            BetLogResult<List<BetResult>> result = BetDetailService.GetRemoteBetDetail(model);

            if (result == null)
            {
                LogUtilService.ForcedDebug("BetLogResult is null");

                return;
            }

            if (result.Code == (int)APIErrorCode.Success)
            {
                List<BetResult> betResult = result.Result;

                if (betResult != null)
                {
                    if (betResult.Any() && result.WriteRemoteContentToOtherMerchant != null)
                    {
                        result.WriteRemoteContentToOtherMerchant.Invoke();
                    }

                    if (CycleTryOrder(betResult))
                    {
                        lastSearchToken = model.EndTime.AddMinutes(-1).ToUnixOfTime().ToString();
                    }
                }
            }
            else if (result.Code == (int)APIErrorCode.NoDataFound)
            {
                lastSearchToken = model.EndTime.AddMinutes(-1).ToUnixOfTime().ToString();

                LogUtilService.Info("目前查询注单明细时间从 " +
                    model.StartTime + " 至 " +
                    model.EndTime + "，共获取 0 笔资料");
            }
            else
            {
                var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Code));

                LogUtilService.Info("获取下注信息失败,返回状态码："
                    + result.Code.ToString()
                    + ", 信息：" + errorMsg);
            }

            if (!result.RemoteFileSeq.IsNullOrEmpty())
            {
                lastSearchToken = result.RemoteFileSeq;
            }

            if (!lastSearchToken.IsNullOrEmpty())
            {
                ResetingNextSearchTime(lastSearchToken);
            }
        }

        private bool CycleTryOrder(List<BetResult> betDetails)
        {
            HashSet<string> tpGameAccounts = betDetails.Select(s => s.PlayerId).Distinct().ToHashSet();
            Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.IMSport, tpGameAccounts);

            //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
            if (!allUserMap.Any())
            {
                return true;
            }

            var singleBetInfoViewModels = new List<SingleBetInfoViewModel>();

            foreach (BetResult betinfo in betDetails)
            {
                //不是我們的不處理
                if (!allUserMap.ContainsKey(betinfo.PlayerId))
                {
                    continue;
                }

                //沒輸贏結果的不處理
                if (betinfo.IsSettled == 0 && betinfo.IsCancelled == 0)
                {
                    continue;
                }

                var bet = new SingleBetInfo()
                {
                    Provider = betinfo.Provider,
                    GameId = betinfo.GameId,
                    BetId = betinfo.BetId,
                    WagerCreationDateTime = betinfo.WagerCreationDateTime,
                    PlayerId = betinfo.PlayerId,
                    ProviderPlayerId = betinfo.ProviderPlayerId,
                    Currency = betinfo.Currency,
                    StakeAmount = betinfo.StakeAmount,
                    TurnOver = betinfo.TurnOver,
                    MemberExposure = betinfo.MemberExposure,
                    PayoutAmount = betinfo.PayoutAmount,
                    WinLoss = betinfo.WinLoss,
                    ResultStatus = betinfo.ResultStatus,
                    OddsType = betinfo.OddsType,
                    WagerType = betinfo.WagerType,
                    Platform = betinfo.Platform,
                    IsSettled = betinfo.IsSettled,
                    IsConfirmed = betinfo.IsConfirmed,
                    IsCancelled = betinfo.IsCancelled,
                    BetTradeStatus = betinfo.BetTradeStatus,
                    BetTradeCommission = betinfo.BetTradeCommission,
                    BetTradeBuybackAmount = betinfo.BetTradeBuybackAmount,
                    ComboType = betinfo.ComboType,
                    LastUpdatedDate = betinfo.LastUpdatedDate,
                    DetailItems = betinfo.DetailItems,
                };

                LocalizationParam localizationParam = CreateLocalizationParam(bet, betinfo.DetailItems);
                string memo = localizationParam.ToLocalizationJsonString();

                var singleBetInfoViewModel = new SingleBetInfoViewModel()
                {
                    SingleBetInfo = bet,
                    Memo = memo
                };

                singleBetInfoViewModels.Add(singleBetInfoViewModel);
            }

            singleBetInfoViewModels = singleBetInfoViewModels.DistinctBy(d => d.SingleBetInfo.BetId).ToList();

            _imsportOldSaveProfitLossInfo.Value.SaveDataToTarget(singleBetInfoViewModels);

            return true;
        }

        private LocalizationParam CreateLocalizationParam(BasicBetInfo basicBetInfo, List<DetailItem> detailItems)
        {
            WagerType wagerType = null;
            IMSportWagerType imSportWagerType = IMSportWagerType.GetSingle(basicBetInfo.WagerType);

            if (imSportWagerType != null)
            {
                wagerType = imSportWagerType.WagerType;
            }

            PlatformHandicap platformHandicap = null;

            IMOneHandicap imOneHandicap = IMOneHandicap.GetSingle(basicBetInfo.OddsType);
            if (imOneHandicap != null)
            {
                platformHandicap = imOneHandicap.PlatformHandicap;
            }

            StringBuilder allDetailContent = new StringBuilder();

            for (int i = 0; i < detailItems.Count; i++)
            {
                DetailItem detailItem = detailItems[i];

                if (i > 0)
                {
                    allDetailContent.Append("；");
                }

                var detailContents = new List<string>();

                if (!detailItem.SportsName.IsNullOrEmpty())
                {
                    detailContents.Add(detailItem.SportsName);
                }

                if (!detailItem.EventName.IsNullOrEmpty())
                {
                    detailContents.Add(detailItem.EventName);
                }

                if (!detailItem.CompetitionName.IsNullOrEmpty())
                {
                    detailContents.Add(detailItem.CompetitionName);
                }

                if (!detailItem.HomeTeamName.IsNullOrEmpty() && !detailItem.AwayTeamName.IsNullOrEmpty())
                {
                    detailContents.Add($"{detailItem.HomeTeamName} VS {detailItem.AwayTeamName}");
                }

                if (!detailItem.BetTypeDesc.IsNullOrEmpty())
                {
                    detailContents.Add(detailItem.BetTypeDesc);
                }

                string odds = detailItem.Odds;

                if (i >= _maxDetailMemoContentCount - 1)
                {
                    odds += "...";
                }

                detailContents.Add(string.Format(ThirdPartyGameElement.SomeOdds, odds));
                allDetailContent.Append(string.Join(",", detailContents));

                if (i >= _maxDetailMemoContentCount - 1)
                {
                    break;
                }
            }

            return LocalizationMemoUtil.CreateLocalizationParam(wagerType, platformHandicap, allDetailContent.ToString(), basicBetInfo.BetId);
        }

        /// <summary>
        /// 重置下一期搜寻时间
        /// </summary>
        /// <param name="ts"></param>
        private void ResetingNextSearchTime(string lastGameEndTs)
        {
            try
            {
                IMSportProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);
            }
            catch (Exception ex)
            {
                LogUtilService.Error("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }
    }
}