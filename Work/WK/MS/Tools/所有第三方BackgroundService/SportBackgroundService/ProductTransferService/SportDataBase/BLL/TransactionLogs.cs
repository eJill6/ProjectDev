using System.Text;
using ProductTransferService.SportDataBase.Common;
using ProductTransferService.SportDataBase.DLL;
using ProductTransferService.SportDataBase.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.Enums;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using ProductTransferService.SportDataBase.Merchant;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.Model.Util;
using JxBackendService.Common.Extensions;
using System.Collections.Concurrent;
using JxBackendService.Resource.Element;

using JxBackendService.Common.Util;

namespace ProductTransferService.SportDataBase.BLL
{
    public class TransactionLogs : BaseTransactionLogs<SportApiParamModel, ApiResult<BetResult>>
    {
        private readonly int _maxDetailMemoContentCount = 8;

        private static readonly ConcurrentDictionary<string, string> s_sportTypeDic = new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<string, string> s_betTypeDic = new ConcurrentDictionary<string, string>();

        private static readonly HashSet<string> s_wagerComboBetTypes = new HashSet<string>() { "29", "38", "2799" };

        private static readonly ConcurrentDictionary<string, string> s_leagueNameMap = new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<string, string> s_teamNameMap = new ConcurrentDictionary<string, string>();

        private readonly IApiClient _apiClient;

        private readonly SportProfitLossInfo _profitLossInfo;

        private readonly Lazy<ISportOldSaveProfitLossInfo> _sportOldSaveProfitLossInfo;

        protected override IBetDetailService<SportApiParamModel, ApiResult<BetResult>> ResolveBetDetailService(PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<ISportBetDetailService>(platformMerchant).Value;
        }

