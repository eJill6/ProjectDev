using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.ServiceModel;

namespace JxBackendService.Interface.Service
{
    [ServiceContract]
    public interface IThirdPartyApiWCFService
    {
        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<UserScore> GetBalance(string productCode);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(string productCode, string loginInfoJson, bool isMobile, string correlationId);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnDataModel<string> GetLoginApiResult(string productCode, string loginInfoJson, bool isMobile);

        //[OperationContract, CommonMOperationBehavior]
        //BaseReturnModel TransferAllOUT();

        //[OperationContract, CommonMOperationBehavior]
        //void SelfGameDoTransferToPlatform();

        [OperationContract, CommonMOperationBehavior]
        void DoTPGameAutoTransfer(string productCode, string correlationId);

        [OperationContract, CommonMOperationBehavior]
        FrontsideMenu GetActiveFrontsideMenu(string productCode, string gameCode);
    }
}