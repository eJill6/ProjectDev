using Autofac;
using IMBGDataBase.Merchant;
using IMPPDataBase.BLL;
using IMPPDataBase.Common;
using IMPPDataBase.DLL;
using IMPPDataBase.Enums;
using IMPPDataBase.Model;
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
using System.Globalization;
using System.Threading;

namespace IMPPTransferService
{
    public partial class IMPPTransferService : OldBaseTransferScheduleServiceV2
    {
        protected static string _merchantCode;

        protected static string _currency;

        protected static string _language;

        protected static string _serviceUrl;

        protected static string _productWallet;

        protected static int _perOnceQueryMinutes;

        private readonly TransactionLogs transactionLogs;

        public override JxApplication Application => JxApplication.IMPPTransferService;

        public override PlatformProduct Product => PlatformProduct.IMPP;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMPPBetDetailMSLService)).Keyed<IIMPPBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMPPProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMPPDataBase.DLL.IMPPProfitLossInfo.InIt();
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
            refreshAvailableScores.Elapsed += RefreshAvailableScores;
            refreshAvailableScores.AutoReset = true;
            refreshAvailableScores.Enabled = true;
            refreshAvailableScores.Start();
            _jobNames.Add(nameof(RefreshAvailableScores));

            return true;
        }

        public IMPPTransferService()
        {
            InitializeComponent();
            transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        /// <summary>
        /// 盈亏储存到本地
        /// </summary>
        /// <param name="_obj"></param>
        protected override void DoSaveBetLogToSQLiteJob()
        {
            if (IMPPDataBase.DLL.IMPPProfitLossInfo.databaseOnline)
            {
                var param = new IMPPApiParamModel
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
                    transactionLogs.GetBetInfo(param);
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
        private void RefreshAvailableScores(object _obj, System.Timers.ElapsedEventArgs e)
        {
            if (IsWork && DateTime.Now.Hour == 5)
            {
                var param = new IMPPApiParamModel
                {
                    ServiceUrl = _serviceUrl,
                    MerchantCode = _merchantCode,
                    ActType = ApiAction.TotalBalance,
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
                LogsManager.InfoToTelegram("IMPPTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new IMPPProfitLossInfo().GetBatchDataFromLocalDB();
            List<InsertTPGameProfitlossParam> insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();
            var tpGameAccounts = new HashSet<string>();

            foreach (DataRow dr in dataTable.Rows)
            {
                tpGameAccounts.Add(dr["PlayerId"].ToString());
            }

            Dictionary<string, int> accountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (DataRow dr in dataTable.Rows)
            {
                string tpGameAccount = dr["PlayerId"].ToString();

                if (!accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    continue;
                }

                string gameType = IMOneProviderType.GetSingle(dr["Provider"].ToString()).OrderGameId.SubGameCode;
                string dateCreated = dr["DateCreated"].ToString();
                string gameDate = dr["GameDate"].ToString();

                DateTime betTime;

                if (!gameDate.IsNullOrEmpty())
                {
                    betTime = DateTime.Parse(gameDate);
                }
                else if (!dateCreated.IsNullOrEmpty())
                {
                    betTime = DateTime.Parse(dateCreated);
                }
                else
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(dr["LastUpdatedDate"].ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                decimal betMoney = Convert.ToDecimal(dr["BetAmount"]);
                decimal winMoney = Convert.ToDecimal(dr["WinLoss"]);
                string palyId = dr["RoundId"].ToString();

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = palyId,
                    Memo = dr["memo"].ToString(),
                    GameType = gameType,
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
            => new IMPPProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);
    }
}