        public TransactionLogs(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            #region init SportType

            if (s_sportTypeDic.Count == 0)
            {
                s_sportTypeDic.TryAdd("1", "Soccer");
                s_sportTypeDic.TryAdd("2", "Basketball");
                s_sportTypeDic.TryAdd("3", "Football");
                s_sportTypeDic.TryAdd("4", "Ice Hockey");
                s_sportTypeDic.TryAdd("5", "Tennis");
                s_sportTypeDic.TryAdd("6", "Volleyball");
                s_sportTypeDic.TryAdd("7", "Billiards");
                s_sportTypeDic.TryAdd("8", "Baseball");
                s_sportTypeDic.TryAdd("9", "Badminton");
                s_sportTypeDic.TryAdd("10", "Golf");
                s_sportTypeDic.TryAdd("11", "Motorsports");
                s_sportTypeDic.TryAdd("12", "Swimming");
                s_sportTypeDic.TryAdd("13", "Politics");
                s_sportTypeDic.TryAdd("14", "Water Polo");
                s_sportTypeDic.TryAdd("15", "Diving");
                s_sportTypeDic.TryAdd("16", "Boxing");
                s_sportTypeDic.TryAdd("17", "Archery");
                s_sportTypeDic.TryAdd("18", "Table Tennis");
                s_sportTypeDic.TryAdd("19", "Weightlifting");
                s_sportTypeDic.TryAdd("20", "Canoeing");
                s_sportTypeDic.TryAdd("21", "Gymnastics");
                s_sportTypeDic.TryAdd("22", "Athletics");
                s_sportTypeDic.TryAdd("23", "Equestrian");
                s_sportTypeDic.TryAdd("24", "Handball");
                s_sportTypeDic.TryAdd("25", "Darts");
                s_sportTypeDic.TryAdd("26", "Rugby");
                s_sportTypeDic.TryAdd("27", "Cricket");
                s_sportTypeDic.TryAdd("28", "Field Hockey");
                s_sportTypeDic.TryAdd("29", "Winter Sport");
                s_sportTypeDic.TryAdd("30", "Squash");
                s_sportTypeDic.TryAdd("31", "Entertainment");
                s_sportTypeDic.TryAdd("32", "Net Ball");
                s_sportTypeDic.TryAdd("33", "Cycling");
                s_sportTypeDic.TryAdd("34", "Fencing");
                s_sportTypeDic.TryAdd("35", "Judo");
                s_sportTypeDic.TryAdd("36", "M. Pentathlon");
                s_sportTypeDic.TryAdd("37", "Rowing");
                s_sportTypeDic.TryAdd("38", "Sailing");
                s_sportTypeDic.TryAdd("39", "Shooting");
                s_sportTypeDic.TryAdd("40", "Taekwondo");
                s_sportTypeDic.TryAdd("41", "Triathlon");
                s_sportTypeDic.TryAdd("42", "Wrestling");
                s_sportTypeDic.TryAdd("43", "E Sports");
                s_sportTypeDic.TryAdd("99", "Other Sports");
                s_sportTypeDic.TryAdd("151", "Horse Racing");
                s_sportTypeDic.TryAdd("152", "Greyhounds");
                s_sportTypeDic.TryAdd("153", "Harness");
                s_sportTypeDic.TryAdd("154", "HorseRacing FixedOdds");
                s_sportTypeDic.TryAdd("161", "Number Game");
                s_sportTypeDic.TryAdd("162", "Live Casino");
                s_sportTypeDic.TryAdd("180", "Virtual Soccer");
                s_sportTypeDic.TryAdd("181", "Virtual Horse Racing");
                s_sportTypeDic.TryAdd("182", "Virtual Greyhound");
                s_sportTypeDic.TryAdd("183", "Virtual Speedway");
                s_sportTypeDic.TryAdd("184", "Virtual F1");
                s_sportTypeDic.TryAdd("185", "Virtual Cycling");
                s_sportTypeDic.TryAdd("186", "Virtual Tennis");
                s_sportTypeDic.TryAdd("202", "Keno");
                s_sportTypeDic.TryAdd("251", "Casino");
            }

            #endregion init SportType

            #region init BetTypeDic

            if (s_betTypeDic.Count == 0)
            {
                s_betTypeDic.TryAdd("1", "Handicap");
                s_betTypeDic.TryAdd("2", "Odd/Even");
                s_betTypeDic.TryAdd("3", "Over/Under");
                s_betTypeDic.TryAdd("4", "Correct Score");
                s_betTypeDic.TryAdd("5", "FT.1X2");
                s_betTypeDic.TryAdd("6", "Total Goal");
                s_betTypeDic.TryAdd("7", "1st Handicap");
                s_betTypeDic.TryAdd("8", "1st Over/Under");
                s_betTypeDic.TryAdd("9", "Mix Parlay");
                s_betTypeDic.TryAdd("10", "Outright");
                s_betTypeDic.TryAdd("11", "Total Corners");
                s_betTypeDic.TryAdd("12", "1st Odds/Even");
                s_betTypeDic.TryAdd("13", "Clean Sheet");
                s_betTypeDic.TryAdd("14", "First Goal/Last Goal");
                s_betTypeDic.TryAdd("15", "1st 1X2");
                s_betTypeDic.TryAdd("16", "HT/FT");
                s_betTypeDic.TryAdd("17", "2nd HDP");
                s_betTypeDic.TryAdd("18", "2nd Over/Under");
                s_betTypeDic.TryAdd("19", "Substitutes");
                s_betTypeDic.TryAdd("20", "Money Line");
                s_betTypeDic.TryAdd("21", "1st Money Line");
                s_betTypeDic.TryAdd("22", "Next Goal");
                s_betTypeDic.TryAdd("23", "Next Corner");
                s_betTypeDic.TryAdd("24", "Double chance");
                s_betTypeDic.TryAdd("25", "Draw No Bet");
                s_betTypeDic.TryAdd("26", "Both/One/Neither team to score");
                s_betTypeDic.TryAdd("27", "To win to nil");
                s_betTypeDic.TryAdd("28", "3-way handicap");
                s_betTypeDic.TryAdd("29", "System Parlay");
                s_betTypeDic.TryAdd("30", "1st Correct Score");
                s_betTypeDic.TryAdd("31", "Win (Horse Racing)");
                s_betTypeDic.TryAdd("32", "Place (Horse Racing)");
                s_betTypeDic.TryAdd("33", "Win/Place (Horse Racing)");
                s_betTypeDic.TryAdd("41", "Win. UK Tote (Horse Racing)");
                s_betTypeDic.TryAdd("42", "Place. UK Tote (Horse Racing)");
                s_betTypeDic.TryAdd("43", "Win/Place. UK Tote (Horse Racing)");
                s_betTypeDic.TryAdd("71", "Casino Games");
                s_betTypeDic.TryAdd("81", "1st ball O/U (Number Game)");
                s_betTypeDic.TryAdd("82", "Last ball O/U (Number Game)");
                s_betTypeDic.TryAdd("83", "1st ball O/E (Number Game)");
                s_betTypeDic.TryAdd("84", "Last ball O/E (Number Game)");
                s_betTypeDic.TryAdd("85", "Over/Under (Number Game)");
                s_betTypeDic.TryAdd("86", "Odd/Even (Number Game)");
                s_betTypeDic.TryAdd("87", "Next High/Low (Number Game)");
                s_betTypeDic.TryAdd("88", "Warrior (Number Game)");
                s_betTypeDic.TryAdd("89", "Next Combo (Number Game)");
                s_betTypeDic.TryAdd("90", "Number wheel (Number Game)");
                s_betTypeDic.TryAdd("121", "Home No Bet");
                s_betTypeDic.TryAdd("122", "Away No Bet");
                s_betTypeDic.TryAdd("123", "Draw / No Draw");
                s_betTypeDic.TryAdd("124", "FT.1X2 HDP");
                s_betTypeDic.TryAdd("125", "1st 1X2 HDP");
                s_betTypeDic.TryAdd("126", "1H Total Goal");
                s_betTypeDic.TryAdd("127", "1H First Goal/Last Goal");
                s_betTypeDic.TryAdd("128", "HT/FT Odd/Even");
                s_betTypeDic.TryAdd("133", "Home To Win Both Halves");
                s_betTypeDic.TryAdd("134", "Away To Win Both Halves");
                s_betTypeDic.TryAdd("135", "Penalty Shootout");
                s_betTypeDic.TryAdd("140", "Highest Scoring Half");
                s_betTypeDic.TryAdd("141", "Highest Scoring Half Home Team");
                s_betTypeDic.TryAdd("142", "Highest Scoring Half Away Team");
                s_betTypeDic.TryAdd("145", "Both Teams To Score");
                s_betTypeDic.TryAdd("146", "2nd Half Both Teams To Score");
                s_betTypeDic.TryAdd("147", "Home To Score In Both Halves");
                s_betTypeDic.TryAdd("148", "Away To Score In Both Halves");
                s_betTypeDic.TryAdd("149", "Home To Win Either Half");
                s_betTypeDic.TryAdd("150", "Away To Win Either Half ");
                s_betTypeDic.TryAdd("151", "1st Half Double Chance");
                s_betTypeDic.TryAdd("152", "Half Time/Full Time Correct Score");
                s_betTypeDic.TryAdd("1101", "Party Baccarat");
                s_betTypeDic.TryAdd("1102", "Party Roulette");
                s_betTypeDic.TryAdd("1103", "Live Baccarat");
                s_betTypeDic.TryAdd("1104", "Live Roulette");
                s_betTypeDic.TryAdd("1201", "Handicap (Virtual Soccer)");
                s_betTypeDic.TryAdd("1203", "Over/Under 2.5 Goals (Virtual Soccer)");
                s_betTypeDic.TryAdd("1204", "Correct Score (Virtual Soccer)");
                s_betTypeDic.TryAdd("1205", "1X2 (Virtual Soccer)");
                s_betTypeDic.TryAdd("1206", "Total Goal (Virtual Soccer)");
                s_betTypeDic.TryAdd("1220", "Player Win (Virtual Tennis)");
                s_betTypeDic.TryAdd("1224", "Double chance (Virtual Soccer)");
                s_betTypeDic.TryAdd("1231", "Win (Virtual Horse Racing)");
                s_betTypeDic.TryAdd("1232", "Place (Virtual Horse Racing)");
                s_betTypeDic.TryAdd("1233", "Win/Place (Virtual Horse Racing)");
                s_betTypeDic.TryAdd("1235", "Score Bet (Virtual Tennis)");
                s_betTypeDic.TryAdd("1236", "Total Points (Virtual Tennis)");
                s_betTypeDic.TryAdd("1237", "Forecast (Virtual Horse Racing)");
                s_betTypeDic.TryAdd("1238", "Tricast(Virtual Horse Racing)");
                s_betTypeDic.TryAdd("153", "Game Handicap");
                s_betTypeDic.TryAdd("154", "Set x Winner");
                s_betTypeDic.TryAdd("155", "Set x Game Handicap");
                s_betTypeDic.TryAdd("156", "Set x Total Game O/U");
                s_betTypeDic.TryAdd("1301", "Match Winner");
                s_betTypeDic.TryAdd("1302", "Match Correct Score");
                s_betTypeDic.TryAdd("1303", "Set Handicap");
                s_betTypeDic.TryAdd("1305", "Match Total Games Odd/Even");
                s_betTypeDic.TryAdd("1306", "Match Total Games Over/under");
                s_betTypeDic.TryAdd("1308", "Match Games Handicap");
                s_betTypeDic.TryAdd("1311", "Set x Winner");
                s_betTypeDic.TryAdd("1312", "Set x Total Games");
                s_betTypeDic.TryAdd("1316", "Set x Game Handicap");
                s_betTypeDic.TryAdd("1317", "Set x Correct Score");
                s_betTypeDic.TryAdd("1318", "Set x Total Game Odd/Even");
                s_betTypeDic.TryAdd("1324", "Set x Game y Winner");
            }

            #endregion init BetTypeDic

            _apiClient = new ApiClient();
            _profitLossInfo = new SportProfitLossInfo();
            _sportOldSaveProfitLossInfo = DependencyUtil.ResolveEnvLoginUserService<ISportOldSaveProfitLossInfo>(envLoginUser);
        }

