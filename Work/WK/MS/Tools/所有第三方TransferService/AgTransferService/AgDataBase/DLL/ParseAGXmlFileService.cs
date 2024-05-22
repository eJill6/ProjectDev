using AgDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendServiceNF.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AgDataBase.DLL
{
    public interface IParseAGXmlFileService
    {
        bool ConvertToAGAndFishInfo(AGGameType agGameType, XMLFile xmlFile, out List<AGInfo> agInfos, out List<AgFishInfo> agFishInfos);
    }

    public class ParseAGXmlFileService : IParseAGXmlFileService
    {
        private static readonly HashSet<string> s_agxinGameTypes =
            new HashSet<string>() { "SB28", "SB07", "SB09", "SB10", "SB08", "SB11", "SB01", "SB06", "SB02", "SLM2", "PKBJ", "FRU" };

        public bool ConvertToAGAndFishInfo(AGGameType agGameType, XMLFile xmlFile, out List<AGInfo> agInfos, out List<AgFishInfo> agFishInfos)
        {
            agInfos = new List<AGInfo>();
            agFishInfos = new List<AgFishInfo>();

            if (xmlFile.IsSkip)
            {
                return true;
            }

            var dataCounter = 0;
            var failureDataCounter = 0;

            if (string.IsNullOrEmpty(xmlFile.FileContent) && !string.IsNullOrEmpty(xmlFile.LocalPath))
            {
                LogUtil.ForcedDebug($"开始解析文件 {xmlFile.LocalPath}");

                try
                {
                    using (var file = new System.IO.StreamReader(xmlFile.LocalPath, Encoding.UTF8))
                    {
                        xmlFile.FileContent = file.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error("读取文件 " + xmlFile.LocalPath + " 失败，详细信息：" + ex.Message);

                    return false;
                }
            }

            if (string.IsNullOrEmpty(xmlFile.FileContent))
            {
                return true;
            }

            string[] lines = xmlFile.FileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                try
                {
                    var dataType = Regex.Match(line, "(?<=dataType=\")[\\s\\S]*?(?=\")").ToString();

                    #region 解析TR

                    if (dataType == "TR")
                    {
                        var flag = Regex.Match(line, "(?<=flag=\")[\\s\\S]*?(?=\")").ToString();
                        var transferType = Regex.Match(line, "(?<=transferType=\")[\\s\\S]*?(?=\")").ToString();

                        if (flag == "0" && (transferType == "IN" || transferType == "OUT"))
                        {
                            var billNo = Regex.Match(line, "(?<=tradeNo=\")[\\s\\S]*?(?=\")").ToString();
                            var mainbillno = Regex.Match(line, "(?<=tradeNo=\")[\\s\\S]*?(?=\")").ToString();
                            var playerName = Regex.Match(line, "(?<=playerName=\")[\\s\\S]*?(?=\")").ToString();
                            var beforeCredit = Regex.Match(line, "(?<=previousAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var betAmount = Regex.Match(line, "(?<=transferAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var validBetAmount = Regex.Match(line, "(?<=currentAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var netAmount = Regex.Match(line, "(?<=transferAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var betTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                            var recalcuTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                            var platformType = Regex.Match(line, "(?<=platformType=\")[\\s\\S]*?(?=\")").ToString();
                            var @round = "";
                            var gameType = transferType;
                            var playType = "";
                            var tableCode = "";
                            var gameCode = Regex.Match(line, "(?<=gameCode=\")[\\s\\S]*?(?=\")").ToString();
                            if (string.IsNullOrEmpty(beforeCredit))
                            {
                                beforeCredit = "0";
                            }

                            AGInfo agInfo = new AGInfo();
                            agInfo.dataType = dataType;
                            agInfo.billNo = billNo;
                            agInfo.mainbillno = mainbillno;
                            agInfo.playerName = playerName;
                            agInfo.beforeCredit = Convert.ToDecimal(beforeCredit);
                            agInfo.betAmount = Convert.ToDecimal(betAmount);
                            agInfo.validBetAmount = Convert.ToDecimal(validBetAmount);
                            agInfo.netAmount = Convert.ToDecimal(netAmount);
                            agInfo.betTime = Convert.ToDateTime(betTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4

                            if (!string.IsNullOrEmpty(recalcuTime))
                            {
                                agInfo.recalcuTime = Convert.ToDateTime(recalcuTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4
                            }
                            else
                            {
                                agInfo.recalcuTime = Convert.ToDateTime(betTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4
                            }

                            agInfo.platformType = platformType;
                            agInfo.round = round;
                            agInfo.gameType = gameType;
                            agInfo.playType = playType.ToLower().Replace("null", "");
                            agInfo.tableCode = tableCode.ToLower().Replace("null", "");
                            agInfo.gameCode = gameCode.ToLower().Replace("null", "");
                            agInfo.flag = flag;
                            agInfo.MiseOrderGameId = agGameType.OrderGameId.SubGameCode;

                            agInfos.Add(agInfo);

                            dataCounter++;
                        }
                    }

                    #endregion 解析TR

                    #region 解析EBR和BR

                    else if (dataType == "EBR" || dataType == "BR")
                    {
                        var flag = Regex.Match(line, "(?<=flag=\")[\\s\\S]*?(?=\")").ToString();

                        if (flag == "1")
                        {
                            var billNo = Regex.Match(line, "(?<=billNo=\")[\\s\\S]*?(?=\")").ToString();
                            var mainbillno = Regex.Match(line, "(?<=mainbillno=\")[\\s\\S]*?(?=\")").ToString();
                            var playerName = Regex.Match(line, "(?<=playerName=\")[\\s\\S]*?(?=\")").ToString();
                            var beforeCredit = Regex.Match(line, "(?<=beforeCredit=\")[\\s\\S]*?(?=\")").ToString();
                            var betAmount = Regex.Match(line, "(?<=betAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var validBetAmount = Regex.Match(line, "(?<=validBetAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var netAmount = Regex.Match(line, "(?<=netAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var betTime = Regex.Match(line, "(?<=betTime=\")[\\s\\S]*?(?=\")").ToString();
                            var recalcuTime = Regex.Match(line, "(?<=recalcuTime=\")[\\s\\S]*?(?=\")").ToString();
                            var platformType = Regex.Match(line, "(?<=platformType=\")[\\s\\S]*?(?=\")").ToString();
                            var @round = Regex.Match(line, "(?<=round=\")[\\s\\S]*?(?=\")").ToString();
                            var gameType = Regex.Match(line, "(?<=gameType=\")[\\s\\S]*?(?=\")").ToString();
                            var playType = Regex.Match(line, "(?<=playType=\")[\\s\\S]*?(?=\")").ToString();
                            var tableCode = Regex.Match(line, "(?<=tableCode=\")[\\s\\S]*?(?=\")").ToString();
                            var gameCode = Regex.Match(line, "(?<=gameCode=\")[\\s\\S]*?(?=\")").ToString();

                            if (string.IsNullOrEmpty(beforeCredit))
                            {
                                beforeCredit = "0";
                            }

                            AGInfo agInfo = new AGInfo();
                            agInfo.dataType = dataType;
                            agInfo.billNo = billNo;
                            agInfo.mainbillno = mainbillno;
                            agInfo.playerName = playerName;
                            agInfo.beforeCredit = Convert.ToDecimal(beforeCredit);
                            agInfo.betAmount = Convert.ToDecimal(betAmount);
                            agInfo.validBetAmount = Convert.ToDecimal(validBetAmount);
                            agInfo.netAmount = Convert.ToDecimal(netAmount);
                            agInfo.betTime = Convert.ToDateTime(betTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4

                            if (!string.IsNullOrEmpty(recalcuTime))
                            {
                                agInfo.recalcuTime = Convert.ToDateTime(recalcuTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4
                            }
                            else
                            {
                                agInfo.recalcuTime = Convert.ToDateTime(betTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4
                            }

                            agInfo.platformType = platformType;
                            agInfo.round = round;
                            agInfo.gameType = gameType;
                            agInfo.playType = playType.ToLower().Replace("null", "");
                            agInfo.tableCode = tableCode.ToLower().Replace("null", "");
                            agInfo.gameCode = gameCode.ToLower().Replace("null", "");
                            agInfo.flag = flag;
                            agInfo.MiseOrderGameId = agGameType.OrderGameId.SubGameCode;

                            //因為AG電子某些特定遊戲platform回傳AG，誤以為是AG真人，所以當轉換AG真人資料時，要把這些遊戲改為AG電子的SubGameCode
                            if (agGameType == AGGameType.AGIN && s_agxinGameTypes.Contains(gameType))
                            {
                                agInfo.MiseOrderGameId = AGGameType.XIN.OrderGameId.SubGameCode;
                            }

                            agInfos.Add(agInfo);

                            dataCounter++;
                        }
                    }

                    #endregion 解析EBR和BR

                    #region 解析HSR

                    else if (dataType == "HSR")
                    {
                        var flag = Regex.Match(line, "(?<=flag=\")[\\s\\S]*?(?=\")").ToString();
                        var type = Regex.Match(line, "(?<=type=\")[\\s\\S]*?(?=\")").ToString();

                        if (flag == "0" && (type == "1" || type == "2" || type == "7"))
                        {
                            var ProfitLossID = Regex.Match(line, "(?<=ID=\")[\\s\\S]*?(?=\")").ToString();
                            var tradeNo = Regex.Match(line, "(?<=tradeNo=\")[\\s\\S]*?(?=\")").ToString();
                            var platformType = Regex.Match(line, "(?<=platformType=\")[\\s\\S]*?(?=\")").ToString();
                            var sceneId = Regex.Match(line, "(?<=sceneId=\")[\\s\\S]*?(?=\")").ToString();
                            var playerName = Regex.Match(line, "(?<=playerName=\")[\\s\\S]*?(?=\")").ToString();
                            var SceneStartTime = Regex.Match(line, "(?<=SceneStartTime=\")[\\s\\S]*?(?=\")").ToString();
                            var SceneEndTime = Regex.Match(line, "(?<=SceneEndTime=\")[\\s\\S]*?(?=\")").ToString();
                            var Roomid = Regex.Match(line, "(?<=Roomid=\")[\\s\\S]*?(?=\")").ToString();
                            var Roombet = Regex.Match(line, "(?<=Roombet=\")[\\s\\S]*?(?=\")").ToString();
                            var Cost = Regex.Match(line, "(?<=Cost=\")[\\s\\S]*?(?=\")").ToString();
                            var Earn = Regex.Match(line, "(?<=Earn=\")[\\s\\S]*?(?=\")").ToString();
                            var Jackpotcomm = Regex.Match(line, "(?<=Jackpotcomm=\")[\\s\\S]*?(?=\")").ToString();
                            var transferAmount = Regex.Match(line, "(?<=transferAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var previousAmount = Regex.Match(line, "(?<=previousAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var currentAmount = Regex.Match(line, "(?<=currentAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var currency = Regex.Match(line, "(?<=currency=\")[\\s\\S]*?(?=\")").ToString();
                            var exchangeRate = Regex.Match(line, "(?<=exchangeRate=\")[\\s\\S]*?(?=\")").ToString();
                            var IP = Regex.Match(line, "(?<=IP=\")[\\s\\S]*?(?=\")").ToString();
                            var creationTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                            var gameCode = Regex.Match(line, "(?<=gameCode=\")[\\s\\S]*?(?=\")").ToString();

                            AgFishInfo agFishInfo = new AgFishInfo();
                            agFishInfo.dataType = dataType;
                            agFishInfo.ProfitLossID = ProfitLossID;
                            agFishInfo.tradeNo = tradeNo;
                            agFishInfo.platformType = platformType;
                            agFishInfo.sceneId = sceneId;
                            agFishInfo.playerName = playerName;
                            agFishInfo.type = type;
                            agFishInfo.SceneStartTime = Convert.ToDateTime(SceneStartTime).ToUniversalTime().AddHours(12);
                            agFishInfo.SceneEndTime = Convert.ToDateTime(SceneEndTime).ToUniversalTime().AddHours(12);
                            agFishInfo.Roomid = Roomid;
                            agFishInfo.Roombet = Roombet;
                            agFishInfo.Cost = decimal.Parse(Cost);
                            agFishInfo.Earn = decimal.Parse(Earn);
                            agFishInfo.Jackpotcomm = decimal.Parse(Jackpotcomm);
                            agFishInfo.transferAmount = decimal.Parse(transferAmount);

                            decimal defaultPreviousAmount = 0M;
                            agFishInfo.previousAmount = decimal.TryParse(previousAmount, out defaultPreviousAmount) ? decimal.Parse(previousAmount) : defaultPreviousAmount;
                            decimal defaultCurrentAmount = 0M;
                            agFishInfo.currentAmount = decimal.TryParse(currentAmount, out defaultCurrentAmount) ? decimal.Parse(currentAmount) : defaultCurrentAmount;

                            agFishInfo.currency = currency;
                            agFishInfo.exchangeRate = exchangeRate;
                            agFishInfo.IP = IP;
                            agFishInfo.flag = flag;
                            agFishInfo.creationTime = Convert.ToDateTime(creationTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4
                            agFishInfo.gameCode = gameCode;
                            agFishInfo.MiseOrderGameId = MiseOrderGameId.AGFishing.SubGameCode;

                            agFishInfos.Add(agFishInfo);

                            dataCounter++;
                        }
                    }

                    #endregion 解析HSR

                    #region 解析HTR

                    else if (dataType == "HTR")
                    {
                        var flag = Regex.Match(line, "(?<=flag=\")[\\s\\S]*?(?=\")").ToString();
                        var type = Regex.Match(line, "(?<=transferType=\")[\\s\\S]*?(?=\")").ToString();

                        if (flag == "0" && type == "3")
                        {
                            var ProfitLossID = Regex.Match(line, "(?<=ID=\")[\\s\\S]*?(?=\")").ToString();
                            var tradeNo = Regex.Match(line, "(?<=ID=\")[\\s\\S]*?(?=\")").ToString();
                            var platformType = Regex.Match(line, "(?<=platformType=\")[\\s\\S]*?(?=\")").ToString();
                            var sceneId = "";
                            var playerName = Regex.Match(line, "(?<=playerName=\")[\\s\\S]*?(?=\")").ToString();
                            var SceneStartTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                            var SceneEndTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                            var Roomid = "";
                            var Roombet = "";
                            var Cost = "0";
                            var Earn = "0";
                            var Jackpotcomm = "0";
                            var transferAmount = Regex.Match(line, "(?<=transferAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var previousAmount = Regex.Match(line, "(?<=previousAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var currentAmount = Regex.Match(line, "(?<=currentAmount=\")[\\s\\S]*?(?=\")").ToString();
                            var currency = Regex.Match(line, "(?<=currency=\")[\\s\\S]*?(?=\")").ToString();
                            var exchangeRate = Regex.Match(line, "(?<=exchangeRate=\")[\\s\\S]*?(?=\")").ToString();
                            var IP = Regex.Match(line, "(?<=IP=\")[\\s\\S]*?(?=\")").ToString();
                            var creationTime = Regex.Match(line, "(?<=creationTime=\")[\\s\\S]*?(?=\")").ToString();
                            var gameCode = Regex.Match(line, "(?<=gameCode=\")[\\s\\S]*?(?=\")").ToString();

                            AgFishInfo agFishInfo = new AgFishInfo();
                            agFishInfo.dataType = dataType;
                            agFishInfo.ProfitLossID = ProfitLossID;
                            agFishInfo.tradeNo = tradeNo;
                            agFishInfo.platformType = platformType;
                            agFishInfo.sceneId = sceneId;
                            agFishInfo.playerName = playerName;
                            agFishInfo.type = type;
                            agFishInfo.SceneStartTime = Convert.ToDateTime(SceneStartTime).ToUniversalTime().AddHours(12);
                            agFishInfo.SceneEndTime = Convert.ToDateTime(SceneEndTime).ToUniversalTime().AddHours(12);
                            agFishInfo.Roomid = Roomid;
                            agFishInfo.Roombet = Roombet;
                            agFishInfo.Cost = decimal.Parse(Cost);
                            agFishInfo.Earn = decimal.Parse(Earn);
                            agFishInfo.Jackpotcomm = decimal.Parse(Jackpotcomm);
                            agFishInfo.transferAmount = decimal.Parse(transferAmount);
                            agFishInfo.previousAmount = decimal.Parse(previousAmount);
                            agFishInfo.currentAmount = decimal.Parse(currentAmount);
                            agFishInfo.currency = currency;
                            agFishInfo.exchangeRate = exchangeRate;
                            agFishInfo.IP = IP;
                            agFishInfo.flag = flag;
                            agFishInfo.creationTime = Convert.ToDateTime(creationTime).ToUniversalTime().AddHours(12);//API server 的时区是GMT-4
                            agFishInfo.gameCode = gameCode;
                            agFishInfo.MiseOrderGameId = MiseOrderGameId.AGFishing.SubGameCode;

                            agFishInfos.Add(agFishInfo);

                            dataCounter++;
                        }
                    }

                    #endregion 解析HTR
                }
                catch (Exception ex)
                {
                    failureDataCounter++;
                    LogUtil.Error("解析文件 " + xmlFile.LocalPath + " 失败，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);

                    return false;
                }
            }

            try
            {
                if (!xmlFile.LocalPath.IsNullOrEmpty() && File.Exists(xmlFile.LocalPath))
                {
                    File.Delete(xmlFile.LocalPath);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("删除文件 " + xmlFile.LocalPath + " 失败，详细信息：" + ex.Message);

                return false;
            }

            if (failureDataCounter > 0)
            {
                LogUtil.ForcedDebug($"解析文件 {agGameType.Value}/{xmlFile.Name} 完成 ，共获取到 " + dataCounter.ToString() + " 条亏赢数据，失败解析 " + failureDataCounter.ToString() + " 条数据");
            }
            else
            {
                LogUtil.ForcedDebug($"解析文件 {agGameType.Value}/{xmlFile.Name} 完成，共获取到 " + dataCounter.ToString() + " 条亏赢数据");
            }

            return true;
        }
    }

    public class ParseAGXmlFileMockService : IParseAGXmlFileService
    {
        public bool ConvertToAGAndFishInfo(AGGameType agGameType, XMLFile xmlFile, out List<AGInfo> agInfos, out List<AgFishInfo> agFishInfos)
        {
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.AGTransferService).AccountPrefixCode;
            string[] playerNames = new string[] { $"jackson", $"cts{accountPrefixCode}_3", $"msl{accountPrefixCode}_888" };
            agInfos = new List<AGInfo>();
            agFishInfos = new List<AgFishInfo>();

            for (int i = 0; i < playerNames.Length; i++)
            {
                string playerName = playerNames[i];
                long billNo = DateTime.Now.AddSeconds(i).ToUnixOfTime();
                DateTime betTime = DateTime.Now.AddMinutes(-10);

                agInfos.Add(new AGInfo()
                {
                    playerName = playerName,
                    billNo = billNo.ToString(),
                    dataType = "EBR",
                    mainbillno = billNo.ToString(),
                    beforeCredit = 40000.58m,
                    betAmount = 10070.89m,
                    validBetAmount = 50071.47m,
                    netAmount = 10070.89m,
                    betTime = betTime,
                    recalcuTime = betTime,
                    round = string.Empty,
                    gameType = "IN",
                    playType = "",
                    tableCode = "",
                    gameCode = "",
                    flag = "0",
                    platformType = "AGIN",
                    MiseOrderGameId = MiseOrderGameId.AGXin.SubGameCode,
                });

                billNo++;

                agFishInfos.Add(new AgFishInfo()
                {
                    playerName = playerName,
                    ProfitLossID = $"H_{billNo}",
                    tradeNo = billNo.ToString(),
                    dataType = "HSR",
                    sceneId = billNo.ToString(),
                    type = "1",
                    SceneStartTime = DateTime.Now.AddMinutes(-10),
                    SceneEndTime = DateTime.Now.AddMinutes(-10).AddSeconds(10),
                    Roomid = "1-26",
                    Roombet = "1",
                    Cost = 350.0000m,
                    Earn = 210.0000m,
                    Jackpotcomm = 1.2250m,
                    transferAmount = -138.775m,
                    previousAmount = 0.0m,
                    currentAmount = 0.0m,
                    currency = "CNY",
                    exchangeRate = "1",
                    IP = "",
                    flag = "0",
                    creationTime = DateTime.Now,
                    gameCode = "HMFP",
                    platformType = "HUNTER",
                    MiseOrderGameId = MiseOrderGameId.AGFishing.SubGameCode,
                });
            }

            return true;
        }
    }
}