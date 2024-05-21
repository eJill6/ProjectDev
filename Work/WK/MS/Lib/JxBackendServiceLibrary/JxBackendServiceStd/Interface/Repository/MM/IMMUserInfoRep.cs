using JxBackendService.Model.Entity.MM;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.MM
{
    public interface IMMUserInfoRep : IBaseDbRepository<MMUserInfo>
    {
        List<BasicMMUserInfo> GetBasicMMUserInfos(List<int> userIds);
    }
}