using Autofac;
using IMBGDataBase.Merchant;
using IMSportsbookDataBase.BLL;
using IMSportsbookDataBase.Common;
using IMSportsbookDataBase.DLL;
using IMSportsbookDataBase.Enums;
using IMSportsbookDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IMSport;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace IMSportsbookTransferService
{
    public partial class IMSportsbookTransferService : OldBaseTransferScheduleServiceV2
    {
        protected static string _merchantCode;

        protected static string _currency;

        protected static string _language;

        protected static string _serviceUrl;

        protected static string _productWallet;

        protected static int _perOnceQueryMinutes;

        private readonly TransactionLogs _transactionLogs;

        public override JxApplication Application => JxApplication.IMSportTransferService;

        public override PlatformProduct Product => PlatformProduct.IMSport;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(IMSportBetDetailMSLService)).Keyed<IIMSportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(IMSportProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoInitSqlLite()
        {
            IMSportsbookDataBase.DLL.IMSportProfitLossInfo.InIt();
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
            refreshAvailableScores.Elapsed += RefreshIMSportAvailableScores;
            refreshAvailableScores.AutoReset = true;
            refreshAvailableScores.Enabled = true;
            refreshAvailableScores.Start();
            _jobNames.Add(nameof(RefreshIMSportAvailableScores));

            return true;
        }

        public IMSportsbookTransferService()
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
            if (IMSportsbookDataBase.DLL.IMSportProfitLossInfo.databaseOnline)
            {
                var param = new IMSportApiParamModel
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
        /// 每天5點刷新棋牌总额數據
        /// </summary>
        /// <param name="_obj"></param>
        private void RefreshIMSportAvailableScores(object _obj, System.Timers.ElapsedEventArgs e)
        {
            if (IsWork && DateTime.Now.Hour == 5)
            {
                var param = new IMSportApiParamModel
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
                LogsManager.InfoToTelegram("IMSportsbookTransferService 异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            return isLoadingFinish;
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new IMSportProfitLossInfo().GetBatchDataFromLocalDB();
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

                if (DateTime.TryParse(dr["LastUpdatedDate"].ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                decimal allBetMoney = Convert.ToDecimal(dr["StakeAmount"]);
                decimal validBetMoney = allBetMoney;
                decimal winMoney = Convert.ToDecimal(dr["WinLoss"]);
                string playId = dr["BetId"].ToString();
                string oddsType = dr["OddsType"].ToString();
                string imsportWagerType = dr["WagerType"].ToString();
                string resultStatus = dr["ResultStatus"].ToString();
                bool isCashout = dr["BetTradeStatus"].ToString() == SingleBetInfo.s_cashOutTradeStatus;
                string memo = dr["Memo"].ToString();
                string detailItems = dr["detailItems"].ToString();
                List<DetailItem> detailItemList = new List<DetailItem>();

                if (StringUtil.IsValidJson(detailItems))
                {
                    detailItemList = detailItems.Deserialize<List<DetailItem>>();
                }

                //兌現處理
                if (isCashout)
                {
                    winMoney = dr["BetTradeBuybackAmount"].ToString().ToDecimal() - allBetMoney;
                }

                //上線時alter add column會有舊資料問題
                BetResultType betResultType = winMoney.ToBetResultType();
                IMSportBetStatus imSportBetStatus = IMSportBetStatus.GetSingle(resultStatus);

                if (isCashout)
                {
                    betResultType = BetResultType.Cashout;
                }
                else
                {
                    if (imSportBetStatus != null && imSportBetStatus.BetResultType != null)
                    {
                        betResultType = imSportBetStatus.BetResultType;
                    }
                }

                if (detailItemList.Any() && IsComputeAdmissionBetMoney)
                {
                    List<HandicapInfo> handicapInfos = detailItemList
                        .Select(s => new HandicapInfo()
                        {
                            Handicap = oddsType,
                            Odds = s.Odds.ToDecimalNullable()
                        })
                        .ToList();

                    WagerType wagerType = ConvertToWagerType(imsportWagerType);

                    validBetMoney = _tpGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                    {
                        AllBetMoney = allBetMoney,
                        HandicapInfos = handicapInfos,
                        ProfitLossMoney = winMoney,
                        BetResultType = betResultType,
                        WagerType = wagerType,
                    });
                }

                InsertTPGameProfitlossParam insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = memo,
                    GameType = _productWallet,
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
             => new IMSportProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);

        private WagerType ConvertToWagerType(string imsportWagerType)
        {
            IMSportWagerType imSportWagerType = IMSportWagerType.GetSingle(imsportWagerType);

            if (imSportWagerType != null && imSportWagerType.WagerType != null)
            {
                return imSportWagerType.WagerType;
            }

            return WagerType.Single;
        }
    }
}