using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using System.Collections.Generic;

namespace JxBackendService.Service.Enums
{
    public class PermissionKeyDetailService : BaseValueModelService<string, PermissionKeyDetail>, IPermissionKeyDetailService
    {
        public PermissionKeyDetail GetSingle(PermissionKeys permissionKey)
        {
            return GetSingle(permissionKey.ToString());
        }

        protected override List<PermissionKeyDetail> CreateAllList()
        {
            List<PermissionKeyDetail> permissionKeyDetails = base.CreateAllList();

            if (SharedAppSettings.GetEnvironmentCode() != EnvironmentCode.Development)
            {
                permissionKeyDetails.RemoveAll(r => r.MenuType == MenuType.Demo);
            }

            return permissionKeyDetails;
        }
    }
}