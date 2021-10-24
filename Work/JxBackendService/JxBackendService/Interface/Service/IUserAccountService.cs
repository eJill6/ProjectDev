using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.User;
using JxBackendService.Model.ViewModel.VIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IUserAccountService
    {
        PagedResultModel<UserScoreLog> GetBackendUserAccountScoreLog(BackendUserScoreSearchParam searchParam);
        PagedResultModel<UserScoreLog> GetUserAccountScoreLog(BaseUserScoreSearchParam searchParam);
    }
}
