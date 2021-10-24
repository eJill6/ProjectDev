using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JxBackendService.Model.ThirdParty.BTI
{
    [XmlRoot(ElementName = "MerchantResponse", Namespace = "http://networkpot.com/")]
    public class BTISWalletResponse
    {
        public string ErrorCode { get; set; }

        public string CustomerID { get; set; }

        public string AuthToken { get; set; }

        public decimal? Balance { get; set; }

        public decimal? OpenBetsBalance { get; set; }

        public string TransactionID { get; set; }

        public bool IsSuccess => ErrorCode == BTISWalletErrorCode.NoError;
    }

    public class BTISWalletErrorCode : BaseStringValueModel<BTISWalletErrorCode>
    {
        private BTISWalletErrorCode() { }

        public static BTISWalletErrorCode NoError = new BTISWalletErrorCode() { Value = "NoError" };
        public static BTISWalletErrorCode AuthenticationFailed = new BTISWalletErrorCode() { Value = "AuthenticationFailed" };
        public static BTISWalletErrorCode MerchantIsFrozen = new BTISWalletErrorCode() { Value = "MerchantIsFrozen" };
        public static BTISWalletErrorCode MerchantNotActive = new BTISWalletErrorCode() { Value = "MerchantNotActive" };
        public static BTISWalletErrorCode Exception = new BTISWalletErrorCode() { Value = "Exception" };
        public static BTISWalletErrorCode TransactionCodeNotFounds = new BTISWalletErrorCode() { Value = "TransactionCodeNotFounds" };
    }
}