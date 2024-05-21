using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABResponseCode : BaseStringValueModel<ABResponseCode>
    {
        private ABResponseCode()
        {
            ResourceType = typeof(TPResponseCodeElement);
        }

        /// <summary>成功</summary>
        public static readonly ABResponseCode Success = new ABResponseCode()
        {
            Value = "OK",
        };

        /// <summary> 玩家已经存在 </summary>
        public static readonly ABResponseCode PlayerExist = new ABResponseCode
        {
            Value = "PLAYER_EXIST",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_PlayerExist)
        };

        /// <summary>服务器错误</summary>
        public static readonly ABResponseCode InternalError = new ABResponseCode()
        {
            Value = "INTERNAL_ERROR",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_InternalError)
        };

        /// <summary>无效的Operator ID</summary>
        public static readonly ABResponseCode InvalidOperatorID = new ABResponseCode()
        {
            Value = "INVALID_OPERATORID",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_InvalidOperatorID)
        };

        /// <summary>无效的Signature</summary>
        public static readonly ABResponseCode InvalidSign = new ABResponseCode()
        {
            Value = "INVALID_SIGN",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_InvalidSign)
        };

        /// <summary>IP地址不在白名单中</summary>
        public static readonly ABResponseCode Forbidden = new ABResponseCode()
        {
            Value = "FORBIDDEN",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_Forbidden)
        };

        /// <summary>错误参数</summary>
        public static readonly ABResponseCode IllegalArgument = new ABResponseCode()
        {
            Value = "ILLEGAL_ARGUMENT",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_IllegalArgument)
        };

        /// <summary>系统维护中</summary>
        public static readonly ABResponseCode SystemMaintenance = new ABResponseCode()
        {
            Value = "SYSTEM_MAINTENANCE",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_SystemMaintenance)
        };

        /// <summary>代理不存在</summary>
        public static readonly ABResponseCode AgenetNotExist = new ABResponseCode()
        {
            Value = "AGENT_NOT_EXIST",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_AgenetNotExist)
        };

        /// <summary>玩家不存在</summary>
        public static readonly ABResponseCode PlayNotExist = new ABResponseCode()
        {
            Value = "PLAYER_NOT_EXIST",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_PlayNotExist)
        };

        /// <summary>转账记录已存在</summary>
        public static readonly ABResponseCode TransExisted = new ABResponseCode()
        {
            Value = "TRANS_EXISTED",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TransExisted)
        };

        /// <summary>代理或玩家额度不足</summary>
        public static readonly ABResponseCode LackOfMoney = new ABResponseCode()
        {
            Value = "LACK_OF_MONEY",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_LackOfMoney)
        };

        /// <summary>转账记录不存在</summary>
        public static readonly ABResponseCode TransNotExist = new ABResponseCode()
        {
            Value = "TRANS_NOT_EXIST",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TransNotExist)
        };

        /// <summary>请求过于频繁</summary>
        public static readonly ABResponseCode TooFrequentRequest = new ABResponseCode()
        {
            Value = "TOO_FREQUENT_REQUEST",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TooFrequentRequest)
        };

        /// <summary>转账失败</summary>
        public static readonly ABResponseCode TransFailure = new ABResponseCode()
        {
            Value = "TRANS_FAILURE",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TransFailure)
        };

        /// <summary>非法状态</summary>
        public static readonly ABResponseCode IllegalState = new ABResponseCode()
        {
            Value = "ILLEGAL_STATE",
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_IllegalState)
        };

        public static string CombineMessage(string resultCode, string returnMessage)
        {
            string name = GetName(resultCode);

            if (name.IsNullOrEmpty())
            {
                return resultCode;
            }

            return $"{name}[{resultCode} {returnMessage.ToNonNullString()}]";
        }
    }

    /// <summary> 转账的状态 </summary>
    public class ABTransferStatus : BaseIntValueModel<ABTransferStatus>
    {
        private ABTransferStatus()
        {
            ResourceType = typeof(TPResponseCodeElement);
        }

        /// <summary> 0: 创建状态，是一个临时状态，请隔5分钟后再查看，如还是这个状态，请联系欧博客服人员。 </summary>
        public static readonly ABTransferStatus None = new ABTransferStatus()
        {
            Value = 0,
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TransferStatusCreate)
        };

        /// <summary> 成功 </summary>
        public static readonly ABTransferStatus Success = new ABTransferStatus()
        {
            Value = 1,
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TransferStatusCreateSuccess)
        };

        /// <summary> 失败 </summary>
        public static readonly ABTransferStatus Fail = new ABTransferStatus()
        {
            Value = 2,
            ResourcePropertyName = nameof(TPResponseCodeElement.AB_TransferStatusCreateFail)
        };

        public static string CombineMessage(int resultCode)
        {
            string name = GetName(resultCode);

            if (name.IsNullOrEmpty())
            {
                return resultCode.ToString();
            }

            return $"{name}[{resultCode}]";
        }
    }

    public class ABBetLogStatus : BaseIntValueModel<ABBetLogStatus>
    {
        private ABBetLogStatus()
        { }

        //public static ABBetLogStatus Open = new ABBetLogStatus() { Value = 100 };

        //public static ABBetLogStatus Fail = new ABBetLogStatus() { Value = 101 };

        public static ABBetLogStatus NotPrize = new ABBetLogStatus() { Value = 110 };

        public static ABBetLogStatus Settled = new ABBetLogStatus() { Value = 111 };

        public static ABBetLogStatus Refund = new ABBetLogStatus() { Value = 120 };
    }

    public class ABGameType : BaseIntValueModel<ABGameType>
    {
        private ABGameType()
        {
            ResourceType = typeof(ThirdPartyGameElement);
        }

        public static readonly ABGameType Type101 = new ABGameType()
        {
            Value = 101,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType101)
        };

        public static readonly ABGameType Type102 = new ABGameType()
        {
            Value = 102,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType102)
        };

        public static readonly ABGameType Type103 = new ABGameType()
        {
            Value = 103,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType103)
        };

        public static readonly ABGameType Type104 = new ABGameType()
        {
            Value = 104,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType104)
        };

        public static readonly ABGameType Type110 = new ABGameType()
        {
            Value = 110,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType110)
        };

        public static readonly ABGameType Type201 = new ABGameType()
        {
            Value = 201,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType201)
        };

        public static readonly ABGameType Type301 = new ABGameType()
        {
            Value = 301,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType301)
        };

        public static readonly ABGameType Type401 = new ABGameType()
        {
            Value = 401,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType401)
        };

        public static readonly ABGameType Type501 = new ABGameType()
        {
            Value = 501,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType501)
        };

        public static readonly ABGameType Type601 = new ABGameType()
        {
            Value = 601,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType601)
        };

        public static readonly ABGameType Type801 = new ABGameType()
        {
            Value = 801,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType801)
        };

        public static readonly ABGameType Type901 = new ABGameType()
        {
            Value = 901,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType901)
        };

        public static readonly ABGameType Type702 = new ABGameType()
        {
            Value = 702,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType702)
        };

        public static readonly ABGameType Type602 = new ABGameType()
        {
            Value = 602,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType602)
        };

        public static readonly ABGameType Type603 = new ABGameType()
        {
            Value = 603,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABGameType603)
        };

        public static string GetGameTypeName(int abGameType)
        {
            string result = GetName(abGameType);

            if (result.IsNullOrEmpty())
            {
                result = abGameType.ToString();
            }

            return result;
        }
    }

    public class ABBetType : BaseIntValueModel<ABBetType>
    {
        private ABBetType()
        {
            ResourceType = typeof(ThirdPartyGameElement);
        }

        public static readonly ABBetType Type1001 = new ABBetType()
        {
            Value = 1001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1001)
        };

        public static readonly ABBetType Type1002 = new ABBetType()
        {
            Value = 1002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1002)
        };

        public static readonly ABBetType Type1003 = new ABBetType()
        {
            Value = 1003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1003)
        };

        public static readonly ABBetType Type1006 = new ABBetType()
        {
            Value = 1006,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1006)
        };

        public static readonly ABBetType Type1007 = new ABBetType()
        {
            Value = 1007,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1007)
        };

        public static readonly ABBetType Type1100 = new ABBetType()
        {
            Value = 1100,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1100)
        };

        public static readonly ABBetType Type1211 = new ABBetType()
        {
            Value = 1211,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1211)
        };

        public static readonly ABBetType Type1212 = new ABBetType()
        {
            Value = 1212,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1212)
        };

        public static readonly ABBetType Type1223 = new ABBetType()
        {
            Value = 1223,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1223)
        };

        public static readonly ABBetType Type1224 = new ABBetType()
        {
            Value = 1224,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1224)
        };

        public static readonly ABBetType Type1231 = new ABBetType()
        {
            Value = 1231,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1231)
        };

        public static readonly ABBetType Type1232 = new ABBetType()
        {
            Value = 1232,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1232)
        };

        public static readonly ABBetType Type1301 = new ABBetType()
        {
            Value = 1301,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1301)
        };

        public static readonly ABBetType Type1302 = new ABBetType()
        {
            Value = 1302,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1302)
        };

        public static readonly ABBetType Type1303 = new ABBetType()
        {
            Value = 1303,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1303)
        };

        public static readonly ABBetType Type1304 = new ABBetType()
        {
            Value = 1304,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1304)
        };

        public static readonly ABBetType Type1401 = new ABBetType()
        {
            Value = 1401,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1401)
        };

        public static readonly ABBetType Type1402 = new ABBetType()
        {
            Value = 1402,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1402)
        };

        public static readonly ABBetType Type1403 = new ABBetType()
        {
            Value = 1403,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1403)
        };

        public static readonly ABBetType Type1404 = new ABBetType()
        {
            Value = 1404,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1404)
        };

        public static readonly ABBetType Type1405 = new ABBetType()
        {
            Value = 1405,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1405)
        };

        public static readonly ABBetType Type1501 = new ABBetType()
        {
            Value = 1501,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1501)
        };

        public static readonly ABBetType Type1502 = new ABBetType()
        {
            Value = 1502,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1502)
        };

        public static readonly ABBetType Type1503 = new ABBetType()
        {
            Value = 1503,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1503)
        };

        public static readonly ABBetType Type1504 = new ABBetType()
        {
            Value = 1504,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1504)
        };

        public static readonly ABBetType Type1601 = new ABBetType()
        {
            Value = 1601,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1601)
        };

        public static readonly ABBetType Type1602 = new ABBetType()
        {
            Value = 1602,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1602)
        };

        public static readonly ABBetType Type1603 = new ABBetType()
        {
            Value = 1603,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1603)
        };

        public static readonly ABBetType Type1604 = new ABBetType()
        {
            Value = 1604,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1604)
        };

        public static readonly ABBetType Type1605 = new ABBetType()
        {
            Value = 1605,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType1605)
        };

        public static readonly ABBetType Type2001 = new ABBetType()
        {
            Value = 2001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType2001)
        };

        public static readonly ABBetType Type2002 = new ABBetType()
        {
            Value = 2002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType2002)
        };

        public static readonly ABBetType Type2003 = new ABBetType()
        {
            Value = 2003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType2003)
        };

        public static readonly ABBetType Type3001 = new ABBetType()
        {
            Value = 3001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3001)
        };

        public static readonly ABBetType Type3002 = new ABBetType()
        {
            Value = 3002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3002)
        };

        public static readonly ABBetType Type3003 = new ABBetType()
        {
            Value = 3003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3003)
        };

        public static readonly ABBetType Type3004 = new ABBetType()
        {
            Value = 3004,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3004)
        };

        public static readonly ABBetType Type3005 = new ABBetType()
        {
            Value = 3005,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3005)
        };

        public static readonly ABBetType Type3006 = new ABBetType()
        {
            Value = 3006,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3006)
        };

        public static readonly ABBetType Type3007 = new ABBetType()
        {
            Value = 3007,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3007)
        };

        public static readonly ABBetType Type3008 = new ABBetType()
        {
            Value = 3008,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3008)
        };

        public static readonly ABBetType Type3009 = new ABBetType()
        {
            Value = 3009,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3009)
        };

        public static readonly ABBetType Type3010 = new ABBetType()
        {
            Value = 3010,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3010)
        };

        public static readonly ABBetType Type3011 = new ABBetType()
        {
            Value = 3011,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3011)
        };

        public static readonly ABBetType Type3012 = new ABBetType()
        {
            Value = 3012,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3012)
        };

        public static readonly ABBetType Type3013 = new ABBetType()
        {
            Value = 3013,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3013)
        };

        public static readonly ABBetType Type3014 = new ABBetType()
        {
            Value = 3014,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3014)
        };

        public static readonly ABBetType Type3015 = new ABBetType()
        {
            Value = 3015,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3015)
        };

        public static readonly ABBetType Type3016 = new ABBetType()
        {
            Value = 3016,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3016)
        };

        public static readonly ABBetType Type3017 = new ABBetType()
        {
            Value = 3017,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3017)
        };

        public static readonly ABBetType Type3018 = new ABBetType()
        {
            Value = 3018,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3018)
        };

        public static readonly ABBetType Type3019 = new ABBetType()
        {
            Value = 3019,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3019)
        };

        public static readonly ABBetType Type3020 = new ABBetType()
        {
            Value = 3020,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3020)
        };

        public static readonly ABBetType Type3021 = new ABBetType()
        {
            Value = 3021,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3021)
        };

        public static readonly ABBetType Type3022 = new ABBetType()
        {
            Value = 3022,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3022)
        };

        public static readonly ABBetType Type3023 = new ABBetType()
        {
            Value = 3023,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3023)
        };

        public static readonly ABBetType Type3024 = new ABBetType()
        {
            Value = 3024,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3024)
        };

        public static readonly ABBetType Type3025 = new ABBetType()
        {
            Value = 3025,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3025)
        };

        public static readonly ABBetType Type3026 = new ABBetType()
        {
            Value = 3026,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3026)
        };

        public static readonly ABBetType Type3027 = new ABBetType()
        {
            Value = 3027,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3027)
        };

        public static readonly ABBetType Type3028 = new ABBetType()
        {
            Value = 3028,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3028)
        };

        public static readonly ABBetType Type3029 = new ABBetType()
        {
            Value = 3029,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3029)
        };

        public static readonly ABBetType Type3030 = new ABBetType()
        {
            Value = 3030,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3030)
        };

        public static readonly ABBetType Type3031 = new ABBetType()
        {
            Value = 3031,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3031)
        };

        public static readonly ABBetType Type3033 = new ABBetType()
        {
            Value = 3033,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3033)
        };

        public static readonly ABBetType Type3034 = new ABBetType()
        {
            Value = 3034,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3034)
        };

        public static readonly ABBetType Type3035 = new ABBetType()
        {
            Value = 3035,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3035)
        };

        public static readonly ABBetType Type3036 = new ABBetType()
        {
            Value = 3036,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3036)
        };

        public static readonly ABBetType Type3037 = new ABBetType()
        {
            Value = 3037,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3037)
        };

        public static readonly ABBetType Type3038 = new ABBetType()
        {
            Value = 3038,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3038)
        };

        public static readonly ABBetType Type3039 = new ABBetType()
        {
            Value = 3039,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3039)
        };

        public static readonly ABBetType Type3040 = new ABBetType()
        {
            Value = 3040,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3040)
        };

        public static readonly ABBetType Type3041 = new ABBetType()
        {
            Value = 3041,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3041)
        };

        public static readonly ABBetType Type3042 = new ABBetType()
        {
            Value = 3042,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3042)
        };

        public static readonly ABBetType Type3043 = new ABBetType()
        {
            Value = 3043,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3043)
        };

        public static readonly ABBetType Type3044 = new ABBetType()
        {
            Value = 3044,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3044)
        };

        public static readonly ABBetType Type3045 = new ABBetType()
        {
            Value = 3045,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3045)
        };

        public static readonly ABBetType Type3046 = new ABBetType()
        {
            Value = 3046,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3046)
        };

        public static readonly ABBetType Type3047 = new ABBetType()
        {
            Value = 3047,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3047)
        };

        public static readonly ABBetType Type3048 = new ABBetType()
        {
            Value = 3048,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3048)
        };

        public static readonly ABBetType Type3049 = new ABBetType()
        {
            Value = 3049,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3049)
        };

        public static readonly ABBetType Type3050 = new ABBetType()
        {
            Value = 3050,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3050)
        };

        public static readonly ABBetType Type3051 = new ABBetType()
        {
            Value = 3051,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3051)
        };

        public static readonly ABBetType Type3052 = new ABBetType()
        {
            Value = 3052,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3052)
        };

        public static readonly ABBetType Type3053 = new ABBetType()
        {
            Value = 3053,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3053)
        };

        public static readonly ABBetType Type3200 = new ABBetType()
        {
            Value = 3200,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3200)
        };

        public static readonly ABBetType Type3201 = new ABBetType()
        {
            Value = 3201,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3201)
        };

        public static readonly ABBetType Type3202 = new ABBetType()
        {
            Value = 3202,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3202)
        };

        public static readonly ABBetType Type3203 = new ABBetType()
        {
            Value = 3203,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3203)
        };

        public static readonly ABBetType Type3204 = new ABBetType()
        {
            Value = 3204,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3204)
        };

        public static readonly ABBetType Type3205 = new ABBetType()
        {
            Value = 3205,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3205)
        };

        public static readonly ABBetType Type3206 = new ABBetType()
        {
            Value = 3206,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3206)
        };

        public static readonly ABBetType Type3207 = new ABBetType()
        {
            Value = 3207,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3207)
        };

        public static readonly ABBetType Type3208 = new ABBetType()
        {
            Value = 3208,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3208)
        };

        public static readonly ABBetType Type3209 = new ABBetType()
        {
            Value = 3209,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3209)
        };

        public static readonly ABBetType Type3210 = new ABBetType()
        {
            Value = 3210,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3210)
        };

        public static readonly ABBetType Type3211 = new ABBetType()
        {
            Value = 3211,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3211)
        };

        public static readonly ABBetType Type3212 = new ABBetType()
        {
            Value = 3212,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3212)
        };

        public static readonly ABBetType Type3213 = new ABBetType()
        {
            Value = 3213,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3213)
        };

        public static readonly ABBetType Type3214 = new ABBetType()
        {
            Value = 3214,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3214)
        };

        public static readonly ABBetType Type3215 = new ABBetType()
        {
            Value = 3215,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3215)
        };

        public static readonly ABBetType Type3216 = new ABBetType()
        {
            Value = 3216,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3216)
        };

        public static readonly ABBetType Type3217 = new ABBetType()
        {
            Value = 3217,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3217)
        };

        public static readonly ABBetType Type3218 = new ABBetType()
        {
            Value = 3218,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3218)
        };

        public static readonly ABBetType Type3219 = new ABBetType()
        {
            Value = 3219,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3219)
        };

        public static readonly ABBetType Type3220 = new ABBetType()
        {
            Value = 3220,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3220)
        };

        public static readonly ABBetType Type3221 = new ABBetType()
        {
            Value = 3221,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3221)
        };

        public static readonly ABBetType Type3222 = new ABBetType()
        {
            Value = 3222,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3222)
        };

        public static readonly ABBetType Type3223 = new ABBetType()
        {
            Value = 3223,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3223)
        };

        public static readonly ABBetType Type3224 = new ABBetType()
        {
            Value = 3224,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3224)
        };

        public static readonly ABBetType Type3225 = new ABBetType()
        {
            Value = 3225,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3225)
        };

        public static readonly ABBetType Type3226 = new ABBetType()
        {
            Value = 3226,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3226)
        };

        public static readonly ABBetType Type3227 = new ABBetType()
        {
            Value = 3227,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3227)
        };

        public static readonly ABBetType Type3228 = new ABBetType()
        {
            Value = 3228,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3228)
        };

        public static readonly ABBetType Type3229 = new ABBetType()
        {
            Value = 3229,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3229)
        };

        public static readonly ABBetType Type3230 = new ABBetType()
        {
            Value = 3230,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3230)
        };

        public static readonly ABBetType Type3231 = new ABBetType()
        {
            Value = 3231,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3231)
        };

        public static readonly ABBetType Type3232 = new ABBetType()
        {
            Value = 3232,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3232)
        };

        public static readonly ABBetType Type3233 = new ABBetType()
        {
            Value = 3233,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3233)
        };

        public static readonly ABBetType Type3234 = new ABBetType()
        {
            Value = 3234,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3234)
        };

        public static readonly ABBetType Type3235 = new ABBetType()
        {
            Value = 3235,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3235)
        };

        public static readonly ABBetType Type3236 = new ABBetType()
        {
            Value = 3236,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3236)
        };

        public static readonly ABBetType Type3237 = new ABBetType()
        {
            Value = 3237,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType3237)
        };

        public static readonly ABBetType Type4001 = new ABBetType()
        {
            Value = 4001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4001)
        };

        public static readonly ABBetType Type4002 = new ABBetType()
        {
            Value = 4002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4002)
        };

        public static readonly ABBetType Type4003 = new ABBetType()
        {
            Value = 4003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4003)
        };

        public static readonly ABBetType Type4004 = new ABBetType()
        {
            Value = 4004,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4004)
        };

        public static readonly ABBetType Type4005 = new ABBetType()
        {
            Value = 4005,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4005)
        };

        public static readonly ABBetType Type4006 = new ABBetType()
        {
            Value = 4006,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4006)
        };

        public static readonly ABBetType Type4007 = new ABBetType()
        {
            Value = 4007,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4007)
        };

        public static readonly ABBetType Type4008 = new ABBetType()
        {
            Value = 4008,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4008)
        };

        public static readonly ABBetType Type4009 = new ABBetType()
        {
            Value = 4009,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4009)
        };

        public static readonly ABBetType Type4010 = new ABBetType()
        {
            Value = 4010,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4010)
        };

        public static readonly ABBetType Type4011 = new ABBetType()
        {
            Value = 4011,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4011)
        };

        public static readonly ABBetType Type4012 = new ABBetType()
        {
            Value = 4012,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4012)
        };

        public static readonly ABBetType Type4013 = new ABBetType()
        {
            Value = 4013,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4013)
        };

        public static readonly ABBetType Type4014 = new ABBetType()
        {
            Value = 4014,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4014)
        };

        public static readonly ABBetType Type4015 = new ABBetType()
        {
            Value = 4015,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4015)
        };

        public static readonly ABBetType Type4016 = new ABBetType()
        {
            Value = 4016,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4016)
        };

        public static readonly ABBetType Type4017 = new ABBetType()
        {
            Value = 4017,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4017)
        };

        public static readonly ABBetType Type4018 = new ABBetType()
        {
            Value = 4018,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4018)
        };

        public static readonly ABBetType Type4019 = new ABBetType()
        {
            Value = 4019,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4019)
        };

        public static readonly ABBetType Type4020 = new ABBetType()
        {
            Value = 4020,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4020)
        };

        public static readonly ABBetType Type4021 = new ABBetType()
        {
            Value = 4021,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4021)
        };

        public static readonly ABBetType Type4022 = new ABBetType()
        {
            Value = 4022,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4022)
        };

        public static readonly ABBetType Type4023 = new ABBetType()
        {
            Value = 4023,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4023)
        };

        public static readonly ABBetType Type4024 = new ABBetType()
        {
            Value = 4024,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4024)
        };

        public static readonly ABBetType Type4025 = new ABBetType()
        {
            Value = 4025,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4025)
        };

        public static readonly ABBetType Type4026 = new ABBetType()
        {
            Value = 4026,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4026)
        };

        public static readonly ABBetType Type4027 = new ABBetType()
        {
            Value = 4027,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4027)
        };

        public static readonly ABBetType Type4028 = new ABBetType()
        {
            Value = 4028,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4028)
        };

        public static readonly ABBetType Type4029 = new ABBetType()
        {
            Value = 4029,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4029)
        };

        public static readonly ABBetType Type4030 = new ABBetType()
        {
            Value = 4030,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4030)
        };

        public static readonly ABBetType Type4031 = new ABBetType()
        {
            Value = 4031,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4031)
        };

        public static readonly ABBetType Type4032 = new ABBetType()
        {
            Value = 4032,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4032)
        };

        public static readonly ABBetType Type4033 = new ABBetType()
        {
            Value = 4033,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4033)
        };

        public static readonly ABBetType Type4034 = new ABBetType()
        {
            Value = 4034,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4034)
        };

        public static readonly ABBetType Type4035 = new ABBetType()
        {
            Value = 4035,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4035)
        };

        public static readonly ABBetType Type4036 = new ABBetType()
        {
            Value = 4036,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4036)
        };

        public static readonly ABBetType Type4037 = new ABBetType()
        {
            Value = 4037,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4037)
        };

        public static readonly ABBetType Type4038 = new ABBetType()
        {
            Value = 4038,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4038)
        };

        public static readonly ABBetType Type4039 = new ABBetType()
        {
            Value = 4039,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4039)
        };

        public static readonly ABBetType Type4040 = new ABBetType()
        {
            Value = 4040,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4040)
        };

        public static readonly ABBetType Type4041 = new ABBetType()
        {
            Value = 4041,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4041)
        };

        public static readonly ABBetType Type4042 = new ABBetType()
        {
            Value = 4042,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4042)
        };

        public static readonly ABBetType Type4043 = new ABBetType()
        {
            Value = 4043,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4043)
        };

        public static readonly ABBetType Type4044 = new ABBetType()
        {
            Value = 4044,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4044)
        };

        public static readonly ABBetType Type4045 = new ABBetType()
        {
            Value = 4045,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4045)
        };

        public static readonly ABBetType Type4046 = new ABBetType()
        {
            Value = 4046,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4046)
        };

        public static readonly ABBetType Type4047 = new ABBetType()
        {
            Value = 4047,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4047)
        };

        public static readonly ABBetType Type4048 = new ABBetType()
        {
            Value = 4048,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4048)
        };

        public static readonly ABBetType Type4049 = new ABBetType()
        {
            Value = 4049,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4049)
        };

        public static readonly ABBetType Type4050 = new ABBetType()
        {
            Value = 4050,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4050)
        };

        public static readonly ABBetType Type4051 = new ABBetType()
        {
            Value = 4051,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4051)
        };

        public static readonly ABBetType Type4052 = new ABBetType()
        {
            Value = 4052,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4052)
        };

        public static readonly ABBetType Type4053 = new ABBetType()
        {
            Value = 4053,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4053)
        };

        public static readonly ABBetType Type4054 = new ABBetType()
        {
            Value = 4054,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4054)
        };

        public static readonly ABBetType Type4055 = new ABBetType()
        {
            Value = 4055,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4055)
        };

        public static readonly ABBetType Type4056 = new ABBetType()
        {
            Value = 4056,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4056)
        };

        public static readonly ABBetType Type4057 = new ABBetType()
        {
            Value = 4057,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4057)
        };

        public static readonly ABBetType Type4058 = new ABBetType()
        {
            Value = 4058,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4058)
        };

        public static readonly ABBetType Type4059 = new ABBetType()
        {
            Value = 4059,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4059)
        };

        public static readonly ABBetType Type4060 = new ABBetType()
        {
            Value = 4060,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4060)
        };

        public static readonly ABBetType Type4061 = new ABBetType()
        {
            Value = 4061,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4061)
        };

        public static readonly ABBetType Type4062 = new ABBetType()
        {
            Value = 4062,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4062)
        };

        public static readonly ABBetType Type4063 = new ABBetType()
        {
            Value = 4063,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4063)
        };

        public static readonly ABBetType Type4064 = new ABBetType()
        {
            Value = 4064,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4064)
        };

        public static readonly ABBetType Type4065 = new ABBetType()
        {
            Value = 4065,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4065)
        };

        public static readonly ABBetType Type4066 = new ABBetType()
        {
            Value = 4066,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4066)
        };

        public static readonly ABBetType Type4067 = new ABBetType()
        {
            Value = 4067,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4067)
        };

        public static readonly ABBetType Type4068 = new ABBetType()
        {
            Value = 4068,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4068)
        };

        public static readonly ABBetType Type4069 = new ABBetType()
        {
            Value = 4069,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4069)
        };

        public static readonly ABBetType Type4070 = new ABBetType()
        {
            Value = 4070,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4070)
        };

        public static readonly ABBetType Type4071 = new ABBetType()
        {
            Value = 4071,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4071)
        };

        public static readonly ABBetType Type4072 = new ABBetType()
        {
            Value = 4072,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4072)
        };

        public static readonly ABBetType Type4073 = new ABBetType()
        {
            Value = 4073,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4073)
        };

        public static readonly ABBetType Type4074 = new ABBetType()
        {
            Value = 4074,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4074)
        };

        public static readonly ABBetType Type4075 = new ABBetType()
        {
            Value = 4075,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4075)
        };

        public static readonly ABBetType Type4076 = new ABBetType()
        {
            Value = 4076,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4076)
        };

        public static readonly ABBetType Type4077 = new ABBetType()
        {
            Value = 4077,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4077)
        };

        public static readonly ABBetType Type4078 = new ABBetType()
        {
            Value = 4078,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4078)
        };

        public static readonly ABBetType Type4079 = new ABBetType()
        {
            Value = 4079,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4079)
        };

        public static readonly ABBetType Type4080 = new ABBetType()
        {
            Value = 4080,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4080)
        };

        public static readonly ABBetType Type4081 = new ABBetType()
        {
            Value = 4081,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4081)
        };

        public static readonly ABBetType Type4082 = new ABBetType()
        {
            Value = 4082,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4082)
        };

        public static readonly ABBetType Type4083 = new ABBetType()
        {
            Value = 4083,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4083)
        };

        public static readonly ABBetType Type4084 = new ABBetType()
        {
            Value = 4084,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4084)
        };

        public static readonly ABBetType Type4085 = new ABBetType()
        {
            Value = 4085,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4085)
        };

        public static readonly ABBetType Type4086 = new ABBetType()
        {
            Value = 4086,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4086)
        };

        public static readonly ABBetType Type4087 = new ABBetType()
        {
            Value = 4087,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4087)
        };

        public static readonly ABBetType Type4088 = new ABBetType()
        {
            Value = 4088,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4088)
        };

        public static readonly ABBetType Type4089 = new ABBetType()
        {
            Value = 4089,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4089)
        };

        public static readonly ABBetType Type4090 = new ABBetType()
        {
            Value = 4090,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4090)
        };

        public static readonly ABBetType Type4091 = new ABBetType()
        {
            Value = 4091,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4091)
        };

        public static readonly ABBetType Type4092 = new ABBetType()
        {
            Value = 4092,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4092)
        };

        public static readonly ABBetType Type4093 = new ABBetType()
        {
            Value = 4093,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4093)
        };

        public static readonly ABBetType Type4094 = new ABBetType()
        {
            Value = 4094,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4094)
        };

        public static readonly ABBetType Type4095 = new ABBetType()
        {
            Value = 4095,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4095)
        };

        public static readonly ABBetType Type4096 = new ABBetType()
        {
            Value = 4096,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4096)
        };

        public static readonly ABBetType Type4097 = new ABBetType()
        {
            Value = 4097,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4097)
        };

        public static readonly ABBetType Type4098 = new ABBetType()
        {
            Value = 4098,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4098)
        };

        public static readonly ABBetType Type4099 = new ABBetType()
        {
            Value = 4099,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4099)
        };

        public static readonly ABBetType Type4100 = new ABBetType()
        {
            Value = 4100,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4100)
        };

        public static readonly ABBetType Type4101 = new ABBetType()
        {
            Value = 4101,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4101)
        };

        public static readonly ABBetType Type4102 = new ABBetType()
        {
            Value = 4102,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4102)
        };

        public static readonly ABBetType Type4103 = new ABBetType()
        {
            Value = 4103,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4103)
        };

        public static readonly ABBetType Type4104 = new ABBetType()
        {
            Value = 4104,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4104)
        };

        public static readonly ABBetType Type4105 = new ABBetType()
        {
            Value = 4105,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4105)
        };

        public static readonly ABBetType Type4106 = new ABBetType()
        {
            Value = 4106,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4106)
        };

        public static readonly ABBetType Type4107 = new ABBetType()
        {
            Value = 4107,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4107)
        };

        public static readonly ABBetType Type4108 = new ABBetType()
        {
            Value = 4108,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4108)
        };

        public static readonly ABBetType Type4109 = new ABBetType()
        {
            Value = 4109,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4109)
        };

        public static readonly ABBetType Type4110 = new ABBetType()
        {
            Value = 4110,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4110)
        };

        public static readonly ABBetType Type4111 = new ABBetType()
        {
            Value = 4111,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4111)
        };

        public static readonly ABBetType Type4112 = new ABBetType()
        {
            Value = 4112,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4112)
        };

        public static readonly ABBetType Type4113 = new ABBetType()
        {
            Value = 4113,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4113)
        };

        public static readonly ABBetType Type4114 = new ABBetType()
        {
            Value = 4114,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4114)
        };

        public static readonly ABBetType Type4115 = new ABBetType()
        {
            Value = 4115,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4115)
        };

        public static readonly ABBetType Type4116 = new ABBetType()
        {
            Value = 4116,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4116)
        };

        public static readonly ABBetType Type4117 = new ABBetType()
        {
            Value = 4117,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4117)
        };

        public static readonly ABBetType Type4118 = new ABBetType()
        {
            Value = 4118,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4118)
        };

        public static readonly ABBetType Type4119 = new ABBetType()
        {
            Value = 4119,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4119)
        };

        public static readonly ABBetType Type4120 = new ABBetType()
        {
            Value = 4120,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4120)
        };

        public static readonly ABBetType Type4121 = new ABBetType()
        {
            Value = 4121,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4121)
        };

        public static readonly ABBetType Type4122 = new ABBetType()
        {
            Value = 4122,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4122)
        };

        public static readonly ABBetType Type4123 = new ABBetType()
        {
            Value = 4123,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4123)
        };

        public static readonly ABBetType Type4124 = new ABBetType()
        {
            Value = 4124,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4124)
        };

        public static readonly ABBetType Type4125 = new ABBetType()
        {
            Value = 4125,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4125)
        };

        public static readonly ABBetType Type4126 = new ABBetType()
        {
            Value = 4126,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4126)
        };

        public static readonly ABBetType Type4127 = new ABBetType()
        {
            Value = 4127,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4127)
        };

        public static readonly ABBetType Type4128 = new ABBetType()
        {
            Value = 4128,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4128)
        };

        public static readonly ABBetType Type4129 = new ABBetType()
        {
            Value = 4129,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4129)
        };

        public static readonly ABBetType Type4130 = new ABBetType()
        {
            Value = 4130,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4130)
        };

        public static readonly ABBetType Type4131 = new ABBetType()
        {
            Value = 4131,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4131)
        };

        public static readonly ABBetType Type4132 = new ABBetType()
        {
            Value = 4132,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4132)
        };

        public static readonly ABBetType Type4133 = new ABBetType()
        {
            Value = 4133,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4133)
        };

        public static readonly ABBetType Type4134 = new ABBetType()
        {
            Value = 4134,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4134)
        };

        public static readonly ABBetType Type4135 = new ABBetType()
        {
            Value = 4135,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4135)
        };

        public static readonly ABBetType Type4136 = new ABBetType()
        {
            Value = 4136,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4136)
        };

        public static readonly ABBetType Type4137 = new ABBetType()
        {
            Value = 4137,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4137)
        };

        public static readonly ABBetType Type4138 = new ABBetType()
        {
            Value = 4138,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4138)
        };

        public static readonly ABBetType Type4139 = new ABBetType()
        {
            Value = 4139,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4139)
        };

        public static readonly ABBetType Type4140 = new ABBetType()
        {
            Value = 4140,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4140)
        };

        public static readonly ABBetType Type4141 = new ABBetType()
        {
            Value = 4141,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4141)
        };

        public static readonly ABBetType Type4142 = new ABBetType()
        {
            Value = 4142,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4142)
        };

        public static readonly ABBetType Type4143 = new ABBetType()
        {
            Value = 4143,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4143)
        };

        public static readonly ABBetType Type4144 = new ABBetType()
        {
            Value = 4144,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4144)
        };

        public static readonly ABBetType Type4145 = new ABBetType()
        {
            Value = 4145,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4145)
        };

        public static readonly ABBetType Type4146 = new ABBetType()
        {
            Value = 4146,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4146)
        };

        public static readonly ABBetType Type4147 = new ABBetType()
        {
            Value = 4147,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4147)
        };

        public static readonly ABBetType Type4148 = new ABBetType()
        {
            Value = 4148,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4148)
        };

        public static readonly ABBetType Type4149 = new ABBetType()
        {
            Value = 4149,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4149)
        };

        public static readonly ABBetType Type4150 = new ABBetType()
        {
            Value = 4150,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4150)
        };

        public static readonly ABBetType Type4151 = new ABBetType()
        {
            Value = 4151,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4151)
        };

        public static readonly ABBetType Type4152 = new ABBetType()
        {
            Value = 4152,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4152)
        };

        public static readonly ABBetType Type4153 = new ABBetType()
        {
            Value = 4153,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4153)
        };

        public static readonly ABBetType Type4154 = new ABBetType()
        {
            Value = 4154,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4154)
        };

        public static readonly ABBetType Type4155 = new ABBetType()
        {
            Value = 4155,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4155)
        };

        public static readonly ABBetType Type4156 = new ABBetType()
        {
            Value = 4156,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4156)
        };

        public static readonly ABBetType Type4157 = new ABBetType()
        {
            Value = 4157,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType4157)
        };

        public static readonly ABBetType Type5001 = new ABBetType()
        {
            Value = 5001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5001)
        };

        public static readonly ABBetType Type5002 = new ABBetType()
        {
            Value = 5002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5002)
        };

        public static readonly ABBetType Type5003 = new ABBetType()
        {
            Value = 5003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5003)
        };

        public static readonly ABBetType Type5004 = new ABBetType()
        {
            Value = 5004,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5004)
        };

        public static readonly ABBetType Type5005 = new ABBetType()
        {
            Value = 5005,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5005)
        };

        public static readonly ABBetType Type5011 = new ABBetType()
        {
            Value = 5011,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5011)
        };

        public static readonly ABBetType Type5012 = new ABBetType()
        {
            Value = 5012,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5012)
        };

        public static readonly ABBetType Type5013 = new ABBetType()
        {
            Value = 5013,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5013)
        };

        public static readonly ABBetType Type5014 = new ABBetType()
        {
            Value = 5014,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5014)
        };

        public static readonly ABBetType Type5015 = new ABBetType()
        {
            Value = 5015,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5015)
        };

        public static readonly ABBetType Type5101 = new ABBetType()
        {
            Value = 5101,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5101)
        };

        public static readonly ABBetType Type5102 = new ABBetType()
        {
            Value = 5102,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5102)
        };

        public static readonly ABBetType Type5103 = new ABBetType()
        {
            Value = 5103,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5103)
        };

        public static readonly ABBetType Type5104 = new ABBetType()
        {
            Value = 5104,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5104)
        };

        public static readonly ABBetType Type5105 = new ABBetType()
        {
            Value = 5105,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5105)
        };

        public static readonly ABBetType Type5106 = new ABBetType()
        {
            Value = 5106,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5106)
        };

        public static readonly ABBetType Type5107 = new ABBetType()
        {
            Value = 5107,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5107)
        };

        public static readonly ABBetType Type5108 = new ABBetType()
        {
            Value = 5108,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5108)
        };

        public static readonly ABBetType Type5109 = new ABBetType()
        {
            Value = 5109,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5109)
        };

        public static readonly ABBetType Type5110 = new ABBetType()
        {
            Value = 5110,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5110)
        };

        public static readonly ABBetType Type5111 = new ABBetType()
        {
            Value = 5111,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType5111)
        };

        public static readonly ABBetType Type6001 = new ABBetType()
        {
            Value = 6001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6001)
        };

        public static readonly ABBetType Type6002 = new ABBetType()
        {
            Value = 6002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6002)
        };

        public static readonly ABBetType Type6003 = new ABBetType()
        {
            Value = 6003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6003)
        };

        public static readonly ABBetType Type6004 = new ABBetType()
        {
            Value = 6004,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6004)
        };

        public static readonly ABBetType Type6005 = new ABBetType()
        {
            Value = 6005,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6005)
        };

        public static readonly ABBetType Type6006 = new ABBetType()
        {
            Value = 6006,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6006)
        };

        public static readonly ABBetType Type6007 = new ABBetType()
        {
            Value = 6007,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6007)
        };

        public static readonly ABBetType Type6008 = new ABBetType()
        {
            Value = 6008,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6008)
        };

        public static readonly ABBetType Type6009 = new ABBetType()
        {
            Value = 6009,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6009)
        };

        public static readonly ABBetType Type6201 = new ABBetType()
        {
            Value = 6201,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6201)
        };

        public static readonly ABBetType Type6202 = new ABBetType()
        {
            Value = 6202,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6202)
        };

        public static readonly ABBetType Type6203 = new ABBetType()
        {
            Value = 6203,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6203)
        };

        public static readonly ABBetType Type6204 = new ABBetType()
        {
            Value = 6204,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6204)
        };

        public static readonly ABBetType Type6205 = new ABBetType()
        {
            Value = 6205,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6205)
        };

        public static readonly ABBetType Type6206 = new ABBetType()
        {
            Value = 6206,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6206)
        };

        public static readonly ABBetType Type6207 = new ABBetType()
        {
            Value = 6207,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6207)
        };

        public static readonly ABBetType Type6208 = new ABBetType()
        {
            Value = 6208,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6208)
        };

        public static readonly ABBetType Type6209 = new ABBetType()
        {
            Value = 6209,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6209)
        };

        public static readonly ABBetType Type6210 = new ABBetType()
        {
            Value = 6210,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6210)
        };

        public static readonly ABBetType Type6211 = new ABBetType()
        {
            Value = 6211,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6211)
        };

        public static readonly ABBetType Type6212 = new ABBetType()
        {
            Value = 6212,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6212)
        };

        public static readonly ABBetType Type6301 = new ABBetType()
        {
            Value = 6301,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6301)
        };

        public static readonly ABBetType Type6302 = new ABBetType()
        {
            Value = 6302,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6302)
        };

        public static readonly ABBetType Type6303 = new ABBetType()
        {
            Value = 6303,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6303)
        };

        public static readonly ABBetType Type6304 = new ABBetType()
        {
            Value = 6304,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6304)
        };

        public static readonly ABBetType Type6305 = new ABBetType()
        {
            Value = 6305,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6305)
        };

        public static readonly ABBetType Type6306 = new ABBetType()
        {
            Value = 6306,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType6306)
        };

        public static readonly ABBetType Type7201 = new ABBetType()
        {
            Value = 7201,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7201)
        };

        public static readonly ABBetType Type7202 = new ABBetType()
        {
            Value = 7202,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7202)
        };

        public static readonly ABBetType Type7203 = new ABBetType()
        {
            Value = 7203,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7203)
        };

        public static readonly ABBetType Type7204 = new ABBetType()
        {
            Value = 7204,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7204)
        };

        public static readonly ABBetType Type7205 = new ABBetType()
        {
            Value = 7205,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7205)
        };

        public static readonly ABBetType Type7206 = new ABBetType()
        {
            Value = 7206,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7206)
        };

        public static readonly ABBetType Type7207 = new ABBetType()
        {
            Value = 7207,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7207)
        };

        public static readonly ABBetType Type7211 = new ABBetType()
        {
            Value = 7211,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7211)
        };

        public static readonly ABBetType Type7212 = new ABBetType()
        {
            Value = 7212,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7212)
        };

        public static readonly ABBetType Type7213 = new ABBetType()
        {
            Value = 7213,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7213)
        };

        public static readonly ABBetType Type7214 = new ABBetType()
        {
            Value = 7214,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7214)
        };

        public static readonly ABBetType Type7215 = new ABBetType()
        {
            Value = 7215,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7215)
        };

        public static readonly ABBetType Type7216 = new ABBetType()
        {
            Value = 7216,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7216)
        };

        public static readonly ABBetType Type7217 = new ABBetType()
        {
            Value = 7217,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7217)
        };

        public static readonly ABBetType Type7221 = new ABBetType()
        {
            Value = 7221,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7221)
        };

        public static readonly ABBetType Type7222 = new ABBetType()
        {
            Value = 7222,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7222)
        };

        public static readonly ABBetType Type7223 = new ABBetType()
        {
            Value = 7223,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7223)
        };

        public static readonly ABBetType Type7224 = new ABBetType()
        {
            Value = 7224,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7224)
        };

        public static readonly ABBetType Type7225 = new ABBetType()
        {
            Value = 7225,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7225)
        };

        public static readonly ABBetType Type7226 = new ABBetType()
        {
            Value = 7226,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7226)
        };

        public static readonly ABBetType Type7227 = new ABBetType()
        {
            Value = 7227,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType7227)
        };

        public static readonly ABBetType Type8001 = new ABBetType()
        {
            Value = 8001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8001)
        };

        public static readonly ABBetType Type8011 = new ABBetType()
        {
            Value = 8011,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8011)
        };

        public static readonly ABBetType Type8101 = new ABBetType()
        {
            Value = 8101,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8101)
        };

        public static readonly ABBetType Type8111 = new ABBetType()
        {
            Value = 8111,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8111)
        };

        public static readonly ABBetType Type8002 = new ABBetType()
        {
            Value = 8002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8002)
        };

        public static readonly ABBetType Type8012 = new ABBetType()
        {
            Value = 8012,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8012)
        };

        public static readonly ABBetType Type8102 = new ABBetType()
        {
            Value = 8102,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8102)
        };

        public static readonly ABBetType Type8112 = new ABBetType()
        {
            Value = 8112,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8112)
        };

        public static readonly ABBetType Type8003 = new ABBetType()
        {
            Value = 8003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8003)
        };

        public static readonly ABBetType Type8013 = new ABBetType()
        {
            Value = 8013,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8013)
        };

        public static readonly ABBetType Type8103 = new ABBetType()
        {
            Value = 8103,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8103)
        };

        public static readonly ABBetType Type8113 = new ABBetType()
        {
            Value = 8113,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8113)
        };

        public static readonly ABBetType Type8021 = new ABBetType()
        {
            Value = 8021,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8021)
        };

        public static readonly ABBetType Type8121 = new ABBetType()
        {
            Value = 8121,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8121)
        };

        public static readonly ABBetType Type8022 = new ABBetType()
        {
            Value = 8022,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8022)
        };

        public static readonly ABBetType Type8122 = new ABBetType()
        {
            Value = 8122,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8122)
        };

        public static readonly ABBetType Type8023 = new ABBetType()
        {
            Value = 8023,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8023)
        };

        public static readonly ABBetType Type8123 = new ABBetType()
        {
            Value = 8123,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType8123)
        };

        public static readonly ABBetType Type9001 = new ABBetType()
        {
            Value = 9001,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9001)
        };

        public static readonly ABBetType Type9002 = new ABBetType()
        {
            Value = 9002,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9002)
        };

        public static readonly ABBetType Type9003 = new ABBetType()
        {
            Value = 9003,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9003)
        };

        public static readonly ABBetType Type9004 = new ABBetType()
        {
            Value = 9004,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9004)
        };

        public static readonly ABBetType Type9005 = new ABBetType()
        {
            Value = 9005,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9005)
        };

        public static readonly ABBetType Type9006 = new ABBetType()
        {
            Value = 9006,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9006)
        };

        public static readonly ABBetType Type9007 = new ABBetType()
        {
            Value = 9007,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9007)
        };

        public static readonly ABBetType Type9101 = new ABBetType()
        {
            Value = 9101,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9101)
        };

        public static readonly ABBetType Type9102 = new ABBetType()
        {
            Value = 9102,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9102)
        };

        public static readonly ABBetType Type9103 = new ABBetType()
        {
            Value = 9103,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9103)
        };

        public static readonly ABBetType Type9114 = new ABBetType()
        {
            Value = 9114,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9114)
        };

        public static readonly ABBetType Type9124 = new ABBetType()
        {
            Value = 9124,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetType9124)
        };

        public static string GetBetTypeName(int abBetType)
        {
            string result = GetName(abBetType);

            if (result.IsNullOrEmpty())
            {
                result = abBetType.ToString();
            }

            return result;
        }
    }

    public class ABBetMethod : BaseIntValueModel<ABBetMethod>
    {
        private ABBetMethod()
        {
            ResourceType = typeof(ThirdPartyGameElement);
        }

        public static readonly ABBetMethod Type0 = new ABBetMethod()
        {
            Value = 0,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod0_Enter)
        };

        public static readonly ABBetMethod Type1 = new ABBetMethod()
        {
            Value = 1,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod1_Multiple)
        };

        public static readonly ABBetMethod Type2 = new ABBetMethod()
        {
            Value = 2,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod2_Quick)
        };

        public static readonly ABBetMethod Type3 = new ABBetMethod()
        {
            Value = 3,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod3_VIP)
        };

        public static readonly ABBetMethod Type4 = new ABBetMethod()
        {
            Value = 4,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod3_VIP_Play)
        };

        public static readonly ABBetMethod Type5 = new ABBetMethod()
        {
            Value = 5,
            ResourcePropertyName = nameof(ThirdPartyGameElement.ABBetMethod3_VIP_View)
        };

        public static string GetBetMethod(int abBetMethod)
        {
            string result = GetName(abBetMethod);

            if (result.IsNullOrEmpty())
            {
                result = abBetMethod.ToString();
            }

            return result;
        }
    }
}