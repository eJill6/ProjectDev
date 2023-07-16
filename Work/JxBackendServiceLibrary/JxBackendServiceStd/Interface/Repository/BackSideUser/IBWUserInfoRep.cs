using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;

namespace JxBackendService.Interface.Repository.BackSideUser
{
    public interface IBWUserInfoRep : IBaseDbRepository<BWUserInfo>
    {
        PagedResultModel<BWUserInfo> GetList(QueryBWUserInfoParam queryParam);

        BWUserInfo GetUserInfoIdByUsername(string userName);

        bool IsExistRoleId(int roleId);

        bool IsExistUserName(string userName);
    }
}