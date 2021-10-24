using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.User
{
    public class DebugUserService : IDebugUserService
    {
        public DebugUserService()
        {

        }

        private static readonly Lazy<HashSet<string>> _debugLowerCaseUsers = new Lazy<HashSet<string>>(() =>
        {
            return new HashSet<string>
            {
                "jackson","普京1", "adad777", "evilgamesx4","gray1", "test00002"
            };
        });

        /// <summary>
        /// 未來資料來源可抽換為web.config
        /// </summary>
        private static HashSet<string> DebugLowerCaseUsers => _debugLowerCaseUsers.Value;

        public void ForcedDebug(string userName, string debugContent)
        {
            if (!IsDebugUser(userName))
            {
                return;
            }

            LogUtil.ForcedDebug($"DebugUser = {userName}, DebugContent = {debugContent}");
        }

        public bool IsDebugUser(string userName)
        {
            return DebugLowerCaseUsers.Contains(userName.ToLower());
        }
    }
}
