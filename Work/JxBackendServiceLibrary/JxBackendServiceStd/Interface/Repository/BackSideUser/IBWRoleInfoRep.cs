﻿using System.Collections.Generic;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;

namespace JxBackendService.Interface.Repository.BackSideUser
{
    public interface IBWRoleInfoRep : IBaseDbRepository<BWRoleInfo>
    {
        List<BWRoleInfo> GetAllBWRoleInfos();

        List<BWRoleInfo> GetBWRoleInfos(List<int> roleIds);
        PagedResultModel<BWRoleInfo> GetList(QueryBWRoleInfoParam queryParam);
        List<BWRoleInfo> GetRoleInfoByRoleName(string roleName);
    }
}