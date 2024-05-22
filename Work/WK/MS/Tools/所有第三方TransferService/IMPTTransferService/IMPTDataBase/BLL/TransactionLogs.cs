using IMBGDataBase.Merchant;
using IMPTDataBase.Common;
using IMPTDataBase.DLL;
using IMPTDataBase.Enums;
using IMPTDataBase.Model;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMPTDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<IMPTApiParamModel, BetLogResult<List<BetResult>>>
    {
        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override IBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<IIMPTBetDetailService>(platformMerchant);
        }

        public void GetBetInfo(IMPTApiParamModel model)
        {
            string lastSearchToken = IMPTProfitLossInfo.SelectLastSearchTime();
            model.LastSearchToken = lastSearchToken;

            if (string.IsNullOrWhiteSpace(lastSearchToken))
            {
                LogsManager.Info("获取上次查询投注时间区间失败");

                return;
            }

            DateTime currentTime = DateTime.Now;
            DateTime nearTime = Utility.UnixTimeStampToDateTime(Convert.ToInt64(lastSearchToken));

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
                LogUtil.ForcedDebug("BetLogResult is null");

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

                LogsManager.Info("目前查询注单明细时间从 " +
                    model.StartTime + " 至 " +
                    model.EndTime + "，共获取 0 笔资料");
            }
            else
            {
                var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Code));

                LogsManager.Info("获取下注信息失败,返回状态码："
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
            try
            {
                HashSet<string> tpGameAccounts = betDetails.Select(s => s.PlayerName).Distinct().ToHashSet();
                Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.IMPT, tpGameAccounts);

                //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
                if (!allUserMap.Any())
                {
                    return true;
                }

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

                    if (!IMPTProfitLossInfo.ExistsOrder(bet.GameCode))
                    {
                        string memo = GetMemo(betinfo);
                        IMPTProfitLossInfo.SaveDataToLocal(bet, memo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogsManager.Error("资料储存在本地端时发生异常，详细信息：" + ex.Message + ",堆栈：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 重置下一期搜寻时间
        /// </summary>
        /// <param name="ts"></param>
        private static void ResetingNextSearchTime(string lastGameEndTs)
        {
            try
            {
                IMPTProfitLossInfo.UpdateSearchTimestamp(lastGameEndTs);
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("更新 LastSearchTime 时失败，Timestamp：" + lastGameEndTs + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
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

    //public interface IIMPTBetDetailService : IBetDetailService<IMPTApiParamModel, BetLogResult<List<BetResult>>> { }

    //public class IMPTBetDetailService : IIMPTBetDetailService
    //{
    //    public BetLogResult<List<BetResult>> GetRemoteBetDetail(IMPTApiParamModel model)
    //    {
    //        BetLogResult<List<BetResult>> result = new BetLogResult<List<BetResult>>();
    //        string apiResult = string.Empty;
    //        try
    //        {
    //            apiResult = ApiClient.GetBetLog(model);

    //            //LogsManager.Error($@"IMPT,Transfer.GetRemoteBetDetail.取得第三方資訊 model：{model} ， apiResult： {apiResult} ");

    //            if (!string.IsNullOrEmpty(apiResult))
    //            {
    //                result = apiResult.Deserialize<BetLogResult<List<BetResult>>>();
    //                //No Data Found
    //                if (result.Code == (int)APIErrorCode.NoDataFound)
    //                {
    //                    return result;
    //                }

    //                // 大於1頁5000筆
    //                if (result.Pagination != null && result.Pagination.TotalPage > 1)
    //                {
    //                    for (int i = 2; i <= result.Pagination.TotalPage; i++)
    //                    {
    //                        model.Page = i;
    //                        var MultiApiResult = ApiClient.GetBetLog(model);
    //                        var MultiResult = MultiApiResult.Deserialize<BetLogResult<List<BetResult>>>();
    //                        result.Result.AddRange(MultiResult.Result);
    //                        apiResult += "\r\n" + MultiApiResult;
    //                    }
    //                }

    //                Task.Factory.StartNew((x) =>
    //                {
    //                    string content = Convert.ToString(x);
    //                    string dailyDate = DateTime.Now.ToString("yyyyMMdd");
    //                    string sequence = string.Empty;

    //                    SaveFile saveFile = null;
    //                    DailySequence_BLL dailySequence = null;

    //                    try
    //                    {
    //                        dailySequence = new DailySequence_BLL(IMPTProfitLossInfo.dbFullName);

    //                        if (false == dailySequence.IsExistDailySequenceRecord(dailyDate))
    //                        {
    //                            dailySequence.InitializeADailySequence(dailyDate);
    //                        }

    //                        sequence = dailySequence.UpdateAndGetSequenceNumber(dailyDate).PadLeft(5, '0');

    //                        saveFile = new SaveFile(content, $"{model.ProductWallet}_{dailyDate}{sequence}");

    //                        saveFile.LoadLocationWriteTo();
    //                        saveFile.DirectoryForLocationWriteToNotExistThenCreate();
    //                        saveFile.WriteRemoteContentToOtherPlace();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        LogsManager.Error($@"IMPTProfitLossInfo.dbFullName:{IMPTProfitLossInfo.dbFullName},
    //                            sequence:{sequence},dailyDate:{dailyDate},LocationWriteTo:{saveFile.LocationWriteTo}
    //                            ,content:{content}
    //                            ", ex);
    //                    }
    //                    finally
    //                    {
    //                        saveFile = null;
    //                        dailySequence = null;
    //                    }
    //                }, apiResult);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogsManager.Error("解析投注讯息发生错误：" + ex + "，返回结果为：" + apiResult);
    //        }

    //        return result;
    //    }
    //}

    //public class IMPTBetDetailMockService : IIMPTBetDetailService
    //{
    //    public BetLogResult<List<BetResult>> GetRemoteBetDetail(IMPTApiParamModel model)
    //    {
    //        var result = new BetLogResult<List<BetResult>>
    //        {
    //            Code = (int)APIErrorCode.Success,
    //            Result = new List<BetResult>()
    //        };

    //        result.Result.Add(new BetResult()
    //        {
    //            //PlayerName = "jxD_69778",
    //            PlayerName = "ctsD_3",
    //            GameName = "test",
    //            GameDate = DateTime.Now.ToFormatDateTimeString(),
    //            Bet = 100,
    //            Win = 110,
    //            GameCode = DateTime.Now.ToUnixOfTime().ToString(),
    //        });

    //        return result;
    //    }
    //}
}