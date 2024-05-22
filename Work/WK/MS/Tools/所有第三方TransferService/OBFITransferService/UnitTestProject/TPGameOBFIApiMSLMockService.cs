using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameOBFIApiMSLMockService : TPGameOBFIApiMSLService
    {
        public TPGameOBFIApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //return GetMockData();
            return base.GetRemoteBetLogApiResult(lastSearchToken);
        }

        private BaseReturnDataModel<RequestAndResponse> GetMockData()
        {
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.OBFITransferService).AccountPrefixCode;
            string[] playerIds = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"cts{accountPrefixCode}_36",
                $"msl{accountPrefixCode}_588",
                $"msl{accountPrefixCode}_888",
            };

            var list = new List<PMBetLog>();

            foreach (string playerId in playerIds)
            {
                list.Add(new PMBetLog()
                {
                    Mmi = playerId,
                    Bi = DateTime.Now.ToUnixOfTime(),
                    Et = Convert.ToInt32(DateTime.Now.ToUnixOfTime() / 1000),
                    Tb = 100 * 100,
                    Mw = 100 * 100,
                    Memo = "test",
                    Gn = "game type",
                    St = Convert.ToInt32(DateTime.Now.ToUnixOfTime() / 1000),
                });
            }

            var responseModel = new PMBetLogResponseModel()
            {
                Code = PMOpreationOrderStatus.Success.Value,
                Data = new PMBetLogModel()
                {
                    List = list
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