using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.BackSideUser
{
    public interface IBWUserInfoRep : IBaseDbRepository<BWUserInfo>
    {
        PagedResultModel<BWUserInfo> GetList(QueryBWUserInfoParam queryParam);

        List<int> GetUserIDsByRoleId(int roleId);

        BWUserInfo GetUserInfoByUserName(string userName);

        bool IsExistRoleId(int roleId);

        bool IsExistUserName(string userName);
    }
}