using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using UnitTestN6;

namespace UnitTestProject
{
    public class TPGamePMSLApiMSLMockService : TPGamePMSLApiService
    {
        public TPGamePMSLApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            DateTime searchStartDate = DateTime.Now.AddHours(-1);
            DateTime searchEndDate = DateTime.Now;

            var request = new PMBetLogRequestModel
            {
                BeginTime = Convert.ToInt32(searchStartDate.ToUnixOfTime() / 1000),
                EndTime = Convert.ToInt32(searchEndDate.ToUnixOfTime() / 1000),
                PageNum = 1,
                PageSize = 10000
            };

            var betLogs = new List<PMBetLog>();

            TestUtil.DoPlayerJobs(
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    betLogs.Add(new PMBetLog()
                    {
                        Mmi = playerId,
                        Gn = "二十一点",
                        Gd = 438841,
                        Tb = 66,
                        Gr = "中级房",
                        Bi = CreateId(),
                        Cn = "109-1580547942-1041343-7"
                    });
                });

            var response = new PMBetLogResponseModel()
            {
                Data = new PMBetLogModel()
                {
                    List = betLogs
                },
                PageNum = 1,
                PageSize = 1000,
                Total = betLogs.Count(),
                Code = 1000
            };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = request.ToJsonString(isCamelCaseNaming: true),
                ResponseContent = response.ToJsonString()
            });
        }

        private static readonly IdGenerator s_idGenerator = new IdGenerator(0);

        private static long CreateId()
        {
            while (true)
            {
                if (s_idGenerator.TryCreateId(out long betId))
                {
                    return betId;
                }

                TaskUtil.DelayAndWait(1000);
            }
        }
    }
}