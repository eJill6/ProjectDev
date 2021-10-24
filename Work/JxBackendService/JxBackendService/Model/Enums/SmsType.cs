using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public enum SmsType
    {
        
        /// <summary>
        /// 您的验证码是" + dataContent + "。有效期为1小时，请尽快验证，如非本人操作，请忽略本短信
        /// </summary>
        VerifyCode = 00,

        /// <summary>
        /// 您的临时登入码是" + dataContent + "。如非本人操作，请忽略本短信
        /// </summary>
        LoginCode = 01,

        /// <summary>
        /// 您的临时登入码是" + dataContent + "。如非本人操作，请忽略本短信
        /// </summary>
        FundCode = 02
    }
}
