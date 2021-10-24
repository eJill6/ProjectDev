using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel.VIP;

namespace JxBackendService.Interface.Service.VIP
{
    public interface IVIPUserChangeLogService
    {
        /// <summary>取得用戶積分帳變</summary>
        PagedResultModel<VIPPointsChangeLogModel> GetVIPPointsChangeLogs(BaseUserScoreSearchParam searchParam);

        /// <summary>取得用戶流水帳變</summary>
        PagedResultModel<VIPFlowChangeLogModel> GetVIPFlowChangeLogs(BaseUserScoreSearchParam searchParam);

        /// <summary>取得用戶代理錢包帳變</summary>
        PagedResultModel<VIPAgentAccountLogModel> GetVIPAgentScoreChangeLogs(BaseUserScoreSearchParam searchParam);
    }
}