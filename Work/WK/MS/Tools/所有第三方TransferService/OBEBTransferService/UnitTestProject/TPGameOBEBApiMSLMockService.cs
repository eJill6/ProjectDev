using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBEB;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameOBEBApiMSLMockService : TPGameOBEBApiMSLService
    {
        public TPGameOBEBApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }
    }

    public class MockDataUtil
    {
        public static BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.OBEBTransferService).AccountPrefixCode.ToLower();
            string merchant = OBEBSharedAppSetting.MerchantCode.ToLower();
            string[] playerIds = new string[]
            {
                $"{merchant}jx{accountPrefixCode}_69778",
                $"{merchant}cts{accountPrefixCode}_3",
                $"{merchant}cts{accountPrefixCode}_36",
                $"{merchant}msl{accountPrefixCode}_588",
                $"{merchant}msl{accountPrefixCode}_888",
            };

            var model = new OBEBBetLogResponseModel
            {
                code = "200",
                message = "成功.",
                data = new OBEBBetLog
                {
                    pageIndex = 1,
                    pageSize = 1,
                    totalRecord = 3,
                    totalPage = 2,
                    record = new List<BetRecordLog>()
                },
            };

            for (int i = 0; i < playerIds.Length; i++)
            {
                string playerId = playerIds[i];

                long betIdByTime = DateTime.Now.ToUnixOfTime();

                model.data.record.Add(new BetRecordLog()
                {
                    id = betIdByTime++,
                    playerId = 23129714,
                    playerName = playerId,
                    agentId = 5302,
                    betAmount = 20,
                    validBetAmount = 20,
                    netAmount = -20,
                    beforeAmount = 40,
                    createdAt = 1661937974000,
                    netAt = 1661938000000,
                    recalcuAt = 0,
                    updatedAt = 1661938000000,
                    gameTypeId = 2002,
                    platformId = 3,
                    platformName = "亚太厅",
                    betStatus = 1,
                    betFlag = 0,
                    betPointId = 3001,
                    odds = "0.95",
                    judgeResult = "50:48:27;49:1:25",
                    currency = "CNY",
                    tableCode = "C12",
                    roundNo = "GC1222831740",
                    bootNo = "B0C122283101EP-GP275",
                    loginIp = "61.220.213.91",
                    deviceType = 1,
                    deviceId = "1661937913020805864",
                    recordType = 4,
                    gameMode = 1,
                    signature = "-",
                    dealerName = "Madison1",
                    tableName = "极速C12",
                    addstr1 = "庄:♥K♦K♠7;闲:♣K♣A♣7;",
                    addstr2 = "庄:7;闲:8;",
                    betPointName = "庄",
                    gameTypeName = "极速百家乐",
                    payAmount = 0,
                    result = "7;8",
                    startid = 772889432919015400,
                    realDeductAmount = 20,
                    bettingRecordType = 1,
                });

                model.data.record.Add(new BetRecordLog()
                {
                    id = betIdByTime++,
                    playerId = 23129714,
                    playerName = playerId,
                    agentId = 5302,
                    betAmount = 20,
                    validBetAmount = 20,
                    netAmount = -20,
                    beforeAmount = 40,
                    createdAt = 1661937974000,
                    netAt = 1661938000000,
                    recalcuAt = 0,
                    updatedAt = 1661938000000,
                    gameTypeId = 2002,
                    platformId = 3,
                    platformName = "亚太厅",
                    betStatus = 1,
                    betFlag = 0,
                    betPointId = 3001,
                    odds = "0.95",
                    judgeResult = "50:48:27;49:1:25",
                    currency = "CNY",
                    tableCode = "C12",
                    roundNo = "GC1222831740",
                    bootNo = "B0C122283101EP-GP275",
                    loginIp = "61.220.213.91",
                    deviceType = 1,
                    deviceId = "1661937913020805864",
                    recordType = 4,
                    gameMode = 1,
                    signature = "-",
                    dealerName = "Madison1",
                    tableName = "极速C12",
                    addstr1 = "庄:♥K♦K♠7;闲:♣K♣A♣7;",
                    addstr2 = "庄:7;闲:8;",
                    betPointName = "庄",
                    gameTypeName = "极速百家乐",
                    payAmount = 0,
                    result = "7;8",
                    startid = 772889432919015400,
                    realDeductAmount = 20,
                    bettingRecordType = 1,
                });

                model.data.record.Add(new BetRecordLog()
                {
                    id = betIdByTime++,
                    playerId = 23129714,
                    playerName = playerId,
                    agentId = 5302,
                    betAmount = 20,
                    validBetAmount = 20,
                    netAmount = -20,
                    beforeAmount = 40,
                    createdAt = 1661937974000,
                    netAt = 1661938000000,
                    recalcuAt = 0,
                    updatedAt = 1661938000000,
                    gameTypeId = 2002,
                    platformId = 3,
                    platformName = "亚太厅",
                    betStatus = 1,
                    betFlag = 0,
                    betPointId = 3001,
                    odds = "0.95",
                    judgeResult = "50:48:27;49:1:25",
                    currency = "CNY",
                    tableCode = "C12",
                    roundNo = "GC1222831740",
                    bootNo = "B0C122283101EP-GP275",
                    loginIp = "61.220.213.91",
                    deviceType = 1,
                    deviceId = "1661937913020805864",
                    recordType = 4,
                    gameMode = 1,
                    signature = "-",
                    dealerName = "Madison1",
                    tableName = "极速C12",
                    addstr1 = "庄:♥K♦K♠7;闲:♣K♣A♣7;",
                    addstr2 = "庄:7;闲:8;",
                    betPointName = "庄",
                    gameTypeName = "极速百家乐",
                    payAmount = 0,
                    result = "7;8",
                    startid = 772889432919015400,
                    realDeductAmount = 20,
                    bettingRecordType = 1,
                });
            }

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "1",
                ResponseContent = new string[] { model.ToJsonString() }.ToJsonString()
            });
        }
    }
}