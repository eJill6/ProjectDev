using JxBackendService.Model.ThirdParty.Base;

namespace ProductTransferService.SportDataBase.Model
{
    public interface IBaseSabaSportBetDetailInfo
    {
        string Home_id { get; set; }

        List<LangName> HometeamName { get; set; }

        string Away_id { get; set; }

        List<LangName> AwayteamName { get; set; }

        string Bet_type { get; set; }

        List<LangName> BettypeName { get; set; }

        string League_id { get; set; }

        List<LangName> LeagueName { get; set; }

        string Odds { get; set; }

        string Sport_type { get; set; }

        List<LangName> SportName { get; set; }
    }

    public interface ISabaSportBetDetailInfo : IBaseSabaSportBetDetailInfo
    {
        string After_amount { get; set; }

        string Currency { get; set; }

        string Match_datetime { get; set; }

        string Match_id { get; set; }

        string Odds_type { get; set; }

        string Stake { get; set; }

        string Ticket_status { get; set; }

        string Ticket_extra_status { get; set; }

        string Trans_id { get; set; }

        string Transaction_time { get; set; }

        string Version_key { get; set; }

        string Winlost_amount { get; set; }

        string Winlost_datetime { get; set; }

        string Vendor_member_id { get; set; }

        List<SabaParlay> ParlayData { get; set; }

        string Settlement_time { get; set; }
    }

    public interface ISabaSportBetDetailName
    {
        string SportTypeName { get; set; }

        string AwayName { get; set; }

        string BetTypeName { get; set; }

        string HomeName { get; set; }

        string LeagueName { get; set; }

        string Memo { get; set; }
    }

    public class BetDetailName : ISabaSportBetDetailName
    {
        public string SportTypeName { get; set; }

        public string AwayName { get; set; }

        public string BetTypeName { get; set; }

        public string HomeName { get; set; }

        public string LeagueName { get; set; }

        public string Memo { get; set; }
    }

    public class BaseSabaSportBetDetailInfo : IBaseSabaSportBetDetailInfo
    {
        /// <summary>主場隊伍id</summary>
        public string Home_id { get; set; } = string.Empty;

        /// <summary>客場隊伍id</summary>
        public string Away_id { get; set; } = string.Empty;

        /// <summary>體育的種類 (1:足球, 2:籃球...在Sports Table中查询)</summary>
        public string Sport_type { get; set; } = string.Empty;

        /// <summary>玩法的種例 (1:Handicap(賠率), 2:單雙...在BetType TAble中查詢)</summary>
        public string Bet_type { get; set; } = string.Empty;

        /// <summary>聯盟id</summary>
        public string League_id { get; set; } = string.Empty;

        /// <summary>賠率</summary>
        public string Odds { get; set; } = string.Empty;

        public List<LangName> HometeamName { get; set; } = new List<LangName>();

        public List<LangName> AwayteamName { get; set; } = new List<LangName>();

        public List<LangName> BettypeName { get; set; } = new List<LangName>();

        public List<LangName> LeagueName { get; set; } = new List<LangName>();

        public List<LangName> SportName { get; set; } = new List<LangName>();
    }

    public class BaseSabaSportBetDetail : BaseSabaSportBetDetailInfo, ISabaSportBetDetailInfo
    {
        /// <summary>賽事id</summary>
        public string Match_id { get; set; } = string.Empty;

        /// <summary>玩家下注注金</summary>
        public string Stake { get; set; } = string.Empty;

        /// <summary>比賽日期</summary>
        public string Match_datetime { get; set; } = string.Empty;

        /// <summary>下單時間 (年/月/日/時/分/秒)</summary>
        public string Transaction_time { get; set; } = string.Empty;

        /// <summary>輸贏開出的日期</summary>
        public string Winlost_datetime { get; set; } = string.Empty;

        /// <summary>"+20" : 玩家贏20；"-30"：玩家輸30 )</summary>
        public string Winlost_amount { get; set; } = string.Empty;

        /// <summary>下完這張注單後剩下的金額</summary>
        public string After_amount { get; set; } = string.Empty;

        /// <summary>幣種(人民幣)</summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>下注状态</summary>
        public string Ticket_status { get; set; } = string.Empty;

        /// <summary>"cashout" : 表示该张注单已被实时兑现</summary>
        public string Ticket_extra_status { get; set; } = string.Empty;

        /// <summary>最后一个单号</summary>
        public string Version_key { get; set; } = string.Empty;

        /// <summary>用户</summary>
        public string Vendor_member_id { get; set; } = string.Empty;

        /// <summary>盤口類型代碼</summary>
        public string Odds_type { get; set; } = string.Empty;

        /// <summary>单据id</summary>
        public string Trans_id { get; set; } = string.Empty;

