using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGamePGSLApiMSLMockService : TPGamePGSLApiMSLService
    {
        public TPGamePGSLApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var model = new PGBetLogResponseModel()
            {
                data = new List<PGSLBetLog>()
            };

            model.data.Add(new PGSLBetLog()
            {
                playerName = "mslD_888",
                betType = PGBetTypes.TrueGame.Value,
                betTime = DateTime.Now.ToUnixOfTime(),
                betEndTime = DateTime.Now.ToUnixOfTime(),
                betAmount = 100,
                winAmount = 200,
                betId = DateTime.Now.ToUnixOfTime(),
                gameId = PGGameIDTypes.PGGameIDTypes000.Value,
                transactionType = PGTransactionTypes.Cash.Value,
            });

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "1",
                ResponseContent = model.ToJsonString()
            });
        }
    }
}