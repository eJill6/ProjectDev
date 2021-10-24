using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.ColourEgg
{
    public enum RedEnvelopeOpenTypes
    {
        OpenedSuccess = 0,

        /// <summary>
        /// 不是老客戶, 不符合發放紅包的用戶等級
        /// </summary>
        IsNotOldUser = 96,

        /// <summary>
        /// 请绑定银行卡以便领取红包
        /// </summary>
        NoBindBankCard = 98,

        /// <summary>
        /// 相同IP重覆領取紅包
        /// </summary>
        SameIpReopen = 99,
    }
}
