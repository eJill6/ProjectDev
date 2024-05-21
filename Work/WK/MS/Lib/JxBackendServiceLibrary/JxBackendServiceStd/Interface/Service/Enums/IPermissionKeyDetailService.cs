using JxBackendService.Model.Enums.BackSideWeb.Permission;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IPermissionKeyDetailService : IBaseValueModelService<string, PermissionKeyDetail>
    {
        PermissionKeyDetail GetSingle(PermissionKeys permissionKey);
    }
}