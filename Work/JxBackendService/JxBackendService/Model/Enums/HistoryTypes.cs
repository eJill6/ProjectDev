using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class ThirdPartyHistoryStatuses : BaseIntValueModel<ThirdPartyHistoryStatuses>
    {
        private ThirdPartyHistoryStatuses() { }

        public List<int> IsWinsForApi { get; private set; }

        /// <summary>未開獎</summary>
        /// 目前只有老虎機會傳
        public static readonly ThirdPartyHistoryStatuses NoFactionAward = new ThirdPartyHistoryStatuses()
        {
            Value = 0,
            IsWinsForApi = new List<int> { PTBetResultType.NoFactionAward.Value }
        };

        /// <summary>已開獎</summary>
        public static readonly ThirdPartyHistoryStatuses Awarded = new ThirdPartyHistoryStatuses() {
            Value = 1 ,
            IsWinsForApi = new List<int> { BetResultType.Draw.Value, PTBetResultType.Win.Value }
        };

        /// <summary>未中獎</summary>
        public static readonly ThirdPartyHistoryStatuses Loss = new ThirdPartyHistoryStatuses()
        {
            Value = 2,
            IsWinsForApi = new List<int> { BetResultType.Lose.Value }
        };

        /// <summary>撤單</summary>
        /// 目前只有老虎機會傳
        public static readonly ThirdPartyHistoryStatuses Cancel = new ThirdPartyHistoryStatuses()
        {
            Value = 4,
            IsWinsForApi = new List<int> { PTBetResultType.Cancel.Value }
        };

        //用 betResultTypeValue 回推 historyStatus
        public static ThirdPartyHistoryStatuses GetSingleByBetResultType(int? isFactionAward, int isWin)
        {
            //老虎機
            if (isFactionAward.HasValue)
            {
                if (isFactionAward == 0)
                {
                    return ThirdPartyHistoryStatuses.NoFactionAward;
                }
                else if (isFactionAward == 1 && isWin == BetResultType.Win.Value)
                {
                    return ThirdPartyHistoryStatuses.Awarded;
                }
                else if (isFactionAward == 1 && isWin == BetResultType.Lose.Value)
                {
                    return ThirdPartyHistoryStatuses.Loss;
                }
                else if (isFactionAward == 3 )
                {
                    return ThirdPartyHistoryStatuses.Cancel;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            //一般第三方
            else
            {
                if (isWin == BetResultType.Lose.Value)
                {
                    return ThirdPartyHistoryStatuses.Loss;
                }
                else
                {
                    return ThirdPartyHistoryStatuses.Awarded;
                }
            }
        }
    }
}
