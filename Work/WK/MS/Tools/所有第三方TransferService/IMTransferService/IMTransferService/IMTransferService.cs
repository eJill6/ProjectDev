using Autofac;
using IMBGDataBase.Merchant;
using IMDataBase.BLL;
using IMDataBase.Common;
using IMDataBase.DLL;
using IMDataBase.Enums;
using IMDataBase.Model;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace IMTransferService
{
    public partial class IMTransferService : OldBaseTransferScheduleServiceV2
    {
        protected static string _merchantCode;

        protected static string _currency;

        protected static string _language;

        protected static string _serviceUrl;

        protected static string _productWallet;

        protected static int _perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        public override JxApplication Application => JxApplication.IMTransferService;

        public override PlatformProduct Product => PlatformProduct.IM;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMBetDetailMSLService)).Keyed<IIMBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMDataBase.DLL.IMProfitLossInfo.InIt();
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
            refreshAvailableScores.Elapsed += RefreshIMAvailableScores;
            refreshAvailableScores.AutoReset = true;
            refreshAvailableScores.Enabled = true;
            refreshAvailableScores.Start();
            _jobNames.Add(nameof(RefreshIMAvailableScores));

            return true;
        }

        public IMTransferService()
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
            if (IMDataBase.DLL.IMProfitLossInfo.databaseOnline)
            {
                var param = new IMApiParamModel
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

                _transactionLogs.GetBetInfo(param);

                //目前ProductWallet只有一個, 先把這段註解
                //若未來改成多個, 請參考ProductCode的方式在下一層做foreach
                //foreach (var product in _productWallet.Split(';'))
                //{
                //    param.ProductWallet = product;
                //    _transactionLogs.GetBetInfo(param);
                //}
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
        private void RefreshIMAvailableScores(object _obj, System.Timers.ElapsedEventArgs e)
        {
            if (IsWork && DateTime.Now.Hour == 5)
            {
                var param = new IMApiParamModel
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
                LogsManager.InfoToTelegram("IMTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new IMProfitLossInfo().GetBatchDataFromLocalDB();
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

                if (!DateTime.TryParse(dr["WagerCreationDateTime"].ToString(), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(dr["SettlementDateTime"].ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                string memo = dr["memo"].ToString();
                decimal allBetMoney = Convert.ToDecimal(dr["StakeAmount"]);
                decimal validBetMoney = allBetMoney;
                decimal winMoney = Convert.ToDecimal(dr["WinLoss"]);
                string playId = dr["BetId"].ToString();
                string oddsType = dr["OddsType"].ToString();
                string gameTypeName;
                BetResultType betResultType = winMoney.ToBetResultType();

                string detailItems = dr["detailItems"].ToString();
                List<DetailItem> detailItemList = detailItems.Deserialize<List<DetailItem>>();
                gameTypeName = string.Join("/", detailItemList
                    .Select(s => s.SportsName.IsNullOrEmpty() ? s.BetDescription : s.SportsName))
                    .ToShortString(IMProfitLossInfo.GameNameMaxLength);

                if (IsComputeAdmissionBetMoney)
                {
                    List<HandicapInfo> handicapInfos = new List<HandicapInfo>();

                    if (detailItemList.AnyAndNotNull())
                    {
                        handicapInfos.AddRange(
                            detailItemList.
                            Select(s => new HandicapInfo()
                            {
                                Handicap = oddsType,
                                Odds = s.Odds.ToDecimalNullable()
                            }).
                            ToList());
                    }

                    validBetMoney = _tpGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                    {
                        AllBetMoney = allBetMoney,
                        HandicapInfos = handicapInfos,
                        ProfitLossMoney = winMoney,
                        BetResultType = betResultType,
                        WagerType = WagerType.Single
                    });
                }

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = memo,
                    GameType = gameTypeName,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = betResultType.Value,
                    KeyId = dr["id"].ToString(),
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        protected override bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
            => new IMProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);
    }
}