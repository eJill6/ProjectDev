using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.MSSeal.Models
{
    /// <summary>
    /// 批次扣款請求
    /// </summary>
    public class DeductBatchRequest : BaseRequest
    {
        /// <summary>
        /// App名稱
        /// </summary>
        public string App { get; set; } = "amd";

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName { get; set; } = string.Empty;

        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNumber { get; set; } = string.Empty;

        /// <summary>
        /// 金额
        /// </summary>
        public int TotalAmount { get; set; } = 0;

        /// <summary>
        /// 訂單
        /// </summary>
        public Order[] Orders { get; set; } = new Order[] { };

        public override Method GetMethod() => Method.Post;

        public override string GetResource() => "/dapi/deduct/amount/batch";

        public override bool IsSkipSignSalt() => false;
    }

    public class Order
    {
        /// <summary>
        /// 注单id
        /// </summary>
        public string OrderNo { get; set; } = string.Empty;

        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; } = string.Empty;

        /// <summary>
        /// 游戏id
        /// </summary>
        public string GameId { get; set; } = string.Empty;

        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; } = string.Empty;

        /// <summary>
        /// 游戏描述
        /// </summary>
        public string GameDetail { get; set; } = string.Empty;

        /// <summary>
        /// 期號
        /// </summary>
        public string PeriodNumber { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 子類別
        /// </summary>
        public string SubType { get; set; }
    }
}