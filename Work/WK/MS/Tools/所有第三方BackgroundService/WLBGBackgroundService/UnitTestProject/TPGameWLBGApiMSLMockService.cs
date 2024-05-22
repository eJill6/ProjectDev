using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.WLBG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using UnitTestN6;

namespace UnitTestProject
{
    [MockService]
    public class TPGameWLBGApiMSLMockService : TPGameWLBGApiMSLService
    {
        public TPGameWLBGApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //return new BaseReturnDataModel<RequestAndResponse>(
            //    ReturnCode.Success,
            //    new RequestAndResponse()
            //    {
            //        RequestBody = null,
            //        //ResponseContent = response.ToJsonString(isCamelCaseNaming: true)
            //        ResponseContent = "{\"code\":0,\"data\":{\"from\":\"2023-11-14 10:53:00\",\"until\":\"2023-11-14 11:52:00\",\"count\":4,\"hasMore\":false,\"list\":{\"uid\":[\"mslD_888\",\"mslD_888\",\"mslD_888\",\"mslD_888\"],\"game\":[100,100,100,100],\"profit\":[\"10.00\",\"10.00\",\"-18.50\",\"0.00\"],\"balance\":[\"43.00\",\"33.00\",\"51.50\",\"35.00\"],\"bet\":[\"0.00\",\"0.00\",\"10.00\",\"10.00\"],\"validBet\":[\"0.00\",\"0.00\",\"8.50\",\"10.00\"],\"tax\":[\"0.00\",\"0.00\",\"0.00\",\"0.00\"],\"gameStartTime\":[\"2023-11-14 10:54:53\",\"2023-11-14 10:55:07\",\"2023-11-14 10:54:52\",\"2023-11-14 10:49:48\"],\"recordTime\":[\"2023-11-14 10:54:53\",\"2023-11-14 10:55:07\",\"2023-11-14 11:29:02\",\"2023-11-14 11:02:01\"],\"gameId\":[\"64173818306193260546\",\"64173818365907574786\",\"64173818306193260546\",\"64173817032118906882\"],\"recordId\":[\"1109267-534\",\"1109267-535\",\"1109267-536\",\"1109276-152\"],\"category\":[5,5,5,5]}}}"
            //    });

            WLBGGetBetLogRequestModel request = new WLBGGetBetLogRequestModel()
            {
                From = DateTime.Now.AddMinutes(-1).ToString("yyyyMMddHHmmss"),
                Until = DateTime.Now.ToString("yyyyMMddHHmmss"),
            };

            var betLogs = new WLBGBetRecordResult();
            string gameDate = request.From.ToDateTime("yyyyMMddHHmmss").AddSeconds(25).ToFormatDateTimeString();

            var recordLogs = new WLBGBetRecordLogs()
            {
                Uid = new List<string>(),
                Game = new List<string>(),
                Profit = new List<string>(),
                Balance = new List<string>(),
                Bet = new List<string>(),
                ValidBet = new List<string>(),
                Tax = new List<string>(),
                GameStartTime = new List<string>(),
                RecordTime = new List<string>(),
                GameId = new List<string>(),
                RecordId = new List<string>(),
                Category = new List<string>()
            };

            betLogs = new WLBGBetRecordResult()
            {
                From = request.From.ToDateTime("yyyyMMddHHmmss"),
                Until = request.Until.ToDateTime("yyyyMMddHHmmss"),
                HasMore = false,
                List = recordLogs
            };

            TestUtil.DoPlayerJobs(
                recordCount: 2000,
                job: (playerId, betId) =>
                {
                    recordLogs.Uid.Add(playerId);
                    recordLogs.Game.Add("13");
                    recordLogs.Profit.Add("0.3");
                    recordLogs.Balance.Add("356.3");
                    recordLogs.Bet.Add("0.5");
                    recordLogs.ValidBet.Add("0.5");
                    recordLogs.Tax.Add("0");
                    recordLogs.GameStartTime.Add(gameDate);
                    recordLogs.RecordTime.Add(gameDate);
                    recordLogs.GameId.Add(betId.ToString());
                    recordLogs.RecordId.Add(TestUtil.CreateId().ToString());
                    recordLogs.Category.Add("4");
                });

            betLogs.Count = betLogs.List.Uid.Count;

            var response = new WLBGBetLogResponseModel()
            {
                Code = 0,
                Data = betLogs
            };

            return new BaseReturnDataModel<RequestAndResponse>(
                ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = DateTime.Now.ToUnixOfTime().ToString(), //oss用timestamp
                    ResponseContent = response.ToJsonString(isCamelCaseNaming: true)
                });
        }
    }
}