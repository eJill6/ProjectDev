using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.User
{
    public interface IDebugUserService
    {
        void ForcedDebug(int userId, string debugContent);

        bool IsDebugUser(int userId);
    }
}