using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceN6.Common.Util;
using ProductTransferService.AgDataBase.DBUtility;
using ProductTransferService.AgDataBase.DLL.FileService;
using ProductTransferService.AgDataBase.Model;
using System.Data;

namespace ProductTransferService.AgDataBase.DLL
{
    public interface IAGProfitLossInfo : IOldProfitLossInfo, IEnvLoginUserService
    {
        List<AgFishInfo> ConvertToAgFishInfos(DataTable dataTable);

        List<AGInfo> ConvertToAGInfos(DataTable dataTable);

        void DownloadRemoteNormalXMLData(AGGameType agGameType);

        void DownloadRemoteLostAndFoundXMLData(AGGameType agGameType);

        void SaveLocalLostAndFoundXMLData(AGGameType agGameType);
    }

    public class AGProfitLossInfo : OldProfitLossInfo, IAGProfitLossInfo
    {
        public static string AGINXMLFileDir { get; private set; } = string.Empty;

        public static string XINXMLFileDir { get; private set; } = string.Empty;

        public static string HUNTERXMLFileDir { get; private set; } = string.Empty;

        public static string YOPLAYXMLFileDir { get; private set; } = string.Empty;

        public static string PartAGINXMLFileDir => @"AGINXMLFileDir\";

        public static string PartXINXMLFileDir => @"XINXMLFileDir\";

        public static string PartHUNTERXMLFileDir => @"HUNTERXMLFileDir\";

        public static string PartYOPLAYXMLFileDir => @"YOPLAYXMLFileDir\";

        private static readonly string dbFullName = string.Empty;

        public static string AGProfitLossInfoTableName => "ProfitLossInfo";

        public static string FishProfitLossInfoTableName => "FishProfitLossInfo";

        private readonly Lazy<IParseAGXmlFileService> _parseAGXmlFileService;

        private readonly Lazy<IAGRemoteXmlFileService> _agRemoteXmlFileService;

        private readonly Lazy<IAGOldSaveProfitLossInfo> _agOldSaveProfitLossInfo;

        public static int FtpPort { get; private set; }

        public static string FtpAddress { get; private set; }

        public static string FtpUser { get; private set; }

        public static string FtpPassword { get; private set; }

        public static string DbFullName => dbFullName;

        protected override object GetSqliteLock() => SQLiteDBHelper.WriteLockobj;

        protected override IDbConnection GetSqliteConnection() => SQLiteDBHelper.GetSQLiteConnection(dbFullName);

        static AGProfitLossInfo()
        {
            try
            {
                dbFullName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + "data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("初始化sqllite数据库失败，详细信息：" + ex.Message);
            }
        }

        public AGProfitLossInfo(EnvironmentUser envLoginUser)
        {
            _parseAGXmlFileService = DependencyUtil.ResolveService<IParseAGXmlFileService>();
            _agRemoteXmlFileService = DependencyUtil.ResolveKeyed<IAGRemoteXmlFileService>(SharedAppSettings.PlatformMerchant);
            _agOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IAGOldSaveProfitLossInfo>(envLoginUser);
            var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;

            try
            {
                FtpAddress = configUtilService.Get("ftpAddress").ToTrimString(); //其餘非主平台改用oss串接,所以ftp設定只有在主平台拉取會用到
                FtpPort = configUtilService.Get("ftpPort").ToTrimString().ToInt32(hasDefaultValue: true);
                FtpUser = configUtilService.Get("ftpUser").ToTrimString();
                FtpPassword = configUtilService.Get("ftpPassword").ToTrimString();

                AGINXMLFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PartAGINXMLFileDir);
                XINXMLFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PartXINXMLFileDir);
                HUNTERXMLFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PartHUNTERXMLFileDir);
                YOPLAYXMLFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PartYOPLAYXMLFileDir);

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
            }
            catch (Exception ex)
            {
                LogUtil.Error("初始化配置参数失败，详细信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 下载远程 XML 文件並且存檔
        /// </summary>
        public void DownloadRemoteNormalXMLData(AGGameType agGameType)
        {
            _agRemoteXmlFileService.Value.DownloadAllRemoteNormalXmlFiles(agGameType);
        }

        /// <summary>
        /// 下载远程 XML 重试文件並且存檔
        /// </summary>
        public void DownloadRemoteLostAndFoundXMLData(AGGameType agGameType)
        {
            _agRemoteXmlFileService.Value.DownloadAllRemoteLostAndFoundXmlFiles(agGameType);
        }

        /// <summary>
        /// 讀取本地端 XML 重试文件並且存檔
        /// </summary>
        public void SaveLocalLostAndFoundXMLData(AGGameType agGameType)
        {
            List<XMLFile> downLoadXmlFiles = _agRemoteXmlFileService.Value.GetAllLocalXmlFiles(agGameType);

            if (!downLoadXmlFiles.Any())
            {
                return;
            }

            var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);

            foreach (XMLFile downLoadXmlFile in downLoadXmlFiles)
            {
                bool isSuccess = SaveDataToLocal(agGameType, downLoadXmlFile);

                if (isSuccess && platformProductAGSettingService.Value.IsDeleteRemoteFileAfterSave)
                {
                    if (downLoadXmlFile.IsDeleteFromRemote)
                    {
                        _agRemoteXmlFileService.Value.DeleteRemoteFile(downLoadXmlFile);
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
            bool isSuccess = _parseAGXmlFileService.Value.ConvertToAGAndFishInfo(agGameType, xmlFile, out List<AGInfo> agInfos, out List<AgFishInfo> agFishInfos);

            try
            {
                if (!agInfos.Any() && !agFishInfos.Any())
                {
                    return isSuccess;
                }

                agInfos = agInfos.DistinctBy(d => d.billNo).ToList();
                agFishInfos = agFishInfos.DistinctBy(d => d.tradeNo).ToList();

                var savedDataCounter = 0;
                var unSavedDataCounter = 0;
                var failureSavedDataCounter = 0;

                #region 保存BR和EBR到本地库

                var agBetLogs = new List<BaseAGInfoModel>();

                if (agInfos.Count > 0)
                {
                    agBetLogs.AddRange(agInfos);
                }

                #endregion 保存BR和EBR到本地库

                #region 保存HSR，HTR到本地库

                if (agFishInfos.Count > 0)
                {
                    agBetLogs.AddRange(agFishInfos);
                }

                _agOldSaveProfitLossInfo.Value.SaveDataToTarget(agBetLogs);
                savedDataCounter += agBetLogs.Count;

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
            catch (Exception)
            {
                isSuccess = false;

                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    xmlFile.IsDeleteFromRemote = true;
                }
            }
        }

        private HashSet<string> GetExistsBillNos(List<string> billNos)
        {
            return GetExistColumnSet("ProfitLossInfo", "billNo", billNos);
        }

        private HashSet<string> GetExistsFishTradeNos(List<string> tradeNos)
        {
            return GetExistColumnSet("FishProfitLossInfo", "tradeNo", tradeNos);
        }
    }
}