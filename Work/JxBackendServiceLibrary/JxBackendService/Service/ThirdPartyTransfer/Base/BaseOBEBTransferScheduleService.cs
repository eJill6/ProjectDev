using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBEB;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseOBEBTransferScheduleService : BaseTransferScheduleService<BetRecordLog>
    {
        protected override bool IsToLowerTPGameAccount => true;

        protected override BaseReturnDataModel<List<BetRecordLog>> ConvertToBetLogs(string apiResult)
        {
            string[] responseContents = apiResult.Deserialize<string[]>();
            var betLogs = new List<BetRecordLog>();

            foreach (string responseContent in responseContents)
            {
                var queryBetListResponse = responseContent.Deserialize<OBEBBetLogResponseModel>();

                if (queryBetListResponse.IsSuccess)
                {
                    if (queryBetListResponse.data != null)
                    {
                        betLogs.AddRange(queryBetListResponse.data.record);
                    }
                }
                else
                {
                    string errorMsg = queryBetListResponse.message;

                    return new BaseReturnDataModel<List<BetRecordLog>>(errorMsg, betLogs);
                }
            }

            return new BaseReturnDataModel<List<BetRecordLog>>(ReturnCode.Success, betLogs);
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<BetRecordLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            int recordType = GetRecordType();

            foreach (BetRecordLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                // 注单状态(betStatus：1 (已結算))
                if (betLog.betStatus == 1 && betLog.recordType == recordType && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.validBetAmount, betLog.betAmount);
                    profitloss.WinMoney = betLog.netAmount;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.id.ToString();
                    profitloss.GameType = betLog.gameTypeName;
                    profitloss.BetTime = betLog.createdAt.ToDateTime();
                    profitloss.KeyId = betLog.KeyId;

                    BetResultType betResultType = betLog.netAmount.ToBetResultType();
                    profitloss.BetResultType = betResultType.Value;

                    if (betLog.netAt.HasValue)
                    {
                        profitloss.ProfitLossTime = betLog.netAt.Value.ToDateTime();
                    }
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }

            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(BetRecordLog betLog)
        {
            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            string resourceName = typeof(ThirdPartyGameElement).FullName;

            if (betLog.id.HasValue)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.OBEBBetID),
                    Args = new List<string>() { betLog.id.ToString() }
                });
            }

            if (!betLog.gameTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.GameTypeName),
                    Args = new List<string>() { betLog.gameTypeName }
                });
            }

            if (!betLog.platformName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.PlatformName),
                    Args = new List<string>() { betLog.platformName }
                });
            }

            if (!betLog.betPointName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.BetPointName),
                    Args = new List<string>() { betLog.betPointName }
                });
            }

            if (!betLog.odds.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.Odds),
                    Args = new List<string>() { betLog.odds }
                });
            }

            if (!betLog.judgeResult.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.JudgeResult),
                    Args = new List<string>() { betLog.judgeResult }
                });
            }

            return localizationParam;
        }

        private int GetRecordType()
        {
            // 注单类别 1：LIVE 4：測試環境
            int recordType = 1;

            if (EnvUser.EnvironmentCode != EnvironmentCode.Production)
            {
                recordType = 4;
            }

            return recordType;
        }
    }
}