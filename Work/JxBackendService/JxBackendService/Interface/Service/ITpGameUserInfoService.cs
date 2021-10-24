using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameUserInfoService
    {
        bool IsUserExists(int userId);

        bool CreateUser(int userId, string userName);

        BaseTPGameUserInfo GetTPGameUserInfo(int userId);
    }
}
