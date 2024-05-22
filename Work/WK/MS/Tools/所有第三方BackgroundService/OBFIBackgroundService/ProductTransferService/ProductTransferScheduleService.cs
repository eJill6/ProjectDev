using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BasePMTransferScheduleService
    {
        protected override PlatformProduct Product => PlatformProduct.OBFI;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<PMBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();             

            foreach (PMBetLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    //bc 有效投注,tb 總投注金額,單位 分
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.ProfitLossTime = DateTimeUtil.ToDateTime((long)betLog.Et * 1000);
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, (decimal)betLog.Bc / 100, (decimal)betLog.Tb / 100);
                    profitloss.WinMoney = (decimal)betLog.Mw / 100;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.Bi.ToString();
                    profitloss.GameType = betLog.Gn;
                    profitloss.BetTime = DateTimeUtil.ToDateTime((long)betLog.St * 1000);
                    profitloss.KeyId = betLog.KeyId;

                    if (profitloss.WinMoney >= 0)
                    {
                        profitloss.BetResultType = 1;
                    }
                    else
                    {
                        profitloss.BetResultType = 0;
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

        protected override LocalizationParam GetCustomizeMemo(PMBetLog betLog)
        {
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            if (!string.IsNullOrEmpty(betLog.Gn))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.OBMemoGameName),
                    Args = new List<string>() { betLog.Gn }
                });
            }

            if (!string.IsNullOrEmpty(betLog.Gr))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.OBMemoGameRoom),
                    Args = new List<string>() { betLog.Gr }
                });
            }

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.OBMemoGameTable),
                Args = new List<string>() { betLog.Gd.ToString() }
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.OBMemoId),
                Args = new List<string>() { betLog.Bi.ToString() }
            });

            if (!string.IsNullOrEmpty(betLog.Cn))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.OBMemoPart),
                    Args = new List<string>() { betLog.Cn }
                });
            }

            return localizationParam;
        }
    }
}