        public void SetBetInfo()
        {
            string last_version_key = _profitLossInfo.SelectVersion_key();

            if (!string.IsNullOrWhiteSpace(last_version_key))
            {
                GetBetDetails(last_version_key);
            }
        }

        /// <summary>
        /// 获取注單資料
        /// </summary>
        private void GetBetDetails(string last_version_key)
        {
            var apiParam = new SportApiParamModel() { LastVersionKey = last_version_key };

            ApiResult<BetResult> betDetail = BetDetailService.GetRemoteBetDetail(apiParam);

            string updateVersionKey = null;

            if (betDetail != null && betDetail.error_code == 0)
            {
                BetResult betResult = betDetail.Data;

                if (betResult == null)
                {
                    return;
                }

                if (betResult.AnyAndNotNull() && betDetail.WriteRemoteContentToOtherMerchant != null)
                {
                    betDetail.WriteRemoteContentToOtherMerchant.Invoke();
                }

                if (betDetail.Data.last_version_key != "0" && CycleTryOrder(betResult))
                {
                    updateVersionKey = betDetail.Data.last_version_key;
                }
            }
            else if (betDetail != null)
            {
                LogUtilService.ForcedDebug($"获取下注信息失败,返回状态码：{betDetail.error_code},返回信息：{betDetail.message}");
            }
            else
            {
                LogUtilService.ForcedDebug("获取下注信息失败 betDetail is null");
            }

            //無論error_code,只要有檔案序號,就以檔案序號為主,更新本地端token
            if (betDetail != null && !betDetail.RemoteFileSeq.IsNullOrEmpty())
            {
                updateVersionKey = betDetail.RemoteFileSeq;
            }

            if (!string.IsNullOrEmpty(updateVersionKey))
            {
                _profitLossInfo.UpdateVersion_key(updateVersionKey);
            }
        }

