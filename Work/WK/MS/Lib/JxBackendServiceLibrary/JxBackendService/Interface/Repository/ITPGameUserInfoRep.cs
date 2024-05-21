using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface ITPGameUserInfoRep<T>
    {
        bool CreateUser(int userId);

        T GetDetail(int userId);

        bool IsUserExists(int userId);

        string GetQuerySingleSQL();

        Type GetUserInfoType();
    }
}