using ApolloServiceModel.Response;
using JxBackendService.Model.Finance.Apollo;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel;

namespace JxBackendServiceNet45.Interface.Service.Base
{
    [ServiceContract]
    public interface IBaseSLPolyGameWCFService
    {
        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel CheckMoneyPwd(int webActionTypeValue, string moneyPassword, bool isEncrypted);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel UpdatePwd(bool isEncrypt, string oldPassword, string newPassword, int passwordTypeValue, 
            string validateCode);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel SaveChildTransferStatus(int childUserId, bool isEnabled);

        [OperationContract, CommonMOperationBehavior]
        GetAllServiceAmountLimit_ResponseV2 ApolloGetAllServiceAmountLimitV2(int cardGroupSeq);

        [OperationContract, CommonMOperationBehavior]
        List<ApolloServiceTypeInfo> GetRechargeServiceTypeInfos();
    }
}
