using MMService.Models.Base;
using MMService.Models.My;
using MS.Core.Extensions;
using MS.Core.MM.Models.Wallets;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Test.MM.StepDefinitions.StepDefinitionsBases.Impl;
using TechTalk.SpecFlow.Assist;

namespace MS.Test.MM.StepDefinitions
{
    [Binding]
    [Scope(Feature = "WalletService")]
    public class WalletServiceStepDefinitions : MMServiceDefinitionsBase
    {
        public WalletServiceStepDefinitions(ScenarioContext current) : base(current)
        {
        }

        [When(@"查詢")]
        public async Task When查詢(Table table)
        {
            var req = table.CreateInstance<ReqWalletInfo>();

            BaseReturnDataModel<ResWalletInfo> result = 
                await WalletService.WalletInfo(req);

            Current.Add("Request", req);
            Current.Add("ReturnResult", result);
        }

        [Then(@"WalletInfo主資料如下")]
        public void ThenWalletInfo主資料如下(Table table)
        {
            var result = Current.Get<BaseReturnDataModel<ResWalletInfo>>("ReturnResult");
            var actul = result.DataModel.ToEnumerable();
            table.CompareToSet(actul);
        }


        [When(@"IncomeInfo查詢")]
        public async Task WhenIncomeInfo查詢(Table table)
        {
            var req = table.CreateInstance<ReqIncomeInfo>();

            BaseReturnDataModel<PageResultModel<ResIncomeInfo>> result =
                await WalletService.IncomeInfo(req);

            Current.Add("Request", req);
            Current.Add("ReturnResult", result);
        }

        [Then(@"ResIncomeInfo主資料如下")]
        public void ThenResIncomeInfo主資料如下(Table table)
        {
            var result = Current.Get<BaseReturnDataModel<PageResultModel<ResIncomeInfo>>>("ReturnResult");
            var actul = result.DataModel.Data;
            table.CompareToSet(actul);
        }


        [When(@"ExpenseInfo查詢")]
        public async Task WhenExpenseInfo查詢(Table table)
        {
            var req = table.CreateInstance<ReqExpenseInfo>();

            BaseReturnDataModel<PageResultModel<ResExpenseInfo>> res =
                await WalletService.ExpenseInfo(req);

            Current.Add("Request", req);
            Current.Add("ReturnResult", res);
        }

        [Then(@"ResExpenseInfo主資料如下")]
        public void ThenResExpenseInfo主資料如下(Table table)
        {
            var res = Current.Get<BaseReturnDataModel<PageResultModel<ResExpenseInfo>>>("ReturnResult");
            var actul = res.DataModel.Data;
            table.CompareToSet(actul);
        }

    }
}