        private bool CycleTryOrder(BetResult betResult)
        {
            betResult.BetDetails.Sort();
            betResult.BetCasinoDetails.Sort();
            betResult.BetHorseDetails.Sort();
            betResult.BetLiveCasinoDetails.Sort();
            betResult.BetNumberDetails.Sort();
            betResult.BetVirtualSportDetails.Sort();

            var allBetInfos = new List<ISabaSportBetDetailInfo>();
            allBetInfos.AddRange(betResult.BetDetails.Select(s => s as ISabaSportBetDetailInfo));
            allBetInfos.AddRange(betResult.BetCasinoDetails.Select(s => s as ISabaSportBetDetailInfo));
            allBetInfos.AddRange(betResult.BetHorseDetails.Select(s => s as ISabaSportBetDetailInfo));
            allBetInfos.AddRange(betResult.BetLiveCasinoDetails.Select(s => s as ISabaSportBetDetailInfo));
            allBetInfos.AddRange(betResult.BetNumberDetails.Select(s => s as ISabaSportBetDetailInfo));
            allBetInfos.AddRange(betResult.BetVirtualSportDetails.Select(s => s as ISabaSportBetDetailInfo));

            HashSet<string> tpGameAccounts = allBetInfos.Select(s => s.Vendor_member_id).Distinct().ToHashSet();
            Dictionary<string, int> allUserMap = TPGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.Sport, tpGameAccounts);

