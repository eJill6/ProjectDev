using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.User
{
    public class BaseUserPasswordType
    {
        public int UserID { get; set; }

        public PasswordType SavePasswordType { get; set; }
    }

    public class BaseSavePasswordParam : BaseUserPasswordType
    {
        /// <summary>
        /// hash過的新密碼
        /// </summary>
        public string NewPasswordHash { get; set; }

        
    }

    public class SavePasswordByOtherWayParam : BaseSavePasswordParam
    {
        /// <summary>
        /// 管道名稱
        /// </summary>
        public string WayName { get; set; }
    }

    public class SaveNonHashManualPasswordByOtherWayParam : BaseUserPasswordType
    {
        /// <summary>
        /// 未加密新密碼
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 管道名稱
        /// </summary>
        public string WayName { get; set; }
    }


    public class SavePasswordParam : BaseSavePasswordParam
    {
        /// <summary>
        /// hash過的原始密碼
        /// </summary>
        public string OldPasswordHash { get; set; }
    }
}
