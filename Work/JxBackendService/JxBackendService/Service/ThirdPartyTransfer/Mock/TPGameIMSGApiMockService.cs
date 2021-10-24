using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Mock
{
    public class TPGameIMSGApiMockService : TPGameIMSGApiService
    {
        public TPGameIMSGApiMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            string jsonString = File.ReadAllText($"MockData/{Product.Value}/CheckAccountExistApiResult.json", Encoding.UTF8);
            return new BaseReturnDataModel<string>(ReturnCode.Success, jsonString);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            string jsonString = File.ReadAllText($"MockData/{Product.Value}/CreateAccountApiResult.json", Encoding.UTF8);
            return new BaseReturnDataModel<string>(ReturnCode.Success, jsonString);
        }

        protected override string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string jsonString = File.ReadAllText($"MockData/{Product.Value}/OrderApiResult.json", Encoding.UTF8);
            return jsonString;
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            string jsonString = File.ReadAllText($"MockData/{Product.Value}/BetLogApiResult.json", Encoding.UTF8);
            
            return new BaseReturnDataModel<RequestAndResponse>()
            {
                Code = ReturnCode.Success.Value,
                DataModel = new RequestAndResponse() { RequestBody = string.Empty, ResponseContent = jsonString },
            };
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile)
        {
            return new BaseReturnDataModel<string>(ReturnCode.Success, "https://www.google.com");
        }

        protected override string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string jsonString = File.ReadAllText($"MockData/{Product.Value}/TransferApiResult.json", Encoding.UTF8);
            return jsonString;
        }

        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            string jsonString = File.ReadAllText($"MockData/{Product.Value}/UserScoreApiResult.json", Encoding.UTF8);
            return jsonString;
        }
    }
}
