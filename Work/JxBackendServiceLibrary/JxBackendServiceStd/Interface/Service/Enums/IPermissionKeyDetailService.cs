﻿using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IPermissionKeyDetailService : IBaseValueModelService<string, PermissionKeyDetail>
    {
        PermissionKeyDetail GetSingle(PermissionKeys permissionKey);
    }
}