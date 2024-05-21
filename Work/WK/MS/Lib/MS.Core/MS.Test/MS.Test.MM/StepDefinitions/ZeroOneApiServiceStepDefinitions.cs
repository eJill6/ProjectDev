using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.Models;
using MS.Test.MM.StepDefinitions.Fakes;
using NSubstitute;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace MS.Test.MM.StepDefinitions
{
    [Binding]
    public class ZeroOneApiServiceStepDefinitions
    {
        protected ScenarioContext Current { get; }
        protected IZeroOneApiService ZeroOneApiService { get; }

        public ZeroOneApiServiceStepDefinitions(ScenarioContext current)
        {
            Current = current;

            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            var zeroOneSettingsOptions = Substitute.For<IOptions<ZeroOneSettings>>();
            zeroOneSettingsOptions.Value.Returns(new ZeroOneSettings()
            {
                Domain = "http://192.168.104.71:8787",
                Salt = "456ERTY",
                UrlDomain = "https://url.luoznz.com/url",
                Xid = "Seal20kdwU29p20K",
                MediaSalt = "Rw6oR*CJ#HTFL2ay",
                RabbitMqConnection = "host=192.168.104.70;port=5672;virtualHost=/;username=hjmqu1;password=qwertyuiop",
                M3U8Key = "FrJPPeprKytSWcpQ",
            });

            IRequestIdentifierProvider provider = new FakeRequestIdentifierProvider();
            IMemoryCache memoryCache = Substitute.For<IMemoryCache>();
            ILogger logger = Substitute.For<ILogger>();

            ZeroOneApiService = new ZeroOneApiService(dateTimeProvider, zeroOneSettingsOptions, provider, memoryCache, logger) { };
        }
        [Given(@"ZOPermissionReq資料")]
        public void GivenZOPermissionReq資料(Table table)
        {
            var req = table.CreateInstance<ZOVipPermissionReq>();
            Current.Set(req, "ZOPermissionReq");
        }

        [When(@"查詢Permission")]
        public async Task When查詢Permission()
        {
            var req = Current.Get<ZOVipPermissionReq>("ZOPermissionReq");
            var res = await ZeroOneApiService.GetPermission(req);
            Current.Set(res, "Result");
        }

        [Then(@"ZOPermissionRes資料應該如下")]
        public void ThenZOPermissionRes資料應該如下(Table table)
        {
            var actul = Current.Get<BaseReturnDataModel<bool>>("Result");

            table.CompareToInstance(actul);
        }

        [Given(@"ZOPointExpenseReq資料")]
        public void GivenZOPointExpenseReq資料(Table table)
        {
            var req = table.CreateInstance<ZOPointIncomeExpenseReq>();
            Current.Set(req, "ZOPointIncomeExpenseReq");
        }

        [When(@"執行PointExpense")]
        public async Task When執行PointExpense()
        {
            var req = Current.Get<ZOPointIncomeExpenseReq>("ZOPointIncomeExpenseReq");
            var res = await ZeroOneApiService.PointExpense(req);
            Current.Set(res, "Result");
        }

        [When(@"執行PointIncome")]
        public async Task When執行PointIncome()
        {
            var req = Current.Get<ZOPointIncomeExpenseReq>("ZOPointIncomeExpenseReq");
            var res = await ZeroOneApiService.PointIncome(req);
            Current.Set(res, "Result");
        }

        [Then(@"ZOPointExpenseRes資料應該如下")]
        public void ThenZOPointExpenseRes資料應該如下(Table table)
        {
            var actul = Current.Get<BaseReturnModel>("Result");

            table.CompareToInstance(actul);
        }


       

    }
}
