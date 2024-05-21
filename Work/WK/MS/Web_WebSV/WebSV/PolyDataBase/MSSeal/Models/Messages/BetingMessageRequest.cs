using SLPolyGame.Web.Model;
using System;

namespace SLPolyGame.Web.MSSeal.Models.Messages
{
    /// <summary>
    /// 投注訊息
    /// </summary>
    public class BetingMessageRequest : MessageRequest<BetingContent>
    {
        public BetingMessageRequest(PalyInfo palyInfo, string orderNumber, string salt)
        {
            Type = (int)Enums.MessageType.Beting;
            Content = new BetingContent[] { new BetingContent(palyInfo, orderNumber) };
            Salt = salt;
        }
    }

    /// <summary>
    /// 投注內容
    /// </summary>
    public class BetingContent
    {
        public BetingContent(PalyInfo palyInfo, string orderNumber)
        {
            UserId = palyInfo.UserID.Value;
            NickName = palyInfo.UserName;
            OrderNo = orderNumber;
            BetAmount = palyInfo.NoteMoney.Value;
            GameId = palyInfo.LotteryID.Value;
            GameName = palyInfo.LotteryType;
            RoomNo = Convert.ToInt32(palyInfo.RoomId);
        }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 投注金額
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 彩種編號
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// 彩種名稱
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 房間編號
        /// </summary>
        public int RoomNo { get; set; }
    }
}