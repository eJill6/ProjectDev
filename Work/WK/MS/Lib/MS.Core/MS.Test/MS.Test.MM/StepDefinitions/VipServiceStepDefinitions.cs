using MS.Core.Extensions;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Vip;
using MS.Core.MM.Services.interfaces;
using MS.Test.MM.StepDefinitions.StepDefinitionsBases.Impl;
using TechTalk.SpecFlow.Assist;

namespace MS.Test.MM.StepDefinitions
{
    [Binding]
    [Scope(Feature = "VipService")]
    public class VipServiceStepDefinitions : MMServiceDefinitionsBase
    {
        public string NewSeqId { get; } = "2023050200000001";
        public VipServiceStepDefinitions(ScenarioContext current) : base(current)
        {
            
        }

        [When(@"購買VIP卡")]
        public async Task When購買VIP卡(Table table)
        {
            //var inputData = table.CreateInstance<ReqBuyVip>();
            //var result = await VipService.BuyVipTransaction(inputData);
            //Current.Add("ReqBuyVip", inputData);
            //Current.Add("ReturnResult", result);
        }

        [Then(@"UserVip資料結果如下")]
        public async Task ThenUserVip資料結果如下(Table table)
        {
            ReqBuyVip buyVip = Current.Get<ReqBuyVip>("ReqBuyVip");
            var userVips = await DB.QueryTable<MMUserVip>().Where(e => e.UserId == buyVip.UserId).QueryAsync().ToArrayAsync();
            table.CompareToSet(userVips);
        }
    }
}
