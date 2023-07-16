using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.Util;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.User
{
    public class DebugUserService : IDebugUserService
    {
        private static readonly HashSet<int> s_debugUserIds = GetDebugUserIds();

        private readonly ILogUtilService _logUtilService;

        public DebugUserService()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        private static HashSet<int> GetDebugUserIds()
        {
            return DependencyUtil.ResolveService<IConfigUtilService>().Get("DebugUserIds", string.Empty)
                .Split(';')
                .Where(w => !w.IsNullOrEmpty())
                .Select(s => s.ToInt32(hasDefaultValue: true))
                .Distinct()
                .ConvertToHashSet();
        }

        public void ForcedDebug(int userId, string debugContent)
        {
            if (!IsDebugUser(userId))
            {
                return;
            }

            _logUtilService.ForcedDebug($"DebugUserId = {userId}, DebugContent = {debugContent}");
        }

        public bool IsDebugUser(int userId)
        {
            return userId != 0 && s_debugUserIds.Contains(userId);
        }
    }
}