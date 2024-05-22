using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.JDB;
using JxBackendService.Model.ThirdParty.JDB.JDBFI;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<JDBFIBetLog>
    {
        private readonly Lazy<IGameLobbyListService> _gameLobbyListService;

        public ProductTransferScheduleService()
        {
            _gameLobbyListService = DependencyUtil.ResolveJxBackendService<IGameLobbyListService>(EnvUser, DbConnectionTypes.Slave);
        }

        protected override PlatformProduct Product => PlatformProduct.JDBFI;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override bool IsToLowerTPGameAccount => true;

        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;

            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override BaseReturnDataModel<List<JDBFIBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<JDBFIBetLog>();

            var responseModel = apiResult.Deserialize<JDBFIBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.Status == JDBFIResponseCode.Success)
                {
                    betLogs.AddRange(responseModel.Data);
                }
                else
                {
                    errorMsg = responseModel.Err_text;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<JDBFIBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<JDBFIBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<JDBFIBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();

            foreach (JDBFIBetLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    DateTime profitLossTime = ConvertRemoteServerToChinaDateTime(betLog.LastModifyTime);
                    DateTime betTime = ConvertRemoteServerToChinaDateTime(betLog.GameDate);
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.GetBetMoney(), betLog.GetBetMoney());
                    profitloss.ProfitLossTime = profitLossTime;
                    profitloss.WinMoney = betLog.Total;
                    profitloss.PlayID = betLog.KeyId;
                    profitloss.GameType = GetMTypeName(betLog.MType);
                    profitloss.BetTime = betTime;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.Memo = betLog.Memo;
                    profitloss.BetResultType = profitloss.WinMoney.ToBetResultType().Value;
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }
            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(JDBFIBetLog betLog)
        {
            DateTime betTime = ConvertRemoteServerToChinaDateTime(betLog.GameDate);
            DateTime profitLossTime = ConvertRemoteServerToChinaDateTime(betLog.LastModifyTime);

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
                {
                    new LocalizationSentence()
                    {
                        ResourceName = typeof(ThirdPartyGameElement).FullName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.JDBFIMemo),
                        Args = new List<string>()
                        {
                            GetMTypeName(betLog.MType),
                            betLog.KeyId,
                            betLog.GetBetMoney().ToString(),
                            betLog.Win.ToString(),
                            betTime.ToFormatDateTimeString(),
                            profitLossTime.ToFormatDateTimeString()
                        }
                    }
                }
            };

            return localizationParam;
        }

        private string GetMTypeName(string mType)
        {
            Dictionary<string, GameLobbyInfo> mTypeMap = _gameLobbyListService.Value.GetActiveGameLobbyMap(GameLobbyType.JDBFI);

            if (!mTypeMap.TryGetValue(mType, out GameLobbyInfo gameLobbyInfo))
            {
                return mType;
            }

            return gameLobbyInfo.ChineseName;
        }

        private DateTime ConvertRemoteServerToChinaDateTime(string dateTimeString)
        {
            return dateTimeString.ToDateTime(TPGameJDBFIApiService.JDBResponseDateTimeFormat)
               .AddHours(-TPGameJDBFIApiService.ServerTimeZone)
               .ToChinaDateTime();
        }
    }
}