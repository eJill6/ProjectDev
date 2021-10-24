using System.Collections.Generic;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ViewModel.VIP;
using System.ServiceModel;
using JxBackendService.Model.Entity.VIP;

namespace JxBackendServiceNet45.Interface.Service.VIP
{
    [ServiceContract]
    public interface IVIPUserApiService
    {
        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel ReceiveVIPBonus(int vipBonusTypeValue);

        [OperationContract, CommonMOperationBehavior]
        int GetVIPLevel(int userId);

        [OperationContract, CommonMOperationBehavior]
        BaseReturnModel ApplyForMonthlyDesposit(int vipEventTypeValue);
        
        [OperationContract, CommonMOperationBehavior]
        PagedResultModel<VIPPointsChangeLogModel> GetVIPPointsChangeLogs(BaseScoreSearchParam param);

        [OperationContract, CommonMOperationBehavior]
        PagedResultModel<VIPFlowChangeLogModel> GetVIPFlowChangeLogs(BaseScoreSearchParam param);

        [OperationContract, CommonMOperationBehavior]
        PagedResultModel<VIPAgentAccountLogModel> GetVIPAgentScoreChangeLogs(BaseScoreSearchParam param);

        [OperationContract]
        List<VIPLevelSetting> GetVIPSettings();
    }
}
