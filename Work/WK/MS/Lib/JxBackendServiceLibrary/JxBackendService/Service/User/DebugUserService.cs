using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.User
{
    public class DebugUserService : IDebugUserService
    {
        public DebugUserService()
        {
        }

        private static readonly HashSet<int> s_debugUserIds = GetDebugUserIds();

        private static HashSet<int> GetDebugUserIds()
        {
            return ConfigUtil.Get("DebugUserIds", string.Empty)
                .Split(';')
                .Where(w => !w.IsNullOrEmpty())
                .Select(s => s.ToInt32(hasDefaultValue: true))
                .Distinct()
                .ToHashSet();
        }

        public void ForcedDebug(int userId, string debugContent)
        {
            if (!IsDebugUser(userId))
            {
                return;
            }

            LogUtil.ForcedDebug($"DebugUserId = {userId}, DebugContent = {debugContent}");
        }

        public bool IsDebugUser(int userId)
        {
            return userId != 0 && s_debugUserIds.Contains(userId);
        }
    }
}