using AgDataBase.BLL;
using AgDataBase.Common;
using AgDataBase.DLL;
using AgDataBase.DLL.FileService;
using AgDataBase.Model;
using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;

namespace AgTransferService
{
    public partial class AgTransferService : OldBaseTransferScheduleServiceV2
    {
        public AgTransferService()
        {
            InitializeComponent();
            _userBll = new UserBll(EnvUser, DbConnectionTypes.Master);
            _agProfitLossInfo = DependencyUtil.ResolveService<IAGProfitLossInfo>();
        }

        protected static string url;

        protected static string cagent;

        protected static string cur;

        protected static int actype;

        protected static string desKey;

        protected static string md5Key;

        protected static bool IsWorkAgTransferProfitLoss;

        protected static bool IsWorkAgTransferLostAndFoundProfitLoss;

        protected static bool IsWorkClearExpiredAgProfitLoss;

        protected static bool IsWorkRepaireAgAvailableScores;

        protected static bool IsWorkRefreshAgAvailableScores;

        private static readonly ConcurrentDictionary<string, int> _accountMap = new ConcurrentDictionary<string, int>();

        private static readonly string[] _noRebateKeywords = new string[] { "免费", "抽奖", "奖励" };

        private readonly UserBll _userBll;

        private readonly IAGProfitLossInfo _agProfitLossInfo;

        protected virtual bool IsAgTransferProfitLossEnabled => true;

        protected virtual bool IsDownloadRemoteLostAndFoundProfitLossEnabled => true;

        protected virtual bool IsRepaireAgAvailableScoresEnabled => true;

        protected virtual bool IsRefreshAgAvailableScoresEnabled => true;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        public override JxApplication Application => JxApplication.AGTransferService;

        public override PlatformProduct Product => PlatformProduct.AG;

        private readonly Dictionary<PlatformMerchant, Type> _agRemoteXmlFileServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
        {
            { PlatformMerchant.MiseLiveStream, typeof(AGRemoteOssXmlFileService) }
        };

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            PlatformProduct product = PlatformProduct.AG;

            foreach (PlatformMerchant merchant in PlatformMerchant.GetAll())
            {
                if (product.PlatformProductSettingServiceTypeMap != null)
                {
                    Type platformProductSettingServiceType = product.PlatformProductSettingServiceTypeMap[merchant];
                    containerBuilder.RegisterType(platformProductSettingServiceType).Keyed<IPlatformProductAGSettingService>(merchant.Value).SingleInstance();
                }
            }

            foreach (KeyValuePair<PlatformMerchant, Type> keyValuePair in _agRemoteXmlFileServiceTypeMap)
            {
                Type agRemoteXmlFileServiceType = keyValuePair.Value;
                containerBuilder.RegisterType(agRemoteXmlFileServiceType).Keyed<IAGRemoteXmlFileService>(keyValuePair.Key.Value).SingleInstance();
            }

            var registerTypes = new List<Type>()
            {
                typeof(AgApi),
                typeof(AGProfitLossInfo),
                typeof(ParseAGXmlFileService),
                typeof(AGProfitLoss),
            };

            foreach (Type registerType in registerTypes)
            {
                containerBuilder.RegisterType(registerType).AsImplementedInterfaces();
            }
        }

        protected override void DoInitSqlLite()
        {
            //建構子已經處理過了
        }

        protected override bool DoInitialJobOnStart()
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            if (IsAgTransferProfitLossEnabled)
            {
                AddNewThreadJob(nameof(AgTransferProfitLoss), AgTransferProfitLoss);
            }

            if (IsDownloadRemoteLostAndFoundProfitLossEnabled)
            {
                AddNewThreadJob(nameof(AgTransferLostAndFoundProfitLoss), AgTransferLostAndFoundProfitLoss);
            }

            if (IsRepaireAgAvailableScoresEnabled)
            {
                AddNewThreadJob(nameof(RepaireAgAvailableScores), RepaireAgAvailableScores);
            }

            if (IsRefreshAgAvailableScoresEnabled)
            {
                AddNewThreadJob(nameof(RefreshAgAvailableScores), RefreshAgAvailableScores);
            }

