using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABResponseCode : BaseStringValueModel<ABResponseCode>
    {
        private ABResponseCode() { }

        /// <summary>成功</summary>
        public static readonly ABResponseCode Success = new ABResponseCode() { Value = "OK" };

        public static readonly ABResponseCode AccountExist = new ABResponseCode { Value = "CLIENT_EXIST" };

        /// <summary>無數據</summary>
        public static readonly ABResponseCode NoDataFound = new ABResponseCode() { Value = "" };
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public class ABTransferOrderStatus : BaseIntValueModel<ABTransferOrderStatus>
    {
        private ABTransferOrderStatus() { }

        public static ABTransferOrderStatus None = new ABTransferOrderStatus() { Value = 0 };
        public static ABTransferOrderStatus Success = new ABTransferOrderStatus() { Value = 1 };
        public static ABTransferOrderStatus Fail = new ABTransferOrderStatus() { Value = 2 };

    }


    public class ABOrderStatus : BaseIntValueModel<ABOrderStatus>
    {
        private ABOrderStatus() { }

        public static ABOrderStatus Open = new ABOrderStatus() { Value = 100 };
        public static ABOrderStatus Fail = new ABOrderStatus() { Value = 101 };
        public static ABOrderStatus NotPrize = new ABOrderStatus() { Value = 110 };
        public static ABOrderStatus Settled = new ABOrderStatus() { Value = 111 };
        public static ABOrderStatus Refund = new ABOrderStatus() { Value = 120 };
    }
    public class ABGameType : BaseIntValueModel<ABGameType>
    {
        private ABGameType() { }
                                                
        public static ABGameType Type101 = new ABGameType() { Value = 101, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType101) };
        public static ABGameType Type102 = new ABGameType() { Value = 102, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType102) };
        public static ABGameType Type103 = new ABGameType() { Value = 103, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType103) };
        public static ABGameType Type104 = new ABGameType() { Value = 104, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType104) };
        public static ABGameType Type201 = new ABGameType() { Value = 201, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType201) };
        public static ABGameType Type301 = new ABGameType() { Value = 301, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType301) };
        public static ABGameType Type401 = new ABGameType() { Value = 401, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType401) };
        public static ABGameType Type501 = new ABGameType() { Value = 501, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType501) };
        public static ABGameType Type801 = new ABGameType() { Value = 801, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType801) };
        public static ABGameType Type901 = new ABGameType() { Value = 901, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType901) };

    }


    public class ABBetType : BaseIntValueModel<ABBetType>
    {
        private ABBetType() { }

        public static ABBetType Type1001 = new ABBetType() { Value = 1001, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1001) };
        public static ABBetType Type1002 = new ABBetType() { Value = 1002, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1002) };
        public static ABBetType Type1003 = new ABBetType() { Value = 1003, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1003) };
        public static ABBetType Type1006 = new ABBetType() { Value = 1006, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1006) };
        public static ABBetType Type1007 = new ABBetType() { Value = 1007, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1007) };
        public static ABBetType Type1100 = new ABBetType() { Value = 1100, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1100) };
        public static ABBetType Type1211 = new ABBetType() { Value = 1211, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1211) };
        public static ABBetType Type1212 = new ABBetType() { Value = 1212, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1212) };
        public static ABBetType Type1223 = new ABBetType() { Value = 1223, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1223) };
        public static ABBetType Type1224 = new ABBetType() { Value = 1224, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1224) };
        public static ABBetType Type1231 = new ABBetType() { Value = 1231, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1231) };
        public static ABBetType Type1232 = new ABBetType() { Value = 1232, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1232) };

        public static ABBetType Type2001 = new ABBetType() { Value = 2001, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType2001) };
        public static ABBetType Type2002 = new ABBetType() { Value = 2002, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType2002) };
        public static ABBetType Type2003 = new ABBetType() { Value = 2003, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType2003) };

        public static ABBetType Type5001 = new ABBetType() { Value = 5001, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5001) };
        public static ABBetType Type5002 = new ABBetType() { Value = 5002, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5002) };
        public static ABBetType Type5003 = new ABBetType() { Value = 5003, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5003) };
        public static ABBetType Type5004 = new ABBetType() { Value = 5004, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5004) };
        public static ABBetType Type5005 = new ABBetType() { Value = 5005, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5005) };
        public static ABBetType Type5011 = new ABBetType() { Value = 5011, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5011) };
        public static ABBetType Type5012 = new ABBetType() { Value = 5012, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5012) };
        public static ABBetType Type5013 = new ABBetType() { Value = 5013, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5013) };
        public static ABBetType Type5014 = new ABBetType() { Value = 5014, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5014) };
        public static ABBetType Type5015 = new ABBetType() { Value = 5015, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5015) };

        public static ABBetType Type8001 = new ABBetType() { Value = 8001, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8001) };
        public static ABBetType Type8011 = new ABBetType() { Value = 8011, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8011) };
        public static ABBetType Type8101 = new ABBetType() { Value = 8101, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8101) };
        public static ABBetType Type8111 = new ABBetType() { Value = 8111, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8111) };
        public static ABBetType Type8002 = new ABBetType() { Value = 8002, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8002) };
        public static ABBetType Type8012 = new ABBetType() { Value = 8012, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8012) };
        public static ABBetType Type8102 = new ABBetType() { Value = 8102, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8102) };
        public static ABBetType Type8112 = new ABBetType() { Value = 8112, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8112) };
        public static ABBetType Type8003 = new ABBetType() { Value = 8003, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8003) };
        public static ABBetType Type8013 = new ABBetType() { Value = 8013, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8013) };
        public static ABBetType Type8103 = new ABBetType() { Value = 8103, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8103) };
        public static ABBetType Type8113 = new ABBetType() { Value = 8113, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8113) };
        public static ABBetType Type8021 = new ABBetType() { Value = 8021, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8021) };
        public static ABBetType Type8121 = new ABBetType() { Value = 8121, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8121) };
        public static ABBetType Type8022 = new ABBetType() { Value = 8022, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8022) };
        public static ABBetType Type8122 = new ABBetType() { Value = 8122, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8122) };
        public static ABBetType Type8023 = new ABBetType() { Value = 8023, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8023) };
        public static ABBetType Type8123 = new ABBetType() { Value = 8123, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8123) };

        public static ABBetType Type9001 = new ABBetType() { Value = 9001, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9001) };
        public static ABBetType Type9002 = new ABBetType() { Value = 9002, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9002) };
        public static ABBetType Type9003 = new ABBetType() { Value = 9003, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9003) };
        public static ABBetType Type9004 = new ABBetType() { Value = 9004, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9004) };
        public static ABBetType Type9005 = new ABBetType() { Value = 9005, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9005) };
        public static ABBetType Type9006 = new ABBetType() { Value = 9006, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9006) };
        public static ABBetType Type9007 = new ABBetType() { Value = 9007, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9007) };

    }

    public class ABBetMethod : BaseIntValueModel<ABBetMethod>
    {
        private ABBetMethod() { }

        public static ABBetMethod Type0 = new ABBetMethod() { Value = 0, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod0_Enter) };
        public static ABBetMethod Type1 = new ABBetMethod() { Value = 1, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod1_Multiple) };
        public static ABBetMethod Type2 = new ABBetMethod() { Value = 2, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod2_Quick) };
        public static ABBetMethod Type3 = new ABBetMethod() { Value = 3, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod3_VIP) };
        public static ABBetMethod Type4 = new ABBetMethod() { Value = 4, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod3_VIP_Play) };
        public static ABBetMethod Type5 = new ABBetMethod() { Value = 5, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod3_VIP_View) };

    }

}
