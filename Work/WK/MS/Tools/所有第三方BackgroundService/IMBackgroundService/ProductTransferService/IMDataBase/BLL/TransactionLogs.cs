using IMBGDataBase.Merchant;
using IMDataBase.Common;
using IMDataBase.DLL;
using IMDataBase.Enums;
using IMDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<IMApiParamModel, BetLogResult<List<BetResult>>>
    {
        private readonly IMProfitLossInfo _profitLossInfo;

        private readonly Lazy<IIMOldSaveProfitLossInfo> _imOldSaveProfitLossInfo;

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossInfo = new IMProfitLossInfo();
            _imOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IIMOldSaveProfitLossInfo>(envLoginUser);
        }

        protected override IBetDetailService<IMApiParamModel, BetLogResult<List<BetResult>>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<IIMBetDetailService>(platformMerchant).Value;
        }

        public void SaveBetInfo(IMApiParamModel model)
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

            bool isResetingNextSearchTime = true;

            foreach (IMProductCode productCode in IMProductCode.GetAll())
            {
                model.ProductCode = productCode.Value;
                //取得第三方搜尋條件注單
                BetLogResult<List<BetResult>> result = BetDetailService.GetRemoteBetDetail(model);

                if (result == null)
                {
                    isResetingNextSearchTime = false;
                    break;
                }

                if (result.Code == (int)APIErrorCode.Success)
                {
                    List<BetResult> betResults = result.Result;

                    if (betResults == null)
                    {
                        isResetingNextSearchTime = false;
                        break;
                    }

                    if (betResults.Any() && result.WriteRemoteContentToOtherMerchant != null)
                    {
                        result.WriteRemoteContentToOtherMerchant.Invoke();
                    }

                    if (!CycleTryOrder(betResults)) // 儲存資料到SQLite
                    {
                        isResetingNextSearchTime = false;
                    }
                    else
                    {
                        lastSearchToken = model.EndTime.AddMinutes(-1).ToUnixOfTime().ToString();
                    }
                }
                else if (result.Code == (int)APIErrorCode.NoDataFound)
                {
                    lastSearchToken = model.EndTime.AddMinutes(-1).ToUnixOfTime().ToString();

                    LogUtilService.Info("目前查询" + productCode.ToString()
                        + "注单明细时间从 " + model.StartTime + " 至 "
                        + model.EndTime + "，共获取 0 笔资料");
                }
                else
                {
                    isResetingNextSearchTime = false;
                    var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Code));

                    LogUtilService.Info("获取" + productCode.ToString()
                        + "下注信息失败,返回状态码：" + result.Code.ToString()
                        + ", 信息：" + errorMsg);
                }

                //如果是FTP的資料, 每次成功就推進SQLITE token,並且更新model的LastSearchToken,讓下一次的ftp撈取可以獲得最新的資料
                if (!result.RemoteFileSeq.IsNullOrEmpty() && ResetingNextSearchTime(result.RemoteFileSeq))
                {
                    isResetingNextSearchTime = false;
                    model.LastSearchToken = result.RemoteFileSeq;
                }
                else
                {
                    Thread.Sleep(3000); // 避免併發意外
                }
            }

            if (isResetingNextSearchTime)
            {
                ResetingNextSearchTime(lastSearchToken); // 記錄這次執行時間
            }
        }

        private bool CycleTryOrder(List<BetResult> betDetails)
        {
            HashSet<string> tpGameAccounts = betDetails.Select(s => s.PlayerId).Distinct().ToHashSet();
            Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.IM, tpGameAccounts);

            //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
            if (!allUserMap.Any())
            {
                return true;
            }

            var viewModels = new List<SingleBetInfoViewModel>();

            foreach (BetResult betinfo in betDetails)
            {
                //不是我們的不處理
                if (!allUserMap.ContainsKey(betinfo.PlayerId))
                {
                    continue;
                }
                //沒輸贏結果的不處理
                //未結算&未取消
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
                    LastUpdatedDate = betinfo.LastUpdatedDate,
                    PlayerId = betinfo.PlayerId,
                    ProviderPlayerId = betinfo.ProviderPlayerId,
                    Currency = betinfo.Currency,
                    StakeAmount = betinfo.StakeAmount,
                    WinLoss = betinfo.WinLoss,
                    OddsType = betinfo.OddsType,
                    WagerType = betinfo.WagerType,
                    Platform = betinfo.Platform,
                    IsSettled = betinfo.IsSettled,
                    IsCancelled = betinfo.IsCancelled,
                    SettlementDateTime = betinfo.SettlementDateTime,
                    DetailItems = betinfo.DetailItems.ToJsonString(),
                    PlayName = "IM电竞"
                };

                string memo = GetMemo(betinfo);

                var viewModel = new SingleBetInfoViewModel()
                {
                    SingleBetInfo = bet,
                    Memo = memo
                };

                viewModels.Add(viewModel);
            }

            viewModels = viewModels.DistinctBy(d => d.SingleBetInfo.BetId).ToList();

            _imOldSaveProfitLossInfo.Value.SaveDataToTarget(viewModels);

            return true;
        }

        /// <summary>
        /// 重置下一期搜寻时间
        /// </summary>
        /// <param name="ts"></param>
        private bool ResetingNextSearchTime(string lastGameEndTs)
        {
            try
            {
                IMProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);

                return true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);

                return false;
            }
        }

        private string GetMemo(BetResult betinfo)
        {
            string thirdPartyGameElement = typeof(ThirdPartyGameElement).FullName;

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = thirdPartyGameElement,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoBetId),
                Args = new List<string>() { betinfo.BetId, },
            });

            foreach (var Items in betinfo.DetailItems)
            {
                // IM電競的不同產品會有不一樣的資料格式
                if (!Items.CompetitionName.IsNullOrEmpty())// For 電競
                {
                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = thirdPartyGameElement,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoBetDetail_Competition),
                        Args = new List<string>() { Items.CompetitionName, Items.Odds, },
                    });
                }
                else
                {
                    string game = betinfo.GameId; // 預設 避免為空

                    if (!Items.BetDescription.IsNullOrEmpty()) // For 虛擬電競
                    {
                        game = Items.BetDescription;
                    }
                    else if (!Items.SportsName.IsNullOrEmpty()) // For PK拾
                    {
                        game = Items.SportsName;
                    }

                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = thirdPartyGameElement,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoBetDetail),
                        Args = new List<string>() { game, },
                    });
                }
            }

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = thirdPartyGameElement,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoBetInfo),
                Args = new List<string>() { betinfo.StakeAmount, betinfo.WinLoss, betinfo.WagerCreationDateTime, betinfo.SettlementDateTime },
            });

            return localizationParam.ToLocalizationJsonString();
        }
    }
}