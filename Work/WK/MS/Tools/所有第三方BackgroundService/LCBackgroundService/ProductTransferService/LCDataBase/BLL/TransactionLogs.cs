using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.LCDataBase.Common;
using ProductTransferService.LCDataBase.DLL;
using ProductTransferService.LCDataBase.Enums;
using ProductTransferService.LCDataBase.Merchant;
using ProductTransferService.LCDataBase.Model;

namespace ProductTransferService.LCDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<LCApiParamModel, ApiResult<BetResult>>
    {
        private readonly LCProfitLossInfo _lcProfitLossInfo;

        private readonly Lazy<ILCOldSaveProfitLossInfo> _lcOldSaveProfitLossInfo;

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _lcProfitLossInfo = new LCProfitLossInfo();
            _lcOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<ILCOldSaveProfitLossInfo>(envLoginUser);
        }

        protected override IBetDetailService<LCApiParamModel, ApiResult<BetResult>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<ILCBetDetailService>(platformMerchant).Value;
        }

        public void SetBetInfo(LCApiParamModel model)
        {
            string lastSearchToken = _lcProfitLossInfo.SelectLastSearchTime();
            model.LastSearchToken = lastSearchToken;

            if (string.IsNullOrWhiteSpace(lastSearchToken))
            {
                LogUtilService.Info("获取上次查询投注时间区间失败");

                return;
            }

            DateTime currentTime = DateTime.Now;
            DateTime nearTime = Convert.ToInt64(lastSearchToken).ToDateTime();

            if ((currentTime - nearTime).TotalMinutes > model.PerOnceQueryMinutes)
            {
                model.StartTime = nearTime.ToUnixOfTime().ToString();
                model.EndTime = nearTime.AddMinutes(model.PerOnceQueryMinutes).ToUnixOfTime().ToString();
            }
            else
            {
                model.StartTime = currentTime.AddMinutes(-model.PerOnceQueryMinutes).ToUnixOfTime().ToString();
                model.EndTime = currentTime.ToUnixOfTime().ToString();
            }

            //取得第三方搜尋條件注單
            ApiResult<BetResult> result = BetDetailService.GetRemoteBetDetail(model);

            if (result == null || result.Data == null)
            {
                LogUtilService.ForcedDebug("BetLogResult is null");

                return;
            }

            if (result.Data.Code == (int)APIErrorCode.Success)
            {
                BetResult betResult = result.Data;

                if (betResult != null)
                {
                    if (betResult.BetDetails != null &&
                        betResult.BetDetails.Accounts.AnyAndNotNull() &&
                        result.WriteRemoteContentToOtherMerchant != null)
                    {
                        result.WriteRemoteContentToOtherMerchant.Invoke();
                    }

                    if (CycleTryOrder(betResult.BetDetails, model.AgentID, model.Linecode))
                    {
                        lastSearchToken = Convert.ToInt64(model.EndTime).ToDateTime().AddMinutes(-1).ToUnixOfTime().ToString();
                    }
                }
            }
            else if (result.Data.Code == (int)APIErrorCode.DataNotExist)
            {
                lastSearchToken = Convert.ToInt64(model.EndTime).ToDateTime().AddMinutes(-1).ToUnixOfTime().ToString();

                LogUtilService.Info("目前查询注单明细时间从 " +
                    Convert.ToInt64(model.StartTime).ToDateTime() + " 至 " +
                    Convert.ToInt64(model.EndTime).ToDateTime() + "，共获取 0 笔资料");
            }
            else
            {
                var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Data.Code));

                LogUtilService.Info("获取下注信息失败,返回状态码："
                    + result.Data.Code.ToString()
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

        private bool CycleTryOrder(BetDetails betDetails, string agentID, string linecode)
        {
            try
            {
                string lcUserHeader = agentID + "_";
                string jxPlayerHeader = agentID + "_" + linecode;
                var tpGameAccounts = new HashSet<string>();

                for (int i = 0; i < betDetails.LineCode.Count(); i++)
                {
                    if (betDetails.LineCode[i].Equals(jxPlayerHeader))
                    {
                        tpGameAccounts.Add(betDetails.Accounts[i].Replace(lcUserHeader, string.Empty));
                    }
                }

                Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.LC, tpGameAccounts);

                //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
                if (!allUserMap.Any())
                {
                    return true;
                }

                var singleBetInfoViewModels = new List<SingleBetInfoViewModel>();

                for (int i = 0; i < betDetails.GameID.Count(); i++)
                {
                    if (!betDetails.LineCode[i].Equals(jxPlayerHeader))
                    {
                        continue;
                    }

                    // 如果沒有投注則不紀錄
                    decimal myScore = 0;
                    decimal myBet = 0;
                    decimal myProfit = 0;
                    decimal myRevenue = 0;

                    if (decimal.TryParse(betDetails.CellScore[i], out myScore) &&
                        decimal.TryParse(betDetails.AllBet[i], out myBet) &&
                        decimal.TryParse(betDetails.Profit[i], out myProfit) &&
                        decimal.TryParse(betDetails.Revenue[i], out myRevenue))
                    {
                        if (myScore == 0 && myBet == 0 && myProfit == 0 && myRevenue == 0)
                        {
                            continue;
                        }
                    }

                    SingleBetInfo bet = new SingleBetInfo()
                    {
                        GameID = betDetails.GameID[i],
                        Account = betDetails.Accounts[i],
                        ServerID = betDetails.ServerID[i],
                        KindID = betDetails.KindID[i],
                        TableID = betDetails.TableID[i],
                        ChairID = betDetails.ChairID[i],
                        UserCount = betDetails.UserCount[i],
                        CellScore = betDetails.CellScore[i],
                        AllBet = betDetails.AllBet[i],
                        Profit = betDetails.Profit[i],
                        Revenue = betDetails.Revenue[i],
                        GameStartTime = betDetails.GameStartTime[i],
                        GameEndTime = betDetails.GameEndTime[i],
                        CardValue = betDetails.CardValue[i],
                        ChannelID = betDetails.ChannelID[i],
                        LineCode = betDetails.LineCode[i]
                    };

                    string tpGameAccount = bet.Account.Replace(lcUserHeader, string.Empty);

                    if (!allUserMap.ContainsKey(tpGameAccount))
                    {
                        LogUtilService.ForcedDebug($"比对帐号 {bet.Account} 无匹配对应之 UserID");

                        continue;
                    }

                    bet.UserID = allUserMap[tpGameAccount];
                    string memo = GetMemo(bet);

                    var singleBetInfoViewModel = new SingleBetInfoViewModel()
                    {
                        SingleBetInfo = bet,
                        Memo = memo
                    };

                    singleBetInfoViewModels.Add(singleBetInfoViewModel);
                }

                singleBetInfoViewModels = singleBetInfoViewModels.DistinctBy(d => d.SingleBetInfo.GameID).ToList();

                _lcOldSaveProfitLossInfo.Value.SaveDataToTarget(singleBetInfoViewModels);

                return true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("资料储存在本地端时发生异常，详细信息：" + ex.Message + ",堆栈：" + ex.Message);

                return false;
            }
        }

        /// <summary>
        /// 重置下一期搜寻时间
        /// </summary>
        /// <param name="ts"></param>
        private void ResetingNextSearchTime(string lastGameEndTs)
        {
            try
            {
                LCProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);
            }
            catch (Exception ex)
            {
                LogUtilService.Info("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }

        private string GetMemo(SingleBetInfo bet)
        {
            string playGameName = $"{EnumHelper<GameKind>.GetEnumDescription(Enum.GetName(typeof(GameKind), bet.KindID))}({bet.KindID})";

            string playRoomName = EnumHelper<RoomType>.GetEnumDescription(Enum.GetName(typeof(RoomType), bet.ServerID));
            playRoomName = string.IsNullOrEmpty(playRoomName) ? "无" : playRoomName + "(" + bet.KindID + ")";

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = typeof(ThirdPartyGameElement).FullName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.LCBetMemo),
                Args = new List<string>() {
                    playGameName,
                    playRoomName,
                    bet.AllBet,
                    bet.CellScore,
                    bet.Profit,
                    bet.Revenue,
                    bet.GameStartTime,
                    bet.GameEndTime,
                },
            });

            return localizationParam.ToLocalizationJsonString();
        }
    }
}