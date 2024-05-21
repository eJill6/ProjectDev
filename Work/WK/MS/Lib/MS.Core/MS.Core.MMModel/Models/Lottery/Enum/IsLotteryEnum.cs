using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Lottery.Enum
{
    public enum IsLotteryEnum
    {
        /// <summary>
        /// 未派奖
        /// </summary>
        [Description("未开奖")]
        NotAwarded = 0,

        /// <summary>
        /// 已开奖
        /// </summary>
        [Description("已开奖")]
        Awarded = 1,
    }
}
