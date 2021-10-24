using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 用戶等級，方便回對所以建這個列舉，實際的值在 ConfigSettings WHERE GroupSerial = 5
    /// </summary>
    public enum UserLevelTypes
    {
        //新用戶
        NewUser = 1,
        //一般用户
        GeneralUser = 2,
        //老用户
        OldUser = 3,
        //真实用户
        RealUser = 4,
    }
}
