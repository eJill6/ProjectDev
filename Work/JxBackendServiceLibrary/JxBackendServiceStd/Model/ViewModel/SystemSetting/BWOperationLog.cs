using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums.BackSideWeb.Permission;

namespace JxBackendService.Model.ViewModel.SystemSetting
{
    public class OperationLogViewModel : BWOperationLog
    {
        public string PermissionKeyText => PermissionKeyDetail.GetName(PermissionKey);
    }
}