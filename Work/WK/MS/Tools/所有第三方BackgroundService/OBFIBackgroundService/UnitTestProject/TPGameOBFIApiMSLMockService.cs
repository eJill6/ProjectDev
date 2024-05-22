using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using UnitTestN6;

namespace UnitTestProject
{
    public class TPGameOBFIApiMSLMockService : TPGameOBFIApiMSLService
    {

        public TPGameOBFIApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return GetMockData();
            //return base.GetRemoteBetLogApiResult(lastSearchToken);
        }

        private BaseReturnDataModel<RequestAndResponse> GetMockData()
        {
            var pmBetLogs = new List<PMBetLog>();

            TestUtil.DoPlayerJobs(                
                recordCount: 10000, 
                job: (playerId, betId) =>
                {
                    pmBetLogs.Add(new PMBetLog()
                    {
                        Mmi = playerId,
                        Bi = TestUtil.CreateId(),
                        Et = Convert.ToInt32(DateTime.Now.ToUnixOfTime() / 1000),
                        Tb = 100 * 100,
                        Mw = 100 * 100,
                        Memo = "test",
                        Gn = "game type",
                        St = Convert.ToInt32(DateTime.Now.ToUnixOfTime() / 1000),
                    });
                });

            
            var responseModel = new PMBetLogResponseModel()
            {
                Code = PMOpreationOrderStatus.Success.Value,
                Data = new PMBetLogModel()
                {
                    List = pmBetLogs
                }
            };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "0",
                ResponseContent = responseModel.ToJsonString()
            });
        }

         
    }
}