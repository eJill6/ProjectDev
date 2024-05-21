using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminBooking
{
    /// <summary>
    /// 审核參數
    /// </summary>
    public class AdminBookingRefundParam
    {
        public string RefundId { get; set; }
        public byte Status { get; set; }
        public string Memo { get; set; }
        public string ExamineMan { get; set; }
    }
}
