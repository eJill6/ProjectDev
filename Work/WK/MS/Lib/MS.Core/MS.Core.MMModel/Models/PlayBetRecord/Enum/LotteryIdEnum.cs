using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.PlayBetRecord.Enum
{
    public enum LotteryIdEnum
    {
        /// <summary>
        /// 一分快三
        /// </summary>
        [Description("一分快三")]
        OMKS = 65,
        /// <summary>
        /// 一分快车
        /// </summary>
        [Description("一分快车")]
        OMPK10 = 66,
        /// <summary>
        /// 一分时时彩
        /// </summary>
        [Description("一分时时彩")]
        OMSSC = 67,
        /// <summary>
        /// 一分六合彩
        /// </summary>
        [Description("一分六合彩")]
        OMLHC = 68
    }
}
