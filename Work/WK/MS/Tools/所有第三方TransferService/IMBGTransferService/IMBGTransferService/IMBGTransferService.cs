using Autofac;
using IMBGDataBase.BLL;
using IMBGDataBase.Common;
using IMBGDataBase.DLL;
using IMBGDataBase.Enums;
using IMBGDataBase.Merchant;
using IMBGDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace IMBGTransferService
{
    public partial class IMBGTransferService : OldBaseTransferScheduleServiceV2
    {
        protected static string _merchantCode;

        protected static string _language;

        protected static string _serviceUrl;

        protected static string _md5Key;

        protected static string _desKey;

        protected static int _perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        public override JxApplication Application => JxApplication.IMBGTransferService;

        public override PlatformProduct Product => PlatformProduct.IMBG;

        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMBGBetDetailMSLService)).Keyed<IIMBGBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMBGProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMBGDataBase.DLL.IMBGProfitLossInfo.InIt();
        }

        protected override bool DoInitialJobOnStart()
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            //因為有時序性，因此保留下列作法
            var hourTimeInterval = 1 * 60 * 1000;

            var refreshAvailableScores = new System.Timers.Timer(hourTimeInterval);
            refreshAvailableScores.Elapsed += RefreshIMBGAvailableScores;
            refreshAvailableScores.AutoReset = true;
            refreshAvailableScores.Enabled = true;
            refreshAvailableScores.Start();
            _jobNames.Add(nameof(RefreshIMBGAvailableScores));

            return true;
        }

        public IMBGTransferService()
        {
            InitializeComponent();
            _transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        /// <summary>
        /// 盈亏储存到本地
        /// </summary>
        /// <param name="_obj"></param>
        protected override void DoSaveBetLogToSQLiteJob()
        {
            if (IMBGDataBase.DLL.IMBGProfitLossInfo.databaseOnline)
            {
                var param = new IMBGApiParamModel
                {
                    ServiceUrl = _serviceUrl,
                    MerchantCode = _merchantCode,
                    ActType = ApiAction.PlayGameResult,
                    Language = _language,
                    Page = 1,
                    PerOnceQueryMinutes = _perOnceQueryMinutes,
                    MD5Key = _md5Key,
                    DesKey = _desKey
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
        private void RefreshIMBGAvailableScores(object _obj, System.Timers.ElapsedEventArgs e)
        {
            if (IsWork && DateTime.Now.Hour == 5)
            {
                var param = new IMBGApiParamModel
                {
                    ServiceUrl = _serviceUrl,
                    MerchantCode = _merchantCode,
                    ActType = ApiAction.PlayGameResult,
                    Language = _language,
                    MD5Key = _md5Key,
                    DesKey = _desKey
                };

                new Transfer(EnvUser, DbConnectionTypes.Master)
                    .RefreshAvailableScores(param);
            }
        }

        protected bool InitLocalAppSettings()
        {
            bool isLoadingFinish = false;
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                _merchantCode = ConfigurationManager.AppSettings["MerchantCode"].Trim();
                _serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"].Trim();
                _language = ConfigurationManager.AppSettings["Language"].Trim();
                _md5Key = ConfigurationManager.AppSettings["MD5Key"].Trim();
                _desKey = ConfigurationManager.AppSettings["DesKey"].Trim();
                _perOnceQueryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["PerOnceQueryMinutes"].Trim());

                isLoadingFinish = true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("IMBGTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new IMBGProfitLossInfo().GetBatchDataFromLocalDB();
            List<InsertTPGameProfitlossParam> insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();
            var tpGameAccounts = new HashSet<string>();

            foreach (DataRow dr in dataTable.Rows)
            {
                tpGameAccounts.Add(dr["UserCode"].ToString());
            }

            Dictionary<string, int> accountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (DataRow dr in dataTable.Rows)
            {
                string tpGameAccount = dr["UserCode"].ToString();

                if (!accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    continue;
                }

                int? gameId = null;

                if (int.TryParse(dr["GameId"].ToString(), out int value))
                {
                    gameId = value;
                }

                string gameName = TransactionLogs.GetGameName(gameId);

                DateTime gameStartTime = DateTime.Now;
                DateTime gameEndTime = DateTime.Now;

                if (!string.IsNullOrEmpty(dr["OpenTime"].ToString()))
                {
                    gameStartTime = DateTimeUtility.Instance.ToLocalTime(dr["OpenTime"].ToString());
                }

                if (!string.IsNullOrEmpty(dr["EndTime"].ToString()))
                {
                    gameEndTime = DateTimeUtility.Instance.ToLocalTime(dr["EndTime"].ToString());
                }

                decimal validBetMoney = Convert.ToDecimal(dr["AllBills"]);
                decimal allBetMoney = Convert.ToDecimal(dr["EffectBet"]);
                decimal winMoney = Convert.ToDecimal(dr["WinLost"]);
                string playId = dr["Id"].ToString();

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = dr["memo"].ToString(),
                    GameType = gameName,
                    BetTime = gameStartTime,
                    ProfitLossTime = gameEndTime,
                    WinMoney = winMoney,
                    BetResultType = winMoney.ToBetResultType().Value,
                    KeyId = playId,
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        protected override bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
            => new IMBGProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);
    }
}