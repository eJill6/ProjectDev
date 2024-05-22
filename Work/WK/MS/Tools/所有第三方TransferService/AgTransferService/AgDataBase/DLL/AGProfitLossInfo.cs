using AgDataBase.DLL.FileService;
using AgDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Util;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Common.Util;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace AgDataBase.DLL
{
    public interface IAGProfitLossInfo : IOldProfitLossInfo
    {
        List<AgFishInfo> ConvertToAgFishInfos(DataTable dataTable);

        List<AGInfo> ConvertToAGInfos(DataTable dataTable);

        string CreateAGMemo(AGInfo agInfo);

        string CreateFishMemo(AgFishInfo agFishInfo);

        void TransferRemoteLostAndFoundXMLData(AGGameType agGameType);

        void TransferRemoteXMLData(AGGameType agGameType);
    }

    public class AGProfitLossInfo : OldProfitLossInfo, IAGProfitLossInfo
    {
        private static readonly string ftpAddress = string.Empty;

        private static readonly int ftpPort = 21;

        private static readonly string ftpUser = string.Empty;

        private static readonly string ftpPassword = string.Empty;

        private static readonly List<string> s_profitLossInfoTableNames = new List<string>() { "ProfitLossInfo", "FishProfitLossInfo" };

        public static string AGINXMLFileDir { get; private set; } = string.Empty;

        public static readonly string XINXMLFileDir = string.Empty;

        public static string HUNTERXMLFileDir { get; private set; } = string.Empty;

        public static string YOPLAYXMLFileDir { get; private set; } = string.Empty;

        public static string AGINLostAndFoundXMLFileDir { get; private set; } = string.Empty;

        public static string XINLostAndFoundXMLFileDir { get; private set; } = string.Empty;

        public static string HUNTERLostAndFoundXMLFileDir { get; private set; } = string.Empty;

        public static string YOPLAYLostAndFoundXMLFileDir { get; private set; } = string.Empty;

        public static string PartAGINXMLFileDir => @"AGINXMLFileDir\";

        public static string PartXINXMLFileDir => @"XINXMLFileDir\";

        public static string PartHUNTERXMLFileDir => @"HUNTERXMLFileDir\";

        public static string PartYOPLAYXMLFileDir => @"YOPLAYXMLFileDir\";

        public static string PartAGINLostAndFoundXMLFileDir => @"AGINLostAndFoundXMLFileDir\";

        public static string PartXINLostAndFoundXMLFileDir => @"XINLostAndFoundXMLFileDir\";

        public static string PartHUNTERLostAndFoundXMLFileDir => @"HUNTERLostAndFoundXMLFileDir\";

        public static string PartYOPLAYLostAndFoundXMLFileDir => @"YOPLAYLostAndFoundXMLFileDir\";

        private static readonly bool dataBaseOnline = true;

        private static readonly string dbFullName = string.Empty;

        public static readonly string AGProfitLossInfoTableName = "ProfitLossInfo";

        public static readonly string FishProfitLossInfoTableName = "FishProfitLossInfo";

        private readonly IParseAGXmlFileService _parseAGXmlFileService;

        private readonly IAGRemoteXmlFileService _agRemoteXmlFileService;

        protected override string SqliteProfitLossInfoTableName => throw new NotSupportedException();

        public static int FtpPort => ftpPort;

        public static string FtpAddress => ftpAddress;

        public static string FtpUser => ftpUser;

        public static string FtpPassword => ftpPassword;

        protected override int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, sqlParams);

        protected override DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);

        static AGProfitLossInfo()
        {
            try
            {
                ftpAddress = ConfigurationManager.AppSettings["ftpAddress"].ToTrimString(); //其餘非主平台改用oss串接,所以ftp設定只有在主平台拉取會用到
                ftpPort = ConfigurationManager.AppSettings["ftpPort"].ToTrimString().ToInt32(hasDefaultValue: true);
                ftpUser = ConfigurationManager.AppSettings["ftpUser"].ToTrimString();
                ftpPassword = ConfigurationManager.AppSettings["ftpPassword"].ToTrimString();

                AGINXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartAGINXMLFileDir}";
                XINXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartXINXMLFileDir}";
                HUNTERXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartHUNTERXMLFileDir}";
                YOPLAYXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartYOPLAYXMLFileDir}";
                AGINLostAndFoundXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartAGINLostAndFoundXMLFileDir}";
                XINLostAndFoundXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartXINLostAndFoundXMLFileDir}";
                HUNTERLostAndFoundXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartHUNTERLostAndFoundXMLFileDir}";
                YOPLAYLostAndFoundXMLFileDir = $"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\\{PartYOPLAYLostAndFoundXMLFileDir}";

                if (!Directory.Exists(AGINXMLFileDir))
                {
                    Directory.CreateDirectory(AGINXMLFileDir);
                }

                if (!Directory.Exists(XINXMLFileDir))
                {
                    Directory.CreateDirectory(XINXMLFileDir);
                }

                if (!Directory.Exists(HUNTERXMLFileDir))
                {
                    Directory.CreateDirectory(HUNTERXMLFileDir);
                }

                if (!Directory.Exists(YOPLAYXMLFileDir))
                {
                    Directory.CreateDirectory(YOPLAYXMLFileDir);
                }

                if (!Directory.Exists(AGINLostAndFoundXMLFileDir))
                {
                    Directory.CreateDirectory(AGINLostAndFoundXMLFileDir);
                }

                if (!Directory.Exists(XINLostAndFoundXMLFileDir))
                {
                    Directory.CreateDirectory(XINLostAndFoundXMLFileDir);
                }

                if (!Directory.Exists(HUNTERLostAndFoundXMLFileDir))
                {
                    Directory.CreateDirectory(HUNTERLostAndFoundXMLFileDir);
                }

                if (!Directory.Exists(YOPLAYLostAndFoundXMLFileDir))
                {
                    Directory.CreateDirectory(YOPLAYLostAndFoundXMLFileDir);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("初始化配置参数失败，详细信息：" + ex.Message);
            }

            try
            {
                dbFullName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "ProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateProfitLossInfo(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "FishProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateFishProfitLossInfo(dbFullName);
                }

                dataBaseOnline = true;
            }
            catch (Exception ex)
            {
                LogUtil.Error("初始化sqllite数据库失败，详细信息：" + ex.Message);
                dataBaseOnline = false;
            }
        }

        public AGProfitLossInfo()
        {
            _parseAGXmlFileService = DependencyUtil.ResolveService<IParseAGXmlFileService>();
            _agRemoteXmlFileService = DependencyUtil.ResolveKeyed<IAGRemoteXmlFileService>(SharedAppSettings.PlatformMerchant);
        }

        /// <summary>
        /// 下载远程 XML 文件並且存檔
        /// </summary>
        public void TransferRemoteXMLData(AGGameType agGameType)
        {
            List<XMLFile> downLoadXmlFiles = _agRemoteXmlFileService.GetAllXmlFiles(agGameType);

            if (!downLoadXmlFiles.Any())
            {
                return;
            }

            downLoadXmlFiles.Sort();

            foreach (XMLFile downLoadXmlFile in downLoadXmlFiles)
            {
                bool isSuccess = SaveDataToLocal(agGameType, downLoadXmlFile);
                var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);

                if (isSuccess && platformProductAGSettingService.IsDeleteRemoteFileAfterSave)
                {
                    if (downLoadXmlFile.IsDeleteFromRemote)
                    {
                        _agRemoteXmlFileService.DeleteRemoteFile(downLoadXmlFile);
                    }
                }
            }
        }

        /// <summary>
        /// 下载远程 XML 重试文件並且存檔
        /// </summary>
        public void TransferRemoteLostAndFoundXMLData(AGGameType agGameType)
        {
            List<XMLFile> downLoadXmlFiles = _agRemoteXmlFileService.GetAllLostAndFoundXmlFiles(agGameType);

            if (!downLoadXmlFiles.Any())
            {
                return;
            }

            downLoadXmlFiles.Sort();

            foreach (XMLFile downLoadXmlFile in downLoadXmlFiles)
            {
                bool isSuccess = SaveDataToLocal(agGameType, downLoadXmlFile);
                var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);

                if (isSuccess && platformProductAGSettingService.IsDeleteRemoteFileAfterSave)
                {
                    if (downLoadXmlFile.IsDeleteFromRemote)
                    {
                        _agRemoteXmlFileService.DeleteRemoteFile(downLoadXmlFile);
                    }
                }
            }
        }

        public List<AGInfo> ConvertToAGInfos(DataTable dataTable)
        {
            var agInfos = new List<AGInfo>();

            foreach (DataRow dr in dataTable.Rows)
            {
                DateTime betTime = Convert.ToDateTime(dr["betTime"]);

                if (!DateTime.TryParse(dr["recalcuTime"].ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = betTime;
                }

                var agInfo = new AGInfo
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    billNo = dr["billNo"].ToString(),
                    dataType = dr["dataType"].ToString(),
                    mainbillno = dr["mainbillno"].ToString(),
                    playerName = dr["playerName"].ToString(),
                    beforeCredit = Convert.ToDecimal(dr["beforeCredit"]),
                    betAmount = Convert.ToDecimal(dr["betAmount"]),
                    validBetAmount = Convert.ToDecimal(dr["validBetAmount"]),
                    netAmount = Convert.ToDecimal(dr["netAmount"]),
                    betTime = betTime,
                    platformType = dr["platformType"].ToString(),
                    round = dr["round"].ToString(),
                    gameType = dr["gameType"].ToString(),
                    playType = dr["playType"].ToString(),
                    tableCode = dr["tableCode"].ToString(),
                    gameCode = dr["gameCode"].ToString(),
                    flag = dr["flag"].ToString(),
                    MiseOrderGameId = dr["MiseOrderGameId"].ToString(),
                    recalcuTime = profitLossTime
                };

                agInfos.Add(agInfo);
            }

            return agInfos;
        }

        public List<AgFishInfo> ConvertToAgFishInfos(DataTable dataTable)
        {
            var agFishInfos = new List<AgFishInfo>();

            foreach (DataRow dr in dataTable.Rows)
            {
                var agFishInfo = new AgFishInfo
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    dataType = dr["dataType"].ToString(),
                    ProfitLossID = dr["ProfitLossID"].ToString(),
                    tradeNo = dr["tradeNo"].ToString(),
                    platformType = dr["platformType"].ToString(),
                    sceneId = dr["sceneId"].ToString(),
                    playerName = dr["playerName"].ToString(),
                    type = dr["type"].ToString(),
                    SceneStartTime = Convert.ToDateTime(dr["SceneStartTime"]),
                    SceneEndTime = Convert.ToDateTime(dr["SceneEndTime"]),
                    Roomid = dr["Roomid"].ToString(),
                    Roombet = dr["Roombet"].ToString(),
                    Cost = Convert.ToDecimal(dr["Cost"]),
                    Earn = Convert.ToDecimal(dr["Earn"]),
                    Jackpotcomm = Convert.ToDecimal(dr["Jackpotcomm"]),
                    transferAmount = Convert.ToDecimal(dr["transferAmount"]),
                    previousAmount = Convert.ToDecimal(dr["previousAmount"]),
                    currentAmount = Convert.ToDecimal(dr["currentAmount"]),
                    currency = dr["currency"].ToString(),
                    exchangeRate = dr["exchangeRate"].ToString(),
                    IP = dr["IP"].ToString(),
                    flag = dr["flag"].ToString(),
                    creationTime = Convert.ToDateTime(dr["creationTime"]),
                    gameCode = dr["gameCode"].ToString(),
                    MiseOrderGameId = dr["MiseOrderGameId"].ToString()
                };

                agFishInfos.Add(agFishInfo);
            }

            return agFishInfos;
        }

        public string CreateAGMemo(AGInfo agInfo)
        {
            string dbContentElement = typeof(DBContentElement).FullName;

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = "，",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            if (agInfo.dataType == "EBR" && agInfo.validBetAmount == 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_Game),
                });
            }

            if (!agInfo.PlatformTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_PlatformTypeName),
                    Args = new List<string>() { agInfo.PlatformTypeName, },
                });
            }

            if (!agInfo.RoundName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_RoundName),
                    Args = new List<string>() { agInfo.RoundName, },
                });
            }

            if (!agInfo.GameTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_GameTypeName),
                    Args = new List<string>() { agInfo.GameTypeName, },
                });
            }

            if (!agInfo.PlayTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_PlayTypeName),
                    Args = new List<string>() { agInfo.PlayTypeName, },
                });
            }

            if (!agInfo.tableCode.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_TableCode),
                    Args = new List<string>() { agInfo.tableCode, },
                });
            }

            if (!agInfo.gameCode.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_GameCode),
                    Args = new List<string>() { agInfo.gameCode, },
                });
            }

            return localizationParam.ToLocalizationJsonString();
        }

        public string CreateFishMemo(AgFishInfo agFishInfo)
        {
            string dbContentElement = typeof(DBContentElement).FullName;

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = "，",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            if (agFishInfo.type == "1")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo1),
                    Args = new List<string>() {
                        agFishInfo.PlatformTypeName,
                        agFishInfo.transferAmount.ToString(),
                        agFishInfo.sceneId,
                        agFishInfo.SceneStartTime.ToFormatDateTimeString(),
                        agFishInfo.SceneEndTime.ToFormatDateTimeString(),
                        agFishInfo.Roomid,
                        agFishInfo.Roombet,
                        agFishInfo.Cost.ToString(),
                        agFishInfo.Earn.ToString(),
                    },
                });
            }
            else if (agFishInfo.type == "2")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo2),
                    Args = new List<string>() {
                        agFishInfo.PlatformTypeName,
                        agFishInfo.sceneId,
                        agFishInfo.transferAmount.ToString(),
                    },
                });
            }
            else if (agFishInfo.type == "7")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo7),
                    Args = new List<string>() {
                        agFishInfo.PlatformTypeName,
                        agFishInfo.transferAmount.ToString(),
                        agFishInfo.sceneId,
                        agFishInfo.SceneStartTime.ToFormatDateTimeString(),
                        agFishInfo.SceneEndTime.ToFormatDateTimeString(),
                        agFishInfo.Roomid,
                        agFishInfo.Roombet,
                    },
                });
            }
            else if (agFishInfo.type == "3")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo3),
                });
            }

            return localizationParam.ToLocalizationJsonString();
        }

        public override void ClearExpiredData()
        {
            foreach (string profitLossInfoTableName in s_profitLossInfoTableNames)
            {
                DoClearExpiredData(profitLossInfoTableName);
            }
        }

        //public void ClearExpiredData()
        //{
        //    if (dataBaseOnline)
        //    {
        //        string deleteSql = @"
        //            DELETE FROM [{0}]
        //            WHERE Id IN (
        //                SELECT Id FROM [{0}]
        //                WHERE
        //                localSavedTime < @localSavedTime
        //                AND (RemoteSaved = 1 OR RemoteSaveTryCount >= 10)
        //                ORDER BY Id LIMIT 300)";

        //        string deleteProfitLossInfoSql = string.Format(deleteSql, "ProfitLossInfo");
        //        string deleteFishProfitLossInfoSql = string.Format(deleteSql, "FishProfitLossInfo");

        //        var localSavedTime = DateTime.Now.AddMonths(-1);

        //        SQLiteParameter[] parameters = { new SQLiteParameter
        //        {
        //            ParameterName = "@localSavedTime",
        //            Value = localSavedTime
        //        } };

        //        try
        //        {
        //            int rowCount1 = -1;
        //            int rowCount2 = -1;

        //            while (true)
        //            {
        //                if (rowCount1 != 0)
        //                {
        //                    rowCount1 = SQLiteDBHelper.ExecuteNonQuery(dbFullName, deleteProfitLossInfoSql, parameters);
        //                    LogUtil.ForcedDebug("从SqlLite数据库中删除 " + rowCount1.ToString() + " 条过期真人电子亏赢数据");
        //                }

        //                if (rowCount2 != 0)
        //                {
        //                    rowCount2 = SQLiteDBHelper.ExecuteNonQuery(dbFullName, deleteFishProfitLossInfoSql, parameters);
        //                    LogUtil.ForcedDebug("从SqlLite数据库中删除 " + rowCount2.ToString() + " 条过期捕鱼王亏赢数据");
        //                }

        //                if (rowCount1 == 0 && rowCount2 == 0)
        //                {
        //                    break;
        //                }

        //                Task.Delay(2000).Wait();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogUtil.ForcedDebug("从SqlLite数据库中删除过期亏赢数据失败，详细信息：" + ex.Message);
        //        }
        //    }
        //}

        /// <summary>
        /// 保存数据到本地数据库
        /// </summary>
        /// <param name="xmlFiles"></param>
        private bool SaveDataToLocal(AGGameType agGameType, XMLFile xmlFile)
        {
            bool isSuccess = _parseAGXmlFileService.ConvertToAGAndFishInfo(agGameType, xmlFile, out List<AGInfo> agInfos, out List<AgFishInfo> agFishInfos);

            try
            {
                if (!agInfos.Any() && !agFishInfos.Any())
                {
                    return isSuccess;
                }

                var savedDataCounter = 0;
                var unSavedDataCounter = 0;
                var failureSavedDataCounter = 0;

                #region 保存BR和EBR到本地库

                if (agInfos.Count > 0)
                {
                    string insertSql = @"
                        INSERT INTO [ProfitLossInfo](
                            [billNo]
                            ,[dataType]
                            ,[mainbillno]
                            ,[playerName]
                            ,[beforeCredit]
                            ,[betAmount]
                            ,[validBetAmount]
                            ,[netAmount]
                            ,[betTime]
                            ,[recalcuTime]
                            ,[platformType]
                            ,[round]
                            ,[gameType]
                            ,[playType]
                            ,[tableCode]
                            ,[gameCode]
                            ,[flag]
                            ,[MiseOrderGameId])
                        VALUES(
                            @billNo
                            ,@dataType
                            ,@mainbillno
                            ,@playerName
                            ,@beforeCredit
                            ,@betAmount
                            ,@validBetAmount
                            ,@netAmount
                            ,@betTime
                            ,@recalcuTime
                            ,@platformType
                            ,@round
                            ,@gameType
                            ,@playType
                            ,@tableCode
                            ,@gameCode
                            ,@flag
                            ,@MiseOrderGameId) ";

                    SQLiteParameter[] parameter =
                    {
                        new SQLiteParameter { ParameterName = "@billNo" },
                        new SQLiteParameter { ParameterName = "@dataType" },
                        new SQLiteParameter { ParameterName = "@mainbillno" },
                        new SQLiteParameter { ParameterName = "@playerName" },
                        new SQLiteParameter { ParameterName = "@beforeCredit" },
                        new SQLiteParameter { ParameterName = "@betAmount" },
                        new SQLiteParameter { ParameterName = "@validBetAmount" },
                        new SQLiteParameter { ParameterName = "@netAmount" },
                        new SQLiteParameter { ParameterName = "@betTime" },
                        new SQLiteParameter { ParameterName = "@recalcuTime" },
                        new SQLiteParameter { ParameterName = "@platformType" },
                        new SQLiteParameter { ParameterName = "@round" },
                        new SQLiteParameter { ParameterName = "@gameType" },
                        new SQLiteParameter { ParameterName = "@playType" },
                        new SQLiteParameter { ParameterName = "@tableCode" },
                        new SQLiteParameter { ParameterName = "@gameCode" },
                        new SQLiteParameter { ParameterName = "@flag" }, //16
                        new SQLiteParameter { ParameterName = "@MiseOrderGameId" } //17
                    };

                    agInfos.Sort();

                    foreach (AGInfo agInfo in agInfos)
                    {
                        try
                        {
                            if (!ExistsOrder(agInfo.billNo))
                            {
                                parameter[0].Value = agInfo.billNo;
                                parameter[1].Value = agInfo.dataType;
                                parameter[2].Value = agInfo.mainbillno;
                                parameter[3].Value = agInfo.playerName;
                                parameter[4].Value = agInfo.beforeCredit;
                                parameter[5].Value = agInfo.betAmount;
                                parameter[6].Value = agInfo.validBetAmount;
                                parameter[7].Value = agInfo.netAmount;
                                parameter[8].Value = agInfo.betTime;
                                parameter[9].Value = agInfo.recalcuTime;
                                parameter[10].Value = agInfo.platformType;
                                parameter[11].Value = agInfo.round;
                                parameter[12].Value = agInfo.gameType;
                                parameter[13].Value = agInfo.playType;
                                parameter[14].Value = agInfo.tableCode;
                                parameter[15].Value = agInfo.gameCode;
                                parameter[16].Value = agInfo.flag;
                                parameter[17].Value = agInfo.MiseOrderGameId;

                                SQLiteDBHelper.ExecuteNonQuery(dbFullName, insertSql, parameter);

                                savedDataCounter++;
                            }
                            else
                            {
                                unSavedDataCounter++;
                            }
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            failureSavedDataCounter++;
                            LogUtil.ForcedDebug("保存 " + agInfo.dataType + " 亏赢数据 " + agInfo.billNo + " 到本地SqlLite数据库失败，详细信息：" + ex.Message);
                        }
                    }
                }

                #endregion 保存BR和EBR到本地库

                #region 保存HSR，HTR到本地库

                if (agFishInfos.Count > 0)
                {
                    string sql1 = @"
                        SELECT
                            tradeNo
                        FROM [FishProfitLossInfo]
                        WHERE tradeNo = @tradeNo
                        LIMIT 0,1 ";

                    string sql2 = @"
                        INSERT INTO [FishProfitLossInfo](
                            [dataType]
                            ,[ProfitLossID]
                            ,[tradeNo]
                            ,[platformType]
                            ,[sceneId]
                            ,[playerName]
                            ,[type]
                            ,[SceneStartTime]
                            ,[SceneEndTime]
                            ,[Roomid]
                            ,[Roombet]
                            ,[Cost]
                            ,[Earn]
                            ,[Jackpotcomm]
                            ,[transferAmount]
                            ,[previousAmount]
                            ,[currentAmount]
                            ,[currency]
                            ,[exchangeRate]
                            ,[IP]
                            ,[flag]
                            ,[creationTime]
                            ,[gameCode]
                            ,[MiseOrderGameId])
                        VALUES(
                            @dataType
                            ,@ProfitLossID
                            ,@tradeNo
                            ,@platformType
                            ,@sceneId
                            ,@playerName
                            ,@type
                            ,@SceneStartTime
                            ,@SceneEndTime
                            ,@Roomid
                            ,@Roombet
                            ,@Cost
                            ,@Earn
                            ,@Jackpotcomm
                            ,@transferAmount
                            ,@previousAmount
                            ,@currentAmount
                            ,@currency
                            ,@exchangeRate
                            ,@IP
                            ,@flag
                            ,@creationTime
                            ,@gameCode
                            ,@MiseOrderGameId) ";

                    agFishInfos.Sort();

                    SQLiteParameter[] parameter1 = { new SQLiteParameter { ParameterName = "@tradeNo" } };

                    SQLiteParameter[] parameter2 =
                    {
                        new SQLiteParameter { ParameterName = "@dataType" },
                        new SQLiteParameter { ParameterName = "@ProfitLossID" },
                        new SQLiteParameter { ParameterName = "@tradeNo" },
                        new SQLiteParameter { ParameterName = "@platformType" },
                        new SQLiteParameter { ParameterName = "@sceneId" },
                        new SQLiteParameter { ParameterName = "@playerName" },
                        new SQLiteParameter { ParameterName = "@type" },
                        new SQLiteParameter { ParameterName = "@SceneStartTime" },
                        new SQLiteParameter { ParameterName = "@SceneEndTime" },
                        new SQLiteParameter { ParameterName = "@Roomid" },
                        new SQLiteParameter { ParameterName = "@Roombet" },
                        new SQLiteParameter { ParameterName = "@Cost" },
                        new SQLiteParameter { ParameterName = "@Earn" },
                        new SQLiteParameter { ParameterName = "@Jackpotcomm" },
                        new SQLiteParameter { ParameterName = "@transferAmount" },
                        new SQLiteParameter { ParameterName = "@previousAmount" },
                        new SQLiteParameter { ParameterName = "@currentAmount" },
                        new SQLiteParameter { ParameterName = "@currency" },
                        new SQLiteParameter { ParameterName = "@exchangeRate" },
                        new SQLiteParameter { ParameterName = "@IP" },
                        new SQLiteParameter { ParameterName = "@flag" },
                        new SQLiteParameter { ParameterName = "@creationTime" },
                        new SQLiteParameter { ParameterName = "@gameCode" }, //22
                        new SQLiteParameter { ParameterName = "@MiseOrderGameId" }, //23
                    };

                    foreach (AgFishInfo agFishInfo in agFishInfos)
                    {
                        try
                        {
                            parameter1[0].Value = agFishInfo.tradeNo;

                            if (SQLiteDBHelper.ExecuteScalar(dbFullName, sql1, parameter1) == null)
                            {
                                parameter2[0].Value = agFishInfo.dataType;
                                parameter2[1].Value = agFishInfo.ProfitLossID;
                                parameter2[2].Value = agFishInfo.tradeNo;
                                parameter2[3].Value = agFishInfo.platformType;
                                parameter2[4].Value = agFishInfo.sceneId;
                                parameter2[5].Value = agFishInfo.playerName;
                                parameter2[6].Value = agFishInfo.type;
                                parameter2[7].Value = agFishInfo.SceneStartTime;
                                parameter2[8].Value = agFishInfo.SceneEndTime;
                                parameter2[9].Value = agFishInfo.Roomid;
                                parameter2[10].Value = agFishInfo.Roombet;
                                parameter2[11].Value = agFishInfo.Cost;
                                parameter2[12].Value = agFishInfo.Earn;
                                parameter2[13].Value = agFishInfo.Jackpotcomm;
                                parameter2[14].Value = agFishInfo.transferAmount;
                                parameter2[15].Value = agFishInfo.previousAmount;
                                parameter2[16].Value = agFishInfo.currentAmount;
                                parameter2[17].Value = agFishInfo.currency;
                                parameter2[18].Value = agFishInfo.exchangeRate;
                                parameter2[19].Value = agFishInfo.IP;
                                parameter2[20].Value = agFishInfo.flag;
                                parameter2[21].Value = agFishInfo.creationTime;
                                parameter2[22].Value = agFishInfo.gameCode;
                                parameter2[23].Value = agFishInfo.MiseOrderGameId;

                                SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql2, parameter2);

                                savedDataCounter++;
                            }
                            else
                            {
                                unSavedDataCounter++;
                            }
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            failureSavedDataCounter++;
                            LogUtil.ForcedDebug("保存 " + agFishInfo.dataType + " 亏赢数据 " + agFishInfo.tradeNo + " 到本地SqlLite数据库失败，详细信息：" + ex.Message);
                        }
                    }
                }

                #endregion 保存HSR，HTR到本地库

                if (agInfos.Count > 0 || agFishInfos.Count > 0)
                {
                    if (failureSavedDataCounter > 0)
                    {
                        LogUtil.ForcedDebug("保存亏赢数据到本地数据文件完成，共保存 " + savedDataCounter.ToString() + " 条数据，过滤 " + unSavedDataCounter.ToString() + " 条重复数据，失败保存 " + failureSavedDataCounter.ToString() + " 条数据");
                    }
                    else
                    {
                        LogUtil.ForcedDebug("保存亏赢数据到本地数据文件完成，共保存 " + savedDataCounter.ToString() + " 条数据，过滤 " + unSavedDataCounter.ToString() + " 条重复数据");
                    }
                }

                return isSuccess;
            }
            finally
            {
                if (isSuccess)
                {
                    xmlFile.IsDeleteFromRemote = true;
                }
            }
        }

        public static bool ExistsOrder(string billNo)
        {
            string sql = @"SELECT billNo FROM [ProfitLossInfo] WHERE billNo = @billNo LIMIT 0,1 ";

            SQLiteParameter[] parameter = {
                new SQLiteParameter { ParameterName = "@billNo" }
            };

            parameter[0].Value = billNo;

            return SQLiteDBHelper.ExecuteScalar(dbFullName, sql, parameter) != null;
        }
    }
}