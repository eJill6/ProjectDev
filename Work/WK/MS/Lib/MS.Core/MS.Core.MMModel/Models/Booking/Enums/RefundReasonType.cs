using System.ComponentModel;

namespace MS.Core.MMModel.Models.Booking.Enums
{
    public enum RefundReasonType : byte
    {
        /// <summary>
        /// 骗子
        /// </summary>
        [Description("存在欺骗")]
        Fraud = 1,

        /// <summary>
        /// 货不对版
        /// </summary>
        [Description("货不对版")]
        Fake = 2,
    }
}