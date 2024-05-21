using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface ILiveGameManageRep : IBaseDbRepository<LiveGameManage>
    {
        IEnumerable<LiveGameManage> GetAll();

        LiveGameManage GetDetail(int no);

        PagedResultModel<LiveGameManage> GetPagedAll(LiveGameManageQueryParam parm);
    }
}