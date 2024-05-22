using AgDataBase.DLL.FileService;
using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Old;
using ProductTransferService.AgDataBase.Common;
using ProductTransferService.AgDataBase.DLL;
using ProductTransferService.AgDataBase.DLL.FileService;
using ProductTransferService.AgDataBase.Model;
using System.Collections.Concurrent;
using System.Data;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : OldBaseTransferScheduleServiceV2
    {
        private readonly Lazy<IAGProfitLossInfo> _agProfitLossInfo;

        private readonly Lazy<IAGProfitlossSettingService> _agProfitlossSettingService;

        public ProductTransferScheduleService()
        {
            _agProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<IAGProfitLossInfo>(EnvUser);
            _agProfitlossSettingService = DependencyUtil.ResolveService<IAGProfitlossSettingService>();
        }

        private static readonly ConcurrentDictionary<string, int> _accountMap = new ConcurrentDictionary<string, int>();

        protected virtual bool IsAgTransferProfitLossEnabled => true;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override PlatformProduct Product => PlatformProduct.AG;

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
                typeof(AGProfitlossSettingService),
                typeof(AGOldSaveRemoteProfitLossInfo)
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

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            if (IsAgTransferProfitLossEnabled)
            {
                AddNewTaskJob(nameof(AgTransferProfitLoss), () => AgTransferProfitLoss(cancellationToken), cancellationToken);
            }

            //if (IsDownloadRemoteLostAndFoundProfitLossEnabled)
            //{
            //    AddNewTaskJob(nameof(AgTransferLostAndFoundProfitLoss), () => AgTransferLostAndFoundProfitLoss(cancellationToken), cancellationToken);
            //}

            return true;
        }

        /// <summary>
        /// 远程XML文件下载並且存檔
        /// </summary>
        /// <param name="_obj"></param>
        protected void AgTransferProfitLoss(CancellationToken cancellationToken)
        {
            DoJobWithCancellationToken(
                cancellationToken,
                jobIntervalSeconds: 60,
                doJob: () =>
                {
                    DoSaveRemoteBetLogToPlatformJob();

                    return true;
                });
        }

        public bool InitLocalAppSettings()
        {
            try
            {
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
                string url = configUtilService.Get("URL").Trim() + "/doBusiness.do?params={0}&key={1}";
                string cagent = configUtilService.Get("Cagent").Trim();
                string cur = configUtilService.Get("Cur").Trim();
                int actype = Convert.ToInt32(configUtilService.Get("Actype").Trim());
                string desKey = configUtilService.Get("DesKey").Trim();
                string md5Key = configUtilService.Get("Md5Key").Trim();

                AGConstParams.Url = configUtilService.Get("URL").Trim();
                AGConstParams.Cagent = cagent;
                AGConstParams.Cur = cur;
                AGConstParams.Actype = actype;
                AGConstParams.DesKey = desKey;
                AGConstParams.Md5Key = md5Key;

                return true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("AgTransferService异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);

                return false;
            }
        }

        private List<InsertTPGameProfitlossParam> GetInsertAGTPGameProfitlossParams()
        {
            DataTable dataTable = _agProfitLossInfo.Value.GetBatchDataFromLocalDB(AGProfitLossInfo.AGProfitLossInfoTableName);
            List<AGInfo> agInfos = _agProfitLossInfo.Value.ConvertToAGInfos(dataTable);
            HashSet<string> tpGameAccounts = agInfos.Select(s => s.playerName).Where(w => !_accountMap.ContainsKey(w)).ToHashSet();
            Dictionary<string, int> addAccountMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

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
                string memo = _agProfitlossSettingService.Value.CreateAGMemo(agInfo);
                decimal validBetMoney = agInfo.validBetAmount;
                decimal allBetMoney = agInfo.betAmount;
                decimal winMoney = agInfo.netAmount;
                BetResultType betResultType = winMoney.ToBetResultType();
                string playId = agInfo.billNo;
                string gameType = agInfo.MiseOrderGameId;
                bool isNoRebateAmount = false;

                if (_agProfitlossSettingService.Value.NoRebateKeywords.Any(keyword => memo.IndexOf(keyword) >= 0) ||
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
            DataTable dataTable = _agProfitLossInfo.Value.GetBatchDataFromLocalDB(AGProfitLossInfo.FishProfitLossInfoTableName);
            List<AgFishInfo> agFishInfos = _agProfitLossInfo.Value.ConvertToAgFishInfos(dataTable);

            HashSet<string> tpGameAccounts = agFishInfos.Select(s => s.playerName).ToHashSet();
            Dictionary<string, int> addAccountMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

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
                string memo = _agProfitlossSettingService.Value.CreateFishMemo(agFishInfo);
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

        protected override void DoSaveRemoteBetLogToPlatformJob()
        {
            foreach (AGGameType agGameType in AGGameType.GetAll())
            {
                //祕色目前只有直接抓oss的需求，不需額外處理LostAndFound資料夾，這邊寫法跟amd/d3不同，下載與處理沒有分為兩個排程
                _agProfitLossInfo.Value.DownloadRemoteNormalXMLData(agGameType);
                _agProfitLossInfo.Value.SaveLocalLostAndFoundXMLData(agGameType);
            }
        }

        protected override void DoDeleteExpiredProfitLoss()
        {
            var transferSqlLiteBackupRepository = DependencyUtil.ResolveService<ITransferSqlLiteBackupRepository>().Value;
            transferSqlLiteBackupRepository.DeleteExpiredDbFile();
        }
    }
}