using IMBGDataBase.Merchant;
using IMPTDataBase.Common;
using IMPTDataBase.DLL;
using IMPTDataBase.Enums;
using IMPTDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMPTDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<IMPTApiParamModel, BetLogResult<List<BetResult>>>
    {
        private readonly IMPTProfitLossInfo _profitLossInfo;

        private readonly Lazy<IIMPTOldSaveProfitLossInfo> _imptOldSaveProfitLossInfo;

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossInfo = new IMPTProfitLossInfo();
            _imptOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IIMPTOldSaveProfitLossInfo>(envLoginUser);
        }

        protected override IBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<IIMPTBetDetailService>(platformMerchant).Value;
        }

        public void SaveBetInfo(IMPTApiParamModel model)
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
            HashSet<string> tpGameAccounts = betDetails.Select(s => s.PlayerName).Distinct().ToHashSet();
            Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.IMPT, tpGameAccounts);

            //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
            if (!allUserMap.Any())
            {
                return true;
            }

            var singleBetInfoViewModels = new List<SingleBetInfoViewModel>();

            foreach (BetResult betinfo in betDetails)
            {
                //不是我們的不處理
                if (!allUserMap.ContainsKey(betinfo.PlayerName))
                {
                    continue;
                }

                //常規旋轉且沒輸贏結果的不處理
                if (betinfo.Bet == 0 && betinfo.BonusType == "0")
                {
                    continue;
                }

                ISingleBetInfo bet = new SingleBetInfo()
                {
                    PlayerName = betinfo.PlayerName,
                    ProviderPlayerId = betinfo.ProviderPlayerId,
                    WindowCode = betinfo.WindowCode,
                    GameId = betinfo.GameId,
                    GameCode = betinfo.GameCode,
                    GameType = betinfo.GameType,
                    GameName = betinfo.GameName,
                    SessionId = betinfo.SessionId,
                    Bet = betinfo.Bet,
                    Win = betinfo.Win,
                    ProgressiveBet = betinfo.ProgressiveBet,
                    ProgressiveWin = betinfo.ProgressiveWin,
                    Balance = betinfo.Balance,
                    CurrentBet = betinfo.CurrentBet,
                    GameDate = betinfo.GameDate,
                    LiveNetwork = betinfo.LiveNetwork,
                    ExitGame = betinfo.ExitGame,
                    BonusType = betinfo.BonusType,
                    RNum = betinfo.RNum
                };

                string memo = GetMemo(betinfo);

                var singleBetInfoViewModel = new SingleBetInfoViewModel()
                {
                    SingleBetInfo = bet,
                    Memo = memo
                };

                singleBetInfoViewModels.Add(singleBetInfoViewModel);
            }

            singleBetInfoViewModels = singleBetInfoViewModels.DistinctBy(d => d.SingleBetInfo.GameCode).ToList();

            _imptOldSaveProfitLossInfo.Value.SaveDataToTarget(singleBetInfoViewModels);

            return true;
        }

        /// <summary>
        /// 重置下一期搜寻时间
        /// </summary>
        /// <param name="ts"></param>
        private void ResetingNextSearchTime(string lastGameEndTs)
        {
            try
            {
                IMPTProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);
            }
            catch (Exception ex)
            {
                LogUtilService.Error("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }

        private string GetMemo(BetResult betinfo)
        {
            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = typeof(ThirdPartyGameElement).FullName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMPTBetMemo),
                Args = new List<string>() {
                    betinfo.GameName,
                    betinfo.Bet.ToString(),
                    betinfo.GameDate
                },
            });

            return localizationParam.ToLocalizationJsonString();
        }
    }
}