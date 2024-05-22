using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;
using LCDataBase.BLL;
using LCDataBase.Common;
using LCDataBase.DLL;
using LCDataBase.Enums;
using LCDataBase.Merchant;
using LCDataBase.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace LCTransferService
{
    public partial class LCTransferService : OldBaseTransferScheduleServiceV2
    {
        protected static string _agentID;

        protected static string _currency;

        protected static string _serviceUrl;

        protected static string _serviceUrlWithRecord;

        protected static string _md5Key;

        protected static string _desKey;

        protected static string _linecode;

        protected static int _perOnceQueryMinutes;

        private string LCUserHeader => _agentID + "_";

        private readonly TransactionLogs _transactionLogs;

        public override JxApplication Application => JxApplication.LCTransferService;

        public override PlatformProduct Product => PlatformProduct.LC;

        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(LCBetDetailMSLService)).Keyed<ILCBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(LCProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            LCProfitLossInfo.InIt();
        }

        protected override bool DoInitialJobOnStart()
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            //因為有時序性，因此保留下列作法
            var hourTimeInterval = 60 * 60 * 1000;

            var refreshAvailableScores = new System.Timers.Timer(hourTimeInterval);
            refreshAvailableScores.Elapsed += RefreshLCAvailableScores;
            refreshAvailableScores.AutoReset = true;
            refreshAvailableScores.Enabled = true;
            refreshAvailableScores.Start();
            _jobNames.Add(nameof(RefreshLCAvailableScores));

            return true;
        }

        public LCTransferService()
        {
            InitializeComponent();
            _transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        protected override void DoSaveBetLogToSQLiteJob()
        {
            if (LCDataBase.DLL.LCProfitLossInfo.databaseOnline)
            {
                var param = new LCApiParamModel
                {
                    ServiceUrl = _serviceUrlWithRecord,
                    AgentID = _agentID,
                    ActType = ApiAction.PlayGameResult,
                    Currency = _currency,
                    DESKey = _desKey,
                    MD5Key = _md5Key,
                    Linecode = _linecode,
                    PerOnceQueryMinutes = _perOnceQueryMinutes
                };

                _transactionLogs.GetBetInfo(param);
            }
            else
            {
                LogsManager.Info("等待初始化 sqllite 数据库");
            }
        }

        /// <summary>
        /// 每天5點刷新棋牌总额數據
        /// </summary>
        /// <param name="_obj"></param>
        private void RefreshLCAvailableScores(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsWork && DateTime.Now.Hour == 5)
            {
                var param = new LCApiParamModel
                {
                    ServiceUrl = _serviceUrl,
                    AgentID = _agentID,
                    Currency = _currency,
                    DESKey = _desKey,
                    MD5Key = _md5Key
                };

                new Transfer(EnvUser, DbConnectionTypes.Master).RefreshAvailableScores(param);
            }
        }

        /// <summary>
        /// 初始化設置資料
        /// </summary>
        /// <returns></returns>
        public bool InitLocalAppSettings()
        {
            bool isLoadingFinish = false;
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                _serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"].Trim();
                _serviceUrlWithRecord = ConfigurationManager.AppSettings["ServiceUrlWithRecord"].Trim();
                _agentID = ConfigurationManager.AppSettings["AgentID"].Trim();
                _currency = ConfigurationManager.AppSettings["Currency"].Trim();
                _md5Key = ConfigurationManager.AppSettings["MD5Key"].Trim();
                _desKey = ConfigurationManager.AppSettings["DesKey"].Trim();
                _linecode = ConfigurationManager.AppSettings["LCLinecode"].Trim();
                _perOnceQueryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["PerOnceQueryMinutes"].Trim());

                isLoadingFinish = true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("LCTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new LCProfitLossInfo().GetBatchDataFromLocalDB();
            List<InsertTPGameProfitlossParam> insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();
            var tpGameAccounts = new HashSet<string>();

            foreach (DataRow dr in dataTable.Rows)
            {
                string account = dr["Account"].ToString().Replace(LCUserHeader, string.Empty);
                tpGameAccounts.Add(account);
            }

            Dictionary<string, int> accountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (DataRow dr in dataTable.Rows)
            {
                string tpGameAccount = dr["Account"].ToString().Replace(LCUserHeader, string.Empty);
                bool isIgnore = false;

                if (!accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    isIgnore = true;
                }

                if (!DateTime.TryParse(dr["GameStartTime"].ToString(), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(dr["GameEndTime"].ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                string playGameName = EnumHelper<GameKind>
                    .GetEnumDescription(Enum.GetName(typeof(GameKind), Convert.ToInt32(dr["KindID"].ToString())));

                string playRoomName = EnumHelper<RoomType>
                    .GetEnumDescription(Enum.GetName(typeof(RoomType), Convert.ToInt32(dr["ServerID"].ToString())));

                string playGameAllName = playGameName + (string.IsNullOrEmpty(playRoomName) ? "" : "-" + playRoomName);

                decimal validBetMoney = Convert.ToDecimal(dr["CellScore"]);
                decimal winMoney = Convert.ToDecimal(dr["Profit"]);
                string palyId = dr["GameID"].ToString();
                string memo = dr["memo"].ToString();
                decimal allBetMoney = Convert.ToDecimal(dr["AllBet"]);

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = palyId,
                    Memo = memo,
                    GameType = playGameAllName,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = winMoney.ToBetResultType().Value,
                    KeyId = dr["id"].ToString(),
                    IsIgnore = isIgnore,
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        protected override bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
             => new LCProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);
    }
}