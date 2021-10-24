using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface ITPGameUserInfoRep<T>
    {
        bool CreateUser(int userId, string userName);
        T GetDetail(int userId);
        bool IsUserExists(int userId);
    }
}
