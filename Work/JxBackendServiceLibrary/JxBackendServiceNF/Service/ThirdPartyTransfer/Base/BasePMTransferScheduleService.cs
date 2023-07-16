using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;

namespace JxBackendServiceNF.Service.ThirdPartyTransfer.Base
{
    public abstract class BasePMTransferScheduleService : BaseTransferScheduleService<PMBetLog>
    {
        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override BaseReturnDataModel<List<PMBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<PMBetLog>();

            var responseModel = apiResult.Deserialize<PMBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.Code == PMOpreationOrderStatus.Success)
                {
                    if (responseModel.Data != null && responseModel.Data.List != null)
                    {
                        betLogs = responseModel.Data.List;
                    }
                    //else not found data
                }
                else
                {
                    errorMsg = responseModel.Msg;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<PMBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<PMBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }
    }
}