using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums.MiseOrder;
using JxLottery.Models.Lottery;
using JxLottery.Models.Lottery.Bet;
using JxLottery.Services.BonusService;
using JxLottery.Services.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolyDataBase.Extensions;
using PolyDataBase.Services;
using RestSharp;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.DBUtility;
using SLPolyGame.Web.Enums;
using SLPolyGame.Web.Model;
using SLPolyGame.Web.MSSeal.Models;
using SLPolyGame.Web.MSSeal.Models.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:PalyInfo
    /// </summary>
    public class PalyInfo
    {
        protected readonly IEnumerable<IBonusService> _bonusServiceList = new List<IBonusService>();

        private readonly IZipService _zip = null;

        private readonly DAL.CurrentLotteryInfo dal = new DAL.CurrentLotteryInfo();

        private readonly ILogger<PalyInfo> _logger = null;

        private readonly int[] ClientAllowStatus = new int[] { (int)AwardStatus.Unawarded, (int)AwardStatus.IsDone, (int)AwardStatus.Canceled, (int)AwardStatus.SystemCancel, (int)AwardStatus.SystemRefund };

        /// <summary>
        /// 機器人注單中的期號位置
        /// </summary>
        private readonly int BotIssueNoPositionIndex = 2;

        /// <summary>
        /// 取得Redis注單資訊的Key
        /// </summary>
        /// <param name="lotteryId">彩種編號</param>
        /// <returns>Redis注單資訊的Key</returns>
        private string RedisPlayInfosKey(int lotteryId, string issueNo) => $"BotData:PlayInfos_{lotteryId}_{issueNo}";

        public PalyInfo(IEnumerable<IBonusService> bonusServiceList,
            IZipService zip,
            ILogger<PalyInfo> logger)
        {
            _bonusServiceList = bonusServiceList;
            _logger = logger;
            _zip = zip;
        }

        #region Method

        /// <summary>
        ///  DataTable转List<model>
        /// </summary>
        public static List<Model.PalyInfo> ToList(DataTable dt)
        {
            // 定义集合
            List<Model.PalyInfo> ts = new List<Model.PalyInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.PalyInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.PalyInfo t = new Model.PalyInfo();

                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (value.GetType() == typeof(string) && pi.PropertyType == typeof(Int64))
                            {
                                pi.SetValue(t, Int64.Parse(value.ToString()), null);
                            }
                            else if ((value.GetType() == typeof(Int64) && pi.PropertyType == typeof(string)) ||
                                (value.GetType() == typeof(Int32) && pi.PropertyType == typeof(string)))
                                pi.SetValue(t, value.ToString(), null);
                            else
                                pi.SetValue(t, value, null);
                        }
                    }
                }

                ts.Add(t);
            }

            return ts;
        }

        #endregion Method

        public Model.PalyInfo InsertPlayInfo(Model.PalyInfo model)
        {
            Model.PalyInfo models = new Model.PalyInfo();

            if (model.SingleMoney <= 0)
            {
                models.UserName = "参数错误,单价错误！";
                return models;
            }

            decimal sumBetMoney = 0;
            decimal sumReBateMoney = 0;
            decimal minBetMoney = 0;
            decimal maxBetMoney = 0;
            var bonusService = _bonusServiceList.FirstOrDefault(x => (int)x.LotteryId == model.LotteryID);

            #region PK10特殊处理

            if (bonusService != null &&
                        bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.PK10 &&
                        (bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayTypeRadio.lm ||
                        bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayTypeRadio.dwd ||
                        bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayTypeRadio.gyh))
            {
                var vPlayNumList = model.PalyNum.Split('|');
                var newPalyNum = new StringBuilder();
                foreach (string sNum in vPlayNumList)
                {
                    var sNumList = sNum.Split(' ');
                    if (sNumList.Length != 3)
                    {
                        models.UserName = "参数错误,代码：0N5！";
                        return models;
                    }
                    decimal sPrice = 0;
                    if (!decimal.TryParse(sNumList[2], out sPrice) || sPrice <= 0)
                    {
                        models.UserName = "参数错误,代码：0N6！";
                        return models;
                    }
                    newPalyNum.AppendFormat("{0} {1} {2}|", sNumList[0], sNumList[1], sPrice * model.SingleMoney);

                    // PK10針對手動即定倍特別處理
                    var betMoney = sPrice * model.SingleMoney.Value;
                    sumBetMoney += betMoney;
                    if (maxBetMoney < betMoney)
                    {
                        maxBetMoney = betMoney;
                    }
                    if (minBetMoney > betMoney)
                    {
                        minBetMoney = betMoney;
                    }
                }
                model.PalyNum = newPalyNum.ToString().TrimEnd('|');
            }

            #endregion PK10特殊处理

            if (string.IsNullOrWhiteSpace(model.PalyNum))
            {
                models.UserName = "参数错误,代码：0C1！";
                return models;
            }

            int count = GetBetCount(model);
            if (count == 0)
            {
                models.UserName = "参数错误,代码：0B1！";
                return models;
            }
            if (count != model.NoteNum)
            {
                models.UserName = "参数错误,代码：0C2！";
                return models;
            }
            if (model.RebatePro < 0)
            {
                models.UserName = "参数错误,代码：0B！";
                return models;
            }

            //取得返點、額外反點、及限制投注號碼
            decimal RebatePro;
            GetRebateAndLimitNumber(model, out RebatePro);

            if (RebatePro < 0)
            {
                models.UserName = "参数错误,代码0PU！";
                return models;
            }
            //end
            model.UserRebatePro = RebatePro;

            if (model.RebatePro != 0
                && model.RebatePro != RebatePro)
            {
                models.UserName = "参数错误,代码0N1！";
                return models;
            }

            decimal MinReBateMoney = 0;
            decimal MaxReBateMoney = 0;
            decimal calculateBouns = 0;

            PlayInfo calMinPlayInfo = null;
            if (bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.LHC)
            {
                var current = new CurrentLotteryInfo();
                var currentResult = current.GetLotteryInfos(model.LotteryID.Value);
                bonusService.CloseDateTime = currentResult.EndTime;
            }

            calMinPlayInfo = new PlayInfo()
            {
                PlayTypeRadioId = (int)model.PlayTypeRadioID,
                Rebate = (decimal)model.RebatePro,
                UserRebate = RebatePro,
                LotteryId = (int)model.LotteryID,
            };
            calculateBouns = bonusService.GetOdds(calMinPlayInfo);
            calculateBouns *= 2;

            List<decimal> BounsList = new List<decimal>();
            if (calculateBouns == 0)
            {
                BounsList = bonusService.GetMultiOdds(calMinPlayInfo);
                if (BounsList == null)
                {
                    models.UserName = "参数错误,代码：0C8！";
                    return models;
                }
            }
            else
            {
                MaxReBateMoney = calculateBouns / 2;
            }

            model.RebateProMoney = MaxReBateMoney.ToString();

            if (MaxReBateMoney <= 0)
            {
                models.UserName = "参数错误,代码：0B9！";
                return models;
            }

            if (sumBetMoney == 0)
            {
                if ((model.SingleMoney * model.NoteNum) != model.NoteMoney)
                {
                    models.UserName = "参数错误,代码：0C9！";
                    return models;
                }
            }
            else
            {
                model.SingleMoney = maxBetMoney;
                model.NoteMoney = sumBetMoney;
            }

            // 20180731 Yark 依需求修改驗證單注金額不得多於小數第4位
            if ((model.SingleMoney % 1).ToString().Length - 2 > 3 || (model.NoteMoney % 1).ToString().Length - 2 > 3)
            {
                models.UserName = "投注金额最大为三位小数";
                return models;
            }

            if (model.RebatePro != 0)
            {
                model.RebatePro = RebatePro;
            }

            // 以下玩法根據投注內容取賠率
            if (bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId._11X5 &&
                  model.LotteryID != (int)JxLottery.Adapters.Models.Lottery.LotteryId.HS115 && (
                  bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio._11X5.PlayTypeRadio.dingdanshaung ||
                  bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio._11X5.PlayTypeRadio.caizhongwei)
               )
            {
                model.RebateProMoney = bonusService.GetMaxOdds(new PlayInfo()
                {
                    LotteryId = bonusService.LotteryId,
                    DrawNumber = model.PalyNum,
                    UserRebate = model.UserRebatePro,
                    Rebate = model.RebatePro.Value,
                    PlayTypeRadioId = model.PlayTypeRadioID.Value
                }).ToString();
            }

            //檢查是否需要壓縮投注號
            var origin = model.PalyNum;
            model.PalyNum = ZipBetPlayInfo(origin);

            //寫入db
            string replyPlayID;
            string replyUsername;
            InsertBetPlayInfo(model, out replyPlayID, out replyUsername, origin);

            models.PalyID = replyPlayID;
            if (!string.IsNullOrWhiteSpace(replyUsername))
            {
                models.UserName = replyUsername;
            }

            return models;
        }

        public Model.PalyInfo GetPlayBetByAnonymous(string playId)
        {
            string strSql = string.Concat(@"SELECT TOP 1",
                                            GetPlayBetSqlString(),
                                            @"WHERE PalyID=@PalyID
                                            ORDER BY a.PalyID DESC");
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@PalyID", playId));
            try
            {
                DataSet ds = DbHelperSQL.Main.Query(strSql, parameters.ToArray());
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return ToList(ds.Tables[0])
                                .Where(x => ClientAllowStatus.Contains(x.IsFactionAward))
                                .Select(x =>
                                {
                                    try
                                    {
                                        if (_zip.IsBase64(x.PalyNum))
                                        {
                                            x.PalyNum = _zip.Decompress(x.PalyNum);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "解壓縮注單失敗");
                                    }

                                    return x;
                                })
                                .FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPlayBetsByAnonymous fails");
            }
            return null;
        }

        public Model.PalyInfo[] GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            string strSql = string.Concat(@"SELECT",
                GetPlayBetSqlString(),
                @"WHERE a.NoteTime BETWEEN @StartTime AND @EndTime
                    #lotteryId#
                    ORDER BY a.PalyID DESC");

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@StartTime", startTime));
            parameters.Add(new SqlParameter("@EndTime", endTime));
            if (string.IsNullOrWhiteSpace(gameId))
            {
                strSql = strSql.Replace("#lotteryId#", string.Empty);
            }
            else
            {
                strSql = strSql.Replace("#lotteryId#", "AND a.[LotteryID] = @lotteryId");
                parameters.Add(new SqlParameter("@lotteryId", Convert.ToInt32(gameId)));
            }

            try
            {
                DataSet ds = DbHelperSQL.Main.Query(strSql, parameters.ToArray());

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            var result = ToList(ds.Tables[0])
                                .Where(x => ClientAllowStatus.Contains(x.IsFactionAward))
                                .Select(x =>
                                {
                                    try
                                    {
                                        if (_zip.IsBase64(x.PalyNum))
                                        {
                                            x.PalyNum = _zip.Decompress(x.PalyNum);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "解壓縮注單失敗");
                                    }

                                    return x;
                                })
                                .ToArray();

                            DataProcessing(result);

                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPlayBetsByAnonymous fails");
            }
            return new Model.PalyInfo[] { };
        }

        private static string GetPlayBetSqlString()
        {
            return @"
                       a.[PalyID]
                      ,a.[PalyCurrentNum]
                      ,a.[PlayTypeID]
                      ,a.[LotteryID]
                      ,a.[UserName]
                      ,a.[PalyNum]
                      ,a.[NoteNum]
                      ,a.[SingleMoney]
                      ,a.[NoteMoney]
                      ,a.[NoteTime]
                      ,a.[IsWin]
                      ,a.[WinMoney]
                      ,a.[IsFactionAward]
                      ,a.[PlayTypeRadioID]
                      ,a.[RebatePro]
                      ,a.[RebateProMoney]
                      ,a.[WinNum]
                      ,a.[UserID]
                      ,a.[UserRebatePro]
                      ,a.[NoticeID]
                      ,a.[LotteryTime] AS CurrentLotteryTime
                      ,a.[ResultJson]
                      ,a.[RoomId]
                      ,b.LotteryType
                      ,c.PlayTypeName
                      ,d.PlayTypeRadioName
            	      ,c.UserType
            	      ,c.PlayTypeID
            	      FROM [Inlodb_bak].[dbo].[VW_PalyInfo] a WITH(NOLOCK)
                LEFT JOIN dbo.LotteryInfo b WITH(NOLOCK) ON a.LotteryID=b.LotteryID
                LEFT JOIN dbo.PlayTypeInfo c WITH(NOLOCK) ON a.PlayTypeID=c.PlayTypeID
                LEFT JOIN dbo.PlayTypeRadio d  WITH(NOLOCK) ON a.PlayTypeRadioID=d.PlayTypeRadioID ";
        }

        private void InsertBetPlayInfo(Model.PalyInfo model, out string replyPlayID, out string replyUsername, string rowBetNumber)
        {
            var miseOrderGameId = MiseOrderGameId.GetSingle(model.LotteryID.ToString());
            if (miseOrderGameId != null)
            {
                _logger.LogInformation($@"获取用户投注消息:
                                            Content: {JsonConvert.SerializeObject(model)}");
                var dbStartTime = DateTime.Now;
                var dbTimer = Stopwatch.StartNew();
                var onlyDbStartTime = DateTime.Now;
                var onlyDbTimer = Stopwatch.StartNew();
                InsertBetPlayInfoToDB(model, out replyPlayID, out replyUsername);
                onlyDbTimer.Stop();
                if (onlyDbTimer.ElapsedMilliseconds > 5000)
                {
                    _logger.LogError($"申請扣款資料庫寫入耗時異常，開始時間:{onlyDbStartTime}，結束時間:{DateTime.Now}，耗時:{onlyDbTimer.ElapsedMilliseconds}毫秒，訂單內容:{JsonConvert.SerializeObject(model)}，注單編號:{replyPlayID}");
                }
                if (!string.IsNullOrWhiteSpace(replyPlayID) && !string.Equals(replyPlayID, "0"))
                {
                    var client = new RestClient(baseUrl: $"{GlobalCache.MSSealAddress}");

                    var requestModel = new DeductBatchRequest()
                    {
                        UserId = model.UserID.ToString(),
                        TotalAmount = (int)model.NoteMoney,
                        NickName = model.UserName,
                        RoomNumber = model.RoomId,
                        Orders = new Order[]
                        {
                        new Order()
                        {
                            Amount = model.NoteMoney.ToString(),
                            OrderNo = replyPlayID,
                            GameId = model.LotteryID.Value.ToString(),
                            GameName = model.LotteryType,
                            GameDetail = rowBetNumber,
                            PeriodNumber = model.PalyCurrentNum,
                            Type = miseOrderGameId.OrderSubType.OrderType.Value,
                            SubType = miseOrderGameId.OrderSubType.Value
                        }
                        },
                        Salt = GlobalCache.MSSealSalt
                    };
                    var request = requestModel.GetRequest();
                    var startTime = DateTime.Now;
                    var timer = Stopwatch.StartNew();
                    var rawResult = client.Execute(request);
                    timer.Stop();
                    if (timer.ElapsedMilliseconds > 5000)
                    {
                        _logger.LogError($"申請扣款耗時異常，開始時間:{startTime}，結束時間:{DateTime.Now}，耗時:{timer.ElapsedMilliseconds}毫秒，訂單內容:{JsonConvert.SerializeObject(requestModel)}，注單編號:{replyPlayID}");
                    }

                    //2024-05-02 確認一分快三在LIVE上的呼叫結果，後續要拿掉
                    if (model.LotteryID == 65)
                    {
                        _logger.LogError($@"纪录投注讯息:
                                            Request: {System.Text.Json.JsonSerializer.Serialize(requestModel)},
                                            Result: {System.Text.Json.JsonSerializer.Serialize(rawResult)}");
                    }
                    
                    if (rawResult.IsSuccessful)
                    {
                        var result = new BalanceResult();
                        try
                        {
                            result = JsonConvert.DeserializeObject<BalanceResult>(rawResult.Content);
                            if (result?.Data != null && result.Success)
                            {
                                UpdateBetPlayInfoStatus(replyPlayID, model.UserID.Value, (int)AwardStatus.Unawarded);
                                var bettingRawResult = null as string;
                                var httpStatusCode = System.Net.HttpStatusCode.BadRequest;

                                // TODO 2023-09-22 PM說直接傳01，但是如果01遇到問題要濾掉
                                // if (!string.Equals(model.RoomId, "0"))
                                {
                                    // 滿版彩票RoomId為0，不去通知投注
                                    try
                                    {
                                        // 發送投注訊息
                                        var bettingMsg = new BetingMessageRequest(model, replyPlayID, GlobalCache.MSSealSalt);
                                        bettingRawResult = bettingMsg.GetResponse(GlobalCache.MSSealAddress,
                                            out httpStatusCode,
                                            null);

                                        if (httpStatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            var bettingResult = JsonConvert.DeserializeObject<BalanceResult>(bettingRawResult);
                                            if (!bettingResult.Success)
                                            {
                                                _logger.LogError($@"推送投注消息时异常 By Api:
                                            Msg: {bettingResult?.Msg ?? bettingResult?.Error ?? string.Empty},
                                            Request: {JsonConvert.SerializeObject(bettingMsg)},
                                            Content: {bettingRawResult},
                                            Result: {JsonConvert.SerializeObject(bettingResult)}");
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogError($@"推送投注消息时异常 By Api:
                                            Request: {JsonConvert.SerializeObject(bettingMsg)},
                                            Content: {bettingRawResult},
                                            Result: {httpStatusCode}");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, $@"推送投注消息时异常 By Api:
                                    ErrorMessage: {bettingRawResult ?? string.Empty}");
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(result?.Msg ?? result?.Error))
                            {
                                UpdateBetPlayInfoStatus(replyPlayID, model.UserID.Value, (int)AwardStatus.Abandoned);
                                replyUsername = result?.Msg ?? result.Error;
                                _logger.LogError($"获取用户信息时异常 By Api: Msg: {result?.Msg ?? result?.Error ?? string.Empty}, Request:{JsonConvert.SerializeObject(requestModel)}, Content: {rawResult.Content}, Result: {JsonConvert.SerializeObject(result)}");
                            }
                            else
                            {
                                UpdateBetPlayInfoStatus(replyPlayID, model.UserID.Value, (int)AwardStatus.Abandoned);
                                replyUsername = "投注失败，请稍后重试";
                                _logger.LogError($"获取用户信息时异常 By Api: Msg: {result?.Msg ?? result?.Error ?? string.Empty}, Request:{JsonConvert.SerializeObject(requestModel)}, Content: {rawResult.Content}, Result: {JsonConvert.SerializeObject(result)}");
                            }
                        }
                        catch (Exception ex)
                        {
                            replyUsername = "投注失败，请稍后重试";
                            _logger.LogError(ex, $"获取用户信息时异常 By Api: ErrorMessage: {rawResult.ErrorMessage}, Request:{JsonConvert.SerializeObject(requestModel)}, Content: {rawResult.Content}, Result: {JsonConvert.SerializeObject(result)}");
                        }
                    }
                    else
                    {
                        replyUsername = "投注失败，请稍后重试";
                        UpdateBetPlayInfoStatus(replyPlayID, model.UserID.Value, (int)AwardStatus.Abandoned);
                        _logger.LogError($"获取用户信息时异常 By Api: ErrorMessage: {rawResult.ErrorMessage}, Request:{JsonConvert.SerializeObject(requestModel)}, Content: {rawResult.Content}");
                    }
                }

                dbTimer.Stop();
                if (dbTimer.ElapsedMilliseconds > 5000)
                {
                    _logger.LogError($"申請扣款總耗時異常，開始時間:{dbStartTime}，結束時間:{DateTime.Now}，耗時:{dbTimer.ElapsedMilliseconds}毫秒，訂單內容:{JsonConvert.SerializeObject(model)}，注單編號:{replyPlayID}");
                }
            }
            else
            {
                _logger.LogError($@"彩種編號不存在:
                                            Content: {JsonConvert.SerializeObject(model)}");
                replyPlayID = "0";
                replyUsername = "投注失败，请稍后重试";
            }
        }

        /// <summary>
        /// 修改訂單狀態
        /// </summary>
        /// <param name="PalyID">注單編號</param>
        /// <param name="userid">使用者編號</param>
        /// <param name="status">訂單狀態</param>
        private void UpdateBetPlayInfoStatus(string PalyID, int userid, int status)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@PalyID", SqlDbType.VarChar, 32),
                new SqlParameter("@Userid", SqlDbType.Int),
                new SqlParameter("@Status", SqlDbType.Int)
            };

            parameters[0].Value = PalyID;
            parameters[1].Value = userid;
            parameters[2].Value = status;

            DataSet ds = DbHelperSQL.RunProcedure("Pro_UpdatePlayInfoStatus", parameters, "ds");
            if (ds.Tables.Count == 0)
            {
                if (ds.Tables[0].Rows[0]["Result"].ToString() != "0")
                {
                    _logger.LogError($"更新注單狀態失敗, Result:{ds.Tables[0].Rows[0]["Result"].ToString()}");
                }
            }
        }

        private int GetBetCount(Model.PalyInfo model)
        {
            var result = 0;
            var bonusService = _bonusServiceList.FirstOrDefault(x => (int)x.LotteryId == model.LotteryID);
            if (bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.LHC)
            {
                var current = new CurrentLotteryInfo();
                var currentResult = current.GetLotteryInfos(model.LotteryID.Value);
                bonusService.CloseDateTime = currentResult.EndTime;
            }

            result = bonusService.GetBetCount(new PlayInfo()
            {
                LotteryId = (int)model.LotteryID,
                PlayTypeId = (int)model.PlayTypeID,
                PlayTypeRadioId = (int)model.PlayTypeRadioID,
                DrawNumber = model.PalyNum
            });
            return result;
        }

        public Model.PalyInfo InsertPlayInfo_v1(Model.PalyInfo model)
        {
            Model.PalyInfo models = new Model.PalyInfo();
            var bonusService = _bonusServiceList.FirstOrDefault(x => (int)x.LotteryId == model.LotteryID);
            try
            {
                if (!SafeSql.CheckParams(model.PalyCurrentNum) || !SafeSql.CheckParams(model.UserName) || !SafeSql.CheckParams(model.RebateProMoney))
                {
                    models.UserName = "参数错误,代码：0UIL！";
                    return models;
                }
            }
            catch (Exception e)
            {
                models.UserName = "参数错误,代码：0CLD！";
                return models;
            }

            Constant.PlayTypeRadio cp; // 玩法名稱
            cp = (Constant.PlayTypeRadio)System.Enum.Parse(typeof(Constant.PlayTypeRadio), model.PlayTypeRadioID.ToString());

            Constant.PlayType PlayType = (Constant.PlayType)model.PlayTypeID;

            if (bonusService != null)
            {
                if (bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.PK10)
                {
                    // PK10龙虎、大小、单双特殊处理
                    if (bonusService.GetPlayTypeId(model.PlayTypeID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayType.LH)
                    {
                        if (!ManBase.isLH(model.PalyNum))
                        {
                            models.UserName = "参数错误,代码：0CET！";
                            return models;
                        }
                        else
                        {
                            model.PalyNum = model.PalyNum.Replace(" ", "").Replace("龙", "L").Replace("虎", "H");
                        }
                    }
                    else if (bonusService.GetPlayTypeId(model.PlayTypeID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayType.BS)
                    {
                        if (!ManBase.isBS(model.PalyNum))
                        {
                            models.UserName = "参数错误,代码：0CET！";
                            return models;
                        }
                        else
                        {
                            model.PalyNum = model.PalyNum.Replace(" ", "").Replace("大", "1").Replace("小", "0");
                        }
                    }
                    else if (bonusService.GetPlayTypeId(model.PlayTypeID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayType.SD)
                    {
                        if (!ManBase.isSD(model.PalyNum))
                        {
                            models.UserName = "参数错误,代码：0CET！";
                            return models;
                        }
                        else
                        {
                            model.PalyNum = model.PalyNum.Replace(" ", "").Replace("单", "1").Replace("双", "0");
                        }
                    }
                }
                else if (bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.SSC)
                {
                    // 时时彩大小单双 特殊处理
                    if (bonusService.GetPlayTypeId(model.PlayTypeID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.SSC.PlayType.Dxds)
                    {
                        if (!ManBase.isDXDS(model.PalyNum))
                        {
                            models.UserName = "参数错误,代码：0CET！";
                            return models;
                        }
                        else
                        {
                            model.PalyNum = model.PalyNum.Replace(" ", "")
                                            .Replace("大", "1")
                                            .Replace("小", "2")
                                            .Replace("单", "3")
                                            .Replace("双", "4");
                        }
                    }
                }
            }
            else if (!ManBase.isNumberic(model.PalyNum))
            {
                models.UserName = "参数错误,代码：0CET！";
                return models;
            }

            int count = GetBetCount(model);

            if (count != model.NoteNum)
            {
                models.UserName = "参数错误,代码：0CE！";
                return models;
            }
            if ((model.SingleMoney * model.NoteNum) != model.NoteMoney)
            {
                models.UserName = "参数错误,代码：0A！";
                return models;
            }

            // 20180731 Yark 依需求修改驗證單注金額不得多於小數第4位
            if ((model.SingleMoney % 1).ToString().Length - 2 > 3 || (model.NoteMoney % 1).ToString().Length - 2 > 3)
            {
                models.UserName = "投注金额最大为三位小数";
                return models;
            }

            // 前端送入的投注返點值
            if (model.RebatePro < 0)
            {
                models.UserName = "参数错误,代码：0B！";
                return models;
            }

            //取得返點、額外反點、及限制投注號碼
            decimal RebatePro;
            GetRebateAndLimitNumber(model, out RebatePro);

            if (RebatePro < 0)
            {
                models.UserName = "参数错误,代码0PU！";
                return models;
            }

            model.UserRebatePro = RebatePro;
            decimal MinReBateMoney = 0;
            decimal MaxReBateMoney = 0;

            if (bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.LHC)
            {
                var current = new CurrentLotteryInfo();
                var currentResult = current.GetLotteryInfos(model.LotteryID.Value);
                bonusService.CloseDateTime = currentResult.EndTime;
            }

            var calMaxPlayInfo = new PlayInfo()
            {
                DrawNumber = model.PalyNum,
                PlayTypeRadioId = (int)model.PlayTypeRadioID,
                Rebate = 0,
                UserRebate = RebatePro,
                LotteryId = (int)model.LotteryID,
            };

            var calMinPlayInfo = new PlayInfo()
            {
                DrawNumber = model.PalyNum,
                PlayTypeRadioId = (int)model.PlayTypeRadioID,
                Rebate = RebatePro,
                UserRebate = RebatePro,
                LotteryId = (int)model.LotteryID,
            };

            MaxReBateMoney = bonusService.GetOdds(calMaxPlayInfo);
            MinReBateMoney = bonusService.GetOdds(calMinPlayInfo);

            if (MaxReBateMoney == 0)
            {
                if (bonusService.GameTypeId >= (int)JxLottery.Models.Lottery.GameTypeId.LHC)
                {
                    MaxReBateMoney = bonusService.GetMaxOdds(calMaxPlayInfo);
                    MinReBateMoney = bonusService.GetMaxOdds(calMinPlayInfo);
                }
                else
                {
                    List<decimal> maxList = bonusService.GetMultiOdds(calMaxPlayInfo);
                    List<decimal> minList = bonusService.GetMultiOdds(calMinPlayInfo);
                    if (maxList != null)
                    {
                        MaxReBateMoney = maxList.Max();
                        MinReBateMoney = minList.Max();
                    }
                }
            }

            MaxReBateMoney *= 2;
            MinReBateMoney *= 2;

            decimal tmpRebateProMoney = 0;
            if (model.RebatePro == 0)
            {
                //model.RebateProMoney = (MaxReBateMoney / 2).ToString("0.00");
                tmpRebateProMoney = (MaxReBateMoney / 2);
            }
            else
            {
                if (model.RebatePro != RebatePro)
                {
                    _logger.LogInformation("下单时,用户名：{0},用户ID：{1},错误提示：上传返点和数据库中不一致,上传返点：{2},数据库中返点：{3}"
                          , model.UserName
                          , model.UserID.GetValueOrDefault()
                          , model.RebatePro.GetValueOrDefault()
                          , RebatePro);
                }
                //16.4.20
                //model.RebatePro = RebatePro;
                model.RebatePro = RebatePro;
                //16.4.20end
                //model.RebateProMoney = (MinReBateMoney / 2).ToString("0.00");
                tmpRebateProMoney = (MinReBateMoney / 2);
            }

            // 以下玩法根據投注內容取賠率
            if (model.PlayTypeRadioID.HasValue &&
                bonusService.GameTypeId == (int)JxLottery.Models.Lottery.GameTypeId.K3 &&
                (bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.K3.PlayTypeRadio.ClassHz ||
                 bonusService.GetPlayTypeRadioId(model.PlayTypeRadioID.Value) == (int)JxLottery.Models.Lottery.PlayTypeRadio.K3.PlayTypeRadio.ClassSanBthSbthz))
            {
                model.RebateProMoney = bonusService.GetMaxOdds(new PlayInfo()
                {
                    LotteryId = bonusService.LotteryId,
                    DrawNumber = model.PalyNum,
                    UserRebate = model.UserRebatePro,
                    Rebate = model.RebatePro.Value,
                    PlayTypeRadioId = model.PlayTypeRadioID.Value
                }).ToString();
            }
            else
            {
                model.RebateProMoney = tmpRebateProMoney.ToString();
            }

            //檢查是否需要壓縮投注號
            var origin = model.PalyNum;
            model.PalyNum = ZipBetPlayInfo(model.PalyNum);

            //寫入db
            string replyPlayID;
            string replyUsername;
            InsertBetPlayInfo(model, out replyPlayID, out replyUsername, origin);

            models.PalyID = replyPlayID;
            if (!string.IsNullOrWhiteSpace(replyUsername))
            {
                models.UserName = replyUsername;
            }

            return models;
        }

        /// <summary>
        /// 取得返點、額外反點、及限制投注號碼
        /// </summary>
        /// <param name="model">投注資訊</param>
        /// <param name="RebatePro">返回-返點</param>
        /// <param name="NewAddedRebatePro">返回-額外反點</param>
        /// <param name="LimitNum">返回-限制投注號碼</param>
        private void GetRebateAndLimitNumber(Model.PalyInfo model,
            out decimal RebatePro)
        {
            SqlParameter[] parameters1 = {
                new SqlParameter("@PlayTypeID", SqlDbType.Int,4),
                new SqlParameter("@UserID", SqlDbType.Int,4),
                new SqlParameter("@LotteryID",SqlDbType.Int),
                new SqlParameter("@PlayTypeRadioID",SqlDbType.Int),
                new SqlParameter("@PalyCurrentNum",SqlDbType.NVarChar,50)
            };
            parameters1[0].Value = model.PlayTypeID;
            parameters1[1].Value = model.UserID;
            parameters1[2].Value = model.LotteryID;
            parameters1[3].Value = model.PlayTypeRadioID;
            parameters1[4].Value = model.PalyCurrentNum;

            string strb = @"
				DECLARE @RebatePro NUMERIC(18, 4);

				SELECT TOP 1
                    @RebatePro=(ISNULL(RebatePro, 0) + ISNULL(AddedRebatePro, 0))
				FROM     dbo.UserInfo WITH ( NOLOCK )
				WHERE    UserID = @UserID

				SELECT  ISNULL(@RebatePro, 0) AS RebatePro;";

            DataSet dt = DbHelperSQL.Query(strb, parameters1);
            RebatePro = 0;
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (dt.Tables[0].Rows[0]["RebatePro"].ToString() != "")
                {
                    decimal.TryParse(dt.Tables[0].Rows[0]["RebatePro"].ToString(), out RebatePro);
                }
            }
        }

        /// <summary>
        /// 壓縮投注號碼
        /// </summary>
        /// <param name="playNum">投注號碼</param>
        private string ZipBetPlayInfo(string playNum)
        {
            string playNumResult = playNum;
            if (playNum.Length > 100)
            {
                string NewPlayNum = string.Empty;
                try
                {
                    NewPlayNum = _zip.Compress(playNum);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("下单时压缩数据失败,描述:" + ex.Message);
                }
                if (!string.IsNullOrWhiteSpace(NewPlayNum))
                {
                    playNumResult = "Base64:" + NewPlayNum;
                }
            }
            return playNumResult;
        }

        /// <summary>
        /// 將投注內容寫入db
        /// </summary>
        /// <param name="model">玩法投注資訊</param>
        /// <param name="playID">返回-注單編號</param>
        /// <param name="username">返回-投注人</param>
        private static void InsertBetPlayInfoToDB(Model.PalyInfo model, out string playID, out string username)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PalyCurrentNum", SqlDbType.NVarChar,100),
                new SqlParameter("@PalyNum", SqlDbType.NVarChar),
                new SqlParameter("@PlayTypeID", SqlDbType.Int,4),
                new SqlParameter("@LotteryID", SqlDbType.Int,4),
                new SqlParameter("@UserName", SqlDbType.NVarChar,50),
                new SqlParameter("@NoteNum", SqlDbType.Int,4),
                new SqlParameter("@SingleMoney", SqlDbType.Decimal,9),
                new SqlParameter("@NoteMoney", SqlDbType.Decimal,9),
                new SqlParameter("@PlayTypeRadioID", SqlDbType.Int,4),
                new SqlParameter("@RebatePro", SqlDbType.Decimal,9),
                new SqlParameter("@RebateProMoney", SqlDbType.NVarChar,50),
                new SqlParameter("@UserID", SqlDbType.Int,4),
                new SqlParameter("@OrderKey", SqlDbType.NVarChar,100),
                new SqlParameter("@CurrencyUnit", SqlDbType.Decimal),
                new SqlParameter("@Ratio", SqlDbType.Int),
                new SqlParameter("@SourceType", SqlDbType.NVarChar,20),
                new SqlParameter("@ClientIP", SqlDbType.VarChar,128),
                new SqlParameter("@RoomId", SqlDbType.NVarChar,50),
            };
            parameters[0].Value = model.PalyCurrentNum;
            parameters[1].Value = model.PalyNum;
            parameters[2].Value = model.PlayTypeID;
            parameters[3].Value = model.LotteryID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.NoteNum;
            parameters[6].Value = model.SingleMoney;
            parameters[7].Value = model.NoteMoney;
            parameters[8].Value = model.PlayTypeRadioID;
            parameters[9].Value = model.RebatePro;
            parameters[10].Value = model.RebateProMoney;
            parameters[11].Value = model.UserID;
            parameters[12].Value = string.Empty;
            parameters[13].Value = model.CurrencyUnit;
            parameters[14].Value = model.Ratio;
            parameters[15].Value = model.SourceType;
            parameters[16].Value = model.ClientIP;
            parameters[17].Value = model.RoomId;

            DataSet ds = null;
            playID = string.Empty;

            ds = DbHelperSQL.RunProcedure("Pro_AddOrder", parameters, "tab");

            username = "";
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["PalyID"].ToString() != "")
                    {
                        playID = ds.Tables[0].Rows[0]["PalyID"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["UserName"].ToString() != "")
                    {
                        username = ds.Tables[0].Rows[0]["UserName"].ToString();
                    }
                }
            }
        }

        public int GetUserType(int playTypeID)
        {
            string strSql = @"SELECT UserType FROM PlayTypeInfo WITH (NOLOCK) WHERE PlayTypeID = @PlayTypeID";

            var inPlayTypeID = new SqlParameter("@PlayTypeID", SqlDbType.Int);

            inPlayTypeID.Value = playTypeID;

            DataSet ds = DbHelperSQL.Bak.Query(strSql, inPlayTypeID);
            if (ds != null)
            {
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var rowUserType = ds.Tables[0].Rows[0]["UserType"];
                        if (rowUserType != DBNull.Value)
                        {
                            return Convert.ToInt32(rowUserType);
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// SL获取当天下单前3条数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<Model.PalyInfo> GetBetRecord(int UserID)
        {
            string strSql = @"select top 3 a.[PalyID]
							  ,a.[PalyCurrentNum]
							  ,a.[PlayTypeID]
							  ,a.[LotteryID]
							  ,a.[UserName]
							  ,a.[NoteNum]
							  ,a.[SingleMoney]
							  ,a.[NoteMoney]
							  ,a.[NoteTime]
							  ,a.[IsWin]
							  ,a.[WinMoney]
							  ,a.[IsFactionAward]
							  ,a.[PlayTypeRadioID]
							  ,a.[RebatePro]
							  ,a.[RebateProMoney]
							  ,a.[WinNum]
							  ,a.[UserID]
							  ,a.[NoticeID]
							  ,a.[LotteryTime]
                              ,a.[ResultJson]
                              ,a.[RoomId],b.LotteryType,c.PlayTypeName,d.PlayTypeRadioName from dbo.PalyInfo a with(nolock)
						left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID
						left join dbo.PlayTypeInfo c with(nolock) on a.PlayTypeID=c.PlayTypeID
						left join dbo.PlayTypeRadio d with(nolock) on a.PlayTypeRadioID=d.PlayTypeRadioID
									where UserID=@UserID and IsFactionAward=0 and NoteTime between @starttime and @endtime order by a.NoteTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                                        new SqlParameter("@starttime",SqlDbType.DateTime),
                                        new SqlParameter("@endtime",SqlDbType.DateTime)};
            parameters[0].Value = UserID;
            parameters[1].Value = System.DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[2].Value = System.DateTime.Now.Date.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        List<Model.PalyInfo> list = ToList(ds.Tables[0]);
                        if (list.Count > 0)
                        {
                            for (int n = 0; n < list.Count; n++)
                            {
                                if (list[n].PalyNum.Length > 200)
                                {
                                    list[n].PalyNum = list[n].PalyNum.Substring(0, 200) + "...";
                                }
                            }
                        }
                        return list;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 游戏记录分页总数
        /// </summary>
        public Model.PalyInfo GetUserPalyBetCount(int lotteryId, string PalyCurrentNum, string iswin, DateTime starttime, DateTime endtime, int userid)
        {
            if (!SafeSql.CheckParams(PalyCurrentNum) || !SafeSql.CheckParams(iswin))
            {
                return null;
            }
            string strSql = @"select count(*) as PalyID,isnull(sum(isnull(NoteMoney,0)),0) as NoteMoney,
							isnull(sum(isnull(WinMoney,0)),0) as WinMoney
							from [Inlodb_bak].[dbo].[VW_PalyInfo] a with(nolock)
							where 1=1 ";
            string filtrate = "";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                                        new SqlParameter("@PalyCurrentNum", SqlDbType.NVarChar,50),
                                        new SqlParameter("@iswin", SqlDbType.Bit),
                                        new SqlParameter("@starttime", SqlDbType.DateTime),
                                        new SqlParameter("@endtime", SqlDbType.DateTime),
                                        new SqlParameter("@UserID", SqlDbType.Int)
                                        };
            if (lotteryId > 0)
            {
                filtrate += " and  a.LotteryID=@LotteryID ";
                parameters[0].Value = lotteryId;
            }
            if (PalyCurrentNum != "")
            {
                filtrate += " and  a.PalyCurrentNum=@PalyCurrentNum ";
                parameters[1].Value = PalyCurrentNum;
            }
            if (iswin != "")
            {
                filtrate += " and  a.IsWin=@iswin ";
                if (iswin == "1")
                {
                    parameters[2].Value = true;
                }
                else
                {
                    parameters[2].Value = false;
                }
            }

            if (endtime != null && starttime != null)
            {
                filtrate += " and  a.NoteTime between @starttime and @endtime ";
                parameters[3].Value = starttime;
                parameters[4].Value = endtime;
            }

            filtrate += " and  a.UserID=@UserID ";
            parameters[5].Value = userid;

            strSql += filtrate;
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            Model.PalyInfo model = new Model.PalyInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PalyID"].ToString() != "")
                {
                    model.PalyID = ds.Tables[0].Rows[0]["PalyID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["NoteMoney"].ToString() != "")
                {
                    model.NoteMoney = decimal.Parse(ds.Tables[0].Rows[0]["NoteMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WinMoney"].ToString() != "")
                {
                    model.WinMoney = decimal.Parse(ds.Tables[0].Rows[0]["WinMoney"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 指定用户投注记录
        /// </summary>
        public List<Model.PalyInfo> GetUserPalyBet(int pageSize, int pageNum, int lotteryId, string PalyCurrentNum, string iswin, DateTime starttime, DateTime endtime, int userid)
        {
            if (!SafeSql.CheckParams(PalyCurrentNum) || !SafeSql.CheckParams(iswin))
            {
                return null;
            }
            int p = 0;
            bool isint = int.TryParse(pageSize.ToString(), out p);
            if (!isint)
            {
                return null;
            }
            bool isintn = int.TryParse(pageNum.ToString(), out p);
            if (!isintn)
            {
                return null;
            }
            string strSql = "";
            strSql += "select  TOP " + pageSize + " * FROM ";
            strSql += @"(  SELECT  ROW_NUMBER() OVER ( ORDER BY a.NoteTime DESC ) AS RowNumber ,
                                a.PalyCurrentNum ,
                                a.PalyID ,
                                a.NoteNum ,
                                a.NoteMoney ,
                                a.NoteTime ,
                                a.WinMoney ,
                                b.LotteryType ,
                                c.PlayTypeName ,
                                d.PlayTypeRadioName ,
                                ( ISNULL(a.WinMoney, 0) - ISNULL(a.NoteMoney, 0) ) AS WinPossMoney
                        FROM    [Inlodb_bak].[dbo].[VW_PalyInfo] a WITH ( NOLOCK )
                        left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID
                        left join dbo.PlayTypeInfo c with(nolock) on a.PlayTypeID=c.PlayTypeID
                        left join dbo.PlayTypeRadio d with(nolock) on a.PlayTypeRadioID=d.PlayTypeRadioID
                        where 1=1";

            string filtrate = "";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                                        new SqlParameter("@PalyCurrentNum", SqlDbType.NVarChar,50),
                                        new SqlParameter("@iswin", SqlDbType.Bit),
                                        new SqlParameter("@starttime", SqlDbType.DateTime),
                                        new SqlParameter("@endtime", SqlDbType.DateTime),
                                        new SqlParameter("@UserID", SqlDbType.Int)
                                        };
            if (lotteryId > 0)
            {
                filtrate += " and  a.LotteryID=@LotteryID ";
                parameters[0].Value = lotteryId;
            }
            if (PalyCurrentNum != "")
            {
                filtrate += " and  a.PalyCurrentNum=@PalyCurrentNum ";
                parameters[1].Value = PalyCurrentNum;
            }
            if (iswin != "")
            {
                filtrate += " and  a.IsWin=@iswin ";
                if (iswin == "1")
                {
                    parameters[2].Value = true;
                }
                else
                {
                    parameters[2].Value = false;
                }
            }

            if (endtime != null && starttime != null)
            {
                filtrate += " and  a.NoteTime between @starttime and @endtime ";
                parameters[3].Value = starttime;
                parameters[4].Value = endtime;
            }

            filtrate += " and  a.UserID=@UserID ";
            parameters[5].Value = userid;

            strSql += filtrate;

            strSql += " ) A WHERE RowNumber > " + pageNum;
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 指定用户投注记录
        /// </summary>
        public List<Model.PalyInfo> GetAGUserPalyBet(int pageSize, int pageNum, int lotteryId, string PalyCurrentNum, string iswin, DateTime starttime, DateTime endtime, int userid)
        {
            if (!SafeSql.CheckParams(PalyCurrentNum) || !SafeSql.CheckParams(iswin))
            {
                return null;
            }
            //int p = 0;
            //bool isint = int.TryParse(pageSize.ToString(), out p);
            //if (!isint)
            //{
            //    return null;
            //}
            //bool isintn = int.TryParse(pageNum.ToString(), out p);
            //if (!isintn)
            //{
            //    return null;
            //}
            string strSql = "";
            strSql += "select  TOP " + pageSize + " * FROM ";
            strSql += @"(  SELECT ROW_NUMBER() OVER (ORDER BY a.BetTime  desc) AS RowNumber, '' as PalyCurrentNum,a.ProfitLossID as PalyID,0 as NoteNum,a.ProfitLossMoney as NoteMoney,a.BetTime as NoteTime,a.WinMoney, a.Memo as LotteryType,'' as PlayTypeName,'' as PlayTypeRadioName,(isnull(a.WinMoney,0)-isnull(a.ProfitLossMoney,0))as WinPossMoney
						from dbo.AGProfitLoss a with(nolock)
						where ProfitLossType='亏盈' ";

            string filtrate = "";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                                        new SqlParameter("@PalyCurrentNum", SqlDbType.NVarChar,50),
                                        new SqlParameter("@iswin", SqlDbType.Int),
                                        new SqlParameter("@starttime", SqlDbType.DateTime),
                                        new SqlParameter("@endtime", SqlDbType.DateTime),
                                        new SqlParameter("@UserID", SqlDbType.Int)
                                        };
            //if (lotteryId > 0)
            //{
            //    filtrate += " and  a.LotteryID=@LotteryID ";
            //    parameters[0].Value = lotteryId;
            //}
            //if (PalyCurrentNum != "")
            //{
            //    filtrate += " and  a.PalyCurrentNum=@PalyCurrentNum ";
            //    parameters[1].Value = PalyCurrentNum;
            //}
            if (iswin != "")
            {
                filtrate += " and  a.IsWin=@iswin ";
                if (iswin == "1")
                {
                    parameters[2].Value = 1;
                }
                else if (iswin == "0")
                {
                    parameters[2].Value = 0;
                }
                else
                {
                    parameters[2].Value = -1;
                }
            }

            if (endtime != null && starttime != null)
            {
                filtrate += " and  a.BetTime between @starttime and @endtime ";
                parameters[3].Value = starttime;
                parameters[4].Value = endtime;
            }

            filtrate += " and  a.UserID=@UserID ";
            parameters[5].Value = userid;

            strSql += filtrate;

            strSql += " ) A WHERE RowNumber > " + pageNum;
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// <summary>
        /// 获取单个订单的信息
        /// </summary>
        /// <param name="PalyID"></param>
        /// <returns></returns>
        public Model.PalyInfo GetPalyIDPalyBet(int PalyID, int userid)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@PalyID", SqlDbType.Int),
                new SqlParameter("@CheckUserID", SqlDbType.Int)
            };

            parameters[0].Value = PalyID;
            parameters[1].Value = userid;
            Model.PalyInfo model = new Model.PalyInfo();

            DataSet ds = DbHelperSQL.RunProcedure("Pro_GetPlayDetail", parameters, "ds");
            if (ds.Tables.Count == 0)
            {
                return null;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PalyID"].ToString() != "")
                {
                    model.PalyID = ds.Tables[0].Rows[0]["PalyID"].ToString();
                }
                model.PalyCurrentNum = ds.Tables[0].Rows[0]["PalyCurrentNum"].ToString();
                if (ds.Tables[0].Rows[0]["PlayTypeID"].ToString() != "")
                {
                    model.PlayTypeID = int.Parse(ds.Tables[0].Rows[0]["PlayTypeID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["NoticeID"].ToString() != "")
                {
                    model.NoticeID = ds.Tables[0].Rows[0]["NoticeID"].ToString();
                }

                model.PalyNum = ds.Tables[0].Rows[0]["PalyNum"].ToString();

                try
                {
                    if (_zip.IsBase64(model.PalyNum))
                    {
                        model.PalyNum = _zip.Decompress(model.PalyNum);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("查单时异常，描述:" + ex.Message);
                    model.PalyNum = "";
                }

                if ((model.LotteryID == 18 && model.PlayTypeID == 139) ||
                    (model.LotteryID == 31 && model.PlayTypeID == 945) ||
                    (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.AHK3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.AHK3.PlayType.ClassSanLhtx) ||
                    (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.GSK3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.GSK3.PlayType.ClassSanLhtx) ||
                    (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR5K3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.VR5K3.PlayType.ClassSanLhtx)
                   )
                {
                    model.PalyNum = "三连号通选";
                }
                else if ((model.LotteryID == 18 && model.PlayTypeID == 142) ||
                         (model.LotteryID == 31 && model.PlayTypeID == 948) ||
                         (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.AHK3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.AHK3.PlayType.ClassSanThtx) ||
                         (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.GSK3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.GSK3.PlayType.ClassSanThtx) ||
                         (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR5K3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.VR5K3.PlayType.ClassSanThtx)
                        )
                {
                    model.PalyNum = "三同号通选";
                }
                else if ((model.LotteryID == 18 && model.PlayTypeID == 148) ||
                         (model.LotteryID == 31 && model.PlayTypeID == 954) ||
                         (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.AHK3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.AHK3.PlayType.ClassDs) ||
                         (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.GSK3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.GSK3.PlayType.ClassDs) ||
                         (model.LotteryID == (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR5K3 && model.PlayTypeID == (int)JxLottery.Adapters.Models.Lottery.PlayTypeRadio.K3.VR5K3.PlayType.ClassDs)
                        )
                {
                    model.PalyNum = model.PalyNum.Replace("1", "单").Replace("0", "双");
                }

                var noteMoney = ds.Tables[0].Rows[0]["NoteMoney"];
                var winMoney = ds.Tables[0].Rows[0]["WinMoney"];
                var winNum = ds.Tables[0].Rows[0]["WinNum"];
                var rebatePro = ds.Tables[0].Rows[0]["RebatePro"]; ;

                if (noteMoney != DBNull.Value)
                {
                    model.NoteMoney = Convert.ToDecimal(noteMoney);
                }

                if (winMoney != DBNull.Value)
                {
                    model.WinMoney = Convert.ToDecimal(winMoney);
                }

                if (winNum != DBNull.Value)
                {
                    model.WinNum = Convert.ToInt32(winNum);
                }

                if (rebatePro != DBNull.Value)
                {
                    model.RebatePro = (Convert.ToDecimal(rebatePro) * Convert.ToDecimal(model.NoteMoney)).ExtFloor(4);
                }

                if (model.WinNum > 0)
                {
                    model.WinPossMoney = Math.Abs(model.WinMoney.Value + model.NoteMoney.Value - model.RebatePro.Value);
                }

                model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                if (ds.Tables[0].Rows[0]["NoteNum"].ToString() != "")
                {
                    model.NoteNum = int.Parse(ds.Tables[0].Rows[0]["NoteNum"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Multiple"].ToString() != "")
                {
                    model.Multiple = int.Parse(ds.Tables[0].Rows[0]["Multiple"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SingleMoney"].ToString() != "")
                {
                    model.SingleMoney = decimal.Parse(ds.Tables[0].Rows[0]["SingleMoney"].ToString());
                }

                if (ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString() != "")
                {
                    model.CurrentLotteryTime = DateTime.Parse(ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["NoteTime"].ToString() != "")
                {
                    model.NoteTime = DateTime.Parse(ds.Tables[0].Rows[0]["NoteTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsWin"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsWin"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsWin"].ToString().ToLower() == "true"))
                    {
                        model.IsWin = true;
                    }
                    else
                    {
                        model.IsWin = false;
                    }
                }

                var strFactionAward = ds.Tables[0].Rows[0]["IsFactionAward"].ToString();
                if (strFactionAward != string.Empty)
                {
                    model.IsFactionAward = int.Parse(strFactionAward);
                    var awardEnum = (AwardStatus)Enum.Parse(typeof(AwardStatus), strFactionAward);
                    model.StFactionAward = awardEnum.ExtGetDescription();
                }
                if (ds.Tables[0].Rows[0]["PlayTypeRadioID"].ToString() != "")
                {
                    model.PlayTypeRadioID = int.Parse(ds.Tables[0].Rows[0]["PlayTypeRadioID"].ToString());
                }

                if (ds.Tables[0].Rows[0]["UserRebatePro"].ToString() != "")
                {
                    model.UserRebatePro = decimal.Parse(ds.Tables[0].Rows[0]["UserRebatePro"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RebateProMoney"].ToString() != "")
                {
                    double dol = double.Parse(ds.Tables[0].Rows[0]["RebatePro"].ToString());

                    model.Odds = ds.Tables[0].Rows[0]["RebateProMoney"].ToString();
                    model.RebateProMoney = model.Odds + "/" + dol.ToString("P");
                }

                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LotteryType"].ToString() != "")
                {
                    model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PlayTypeName"].ToString() != "")
                {
                    model.PlayTypeName = ds.Tables[0].Rows[0]["PlayTypeName"].ToString() + "-" + ds.Tables[0].Rows[0]["PlayTypeRadioName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PlayTypeRadioName"].ToString() != "")
                {
                    model.PlayTypeRadioName = ds.Tables[0].Rows[0]["PlayTypeRadioName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString() != "")
                {
                    model.CurrentLotteryNum = ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString();
                }

                var currencyUnit = ds.Tables[0].Rows[0]["CurrencyUnit"];
                if (currencyUnit != DBNull.Value)
                {
                    model.CurrencyUnit = Convert.ToDecimal(currencyUnit);
                }
                var ratio = ds.Tables[0].Rows[0]["Ratio"];
                if (ratio != DBNull.Value)
                {
                    model.Ratio = Convert.ToInt32(ratio);
                }
                var sourceType = ds.Tables[0].Rows[0]["SourceType"];
                if (sourceType != DBNull.Value)
                {
                    model.SourceType = sourceType.ToString();
                }

                var resultJson = ds.Tables[0].Rows[0]["ResultJson"];
                if (resultJson != DBNull.Value)
                {
                    model.ResultJson = resultJson.ToString();
                }

                var roomId = ds.Tables[0].Rows[0]["RoomId"];
                if (roomId != DBNull.Value)
                {
                    model.RoomId = roomId.ToString();
                }

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 撤单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CancelOrder(Model.PalyInfo model)
        {
            Model.PalyInfo models = new Model.PalyInfo();
            SqlParameter[] parameters = {
                    new SqlParameter("@PalyID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.PalyID;
            parameters[1].Value = model.UserID;

            DataSet ds = DbHelperSQL.Main.RunProcedure("Pro_CancelOrder", parameters, "tab");

            if (ds.Tables == null)
            {
                return "002";
            }
            if (ds.Tables[0].Rows == null)
            {
                return "002";
            }
            if (ds.Tables[0].Columns == null)
            {
                return "002";
            }
            return ds.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// web端获取当天下单数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<Model.PalyInfo> GetAllOrderList(int UserID)
        {
            string strSql = @"select a.[PalyID]
      ,a.[PalyCurrentNum]
      ,a.[PlayTypeID]
      ,a.[LotteryID]
      ,a.[UserName]
      ,SUBSTRING(a.[PalyNum],1,200) AS PalyNum
      ,a.[NoteNum]
      ,a.[SingleMoney]
      ,a.[NoteMoney]
      ,a.[NoteTime]
      ,a.[IsWin]
      ,a.[WinMoney]
      ,a.[IsFactionAward]
      ,a.[PlayTypeRadioID]
      ,a.[RebatePro]
      ,a.[RebateProMoney]
      ,a.[WinNum]
      ,a.[UserID]
      ,a.[ParentID]
      ,a.[NoticeID]
      ,a.[LotteryTime],b.LotteryType,c.PlayTypeName,d.PlayTypeRadioName from dbo.PalyInfo a with(nolock)
left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID
left join dbo.PlayTypeInfo c with(nolock) on a.PlayTypeID=c.PlayTypeID
left join dbo.PlayTypeRadio d with(nolock) on a.PlayTypeRadioID=d.PlayTypeRadioID
            where UserID=@UserID and IsFactionAward=0 and NoteTime between @starttime and @endtime order by a.NoteTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                                        new SqlParameter("@starttime",SqlDbType.DateTime),
                                        new SqlParameter("@endtime",SqlDbType.DateTime)};
            parameters[0].Value = UserID;
            parameters[1].Value = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[2].Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            DataSet ds = DbHelperSQL.Main.Query(strSql, parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        List<Model.PalyInfo> list = ToList(ds.Tables[0]);

                        return list;
                    }
                }
            }
            return null;
        }

        public PlaySummaryModel GetXDaysSummary(int UserID, DateTime start, DateTime end, bool isAnsy)
        {
            PlaySummaryModel result = new PlaySummaryModel();

            string strSql = @"
                                SELECT  SUM(NoteMoney) AS NoteMoney ,
                                        SUM(WinMoney) AS WinMoney ,
                                        SUM(NoteMoney + WinMoney) AS PrizeMoney
                                FROM    dbo.PalyInfo WITH ( NOLOCK )
                                WHERE   UserID = @UserID
                                        AND NoteTime BETWEEN @starttime AND @endtime
                                        AND IsFactionAward = 1;
                                ";

            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                                        new SqlParameter("@starttime",SqlDbType.DateTime),
                                        new SqlParameter("@endtime",SqlDbType.DateTime)};
            parameters[0].Value = UserID;
            parameters[1].Value = start;
            parameters[2].Value = end;

            DataSet ds = null;
            if (!isAnsy)
            {
                ds = DbHelperSQL.Main.Query(strSql, parameters);
            }
            else
            {
                ds = DbHelperSQL.Bak.Query(strSql, parameters);
            }
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        result.TotalWinMoney = ds.Tables[0].Rows[0]["WinMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["WinMoney"]);
                        result.TotalNoteMoney = ds.Tables[0].Rows[0]["NoteMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["NoteMoney"]);
                        result.TotalBonus = ds.Tables[0].Rows[0]["PrizeMoney"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["PrizeMoney"]);
                    }
                }
            }
            return result;
        }

        public List<Model.PalyInfo> GetAllOrderList_FromMainDb(int UserID, DateTime start, DateTime end)
        {
            string strSql = @"select top 20 a.[PalyID]
      ,a.[PalyCurrentNum]
      --,a.[PlayTypeID]
      --,a.[LotteryID]
      --,a.[UserName]
      --,a.[NoteNum]
      --,a.[SingleMoney]
      ,a.[NoteMoney]
      ,a.[NoteTime]
      --,a.[IsWin]
      ,a.[WinMoney]
      ,a.[IsFactionAward]
      --,a.[PlayTypeRadioID]
      --,a.[RebatePro]
      --,a.[RebateProMoney]
      --,a.[WinNum]
      --,a.[UserID]
      --,a.[ParentID]
      --,a.[NoticeID]
      --,a.[LotteryTime]
      ,b.LotteryType
      --,c.PlayTypeName
      --,d.PlayTypeRadioName
from dbo.PalyInfo a with(nolock)
left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID
--left join dbo.PlayTypeInfo c with(nolock) on a.PlayTypeID=c.PlayTypeID
--left join dbo.PlayTypeRadio d with(nolock) on a.PlayTypeRadioID=d.PlayTypeRadioID
where a.UserID=@UserID and a.NoteTime between @starttime and @endtime order by a.NoteTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                                        new SqlParameter("@starttime",SqlDbType.DateTime),
                                        new SqlParameter("@endtime",SqlDbType.DateTime)};
            parameters[0].Value = UserID;
            parameters[1].Value = start.Date.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[2].Value = end.Date.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        List<Model.PalyInfo> list = ToList(ds.Tables[0]);

                        return list;
                    }
                }
            }
            return null;
        }

        public List<Model.PalyInfo> GetAllOrderList_FromBakDb(int UserID, DateTime start, DateTime end)
        {
            string strSql = @"select top 20 a.[PalyID]
      ,a.[PalyCurrentNum]
      --,a.[PlayTypeID]
      --,a.[LotteryID]
      --,a.[UserName]
      --,a.[NoteNum]
      --,a.[SingleMoney]
      ,a.[NoteMoney]
      ,a.[NoteTime]
      --,a.[IsWin]
      ,a.[WinMoney]
      ,a.[IsFactionAward]
      --,a.[PlayTypeRadioID]
      --,a.[RebatePro]
      --,a.[RebateProMoney]
      --,a.[WinNum]
      --,a.[UserID]
      --,a.[ParentID]
      --,a.[NoticeID]
      --,a.[LotteryTime]
      ,b.LotteryType
      --,c.PlayTypeName
      --,d.PlayTypeRadioName
from dbo.PalyInfo a with(nolock)
left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID
--left join dbo.PlayTypeInfo c with(nolock) on a.PlayTypeID=c.PlayTypeID
--left join dbo.PlayTypeRadio d with(nolock) on a.PlayTypeRadioID=d.PlayTypeRadioID
where a.UserID=@UserID and a.NoteTime between @starttime and @endtime order by a.NoteTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                                        new SqlParameter("@starttime",SqlDbType.DateTime),
                                        new SqlParameter("@endtime",SqlDbType.DateTime)};
            parameters[0].Value = UserID;
            parameters[1].Value = start.Date.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[2].Value = end.Date.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        List<Model.PalyInfo> list = ToList(ds.Tables[0]);
                        //if (list.Count > 0)
                        //{
                        //    for (int n = 0; n < list.Count; n++)
                        //    {
                        //        if (list[n].PalyNum.Length > 200)
                        //        {
                        //            list[n].PalyNum = list[n].PalyNum.Substring(0, 200) + "...";
                        //        }
                        //    }
                        //}
                        return list;
                    }
                }
            }
            return null;
        }

        public List<WinningListItem> GetLatestWinningList(string period)
        {
            List<WinningListItem> list = new List<WinningListItem>();
            string sql = @"declare @start DATETIME
                        declare @end DATETIME ";
            switch (period)
            {
                //當日0時
                case "day":
                    sql += @"set @start = dateadd(ms,0,DATEADD(dd, DATEDIFF(dd,0,getdate()), 0)) ";
                    break;
                //前七日0時
                case "week":
                    sql += @"set @start = dateadd(ms,0,DATEADD(dd, DATEDIFF(dd,7,getdate()), 0)) ";
                    break;
                //前三十日0時
                case "month":
                    sql += @"set @start = dateadd(ms,0,DATEADD(dd, DATEDIFF(dd,30,getdate()), 0)) ";
                    break;

                default:
                    sql += @"set @start = dateadd(ms,0,DATEADD(dd, DATEDIFF(dd,0,getdate()), 0)) ";
                    break;
            }

            sql += @"SET @end = DATEADD(ms,-3,DATEADD(dd, DATEDIFF(dd,-1,GETDATE()), 0))
                        SELECT TOP 50
                            a.[UserName],
                            ((a.WinMoney+a.NoteMoney)-a.NoteMoney*a.RebatePro) AS WinMoney,
                            c.LotteryType
                        FROM inlodb.dbo.palyinfo a WITH(NOLOCK)
                        INNER JOIN inlodb.dbo.lotteryinfo c WITH (NOLOCK)
                            ON a.lotteryid = c.lotteryid
                        WHERE isfactionaward=1
	                        AND notetime >=@start AND notetime <=@end
                            AND ((a.WinMoney+a.NoteMoney)-a.NoteMoney*a.RebatePro)>=10000
                        ORDER by WinMoney DESC";
            DataSet ds = DbHelperSQL.Bak.Query(sql);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int day = DateTime.Now.Day;

                            int index2 = day % ds.Tables[0].Rows.Count;
                            int index3 = day % 6;

                            string lotteryType = Constant.LotteryTypeName.HSSEC_MMC;

                            switch (index3)
                            {
                                case 0:
                                    lotteryType = Constant.LotteryTypeName.HSSEC_MMC;
                                    break;

                                case 1:
                                    lotteryType = Constant.LotteryTypeName.HSSEC_PK10;
                                    break;

                                case 2:
                                    lotteryType = Constant.LotteryTypeName.HSSSC;
                                    break;

                                case 3:
                                    lotteryType = Constant.LotteryTypeName.HS115;
                                    break;

                                case 4:
                                    lotteryType = Constant.LotteryTypeName.HSSFC;
                                    break;

                                case 5:
                                    lotteryType = Constant.LotteryTypeName.HSPK;
                                    break;
                            }

                            string allLotteryType = ds.Tables[0].Rows.Cast<DataRow>().Aggregate(string.Empty, (current, row) => current + row["LotteryType"]);
                            ds.Tables[0].Rows.Cast<DataRow>().Aggregate(0, (current, row) => CombineWithLastWinningToList(row, current, index2, lotteryType, allLotteryType, list));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="lotteryType"></param>
        /// <param name="allLotteryType"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private int CombineWithLastWinningToList(DataRow row, int index1, int index2, string lotteryType, string allLotteryType, List<WinningListItem> list)
        {
            string winmoney = Convert.ToDecimal(row["WinMoney"]).ToString("f2");
            string userName = row["UserName"].ToString();

            WinningListItem winningListItem = GetLastWinningItem(index1, index2, lotteryType, allLotteryType, row, winmoney, userName);
            list.Add(winningListItem);

            index1++;
            return index1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="lotteryType"></param>
        /// <param name="allLotteryType"></param>
        /// <param name="row"></param>
        /// <param name="winmoney"></param>
        /// <param name="userName"></param>
        private WinningListItem GetLastWinningItem(int index1, int index2, string lotteryType, string allLotteryType, DataRow row, string winmoney, string userName)
        {
            userName = Regex.Replace(userName, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
            if (userName.Contains('*') == false)
            {
                if (userName.Length > 4)
                {
                    userName = userName.Substring(0, userName.Length - 3) + "***";
                }
                else if (userName.Length > 3)
                {
                    userName = userName.Substring(0, userName.Length - 2) + "***";
                }
                else if (userName.Length > 2)
                {
                    userName = userName.Substring(0, userName.Length - 1) + "***";
                }
                else
                {
                    userName = userName + "***";
                }
            }

            if (index1 == index2 && !allLotteryType.Contains(lotteryType))
            {
                return new WinningListItem()
                {
                    AmountText = winmoney,
                    LotteryName = lotteryType,
                    UserName = userName
                };
            }
            else
            {
                return new WinningListItem()
                {
                    AmountText = winmoney,
                    LotteryName = Convert.ToString(row["LotteryType"]),
                    UserName = userName
                };
            }
        }

        /// <summary>
        /// web端获取一日的銀象快三数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lotteryId"></param>
        /// <param name="status"></param>
        /// <param name="searchDate"></param>
        /// <param name="cursor"></param>
        /// <param name="pageSize"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<CursorPaginationTotalData<Model.PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId,
            string status, DateTime searchDate, string cursor, int pageSize, string roomId)
        {
            var emptyResult = new CursorPaginationTotalData<Model.PalyInfo>
            {
                Data = Array.Empty<Model.PalyInfo>(),
                NextCursor = string.Empty
            };
            if (!cursor.IsNullOrEmpty())
            {
                try
                {
                    cursor = cursor.FromBase64String();
                }
                catch (Exception)
                {
                    return emptyResult;
                }
            }

            switch (status)
            {
                case "Unawarded":
                    status = ((int)AwardStatus.Unawarded).ToString();
                    break;

                case "Won":
                    status = ((int)AwardStatus.IsDone).ToString();
                    break;

                case "Lost":
                    status = ((int)AwardStatus.IsDone).ToString();
                    break;

                case "SystemCancel":
                    status = ((int)AwardStatus.SystemCancel).ToString();
                    break;

                case "SystemRefund":
                    status = ((int)AwardStatus.SystemRefund).ToString();
                    break;

                default:
                    status = string.Empty;
                    break;
            }

            string strSql = string.Concat(@"SELECT TOP (@PageSize)",
                GetPlayBetSqlString(),
                @"WHERE UserID=@UserID
                    AND NoteTime BETWEEN @StartTime AND @EndTime
                    #LotteryId# #Status# #Cursor# #RoomId#
                  ORDER BY a.PalyID DESC ",
                $@"
                  SELECT 
                    SUM(a.NoteNum) AS TotalBetCount,
                    SUM(CASE WHEN IsFactionAward = {(int)AwardStatus.Unawarded} THEN 0 ELSE a.NoteMoney END) + SUM(a.WinMoney) AS TotalPrizeMoney,
                    SUM(a.WinMoney) AS TotalWinMoney
                  FROM [Inlodb_bak].[dbo].[VW_PalyInfo] a WITH(NOLOCK)
                  WHERE UserID=@UserID
                    #LotteryId# #RoomId#
                    AND IsFactionAward IN ( {string.Join(", ", ClientAllowStatus)} ) 
                    AND NoteTime BETWEEN @StartTime AND @EndTime");
            
            DateTime startTime = Convert.ToDateTime(searchDate.ToString("D").ToString());
            DateTime endTime = Convert.ToDateTime(searchDate.AddDays(1).ToString("D").ToString()).AddSeconds(-1);

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@UserID", userId));
            parameters.Add(new SqlParameter("@StartTime", startTime));
            parameters.Add(new SqlParameter("@EndTime", endTime));
            parameters.Add(new SqlParameter("@PageSize", pageSize));

            if (lotteryId.HasValue)
            {
                strSql = strSql.Replace("#LotteryId#", " AND a.LotteryID=@LotteryId");
                parameters.Add(new SqlParameter("@LotteryId", lotteryId));
            }
            else
            {
                strSql = strSql.Replace("#LotteryId#", string.Empty);
            }

            if (!cursor.IsNullOrEmpty())
            {
                strSql = strSql.Replace("#Cursor#", "AND [PalyID] < @Cursor");
                parameters.Add(new SqlParameter("@Cursor", SqlDbType.VarChar, 32)
                {
                    Value = cursor
                });
            }
            else
            {
                strSql = strSql.Replace("#Cursor#", string.Empty);
            }

            if (!status.IsNullOrEmpty())
            {
                strSql = strSql.Replace("#Status#", " AND IsFactionAward=@Status");
                parameters.Add(new SqlParameter("@Status", status));
            }
            else
            {
                strSql = strSql.Replace("#Status#", string.Empty);
            }

            if (!roomId.IsNullOrEmpty())
            {
                strSql = strSql.Replace("#RoomId#", " AND RoomId=@RoomId");
                parameters.Add(new SqlParameter("@RoomId", roomId));
            }
            else
            {
                strSql = strSql.Replace("#RoomId#", " AND RoomId<>'0'");
            }

            DataSet ds = DbHelperSQL.Main.Query(strSql, parameters.ToArray());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        string nextCursor = string.Empty;
                        List<Model.PalyInfo> list = ToList(ds.Tables[0])
                            .Where(x => ClientAllowStatus.Contains(x.IsFactionAward))
                            .ToList();
                        var data = SplitOrder(list, startTime, endTime);
                        if (data.Any())
                        {
                            nextCursor = data.Last().PalyID.ToString().ToBase64String();
                        }

                        DataProcessing(data);

                        var totalRaw = ds.Tables[1].Rows[0];

                        int totalBetCount = int.TryParse(totalRaw["TotalBetCount"].ToString(), out totalBetCount) ? totalBetCount : 0;
                        decimal totalPrizeMoney = decimal.TryParse(totalRaw["TotalPrizeMoney"].ToString(), out totalPrizeMoney) ? totalPrizeMoney : 0;
                        decimal totalWinMoney = decimal.TryParse(totalRaw["TotalWinMoney"].ToString(), out totalWinMoney) ? totalWinMoney : 0;

                        return new CursorPaginationTotalData<Model.PalyInfo>()
                        {
                            Data = data,
                            NextCursor = nextCursor,
                            TotalBetCount = totalBetCount,
                            TotalPrizeMoney = totalPrizeMoney,
                            TotalWinMoney = totalWinMoney
                        };
                    }
                }
            }

            return emptyResult;
        }

        private static void DataProcessing(IEnumerable<Model.PalyInfo> palyInfos)
        {
            foreach (var palyInfo in palyInfos)
            {
                switch ((AwardStatus)palyInfo.IsFactionAward)
                {
                    case AwardStatus.SystemRefund:
                        palyInfo.NoteMoney = 0;
                        palyInfo.NoteNum = 0;
                        palyInfo.SingleMoney = 0;
                        palyInfo.WinMoney = 0;
                        break;
                }
            }
        }

        public List<Model.PalyInfo> SplitOrder(List<Model.PalyInfo> list, DateTime startTime, DateTime endTime)
        {
            List<Model.PalyInfo> result = new List<Model.PalyInfo>();
            char splitSymbol = '|';

            foreach (var model in list)
            {
                try
                {
                    // 先解壓縮
                    if (_zip.IsBase64(model.PalyNum))
                    {
                        model.PalyNum = _zip.Decompress(model.PalyNum);
                    }

                    if (!string.IsNullOrWhiteSpace(model.ResultJson))
                    {
                        var rawResult = model.ResultJson;
                        if (_zip.IsBase64(rawResult))
                        {
                            rawResult = _zip.Decompress(model.ResultJson);
                        }

                        try
                        {
                            model.Result = JsonConvert.DeserializeObject<WinGroupModel[]>(rawResult)
                                    .GroupBy(x => x.WinTypeName, x => x)
                                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "解析開獎明細失敗");
                        }
                    }

                    //切投柱項
                    if (model.PalyNum.Contains(splitSymbol))
                    {
                        var splitList = model.PalyNum.Split(splitSymbol);
                        foreach (var s in splitList)
                        {
                            Model.PalyInfo palyInfo = new Model.PalyInfo();
                            palyInfo.PalyID = model.PalyID;
                            palyInfo.PalyCurrentNum = model.PalyCurrentNum;
                            palyInfo.PlayTypeID = model.PlayTypeID;
                            palyInfo.LotteryID = model.LotteryID;
                            palyInfo.UserName = model.UserName;
                            palyInfo.PalyNum = s;
                            palyInfo.NoteNum = 1;
                            palyInfo.SingleMoney = model.SingleMoney;
                            palyInfo.NoteMoney = model.NoteMoney / model.NoteNum;
                            palyInfo.NoteTime = model.NoteTime;
                            palyInfo.IsFactionAward = model.IsFactionAward;
                            palyInfo.PlayTypeRadioID = model.PlayTypeRadioID;
                            palyInfo.RebatePro = model.RebatePro;
                            palyInfo.RebateProMoney = model.RebateProMoney;
                            palyInfo.UserID = model.UserID;
                            palyInfo.UserRebatePro = model.UserRebatePro;
                            palyInfo.NoticeID = model.NoticeID;
                            palyInfo.CurrentLotteryTime = model.CurrentLotteryTime;
                            palyInfo.LotteryType = model.LotteryType;
                            palyInfo.PlayTypeName = model.PlayTypeName;
                            palyInfo.PlayTypeRadioName = model.PlayTypeRadioName;
                            palyInfo.UserType = model.UserType;
                            var bonusService = _bonusServiceList.FirstOrDefault(x => (int)x.LotteryId == model.LotteryID);

                            var PlayInfoList = new List<PlayInfo>();
                            var PlayInfo = new PlayInfo()
                            {
                                DrawNumber = s,
                                PlayTypeRadioId = (int)model.PlayTypeRadioID,
                                LotteryId = (int)model.LotteryID,
                                IssueNo = model.PalyCurrentNum,
                                BetAmount = model.SingleMoney ?? decimal.Zero,
                                UserRebate = model.UserRebatePro,
                                Rebate = model.RebatePro ?? decimal.Zero,
                            };
                            PlayInfoList.Add(PlayInfo);

                            int status = model.IsFactionAward;
                            //判斷是否有中獎
                            if (model.Result.ContainsKey(s))
                            {
                                var item = model.Result[s];
                                palyInfo.IsWin = true;

                                palyInfo.WinMoney = item.WinAmount - item.BetAmount;
                                palyInfo.WinPossMoney = item.WinAmount;
                                if (item.AwardStatus == (int)AwardStatus.SystemCancel)
                                {
                                    status = item.AwardStatus;
                                }
                                palyInfo.Odds = item.Odds.ToString();
                            }
                            else
                            {
                                palyInfo.Odds = bonusService.GetNumberOdds(PlayInfo)[s].ToString();
                                palyInfo.IsWin = false;
                                palyInfo.WinMoney = -palyInfo.NoteMoney;
                                palyInfo.WinPossMoney = 0;
                            }
                            string getK3LotteryResult = GetLotteryResult(status, palyInfo.IsWin);
                            var splitLotteryResult = getK3LotteryResult.Split(',');
                            palyInfo.Status = splitLotteryResult[0];
                            palyInfo.StFactionAward = splitLotteryResult[1];
                            result.Add(palyInfo);
                        }
                    }
                    else if (model.Result.Count != 0)
                    {
                        model.Odds = model.Result.FirstOrDefault().Value.Odds.ToString();
                        if (model.Result.FirstOrDefault().Value.AwardStatus == (int)AwardStatus.SystemCancel)
                        {
                            model.IsFactionAward = model.Result.FirstOrDefault().Value.AwardStatus;
                        }


                        //派獎金額為0
                        if (model.IsWin == false)
                            model.WinPossMoney = 0;
                        else
                            model.WinPossMoney = Convert.ToDecimal(model.NoteMoney + model.WinMoney);

                        string getK3LotteryResult = GetLotteryResult(model.IsFactionAward, model.IsWin);
                        var splitLotteryResult = getK3LotteryResult.Split(',');
                        model.Status = splitLotteryResult[0];
                        model.StFactionAward = splitLotteryResult[1];
                        result.Add(model);
                    }
                    else
                    {
                        var bonusService = _bonusServiceList.FirstOrDefault(x => (int)x.LotteryId == model.LotteryID);
                        if (bonusService.GameTypeId == (int)GameTypeId.LHC)
                        {
                            bonusService.CloseDateTime = model.NoteTime;
                        }
                        var PlayInfo = new PlayInfo()
                        {
                            DrawNumber = model.PalyNum,
                            PlayTypeRadioId = (int)model.PlayTypeRadioID,
                            LotteryId = (int)model.LotteryID,
                            IssueNo = model.PalyCurrentNum,
                            BetAmount = model.SingleMoney ?? decimal.Zero,
                            UserRebate = model.UserRebatePro,
                            Rebate = model.RebatePro ?? decimal.Zero,
                        };
                        //抓賠率
                        var odds = bonusService.GetNumberOdds(PlayInfo);
                        if (odds.ContainsKey(model.PalyNum))
                        {
                            model.Odds = odds[model.PalyNum].ToString();
                        }
                        else
                        {
                            model.Odds = odds.FirstOrDefault().Value.ToString();
                        }


                        //派獎金額為0
                        if (model.IsWin == false)
                            model.WinPossMoney = 0;
                        else
                            model.WinPossMoney = Convert.ToDecimal(model.NoteMoney + model.WinMoney);

                        string getK3LotteryResult = GetLotteryResult(model.IsFactionAward, model.IsWin);
                        var splitLotteryResult = getK3LotteryResult.Split(',');
                        model.Status = splitLotteryResult[0];
                        model.StFactionAward = splitLotteryResult[1];
                        result.Add(model);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("方法名称:SplitOrder , 一分快三分取得投柱项失败, 描述:" + ex.ToString());
                    continue;
                }
            }

            return result;
        }

        /// <summary>
        /// 轉化機器人投注的注單
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<Model.PalyInfo> SplitOrderByBot(PlayInfoDto info)
        {
            List<Model.PalyInfo> result = new List<Model.PalyInfo>();
            try
            {
                var PlayInfo = new PlayInfo()
                {
                    DrawNumber = info.DrawNumber,
                    PlayTypeRadioId = info.PlayTypeRadioId,
                    LotteryId = info.LotteryId,
                    IssueNo = info.IssueNo,
                    BetAmount = info.BetAmount,
                    UserRebate = info.Rebate,
                    Rebate = info.Rebate,
                };

                var bonusService = _bonusServiceList.FirstOrDefault(x => (int)x.LotteryId == info.LotteryId);
                if (bonusService.GameTypeId == (int)GameTypeId.LHC)
                {
                    bonusService.CloseDateTime = DateTime.Now;
                }

                var betOdds = bonusService.GetNumberOdds(PlayInfo).FirstOrDefault().Value.ToString();

                var model = new Model.PalyInfo();
                model.Odds = betOdds;
                model.PalyID = info.PlayId;
                model.PalyCurrentNum = info.IssueNo;
                model.PlayTypeID = info.PlayTypeId;
                model.LotteryID = info.LotteryId;
                model.UserName = info.UserName;
                model.PalyNum = info.DrawNumber;
                model.NoteNum = 1;
                model.SingleMoney = info.BetAmount;
                model.NoteMoney = info.TotalBetAmount;
                model.NoteTime = DateTime.Now;
                //model.IsFactionAward = model.IsFactionAward;
                model.PlayTypeRadioID = info.PlayTypeRadioId;
                model.RebatePro = info.Rebate;
                //model.RebateProMoney = model.RebateProMoney;
                model.UserID = info.UserId;
                model.UserRebatePro = info.Rebate;
                //model.NoticeID = model.NoticeID;
                //model.CurrentLotteryTime = model.CurrentLotteryTime;
                //model.LotteryType = model.LotteryType;
                model.PlayTypeName = info.PlayTypeName;
                model.PlayTypeRadioName = info.PlayTypeRadioName;
                model.UserType = info.UserType.ToString();

                result.Add(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("方法名称:SplitOrderByBot, 取得機器人跟投失敗, 描述:" + ex.ToString());
            }

            return result;
        }

        //取得注單狀態
        private string GetLotteryResult(int isFactionAward, bool isWin)
        {
            string result = string.Empty;
            string code = string.Empty;
            string codeStr = string.Empty;
            if (isFactionAward == (int)AwardStatus.Unawarded)
            {
                code = AwardStatus.Unawarded.ToString();
                codeStr = AwardStatus.Unawarded.GetDescription();
            }
            else if (isFactionAward == (int)AwardStatus.IsDone && isWin == true)
            {
                code = AwardStatus.Won.ToString();
                codeStr = AwardStatus.Won.GetDescription(); ;
            }
            else if (isFactionAward == (int)AwardStatus.IsDone && isWin == false)
            {
                code = AwardStatus.Lost.ToString();
                codeStr = AwardStatus.Lost.GetDescription(); ;
            }
            else if (isFactionAward == (int)AwardStatus.SystemRefund)
            {
                code = AwardStatus.SystemRefund.ToString();
                codeStr = AwardStatus.SystemRefund.GetDescription(); ;
            }
            else if (isFactionAward == (int)AwardStatus.SystemCancel)
            {
                code = AwardStatus.SystemCancel.ToString();
                codeStr = AwardStatus.SystemCancel.GetDescription(); ;
            }
            else
            {
                code = " ";
                codeStr = " ";
            }
            result = code + "," + codeStr;
            return result;
        }

        /// <summary>
        /// 秘色跟单资讯
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CursorPagination<Model.PalyInfo> GetFollowBet(string palyId, int lottertId)
        {
            var emptyResult = new CursorPagination<Model.PalyInfo>
            {
                Data = Array.Empty<Model.PalyInfo>(),
                NextCursor = string.Empty
            };

            _logger.LogInformation($"GetFollowBet:LotteryId:{lottertId}, PlayId:{palyId}");
            if (palyId.StartsWith($"B-{lottertId}"))
            {
                var issueNo = palyId.Split('-')[BotIssueNoPositionIndex];
                var cacheBase = DependencyUtil.ResolveService<ICacheBase>().Value;
                var list = cacheBase.Get<List<PlayInfoDto>>(RedisPlayInfosKey(lottertId, issueNo))?.Result;

                if (list.Any())
                {
                    var followOrder = list.Where(p => p.PlayId == palyId)?.FirstOrDefault();
                    if (followOrder != null)
                    {
                        return new CursorPagination<Model.PalyInfo>()
                        {
                            Data = SplitOrderByBot(followOrder),
                            NextCursor = ""
                        };
                    }
                }
            }
            else
            {
                string strSql = string.Concat(@"SELECT TOP 1",
                                GetPlayBetSqlString(),
                                @"WHERE PalyID=@PalyID AND a.LotteryID=@LotteryId
                                            ORDER BY a.PalyID DESC");
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PalyID", palyId));
                parameters.Add(new SqlParameter("@LotteryId", lottertId));
                DataSet ds = DbHelperSQL.Main.Query(strSql, parameters.ToArray());
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            List<Model.PalyInfo> list = ToList(ds.Tables[0])
                                .Where(x => ClientAllowStatus.Contains(x.IsFactionAward))
                                .ToList();
                            var data = SplitOrder(list, Convert.ToDateTime(DateTime.Now.ToString("D").ToString()), Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1));

                            return new CursorPagination<Model.PalyInfo>()
                            {
                                Data = data,
                                NextCursor = ""
                            };
                        }
                    }
                }
            }

            return emptyResult;
        }
    }
}