using Autofac;
using IMBGDataBase.Merchant;
using IMPTDataBase.BLL;
using IMPTDataBase.Common;
using IMPTDataBase.DLL;
using IMPTDataBase.Enums;
using IMPTDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;

namespace IMPTTransferService
{
    public partial class IMPTTransferService : OldBaseTransferScheduleServiceV2
    {
        protected static string _merchantCode;

        protected static string _currency;

        protected static string _language;

        protected static string _serviceUrl;

        protected static string _productWallet;

        protected static int _perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        public override JxApplication Application => JxApplication.IMPTTransferService;

        public override PlatformProduct Product => PlatformProduct.IMPT;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMPTBetDetailMSLService)).Keyed<IIMPTBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMPTProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMPTDataBase.DLL.IMPTProfitLossInfo.InIt();
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
            refreshAvailableScores.Elapsed += RefreshAvailableScores_Elapsed;
            refreshAvailableScores.AutoReset = true;
            refreshAvailableScores.Enabled = true;
            refreshAvailableScores.Start();

            return true;
        }

        public IMPTTransferService()
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
            if (IMPTDataBase.DLL.IMPTProfitLossInfo.databaseOnline)
            {
                var param = new IMPTApiParamModel
                {
                    ServiceUrl = _serviceUrl,
                    MerchantCode = _merchantCode,
                    ActType = ApiAction.PlayGameResult,
                    ProductWallet = _productWallet,
                    Currency = _currency,
                    Language = _language,
                    Page = 1,
                    PerOnceQueryMinutes = _perOnceQueryMinutes
                };

                foreach (var product in _productWallet.Split(';'))
                {
                    param.ProductWallet = product;
                    _transactionLogs.GetBetInfo(param);
                }
            }
            else
            {
                LogsManager.Info("等待初始化 sqllite 数据库");
            }
        }

        /// <summary>
        /// 每天5點刷新总额數據
        /// </summary>
        /// <param name="_obj"></param>
        private void RefreshAvailableScores_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsWork && DateTime.Now.Hour == 5)
            {
                var param = new IMPTApiParamModel
                {
                    ServiceUrl = _serviceUrl,
                    MerchantCode = _merchantCode,
                    ActType = ApiAction.PlayGameResult,
                    ProductWallet = _productWallet,
                    Currency = _currency,
                    Language = _language,
                };

                new Transfer(EnvUser, DbConnectionTypes.Master).RefreshAvailableScores(param);
            }
        }

        public bool InitLocalAppSettings()
        {
            bool isLoadingFinish = false;

            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                _serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"].Trim();
                _merchantCode = ConfigurationManager.AppSettings["MerchantCode"].Trim();
                _currency = ConfigurationManager.AppSettings["Currency"].Trim();
                _language = ConfigurationManager.AppSettings["Language"].Trim();
                _productWallet = ConfigurationManager.AppSettings["ProductWallet"].Trim();
                _perOnceQueryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["PerOnceQueryMinutes"].Trim());

                isLoadingFinish = true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("IMPTTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new IMPTProfitLossInfo().GetBatchDataFromLocalDB();
            List<InsertTPGameProfitlossParam> insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();
            var tpGameAccounts = new HashSet<string>();

            foreach (DataRow dr in dataTable.Rows)
            {
                tpGameAccounts.Add(dr["PlayerName"].ToString());
            }

            Dictionary<string, int> accountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (DataRow dr in dataTable.Rows)
            {
                string tpGameAccount = dr["PlayerName"].ToString();

                if (!accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    continue;
                }

                if (!DateTime.TryParse(dr["GameDate"].ToString(), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                DateTime profitLossTime = betTime;
                decimal betMoney = Convert.ToDecimal(dr["Bet"]);
                decimal prizeMoney = Convert.ToDecimal(dr["Win"].ToString());
                decimal winMoney = prizeMoney - betMoney;
                string playId = dr["GameCode"].ToString();
                string sourceGameType = dr["GameType"].ToNonNullString();

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = dr["memo"].ToString(),
                    GameType = IMPTGameType.GetIMPTGameType(sourceGameType).OrderGameId.SubGameCode,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = winMoney.ToBetResultType().Value,
                    KeyId = dr["id"].ToString(),
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, betMoney, betMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        protected override bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
             => new IMPTProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);
    }
}