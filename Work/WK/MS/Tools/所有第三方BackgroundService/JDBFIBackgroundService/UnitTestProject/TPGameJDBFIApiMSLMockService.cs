using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.JDB;
using JxBackendService.Model.ThirdParty.JDB.JDBFI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;

namespace UnitTestProject
{
    public class TPGameJDBFIApiMSLMockService : TPGameJDBFIApiMSLService
    {
        private readonly IdGenerator _idGenerator;

        public TPGameJDBFIApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _idGenerator = new IdGenerator(0);
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //GetJDBFIBetLogRequest(lastSearchToken);
            JDBFIGetBetLogRequestModel request = new JDBFIGetBetLogRequestModel()
            {
                Starttime = DateTime.Now.AddMinutes(-1).ToString(JDBResponseDateTimeFormat)
            };

            var betLogs = new List<JDBFIBetLog>();

            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode().AccountPrefixCode.ToLower();
            string[] playerIds = new string[]
            {
                $"jx{accountPrefixCode}69778",
                $"cts{accountPrefixCode}3",
                $"cts{accountPrefixCode}36",
                $"msl{accountPrefixCode}588",
                $"msl{accountPrefixCode}888",
            };

            string gameDate = request.Starttime.ToDateTime(JDBResponseDateTimeFormat).AddSeconds(25).ToString(JDBResponseDateTimeFormat);

            for (int i = 0; i < playerIds.Length * 10000; i++)
            {
                string userName = playerIds[i % playerIds.Length];

                betLogs.Add(new JDBFIBetLog()
                {
                    SeqNo = CreateId().ToString(),
                    PlayerId = userName,
                    MType = "7001",
                    GameDate = gameDate,
                    Bet = -62.5m,
                    Win = 19.75m,
                    Total = -42.75m,
                    LastModifyTime = gameDate,
                    GType = "7",
                    Currency = "RB",
                    ClientType = "MOBILE",
                    PlayerIp = "61.220.213.91",
                });

                betLogs.Add(new JDBFIBetLog()
                {
                    HistoryId = CreateId().ToString(),
                    PlayerId = userName,
                    MType = "7001",
                    GameDate = gameDate,
                    Bet = -1.98m,
                    Win = 1.46m,
                    Total = -0.52m,
                    Denom = 1,
                    BeforeBalance = 100,
                    AfterBalance = 99.48M,
                    LastModifyTime = gameDate,
                    GType = "7",
                    Currency = "RB",
                    ClientType = "MOBILE",
                    PlayerIp = "61.220.213.91",
                });
            }

            var response = new JDBFIBetLogResponseModel()
            {
                Status = JDBFIResponseCode.Success.Value,
                Data = betLogs,
                Err_text = null,
            };

            return new BaseReturnDataModel<RequestAndResponse>(
                ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = request.ToJsonString(isCamelCaseNaming: true),
                    ResponseContent = response.ToJsonString(isCamelCaseNaming: true)
                });
        }

        private long CreateId()
        {
            while (true)
            {
                if (_idGenerator.TryCreateId(out long id))
                {
                    return id;
                }

                TaskUtil.DelayAndWait(1000);
            }
        }
    }
}