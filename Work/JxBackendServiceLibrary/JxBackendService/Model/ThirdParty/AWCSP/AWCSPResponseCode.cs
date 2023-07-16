using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.AWCSP
{
    public class AWCSPResponseCode : BaseStringValueModel<AWCSPResponseCode>
    {
        private AWCSPResponseCode() { }

        /// <summary>成功</summary>
        public static readonly AWCSPResponseCode Success = new AWCSPResponseCode() { Value = "0000" };

        /// <summary>帳號已存在</summary>
        public static readonly AWCSPResponseCode AccountAlreadyExist = new AWCSPResponseCode() { Value = "1001" };

        /// <summary>Failed</summary>
        public static readonly AWCSPResponseCode Failed = new AWCSPResponseCode() { Value = "9999" };

        /// <summary>交易不存在</summary>
        public static readonly AWCSPResponseCode DataDoesNotExist = new AWCSPResponseCode() { Value = "1017" };

        /// <summary>转账成功</summary>
        public static readonly AWCSPResponseCode TransferSuccess = new AWCSPResponseCode() { Value = "1" };

    }


}
