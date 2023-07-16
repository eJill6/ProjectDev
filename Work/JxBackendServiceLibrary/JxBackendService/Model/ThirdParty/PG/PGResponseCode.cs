using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class PGResponseCode : BaseStringValueModel<PGResponseCode>
    {
        private PGResponseCode() { }

        /// <summary>成功</summary>
        public static readonly PGResponseCode Success = new PGResponseCode() { Value = "1" };

        public static readonly PGResponseCode AccountExist = new PGResponseCode { Value = "1305" };

        /// <summary>無數據</summary>
        public static readonly PGResponseCode NoDataFound = new PGResponseCode() { Value = "" };
    }

    public class PGBetTypes : BaseIntValueModel<PGBetTypes>
    {
        private PGBetTypes() { }

        public static PGBetTypes TrueGame = new PGBetTypes() { Value = 1 };
    }

    public class PGTransactionTypes : BaseIntValueModel<PGTransactionTypes>
    {
        private PGTransactionTypes() { }

        public static PGTransactionTypes All = new PGTransactionTypes() { Value = 0, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGTransactionTypesAll) };
        public static PGTransactionTypes Cash = new PGTransactionTypes() { Value = 1, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGTransactionTypesCash) };
        public static PGTransactionTypes Bonus = new PGTransactionTypes() { Value = 2, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGTransactionTypesBonus) };
        public static PGTransactionTypes Free = new PGTransactionTypes() { Value = 3, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGTransactionTypesFree) };
    }

    public class PGGameIDTypes : BaseIntValueModel<PGGameIDTypes>
    {
        private PGGameIDTypes() { }

        public static PGGameIDTypes PGGameIDTypes000 = new PGGameIDTypes() { Value = 0, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes000) };
        public static PGGameIDTypes PGGameIDTypes001 = new PGGameIDTypes() { Value = 1, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes001) };
        public static PGGameIDTypes PGGameIDTypes002 = new PGGameIDTypes() { Value = 2, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes002) };
        public static PGGameIDTypes PGGameIDTypes003 = new PGGameIDTypes() { Value = 3, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes003) };
        public static PGGameIDTypes PGGameIDTypes004 = new PGGameIDTypes() { Value = 4, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes004) };
        public static PGGameIDTypes PGGameIDTypes006 = new PGGameIDTypes() { Value = 6, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes006) };
        public static PGGameIDTypes PGGameIDTypes007 = new PGGameIDTypes() { Value = 7, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes007) };
        public static PGGameIDTypes PGGameIDTypes008 = new PGGameIDTypes() { Value = 8, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes008) };
        public static PGGameIDTypes PGGameIDTypes011 = new PGGameIDTypes() { Value = 11, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes011) };
        public static PGGameIDTypes PGGameIDTypes012 = new PGGameIDTypes() { Value = 12, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes012) };
        public static PGGameIDTypes PGGameIDTypes017 = new PGGameIDTypes() { Value = 17, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes017) };
        public static PGGameIDTypes PGGameIDTypes018 = new PGGameIDTypes() { Value = 18, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes018) };
        public static PGGameIDTypes PGGameIDTypes019 = new PGGameIDTypes() { Value = 19, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes019) };
        public static PGGameIDTypes PGGameIDTypes020 = new PGGameIDTypes() { Value = 20, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes020) };
        public static PGGameIDTypes PGGameIDTypes021 = new PGGameIDTypes() { Value = 21, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes021) };
        public static PGGameIDTypes PGGameIDTypes024 = new PGGameIDTypes() { Value = 24, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes024) };
        public static PGGameIDTypes PGGameIDTypes025 = new PGGameIDTypes() { Value = 25, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes025) };
        public static PGGameIDTypes PGGameIDTypes026 = new PGGameIDTypes() { Value = 26, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes026) };
        public static PGGameIDTypes PGGameIDTypes027 = new PGGameIDTypes() { Value = 27, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes027) };
        public static PGGameIDTypes PGGameIDTypes028 = new PGGameIDTypes() { Value = 28, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes028) };
        public static PGGameIDTypes PGGameIDTypes029 = new PGGameIDTypes() { Value = 29, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes029) };
        public static PGGameIDTypes PGGameIDTypes031 = new PGGameIDTypes() { Value = 31, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes031) };
        public static PGGameIDTypes PGGameIDTypes033 = new PGGameIDTypes() { Value = 33, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes033) };
        public static PGGameIDTypes PGGameIDTypes034 = new PGGameIDTypes() { Value = 34, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes034) };
        public static PGGameIDTypes PGGameIDTypes035 = new PGGameIDTypes() { Value = 35, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes035) };
        public static PGGameIDTypes PGGameIDTypes036 = new PGGameIDTypes() { Value = 36, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes036) };
        public static PGGameIDTypes PGGameIDTypes037 = new PGGameIDTypes() { Value = 37, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes037) };
        public static PGGameIDTypes PGGameIDTypes038 = new PGGameIDTypes() { Value = 38, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes038) };
        public static PGGameIDTypes PGGameIDTypes039 = new PGGameIDTypes() { Value = 39, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes039) };
        public static PGGameIDTypes PGGameIDTypes040 = new PGGameIDTypes() { Value = 40, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes040) };
        public static PGGameIDTypes PGGameIDTypes041 = new PGGameIDTypes() { Value = 41, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes041) };
        public static PGGameIDTypes PGGameIDTypes042 = new PGGameIDTypes() { Value = 42, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes042) };
        public static PGGameIDTypes PGGameIDTypes043 = new PGGameIDTypes() { Value = 43, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes043) };
        public static PGGameIDTypes PGGameIDTypes044 = new PGGameIDTypes() { Value = 44, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes044) };
        public static PGGameIDTypes PGGameIDTypes045 = new PGGameIDTypes() { Value = 45, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes045) };
        public static PGGameIDTypes PGGameIDTypes048 = new PGGameIDTypes() { Value = 48, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes048) };
        public static PGGameIDTypes PGGameIDTypes050 = new PGGameIDTypes() { Value = 50, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes050) };
        public static PGGameIDTypes PGGameIDTypes052 = new PGGameIDTypes() { Value = 52, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes052) };
        public static PGGameIDTypes PGGameIDTypes053 = new PGGameIDTypes() { Value = 53, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes053) };
        public static PGGameIDTypes PGGameIDTypes054 = new PGGameIDTypes() { Value = 54, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes054) };
        public static PGGameIDTypes PGGameIDTypes057 = new PGGameIDTypes() { Value = 57, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes057) };
        public static PGGameIDTypes PGGameIDTypes058 = new PGGameIDTypes() { Value = 58, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes058) };
        public static PGGameIDTypes PGGameIDTypes059 = new PGGameIDTypes() { Value = 59, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes059) };
        public static PGGameIDTypes PGGameIDTypes060 = new PGGameIDTypes() { Value = 60, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes060) };
        public static PGGameIDTypes PGGameIDTypes061 = new PGGameIDTypes() { Value = 61, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes061) };
        public static PGGameIDTypes PGGameIDTypes062 = new PGGameIDTypes() { Value = 62, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes062) };
        public static PGGameIDTypes PGGameIDTypes063 = new PGGameIDTypes() { Value = 63, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes063) };
        public static PGGameIDTypes PGGameIDTypes064 = new PGGameIDTypes() { Value = 64, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes064) }; 
        public static PGGameIDTypes PGGameIDTypes065 = new PGGameIDTypes() { Value = 65, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes065) }; 
        public static PGGameIDTypes PGGameIDTypes067 = new PGGameIDTypes() { Value = 67, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes067) }; 
        public static PGGameIDTypes PGGameIDTypes068 = new PGGameIDTypes() { Value = 68, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes068) }; 
        public static PGGameIDTypes PGGameIDTypes069 = new PGGameIDTypes() { Value = 69, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes069) }; 
        public static PGGameIDTypes PGGameIDTypes070 = new PGGameIDTypes() { Value = 70, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes070) }; 
        public static PGGameIDTypes PGGameIDTypes071 = new PGGameIDTypes() { Value = 71, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes071) }; 
        public static PGGameIDTypes PGGameIDTypes073 = new PGGameIDTypes() { Value = 73, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes073) }; 
        public static PGGameIDTypes PGGameIDTypes074 = new PGGameIDTypes() { Value = 74, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes074) }; 
        public static PGGameIDTypes PGGameIDTypes075 = new PGGameIDTypes() { Value = 75, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes075) }; 
        public static PGGameIDTypes PGGameIDTypes076 = new PGGameIDTypes() { Value = 76, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes076) }; 
        public static PGGameIDTypes PGGameIDTypes077 = new PGGameIDTypes() { Value = 77, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes077) }; 
        public static PGGameIDTypes PGGameIDTypes078 = new PGGameIDTypes() { Value = 78, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes078) }; 
        public static PGGameIDTypes PGGameIDTypes079 = new PGGameIDTypes() { Value = 79, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes079) }; 
        public static PGGameIDTypes PGGameIDTypes080 = new PGGameIDTypes() { Value = 80, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes080) };
        public static PGGameIDTypes PGGameIDTypes081 = new PGGameIDTypes() { Value = 81, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes081) };
        public static PGGameIDTypes PGGameIDTypes082 = new PGGameIDTypes() { Value = 82, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes082) };
        public static PGGameIDTypes PGGameIDTypes083 = new PGGameIDTypes() { Value = 83, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes083) };
        public static PGGameIDTypes PGGameIDTypes084 = new PGGameIDTypes() { Value = 84, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes084) };
        public static PGGameIDTypes PGGameIDTypes085 = new PGGameIDTypes() { Value = 85, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes085) };
        public static PGGameIDTypes PGGameIDTypes086 = new PGGameIDTypes() { Value = 86, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes086) };
        public static PGGameIDTypes PGGameIDTypes087 = new PGGameIDTypes() { Value = 87, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes087) };
        public static PGGameIDTypes PGGameIDTypes088 = new PGGameIDTypes() { Value = 88, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes088) };
        public static PGGameIDTypes PGGameIDTypes089 = new PGGameIDTypes() { Value = 89, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes089) };
        public static PGGameIDTypes PGGameIDTypes090 = new PGGameIDTypes() { Value = 90, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes090) };
        public static PGGameIDTypes PGGameIDTypes091 = new PGGameIDTypes() { Value = 91, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes091) };
        public static PGGameIDTypes PGGameIDTypes092 = new PGGameIDTypes() { Value = 92, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes092) };
        public static PGGameIDTypes PGGameIDTypes093 = new PGGameIDTypes() { Value = 93, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes093) };
        public static PGGameIDTypes PGGameIDTypes094 = new PGGameIDTypes() { Value = 94, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes094) };
        public static PGGameIDTypes PGGameIDTypes095 = new PGGameIDTypes() { Value = 95, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes095) };
        public static PGGameIDTypes PGGameIDTypes097 = new PGGameIDTypes() { Value = 97, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes097) };
        public static PGGameIDTypes PGGameIDTypes098 = new PGGameIDTypes() { Value = 98, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes098) };
        public static PGGameIDTypes PGGameIDTypes100 = new PGGameIDTypes() { Value = 100, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes100) };
        public static PGGameIDTypes PGGameIDTypes101 = new PGGameIDTypes() { Value = 101, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes101) };
        public static PGGameIDTypes PGGameIDTypes102 = new PGGameIDTypes() { Value = 102, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes102) };
        public static PGGameIDTypes PGGameIDTypes103 = new PGGameIDTypes() { Value = 103, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes103) };
        public static PGGameIDTypes PGGameIDTypes104 = new PGGameIDTypes() { Value = 104, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes104) };
        public static PGGameIDTypes PGGameIDTypes105 = new PGGameIDTypes() { Value = 105, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes105) };
        public static PGGameIDTypes PGGameIDTypes106 = new PGGameIDTypes() { Value = 106, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes106) };
        public static PGGameIDTypes PGGameIDTypes109 = new PGGameIDTypes() { Value = 109, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes109) };
        public static PGGameIDTypes PGGameIDTypes110 = new PGGameIDTypes() { Value = 110, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes110) };
        public static PGGameIDTypes PGGameIDTypes111 = new PGGameIDTypes() { Value = 111, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes111) };
        public static PGGameIDTypes PGGameIDTypes113 = new PGGameIDTypes() { Value = 113, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes113) };
        public static PGGameIDTypes PGGameIDTypes115 = new PGGameIDTypes() { Value = 115, ResourceType = typeof(ThirdPartyGameElement), ResourcePropertyName = nameof(ThirdPartyGameElement.PGGameIDTypes115) };

    }

}