            return true;
        }

        protected override void DoSaveBetLogToSQLiteJob()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 远程XML文件下载並且存檔
        /// </summary>
        /// <param name="_obj"></param>
        protected void AgTransferProfitLoss(object _obj)
        {
            while (IsWork && IsWorkAgTransferProfitLoss)
            {
                try
                {
                    foreach (AGGameType agGameType in AGGameType.GetAll())
                    {
                        _agProfitLossInfo.TransferRemoteXMLData(agGameType);
                    }

                    Thread.Sleep(60000);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }
        }

        /// <summary>
        /// 下载远程 XML 重试文件並且存檔
        /// </summary>
        protected void AgTransferLostAndFoundProfitLoss(object _obj)
        {
            while (IsWork && IsWorkAgTransferLostAndFoundProfitLoss)
            {
                try
                {
                    foreach (AGGameType agGameType in AGGameType.GetAll())
                    {
                        _agProfitLossInfo.TransferRemoteLostAndFoundXMLData(agGameType);
                    }

                    Thread.Sleep(10 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }
        }

        private void RepaireAgAvailableScores(object _obj)
        {
            while (IsWork && IsWorkRepaireAgAvailableScores)
            {
                try
                {
                    _userBll.RepaireNegativeAvailableScores();

                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }
        }

        private void RefreshAgAvailableScores(object _obj)
        {
            while (IsWork && IsWorkRefreshAgAvailableScores)
            {
                try
                {
                    if (DateTime.Now.Hour == 4)
                    {
                        _userBll.RefreshAvailableScores();
                    }

                    Thread.Sleep(60 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }
        }

        public bool InitLocalAppSettings()
        {
            try
            {
                url = ConfigurationManager.AppSettings["URL"].Trim() + "/doBusiness.do?params={0}&key={1}";
                cagent = ConfigurationManager.AppSettings["Cagent"].Trim();
                cur = ConfigurationManager.AppSettings["Cur"].Trim();
                actype = Convert.ToInt32(ConfigurationManager.AppSettings["Actype"].Trim());
                desKey = ConfigurationManager.AppSettings["DesKey"].Trim();
                md5Key = ConfigurationManager.AppSettings["Md5Key"].Trim();

                IsWorkAgTransferProfitLoss = ConfigurationManager.AppSettings["IsWorkAgTransferProfitLoss"].Trim() == "Y" ? true : false;
                IsWorkAgTransferLostAndFoundProfitLoss = ConfigurationManager.AppSettings["IsWorkAgTransferLostAndFoundProfitLoss"].Trim() == "Y" ? true : false;
                IsWorkClearExpiredAgProfitLoss = ConfigurationManager.AppSettings["IsWorkClearExpiredAgProfitLoss"].Trim() == "Y" ? true : false;
                IsWorkRepaireAgAvailableScores = ConfigurationManager.AppSettings["IsWorkRepaireAgAvailableScores"].Trim() == "Y" ? true : false;
                IsWorkRefreshAgAvailableScores = ConfigurationManager.AppSettings["IsWorkRefreshAgAvailableScores"].Trim() == "Y" ? true : false;

                AGConstParams.url = ConfigurationManager.AppSettings["URL"].Trim();
                AGConstParams.cagent = cagent;
                AGConstParams.cur = cur;
                AGConstParams.actype = actype;
                AGConstParams.desKey = desKey;
                AGConstParams.md5Key = md5Key;

                return true;
            }
            catch (Exception ex)
            {
                LogsManager.Info("AgTransferService异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 因為有分ag跟捕魚,直接覆寫處理邏輯
        /// </summary>
        /// <param name="state"></param>
        protected override void SaveBetLogToPlatformJob(object state)
        {
            while (IsWork)
            {
                try
                {
                    var dataMap = new Dictionary<string, List<InsertTPGameProfitlossParam>>
                    {
                        { AGProfitLossInfo.AGProfitLossInfoTableName, GetInsertAGTPGameProfitlossParams() },
                        { AGProfitLossInfo.FishProfitLossInfoTableName, GetInsertFishTPGameProfitlossParams() }
                    };

                    var userIds = new List<int>();

                    foreach (KeyValuePair<string, List<InsertTPGameProfitlossParam>> keyValuePair in dataMap)
                    {
                        userIds.AddRange(keyValuePair.Value.Where(w => !w.IsIgnore).Select(s => s.UserID));
                    }

                    userIds = userIds.Distinct().ToList();
                    Dictionary<int, UserScore> userScoreMap = GetUserScoreMap(userIds);

                    foreach (KeyValuePair<string, List<InsertTPGameProfitlossParam>> keyValuePair in dataMap)
                    {
                        string tableName = keyValuePair.Key;
                        var tpGameProfitlosses = keyValuePair.Value;

                        if (tpGameProfitlosses.Any())
                        {
                            _tpGameApiService.SaveProfitlossToPlatform(new SaveProfitlossToPlatformParam()
                            {
                                TPGameProfitlosses = tpGameProfitlosses,
                                UpdateSQLiteToSavedStatus = (keyId, saveBetLogFlag)
                                    => _agProfitLossInfo.UpdateSQLiteToSavedStatus(tableName, keyId, saveBetLogFlag),
                                UserScoreMap = userScoreMap
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"从{Product.Name}把注單資料送回平台資料庫(SaveBetLogToPlatformJob)出现异常，信息：" + ex.Message + ",堆栈:" + ex.StackTrace);
                }

                Thread.Sleep(SaveBetLogToPlatformJobIntervalSeconds * 1000);
            }
        }

        protected override bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
        {
            throw new NotImplementedException();
        }

        private List<InsertTPGameProfitlossParam> GetInsertAGTPGameProfitlossParams()
        {
            DataTable dataTable = _agProfitLossInfo.GetBatchDataFromLocalDB(AGProfitLossInfo.AGProfitLossInfoTableName);
            List<AGInfo> agInfos = _agProfitLossInfo.ConvertToAGInfos(dataTable);
            HashSet<string> tpGameAccounts = agInfos.Select(s => s.playerName).Where(w => !_accountMap.ContainsKey(w)).ToHashSet();
            Dictionary<string, int> addAccountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (KeyValuePair<string, int> tpGameAccountMap in addAccountMap)
            {
                _accountMap.TryAdd(tpGameAccountMap.Key, tpGameAccountMap.Value);
            }

            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (AGInfo agInfo in agInfos)
            {
                string tpGameAccount = agInfo.playerName;
                bool isIgnore = agInfo.IsIgnoreAddProfitLoss;

                if (!_accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    isIgnore = true; //因為AG會把其他平台的資料也收到sqlite, 必須回寫retry次數
                }

                string dataType = agInfo.dataType;
                string memo = _agProfitLossInfo.CreateAGMemo(agInfo);
                decimal validBetMoney = agInfo.validBetAmount;
                decimal allBetMoney = agInfo.betAmount;
                decimal winMoney = agInfo.netAmount;
                BetResultType betResultType = winMoney.ToBetResultType();
                string playId = agInfo.billNo;
                string gameType = agInfo.MiseOrderGameId;
                bool isNoRebateAmount = false;

                if (_noRebateKeywords.Any(keyword => memo.IndexOf(keyword) >= 0) ||
                    (agInfo.dataType == "EBR" && agInfo.validBetAmount == 0))
                {
                    isNoRebateAmount = true;
                }

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = memo,
                    GameType = gameType,
                    BetTime = agInfo.betTime,
                    ProfitLossTime = agInfo.recalcuTime.Value,
                    WinMoney = winMoney,
                    BetResultType = betResultType.Value,
                    KeyId = agInfo.Id.ToString(),
                    IsIgnore = isIgnore,
                    FromSource = AGProfitLossInfo.AGProfitLossInfoTableName,
                    IsNoRebateAmount = isNoRebateAmount
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        private List<InsertTPGameProfitlossParam> GetInsertFishTPGameProfitlossParams()
        {
            DataTable dataTable = _agProfitLossInfo.GetBatchDataFromLocalDB(AGProfitLossInfo.FishProfitLossInfoTableName);
            List<AgFishInfo> agFishInfos = _agProfitLossInfo.ConvertToAgFishInfos(dataTable);

            HashSet<string> tpGameAccounts = agFishInfos.Select(s => s.playerName).ToHashSet();
            Dictionary<string, int> addAccountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (KeyValuePair<string, int> tpGameAccountMap in addAccountMap)
            {
                _accountMap.TryAdd(tpGameAccountMap.Key, tpGameAccountMap.Value);
            }

            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (AgFishInfo agFishInfo in agFishInfos)
            {
                string tpGameAccount = agFishInfo.playerName;
                bool isIgnore = agFishInfo.IsIgnoreAddProfitLoss;

                if (!_accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    isIgnore = true;
                }

                string dataType = agFishInfo.dataType;
                string memo = _agProfitLossInfo.CreateFishMemo(agFishInfo);
                decimal betMoney = agFishInfo.Cost;
                decimal winMoney = agFishInfo.transferAmount;
                BetResultType betResultType = winMoney.ToBetResultType();
                string playId = agFishInfo.tradeNo;
                string gameType = agFishInfo.MiseOrderGameId;

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = memo,
                    GameType = gameType,
                    BetTime = agFishInfo.creationTime,
                    ProfitLossTime = agFishInfo.SceneEndTime,
                    WinMoney = winMoney,
                    BetResultType = betResultType.Value,
                    KeyId = agFishInfo.Id.ToString(),
                    IsIgnore = isIgnore,
                    FromSource = AGProfitLossInfo.FishProfitLossInfoTableName,
                    IsNoRebateAmount = true
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, betMoney, betMoney);
                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}