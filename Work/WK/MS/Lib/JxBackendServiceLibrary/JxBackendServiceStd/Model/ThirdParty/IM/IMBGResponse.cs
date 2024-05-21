using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.ThirdParty;

namespace JxBackendService.Model.ThirdParty.IM
{
    public enum IMBGActionCodes
    {
        Login = 1,

        Balance = 2,

        Recharge = 3,

        Withdraw = 4,

        QueryOrder = 5,

        QueryBetLog = 9
    }

    public class IMBGResponse<T>
    {
        /// <summary>
        /// 子操作类型，登录游戏传 1
        /// </summary>
        public int Ac { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Me { get; set; }

        /// <summary>
        /// 数据结构返回值
        /// </summary>
        public T Data { get; set; }
    }

    public class BaseIMBGData
    {
        public int Code { get; set; }

        public bool IsSuccess => Code == 0;

        public string ErrorLog
        {
            get
            {
                if (IsSuccess)
                {
                    return null;
                }

                string returnMessage = $"Error Code = {Code}";
                string responseMessage = IMBGResponseCodeType.GetName(Code);

                if (responseMessage.IsNullOrEmpty())
                {
                    return returnMessage;
                }

                return $"{responseMessage}[{returnMessage}]";
            }
        }
    }

    public class IMBGBalanceData : BaseIMBGData
    {
        /// <summary>
        /// 总分数
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 可下分数
        /// </summary>
        public decimal FreeMoney { get; set; }

        /// <summary>
        /// 总分数，字符串类型
        /// </summary>
        public string MoneyStr { get; set; }

        /// <summary>
        /// 可下分数，字符串类型
        /// </summary>
        public string FreeMoneyStr { get; set; }

        /// <summary>
        /// 状态码，用户是否在游戏房间内。（0：不在线，1：在线）
        /// </summary>
        public string Status { get; set; }

        public decimal GameId { get; set; }

        public decimal RoomId { get; set; }
    }

    public class IMBGOrderInfoData : BaseIMBGData
    {
        /// <summary>
        /// 订单状态（1：处理中，2：成功，3：失败）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 订单分数
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 订单分数，字符串类型
        /// </summary>
        public string MoneyStr { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单类型 1 上分 2 下分
        /// </summary>
        public string TradeType { get; set; }
    }

    public class IMBGTransferData : BaseIMBGData
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 总分数
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 可下分数
        /// </summary>
        public decimal FreeMoney { get; set; }

        /// <summary>
        /// 总分数，字符串类型
        /// </summary>
        public string MoneyStr { get; set; }

        /// <summary>
        /// 可下分数，字符串类型
        /// </summary>
        public string FreeMoneyStr { get; set; }

        /// <summary>
        /// 订单类型 1:上分, 2:下分
        /// </summary>
        public string TradeType { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderMoney { get; set; }

        /// <summary>
        /// 订单金额，字符串类型
        /// </summary>
        public string OrderMoneyStr { get; set; }
    }

    public class IMBGLoginData : BaseIMBGData
    {
        /// <summary>
        /// 游戏完整 URL
        /// </summary>
        public string FullUrl { get; set; }

        /// <summary>
        /// 用户登录甲方平台后的通行证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 备用游戏地址(非必需)
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 总分数
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 可下分数
        /// </summary>
        public decimal FreeMoney { get; set; }

        /// <summary>
        /// 总分数，字符串类型
        /// </summary>
        public string MoneyStr { get; set; }

        /// <summary>
        /// 可下分数，字符串类型
        /// </summary>
        public string FreeMoneyStr { get; set; }
    }
}