using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Media.Enums
{
    public enum BusinessAggregateType
    {
        /// <summary>
        /// UN(当日/月第一次访问的新用户)。
        /// </summary>
        UN = 1,

        /// <summary>
        /// AU(当日/月访问次数)。
        /// </summary>
        AU = 2,

        /// <summary>
        /// PV(访问秘觅用户人次)。
        /// </summary>
        PV = 3,

        /// <summary>
        /// 整点人数。
        /// </summary>
        HourlyUsers = 4,

        /// <summary>
        /// PCU(每天24笔记录中取出最大值)。
        /// </summary>
        PCU = 5,

        /// <summary>
        /// ACU(每天24笔记录中算出平均值)。
        /// </summary>
        ACU = 6,

        /// <summary>
        /// 商户数。
        /// </summary>
        Merchants = 7,

        /// <summary>
        /// 累计用户数。
        /// </summary>
        TotalUsers = 8,

        /// <summary>
        /// 付费用户数(PU)。
        /// </summary>
        PU = 9,

        /// <summary>
        /// 付费率。
        /// </summary>
        PaymentRate = 10,

        /// <summary>
        /// 收益。
        /// </summary>
        Revenue = 11,

        /// <summary>
        /// ARPU。
        /// </summary>
        ARPU = 12,

        /// <summary>
        /// ARPPU。
        /// </summary>
        ARPPU = 13,

        /// <summary>
        /// 保证金金额。
        /// </summary>
        DepositsAmount = 14,

        /// <summary>
        /// 保证金笔数。
        /// </summary>
        DepositsCount = 15
    }
}
