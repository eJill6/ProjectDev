using MMService.Models.My;
using MS.Core.MM.Models.Entities.User;
using MS.Core.Models;
using MS.Test.MM.StepDefinitions.StepDefinitionsBases.Impl;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace MS.Test.MM.StepDefinitions
{
    [Binding]
    [Scope(Feature = "UserSummaryService")]
    public class UserSummaryServiceStepDefinitions : MMServiceDefinitionsBase
    {
        public UserSummaryServiceStepDefinitions(ScenarioContext current) : base(current)
        {
        }

        [When(@"執行排程")]
        public async Task When執行排程()
        {
            await UserSummaryService.RestSetUserUnLock();
        }

        [Then(@"UserSummary資料如下")]
        public async Task ThenUserSummary資料如下(Table table)
        {
            var actul = await DB.QueryTable<MMUserSummary>().QueryAsync();
            
            table.CompareToSet(actul);
        }
    }
}