            //假設有注單資料，但沒平台使用者時，要視為正常，讓時間戳推進
            if (!allUserMap.Any())
            {
                return true;
            }

            allBetInfos.RemoveAll(betInfo => !IsTicketStatusValid(betInfo) ||
                !allUserMap.ContainsKey(betInfo.Vendor_member_id) /*不是我們的不處理*/ );

            allBetInfos = allBetInfos.DistinctBy(d => d.Trans_id).ToList();

            return SaveDataToLocal(allBetInfos);
        }

        private bool IsTicketStatusValid(ISabaSportBetDetailInfo bet)
        {
            SabaTicketStatus sabaTicketStatus = SabaTicketStatus.GetSingle(bet.Ticket_status.ToLower());

            if (sabaTicketStatus == null)
            {
                string errorMsg = $"unknown ticket_status, data = {bet.ToJsonString()}";
                ErrorMsgUtil.ErrorHandle(new ArgumentOutOfRangeException(errorMsg), EnvLoginUser);

                return false;
            }

            return sabaTicketStatus.IsAllowSaveDatabase;
        }

        private bool SaveDataToLocal(List<ISabaSportBetDetailInfo> detailInfos)
        {
            var sabaBetDetailViewModels = new List<SabaBetDetailViewModel>();

            foreach (ISabaSportBetDetailInfo detailInfo in detailInfos)
            {
                #region 参数

                string sportTypeName = GetLeagueName(detailInfo.Sport_type, detailInfo.SportName);
                string leagueName = GetLeagueName(detailInfo.League_id, detailInfo.LeagueName);
                string homeName = GetTeamName(detailInfo.Home_id, detailInfo.Bet_type, isHomeTeam: true, detailInfo.HometeamName);
                string awayName = GetTeamName(detailInfo.Away_id, detailInfo.Bet_type, isHomeTeam: false, detailInfo.AwayteamName);
                string betTypeName = GetBetTypeName(detailInfo.Bet_type, detailInfo.BettypeName);

                #endregion 参数

                #region API server 的时区是GMT-4

                DateTime match_datetime1 = DateTime.Now;

                if (DateTime.TryParse(detailInfo.Match_datetime, out match_datetime1))
                {
                    detailInfo.Match_datetime = match_datetime1.AddHours(12).ToFormatDateTimeString();
                }

                DateTime transaction_time1 = DateTime.Now;

                if (DateTime.TryParse(detailInfo.Transaction_time, out transaction_time1))
                {
                    detailInfo.Transaction_time = transaction_time1.AddHours(12).ToFormatDateTimeString();
                }

                DateTime winlost_datetime1 = DateTime.Now;

                if (DateTime.TryParse(detailInfo.Winlost_datetime, out winlost_datetime1))
                {
                    detailInfo.Winlost_datetime = winlost_datetime1.AddHours(12).ToFormatDateTimeString();
                }

                DateTime settlement_time1 = DateTime.Now;

                if (DateTime.TryParse(detailInfo.Settlement_time, out settlement_time1))
                {
                    detailInfo.Settlement_time = settlement_time1.AddHours(12).ToFormatDateTimeString();
                }

                #endregion API server 的时区是GMT-4

                WagerType wagerType = ConvertToWagerType(detailInfo.Bet_type);
                PlatformHandicap platformHandicap = null;
                SabaSportHandicap sabaSportHandicap = SabaSportHandicap.GetSingle(detailInfo.Odds_type);

                if (sabaSportHandicap != null)
                {
                    platformHandicap = sabaSportHandicap.PlatformHandicap;
                }

                string allDetailContent;

                if (wagerType == WagerType.Combo && detailInfo.ParlayData.AnyAndNotNull())
                {
                    allDetailContent = CreateWagerComboAllDetailContent(detailInfo.ParlayData.Select(s => (IBaseSabaSportBetDetailInfo)s).ToList());
                }
                else
                {
                    allDetailContent = CreateWagerSingleAllDetailContent(detailInfo);
                }

                LocalizationParam localizationParam = LocalizationMemoUtil.CreateLocalizationParam(
                    wagerType,
                    platformHandicap,
                    allDetailContent,
                    detailInfo.Trans_id);

                ISabaSportBetDetailName detailName = new BetDetailName()
                {
                    SportTypeName = sportTypeName,
                    HomeName = homeName,
                    AwayName = awayName,
                    BetTypeName = betTypeName,
                    LeagueName = leagueName,
                    Memo = localizationParam.ToLocalizationJsonString()
                };

                var sabaBetDetailViewModel = new SabaBetDetailViewModel()
                {
                    SabaSportBetDetailInfo = detailInfo,
                    SabaSportBetDetailName = detailName
                };

                sabaBetDetailViewModels.Add(sabaBetDetailViewModel);
            }

            _sportOldSaveProfitLossInfo.Value.SaveDataToTarget(sabaBetDetailViewModels);

            return true;
        }

