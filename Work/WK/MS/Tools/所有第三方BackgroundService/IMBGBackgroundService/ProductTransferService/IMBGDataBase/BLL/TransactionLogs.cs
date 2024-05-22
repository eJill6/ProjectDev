using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.IMBGDataBase.Common;
using ProductTransferService.IMBGDataBase.DLL;
using ProductTransferService.IMBGDataBase.Enums;
using ProductTransferService.IMBGDataBase.Merchant;
using ProductTransferService.IMBGDataBase.Model;

namespace ProductTransferService.IMBGDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<IMBGApiParamModel, IMBGResp<IMBGBetList<IMBGBetLog>>>
    {
        private readonly IMBGProfitLossInfo _profitLossInfo;

        private readonly Lazy<IIMBGOldSaveProfitLossInfo> _imbgOldSaveProfitLossInfo;

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossInfo = new IMBGProfitLossInfo();
            _imbgOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IIMBGOldSaveProfitLossInfo>(envLoginUser);
        }

        protected override IBetDetailService<IMBGApiParamModel, IMBGResp<IMBGBetList<IMBGBetLog>>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<IIMBGBetDetailService>(platformMerchant).Value;
        }

        public void SetBetInfo(IMBGApiParamModel model)
        {
            string lastSearchToken = _profitLossInfo.SelectLastSearchTime();
            model.LastSearchToken = lastSearchToken;

            if (string.IsNullOrWhiteSpace(lastSearchToken))
            {
                LogUtilService.Error("获取上次查询投注时间区间失败");

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
            IMBGResp<IMBGBetList<IMBGBetLog>> result = BetDetailService.GetRemoteBetDetail(model);

            if (result == null)
            {
                LogUtilService.ForcedDebug("BetLogResult is null");

                return;
            }

            if (result.Data != null && result.Data.Code == (int)APIErrorCode.Success)
            {
                List<IMBGBetLog> betResult = result.Data.List;

                if (betResult != null && betResult.Count > 0)
                {
                    if (result.WriteRemoteContentToOtherMerchant != null)
                    {
                        result.WriteRemoteContentToOtherMerchant.Invoke();
                    }

                    if (CycleTryOrder(betResult))
                    {
                        lastSearchToken = model.EndTime.AddMinutes(-1).ToUnixOfTime().ToString();
                    }
                }
                else
                {
                    //沒有注單也是要更新時間
                    lastSearchToken = model.EndTime.AddMinutes(-1).ToUnixOfTime().ToString();

                    LogUtilService.ForcedDebug("目前查询注单明细时间从 " +
                        model.StartTime + " 至 " +
                        model.EndTime + "，共获取 0 笔资料");
                }
            }
            else
            {
                var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Data.Code));

                LogUtilService.Error("获取下注信息失败,返回状态码："
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

        private bool CycleTryOrder(List<IMBGBetLog> betDetails)
        {
            try
            {
                HashSet<string> tpGameAccounts = betDetails.Select(s => s.UserCode).Distinct().ToHashSet();
                Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.IMBG, tpGameAccounts);

                //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
                if (!allUserMap.Any())
                {
                    return true;
                }

                var viewModels = new List<IMBGBetLogViewModel>();

                foreach (IMBGBetLog betinfo in betDetails)
                {
                    //不是我們的不處理
                    if (!allUserMap.ContainsKey(betinfo.UserCode))
                    {
                        continue;
                    }

                    string memo = CombineMemo(betinfo);

                    var viewModel = new IMBGBetLogViewModel()
                    {
                        IMBGBetLog = betinfo,
                        Memo = memo
                    };

                    viewModels.Add(viewModel);
                }

                viewModels = viewModels.DistinctBy(d => d.IMBGBetLog.Id).ToList();

                _imbgOldSaveProfitLossInfo.Value.SaveDataToTarget(viewModels);

                return true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("资料储存在本地端时发生异常，详细信息：" + ex.Message + ",堆栈：" + ex.Message);

                return false;
            }
        }

        private string CombineMemo(IMBGBetLog imbgBetLog)
        {
            string betTime = DateTimeUtility.Instance.ToLocalTime(imbgBetLog.OpenTime).ToString("yyyy-MM-dd HH:mm:ss");
            string settleTime = DateTimeUtility.Instance.ToLocalTime(imbgBetLog.EndTime).ToString("yyyy-MM-dd HH:mm:ss");
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMBGMemoBetId),
                Args = new List<string>()
                {
                    imbgBetLog.Id.ToString(),
                },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMBGMemoGameName),
                Args = new List<string>()
                {
                     IMBGGameNameUtil.GetGameName(imbgBetLog.GameId),//對應不到玩法名稱，預設給IM棋牌
                },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMBGMemoAllBet),
                Args = new List<string>()
                {
                    imbgBetLog.AllBills,
                },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMBGMemoWinLost),
                Args = new List<string>()
                {
                    imbgBetLog.WinLost,
                },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMBGMemoBetTime),
                Args = new List<string>()
                {
                    betTime
                },
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMBGMemoSettleTime),
                Args = new List<string>()
                {
                    settleTime
                },
            });

            return localizationParam.ToLocalizationJsonString();
        }

        /// <summary>
        /// 重置下一期搜寻时间
        /// </summary>
        /// <param name="ts"></param>
        private void ResetingNextSearchTime(string lastGameEndTs)
        {
            try
            {
                IMBGProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);
            }
            catch (Exception ex)
            {
                LogUtilService.Error("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }
    }

    public class IMBGGameNameUtil
    {
        private static Dictionary<int, string> _gameNameMapping;

        public static string GetGameName(int? gameId)
        {
            // 預設IM棋牌
            string result = "IM棋牌";

            // gameId對應玩法名稱
            if (gameId.HasValue)
            {
                var gameNameMapping = GetGameNameMapping();

                if (gameNameMapping.ContainsKey(gameId.Value))
                {
                    result = gameNameMapping[gameId.Value];
                }
            }

            return result;
        }

        /// <summary>
        /// 遊戲代碼與遊戲名稱對應, Key: GameId, Value: 中文名稱
        /// </summary>
        private static Dictionary<int, string> GetGameNameMapping()
        {
            if (_gameNameMapping == null)
            {
                _gameNameMapping = new Dictionary<int, string>
                {
                    { 0, "游戏大厅 " },
                    { 1001, "炸金花" },
                    { 1002, "极速炸金花" },
                    { 2001, "德州扑克" },
                    { 2006, "百人德州" },
                    { 3001, "随机庄家牛牛" },
                    { 3002, "看牌抢庄牛牛" },
                    { 3003, "百人牛牛" },
                    { 3008, "通比牛牛" },
                    { 3013, "疯狂点子牛" },
                    { 4001, "港式五张" },
                    { 9004, "二人斗地主 " },
                    { 10103, "二人麻将" },
                    { 10108, "红中麻将" },
                    { 15003, "三公" },
                    { 22001, "百人龙虎" },
                    { 23001, "百人炸金花" },
                    { 24001, "百家乐" },
                    { 25001, "十三水" },
                    { 26001, "二八杠" },
                    { 27001, "二十一点" },
                    { 28001, "抢庄牌九" },
                    { 29001, "血战到底" }
                };
            }

            return _gameNameMapping;
        }
    }
}