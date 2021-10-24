using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IUserOnlineService
    {
        void Add(int userId, string loginKey);
        void ExpireUser(int userId, string expiredMessage, string excludeLoginKey = null);
        bool IsExpired(int userId, string loginKey, out string expiredMessage);
        void RemoveUser(int userId, string loginKey);
    }
}