        public static WagerType ConvertToWagerType(string betType)
        {
            if (s_wagerComboBetTypes.Contains(betType))
            {
                return WagerType.Combo;
            }

            return WagerType.Single;
        }

        private string CreateWagerSingleAllDetailContent(IBaseSabaSportBetDetailInfo baseDetailInfo)
        {
            return CreateWagerComboAllDetailContent(new List<IBaseSabaSportBetDetailInfo>() { baseDetailInfo });
        }

        private string CreateWagerComboAllDetailContent(List<IBaseSabaSportBetDetailInfo> baseDetailInfos)
        {
            if (!baseDetailInfos.AnyAndNotNull())
            {
                return null;
            }

            StringBuilder allDetailContent = new StringBuilder();

            for (int i = 0; i < baseDetailInfos.Count; i++)
            {
                IBaseSabaSportBetDetailInfo sabaParlay = baseDetailInfos[i];

                if (i > 0)
                {
                    allDetailContent.Append("；");
                }

                var detailContents = new List<string>();

                string sportTypeName = GetSportTypeName(sabaParlay.Sport_type, sabaParlay.SportName);
                string leagueName = GetLeagueName(sabaParlay.League_id, sabaParlay.LeagueName);
                string homeTeamName = GetTeamName(sabaParlay.Home_id, sabaParlay.Bet_type, isHomeTeam: true, sabaParlay.HometeamName);
                string awayTeamName = GetTeamName(sabaParlay.Away_id, sabaParlay.Bet_type, isHomeTeam: false, sabaParlay.AwayteamName);

                if (!sportTypeName.IsNullOrEmpty())
                {
                    detailContents.Add(sportTypeName);
                }

                if (!leagueName.IsNullOrEmpty())
                {
                    detailContents.Add(leagueName);
                }

                if (!homeTeamName.IsNullOrEmpty() && !awayTeamName.IsNullOrEmpty())
                {
                    detailContents.Add($"{homeTeamName} VS {awayTeamName}");
                }

                string odds = sabaParlay.Odds;

                if (i >= _maxDetailMemoContentCount - 1)
                {
                    odds += "...";
                }

                detailContents.Add(string.Format(ThirdPartyGameElement.SomeOdds, odds));
                allDetailContent.Append(string.Join(",", detailContents));

                if (i >= _maxDetailMemoContentCount - 1)
                {
                    break;
                }
            }

            return allDetailContent.ToString();
        }

        private string GetLeagueName(string leagueId, List<LangName> leagueNames)
        {
            if (string.IsNullOrWhiteSpace(leagueId))
            {
                return null;
            }

            string leagueName = SearchChineseLangName(leagueNames);

            if (!leagueName.IsNullOrEmpty())
            {
                return leagueName;
            }

            if (s_leagueNameMap.TryGetValue(leagueId, out leagueName))
            {
                return leagueName;
            }

            ApiResult<LeagueName> league_model = _apiClient.GetLeagueName(leagueId);

            if (league_model != null && league_model.error_code == 0)
            {
                foreach (LangName lName in league_model.Data.names)
                {
                    if (lName.Lang == "cs")
                    {
                        leagueName = lName.Name;
                        s_leagueNameMap.TryAdd(leagueId, leagueName);

                        break;
                    }
                }
            }
            else
            {
                leagueName = "联赛ID：" + leagueId;
            }

            return leagueName;
        }