        public List<SabaParlay> ParlayData { get; set; }

        public string Settlement_time { get; set; } = string.Empty;
    }

    public class BetDetails : BaseSabaSportBetDetail, IComparable
    {
        public int CompareTo(object obj)
        {
            try
            {
                int res = 0;
                BetDetails sObj = (BetDetails)obj;
                if (DateTime.Parse(this.Transaction_time) > DateTime.Parse(sObj.Transaction_time))
                {
                    res = 1;
                }
                else if (DateTime.Parse(this.Transaction_time) < DateTime.Parse(sObj.Transaction_time))
                {
                    res = -1;
                }

                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class BetHorseDetails : BaseSabaSportBetDetail, IComparable
    {
        public int CompareTo(object obj)
        {
            try
            {
                int res = 0;
                BetHorseDetails sObj = (BetHorseDetails)obj;
                if (DateTime.Parse(this.Transaction_time) > DateTime.Parse(sObj.Transaction_time))
                {
                    res = 1;
                }
                else if (DateTime.Parse(this.Transaction_time) < DateTime.Parse(sObj.Transaction_time))
                {
                    res = -1;
                }

                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class BetLiveCasinoDetails : BaseSabaSportBetDetail, IComparable
    {
        public int CompareTo(object obj)
        {
            try
            {
                int res = 0;
                BetLiveCasinoDetails sObj = (BetLiveCasinoDetails)obj;
                if (DateTime.Parse(this.Transaction_time) > DateTime.Parse(sObj.Transaction_time))
                {
                    res = 1;
                }
                else if (DateTime.Parse(this.Transaction_time) < DateTime.Parse(sObj.Transaction_time))
                {
                    res = -1;
                }

                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class BetNumberDetails : BaseSabaSportBetDetail, IComparable
    {
        public int CompareTo(object obj)
        {
            try
            {
                int res = 0;
                BetNumberDetails sObj = (BetNumberDetails)obj;
                if (DateTime.Parse(this.Transaction_time) > DateTime.Parse(sObj.Transaction_time))
                {
                    res = 1;
                }
                else if (DateTime.Parse(this.Transaction_time) < DateTime.Parse(sObj.Transaction_time))
                {
                    res = -1;
                }

                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class BetVirtualSportDetails : BaseSabaSportBetDetail, IComparable
    {
        public int CompareTo(object obj)
        {
            try
            {
                int res = 0;
                BetVirtualSportDetails sObj = (BetVirtualSportDetails)obj;
                if (DateTime.Parse(this.Transaction_time) > DateTime.Parse(sObj.Transaction_time))
                {
                    res = 1;
                }
                else if (DateTime.Parse(this.Transaction_time) < DateTime.Parse(sObj.Transaction_time))
                {
                    res = -1;
                }

                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class BetCasinoDetails : BaseSabaSportBetDetail, IComparable
    {
        public int CompareTo(object obj)
        {
            try
            {
                int res = 0;
                BetCasinoDetails sObj = (BetCasinoDetails)obj;
                if (DateTime.Parse(this.Transaction_time) > DateTime.Parse(sObj.Transaction_time))
                {
                    res = 1;
                }
                else if (DateTime.Parse(this.Transaction_time) < DateTime.Parse(sObj.Transaction_time))
                {
                    res = -1;
                }

                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public interface ISabaParlay : IBaseSabaSportBetDetailInfo
    {
        string Away_hdp { get; set; }

        string Away_score { get; set; }

        string Bet_team { get; set; }

        string Hdp { get; set; }

        string Home_hdp { get; set; }

        string Home_score { get; set; }

        string Islive { get; set; }

        string Match_datetime { get; set; }

        string Match_id { get; set; }

        string Parlay_id { get; set; }

        string Ticket_status { get; set; }

        string Winlost_datetime { get; set; }
    }

    public class SabaParlay : BaseSabaSportBetDetailInfo, ISabaParlay
    {
        public string Parlay_id { get; set; }

        public string Match_id { get; set; }

        public string Match_datetime { get; set; }

        public string Bet_team { get; set; }

        public string Home_hdp { get; set; }

        public string Away_hdp { get; set; }

        public string Hdp { get; set; }

        public string Islive { get; set; }

        public string Home_score { get; set; }

        public string Away_score { get; set; }

        public string Ticket_status { get; set; }

        public string Winlost_datetime { get; set; }
    }

    public class SabaBetDetailViewModel : BaseRemoteBetLog
    {
        public ISabaSportBetDetailInfo SabaSportBetDetailInfo { get; set; }

        public ISabaSportBetDetailName SabaSportBetDetailName { get; set; }

        public override string KeyId => SabaSportBetDetailInfo.Trans_id;

        public override string TPGameAccount => SabaSportBetDetailInfo.Vendor_member_id;
    }
}