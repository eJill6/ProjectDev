using System.Collections.Generic;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWRoleInfoService
    {
        BaseReturnDataModel<int> Create(CreateRoleInfoParam createParam);

        BaseReturnModel Delete(int roleId);

        Dictionary<int, string> GetAllBWRoleInfoMaps();

        EditRolePermission GetEditRoleInfo(int roleId);

        PagedResultModel<RoleManagementViewModel> GetPagedBWRoleInfos(QueryBWRoleInfoParam queryParam);

        List<JxBackendSelectListItem> GetRoleSelectListItems();

        Dictionary<string, HashSet<int>> GetUserRolePermissions(int userId);

        bool IsRoleExist(int roleId);

        BaseReturnModel SaveRolePermission(UpdateRolePermissionParam updateParam);
    }
}