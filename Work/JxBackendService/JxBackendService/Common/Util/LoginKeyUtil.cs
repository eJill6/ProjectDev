using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Common.Util
{
    public static class LoginKeyUtil
    {
        public static string Create(JxApplication jxApplication, int userId)
        {
            return HashExtension.MD5(jxApplication.ShortValue + userId.ToString() + StringUtil.CreateRandomString(10));
        }
    }
}