        private string GetTeamName(string teamId, string betType, bool isHomeTeam, List<LangName> teamNames)
        {
            if (teamId.IsNullOrEmpty() || betType.IsNullOrEmpty())
            {
                return null;
            }

            string teamName = SearchChineseLangName(teamNames);

            if (!teamName.IsNullOrEmpty())
            {
                return teamName;
            }

            string mapKey = $"{teamId}.{betType}";

            if (s_teamNameMap.TryGetValue(mapKey, out teamName))
            {
                return teamName;
            }

            ApiResult<TeamName> teamModel = _apiClient.GetTeamName(teamId, betType);

            if (teamModel != null && teamModel.error_code == 0)
            {
                foreach (LangName name in teamModel.Data.names)
                {
                    if (name.Lang == "cs")
                    {
                        teamName = name.Name;
                        s_teamNameMap.TryAdd(mapKey, teamName);

                        break;
                    }
                }
            }
            else
            {
                if (isHomeTeam)
                {
                    teamName = "主队ID：";
                }
                else
                {
                    teamName = "客队ID：";
                }

                teamName += teamId;
            }

            return teamName;
        }

        private string GetBetTypeName(string betType, List<LangName> betTypeNames)
        {
            if (string.IsNullOrEmpty(betType))
            {
                return null;
            }

            string betTypeName = SearchChineseLangName(betTypeNames);

            if (!betTypeName.IsNullOrEmpty())
            {
                return betTypeName;
            }

            if (s_betTypeDic.TryGetValue(betType, out betTypeName))
            {
                return betTypeName;
            }

            return "玩法ID：" + betType;
        }

        public static string GetSportTypeName(string sportType, List<LangName> sportTypeNames)
        {
            if (string.IsNullOrEmpty(sportType))
            {
                return null;
            }

            string sportTypeName = SearchChineseLangName(sportTypeNames);

            if (!sportTypeName.IsNullOrEmpty())
            {
                return sportTypeName;
            }

            if (s_sportTypeDic.TryGetValue(sportType, out sportTypeName))
            {
                return sportTypeName;
            }

            return "体育ID：" + sportType;
        }

        private static string SearchChineseLangName(List<LangName> langNames)
        {
            if (!langNames.AnyAndNotNull())
            {
                return null;
            }

            LangName langName = langNames.SingleOrDefault(s => s.Lang == "cs");

            if (langName != null)
            {
                return langName.Name;
            }

            return null;
        }
    }

    public class SabaTicketStatus : BaseStringValueModel<SabaTicketStatus>
    {
        public BetResultType BetResultType { get; private set; }

        public bool IsAllowSaveDatabase { get; private set; }

        private SabaTicketStatus()
        { }

        public static SabaTicketStatus HalfWon = new SabaTicketStatus()
        {
            Value = "half won",
            BetResultType = BetResultType.HalfWin,
            IsAllowSaveDatabase = true
        };

        public static SabaTicketStatus HalfLose = new SabaTicketStatus()
        {
            Value = "half lose",
            BetResultType = BetResultType.HalfLose,
            IsAllowSaveDatabase = true
        };

        public static SabaTicketStatus Won = new SabaTicketStatus()
        {
            Value = "won",
            BetResultType = BetResultType.Win,
            IsAllowSaveDatabase = true
        };

        public static SabaTicketStatus Lose = new SabaTicketStatus()
        {
            Value = "lose",
            BetResultType = BetResultType.Lose,
            IsAllowSaveDatabase = true
        };

        public static SabaTicketStatus Draw = new SabaTicketStatus()
        {
            Value = "draw",
            BetResultType = BetResultType.Draw,
            IsAllowSaveDatabase = true
        };

        public static SabaTicketStatus Reject = new SabaTicketStatus()
        {
            Value = "reject",
            IsAllowSaveDatabase = true
        };

        public static SabaTicketStatus Void = new SabaTicketStatus()
        {
            Value = "void",
            IsAllowSaveDatabase = false
        };

        public static SabaTicketStatus Running = new SabaTicketStatus()
        {
            Value = "running",
            IsAllowSaveDatabase = false
        };

        public static SabaTicketStatus Refund = new SabaTicketStatus()
        {
            Value = "refund",
            IsAllowSaveDatabase = false
        };

        public static SabaTicketStatus Waiting = new SabaTicketStatus()
        {
            Value = "waiting",
            IsAllowSaveDatabase = false
        };
    }

    public class SabaTicketExtraStatus : BaseStringValueModel<SabaTicketExtraStatus>
    {
        public BetResultType BetResultType { get; private set; }

        private SabaTicketExtraStatus()
        { }

        public static readonly SabaTicketExtraStatus Cashout = new SabaTicketExtraStatus()
        {
            Value = "cashout",
            BetResultType = BetResultType.Cashout,
        };
    }
}