using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.VIP;

namespace JxBackendService.Interface.Repository.User
{
    public interface IUserAccountRep
    {
        PagedResultModel<UserScoreLog> GetAccountScoreLogs(UserScoreSearchParam searchParam);               
    }
}
