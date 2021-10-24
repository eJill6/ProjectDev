using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace JxBackendService.Interface.Service
{
    [ServiceContract]
    public interface IThirdPartyApiWCFService
    {
        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<UserScore> GetBalance(string productCode);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<string> GetForwardGameUrl(string productCode);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel TransferIN(string productCode, decimal amount);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel TransferOUT(string productCode, decimal amount);

        //[OperationContract, CommonMOperationBehavior]
        //void UseMockService(string productCode);

        /// <summary>PGSL 登入遊戲callback用, 不可掛登入檢查</summary>
        [OperationContract]
        PGVerifySessionCallbackModel PGVerifySession(PGVerifySessionModel model);
    }
}
