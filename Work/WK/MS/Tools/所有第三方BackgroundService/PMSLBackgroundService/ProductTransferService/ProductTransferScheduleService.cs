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
        protected override PlatformProduct Product => PlatformProduct.PMSL;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<PMBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();             

            foreach (PMBetLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                //單位為分
                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.ProfitLossTime = DateTimeUtil.ToDateTimeSeconds(betLog.Et);
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, (decimal)betLog.Bc / 100, (decimal)betLog.Tb / 100);
                    profitloss.WinMoney = (decimal)betLog.Mw / 100;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.Bi.ToString();
                    profitloss.GameType = betLog.Gn;
                    profitloss.BetTime = DateTimeUtil.ToDateTimeSeconds(betLog.St);
                    profitloss.KeyId = betLog.KeyId;

                    if (betLog.Mw > 0)
                    {
                        profitloss.BetResultType = 1;
                    }
                    else if (betLog.Mw < 0)
                    {
                        profitloss.BetResultType = 0;
                    }
                    else
                    {
                        profitloss.BetResultType = -1; //和局
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
            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
                {
                    new LocalizationSentence()
                    {
                        ResourceName = typeof(ThirdPartyGameElement).FullName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.PMSLMemo),
                        Args = new List<string>()
                        {
                            ///游戏名称: {0},游戏桌号:{1},下注金额:{2},游戏房间:{3},注单id : {4}
                            betLog.Gn,
                            betLog.Gd.ToString(),
                            ((decimal)betLog.Tb / 100).ToString(),
                            betLog.Gr,
                            betLog.Cn.ToString(),
                        }
                    }
                }
            };

            return localizationParam;
        }
    }
}