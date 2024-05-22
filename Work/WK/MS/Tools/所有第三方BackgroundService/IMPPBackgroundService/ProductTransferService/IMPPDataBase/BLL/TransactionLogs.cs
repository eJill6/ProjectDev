using IMBGDataBase.Merchant;
using IMPPDataBase.Common;
using IMPPDataBase.DLL;
using IMPPDataBase.Enums;
using IMPPDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMPPDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<IMPPApiParamModel, BetLogResult<List<BetResult>>>
    {
        private List<string> _settledStatuses = new List<string> { "Settled", "Closed" };

        private readonly IMPPProfitLossInfo _profitLossInfo;

        private readonly Lazy<IIMPPOldSaveProfitLossInfo> _imppOldSaveProfitLossInfo;

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossInfo = new IMPPProfitLossInfo();
            _imppOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IIMPPOldSaveProfitLossInfo>(envLoginUser);
        }

        protected override IBetDetailService<IMPPApiParamModel, BetLogResult<List<BetResult>>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<IIMPPBetDetailService>(platformMerchant).Value;
        }

        public void SaveBetInfo(IMPPApiParamModel model)
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
            Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.IMPP, tpGameAccounts);

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
                if (!_settledStatuses.Contains(betinfo.Status))
                {
                    continue;
                }

                var bet = new SingleBetInfo()
                {
                    Provider = betinfo.Provider,
                    GameId = betinfo.GameId,
                    GameName = betinfo.GameName,
                    ChineseGameName = betinfo.ChineseGameName,
                    RoundId = betinfo.RoundId,
                    ExternalRoundId = betinfo.ExternalRoundId,
                    PlayerId = betinfo.PlayerId,
                    ProviderPlayerId = betinfo.ProviderPlayerId,
                    BetAmount = betinfo.BetAmount,
                    WinLoss = betinfo.WinLoss,
                    ProgressiveBet = betinfo.ProgressiveBet,
                    ProgressiveWin = betinfo.ProgressiveWin,
                    Bonus = betinfo.Bonus,
                    ProviderBonus = betinfo.ProviderBonus,
                    Status = betinfo.Status,
                    Platform = betinfo.Platform,
                    DateCreated = betinfo.DateCreated,
                    GameDate = betinfo.GameDate,
                    LastUpdatedDate = betinfo.LastUpdatedDate
                };

                string memo = GetMemo(betinfo);

                var singleBetInfoViewModel = new SingleBetInfoViewModel()
                {
                    SingleBetInfo = bet,
                    Memo = memo
                };

                singleBetInfoViewModels.Add(singleBetInfoViewModel);
            }

            singleBetInfoViewModels = singleBetInfoViewModels.DistinctBy(d => d.SingleBetInfo.RoundId).ToList();

            _imppOldSaveProfitLossInfo.Value.SaveDataToTarget(singleBetInfoViewModels);

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
                IMPPProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);
            }
            catch (Exception ex)
            {
                LogUtilService.Error("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }

        private string GetMemo(BetResult betinfo)
        {
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMPPProviderName),
                Args = new List<string>() { IMOneProviderType.GetName(betinfo.Provider), },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMPPMemoGameName),
                Args = new List<string>() { betinfo.ChineseGameName, },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMPPMemoBetAmount),
                Args = new List<string>() { betinfo.BetAmount.ToString(), },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMPPMemoGameDate),
                Args = new List<string>() { betinfo.GameDate.ToString("yyyy/MM/dd HH:mm:ss"), },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMPPMemoLastUpdatedDate),
                Args = new List<string>() { betinfo.LastUpdatedDate.ToString("yyyy/MM/dd HH:mm:ss"), },
            });

            return localizationParam.ToLocalizationJsonString();
        }
    }
}