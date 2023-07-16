using System.Collections.Generic;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Repository.BackSideUser
{
    public interface IBWRolePermissionRep : IBaseDbRepository<BWRolePermission>
    {
        List<BWRolePermission> GetRolePermissionList(int roleId);

        BaseReturnModel SaveBWRolePermission(ProSaveBWRolePermission saveParam);
    }
}