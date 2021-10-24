using ApolloServiceModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Finance.Apollo
{
    public class ApolloServiceTypeInfo
    {
        public string ServiceType { get; set; }

        public string ServiceTypeName { get; set; }

        /// <summary>是否輸入金額</summary>
        public bool IsInputAmount { get; set; }

        /// <summary>是否導轉至第三方,若為否則平台呈現收款資訊</summary>
        public bool IsRedirectToOuterPage { get;  set; }

        /// <summary>是否要驗證用戶綁定銀行卡</summary>
        public bool IsUserBankCardRequired { get;  set; }

        public int ConfigSettingItemKey { get; set; }

        public GetAllServiceAmountLimit_DetailV2_GroupInfo GroupInfo { get; set; }
    }
}
