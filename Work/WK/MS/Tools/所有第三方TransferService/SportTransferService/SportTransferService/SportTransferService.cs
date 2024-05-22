using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Threading;
using SportDataBase.Common;
using SportDataBase.Model;
using SportDataBase.BLL;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendService.Common.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using SportDataBase.DLL;
using SportDataBase.Merchant;
using System.Linq;
using Autofac;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Old;

namespace SportTransferService
{
    public partial class SportTransferService : OldBaseTransferScheduleServiceV2
    {
        private static readonly string s_ticketExtraStatusCashout = "cashout";

        private readonly TransactionLogs transactionLogs = null;

        public SportTransferService()
        {
            InitializeComponent();
            transactionLogs = new TransactionLogs(EnvUser, DbConnectionTypes.Master);
        }

        protected virtual bool IsWorkRefreshSportAvailableScores => true;

        //環境變數
        public override JxApplication Application => JxApplication.SportTransferService;

        public override PlatformProduct Product => PlatformProduct.Sport;

        protected override void DoInitSqlLite()
        {
            SportDataBase.DLL.SportProfitLossInfo.InIt();
        }

        protected override bool DoInitialJobOnStart()
        {
            if (!InitLocalAppSettings())
            {
                return false;
            }

            AddNewThreadJob(nameof(RefreshSportAvailableScores), RefreshSportAvailableScores);

            return true;
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(SportBetDetailMSLService)).Keyed<ISportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
            containerBuilder.RegisterType(typeof(ApiClient)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(SportProfitLossInfo)).AsImplementedInterfaces();
        }

        protected override void DoSaveBetLogToSQLiteJob()
        {
            if (SportDataBase.DLL.SportProfitLossInfo.dataBaseOnline)
            {
                transactionLogs.GetBetInfo();
            }
            else
            {
                LogsManager.Info("等待初始化sqllite数据库");
            }
        }

        private void RefreshSportAvailableScores(object _obj)
        {
            while (IsWork && IsWorkRefreshSportAvailableScores)
            {
                try
                {
                    Thread.Sleep(60 * 60 * 1000);

                    if (DateTime.Now.Hour == 5)
                    {
                        new Transfer(EnvUser, DbConnectionTypes.Master).RefreshAvailableScores(false);
                    }
                }
                catch (Exception ex)
                {
                    LogsManager.Error(ex);
                }
            }
        }        

        public bool InitLocalAppSettings()
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch (Exception ex)
            {
                LogsManager.Info("SportTransferService异常，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        protected override List<InsertTPGameProfitlossParam> GetInsertTPGameProfitlossParams()
        {
            DataTable dataTable = new SportProfitLossInfo().GetBatchDataFromLocalDB();
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();
            var tpGameAccounts = new HashSet<string>();

            foreach (DataRow dr in dataTable.Rows)
            {
                tpGameAccounts.Add(dr["vendor_member_id"].ToString()); //第三方帳號
            }

            Dictionary<string, int> accountMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts);

            foreach (DataRow dr in dataTable.Rows)
            {
                var insertParam = new InsertTPGameProfitlossParam()
                {
                    KeyId = dr["id"].ToString(),
                };

                string tpGameAccount = dr["vendor_member_id"].ToString();

                if (!accountMap.TryGetValue(tpGameAccount, out int userId))
                {
                    insertParam.IsIgnore = true;
                    insertTPGameProfitlossParams.Add(insertParam); //sport寫入sqlite的時候沒有篩選平台帳號

                    continue;
                }

                insertParam.UserID = userId;
                insertParam.PlayID = dr["trans_id"].ToString();

                if (!DateTime.TryParse(dr["transaction_time"].ToString().Replace("T", " "), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(dr["settlement_time"].ToString().Replace("T", " "), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                //saba來的資料會出現輸贏時間<交易時間
                if (profitLossTime < betTime)
                {
                    profitLossTime = DateTime.Now;
                }

                decimal allBetMoney = Convert.ToDecimal(dr["stake"]);
                decimal validBetMoney = allBetMoney;
                decimal winMoney = Convert.ToDecimal(dr["winlost_amount"]);
                string oddsType = dr["Odds_Type"].ToString();
                string odds = dr["odds"].ToString();
                string ticketStatus = dr["ticket_status"].ToString().ToLower();
                string ticketExtraStatus = dr["ticket_extra_status"].ToString().ToLower();
                string betType = dr["bet_type"].ToString();

                BetResultType betResultType = null;

                if (ticketExtraStatus == s_ticketExtraStatusCashout)
                {
                    betResultType = BetResultType.Cashout;
                }
                else
                {
                    SabaTicketStatus sabaTicketStatus = SabaTicketStatus.GetSingle(ticketStatus);

                    if (sabaTicketStatus != null && sabaTicketStatus.BetResultType != null)
                    {
                        betResultType = sabaTicketStatus.BetResultType;
                    }
                }

                if (betResultType == null)
                {
                    betResultType = insertParam.WinMoney.ToBetResultType();
                }

                insertParam.BetTime = betTime;
                insertParam.ProfitLossTime = profitLossTime;
                insertParam.WinMoney = winMoney;
                insertParam.BetResultType = betResultType.Value;
                insertParam.Memo = dr["memo"].ToString();

                var gameTypeItems = new List<string>
                {
                    dr["sport_type_text"].ToString(),
                    dr["league_id_text"].ToString(),
                    dr["home_id_text"].ToString(),
                    dr["away_id_text"].ToString()
                };

                gameTypeItems.RemoveAll(r => r.IsNullOrEmpty());
                insertParam.GameType = string.Join(",", gameTypeItems).ToShortString(197);

                if (IsComputeAdmissionBetMoney)
                {
                    WagerType wagerType = TransactionLogs.ConvertToWagerType(betType);
                    List<HandicapInfo> handicapInfos;

                    if (wagerType == WagerType.Combo && !dr["ParlayDataJson"].ToNonNullString().IsNullOrEmpty())
                    {
                        List<SabaParlay> sabaParlays = dr["ParlayDataJson"].ToString().Deserialize<List<SabaParlay>>();
                        handicapInfos = sabaParlays.Select(s => new HandicapInfo()
                        {
                            Handicap = oddsType,
                            Odds = s.Odds.ToDecimalNullable()
                        }).ToList();
                    }
                    else
                    {
                        handicapInfos = new List<HandicapInfo>()
                        {
                            new HandicapInfo()
                            {
                                Handicap = oddsType,
                                Odds = odds.ToDecimalNullable()
                            }
                        };
                    }

                    validBetMoney = _tpGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                    {
                        AllBetMoney = allBetMoney,
                        HandicapInfos = handicapInfos,
                        ProfitLossMoney = winMoney,
                        BetResultType = betResultType,
                        WagerType = wagerType
                    });
                }

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        protected override bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
             => new SportProfitLossInfo().UpdateSQLiteToSavedStatus(keyId, saveBetLogFlag);
    }
}