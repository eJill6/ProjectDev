using System;
using System.Collections.Generic;
using System.Linq;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class IMPTGameType : BaseStringValueModel<IMPTGameType>
    {
        public MiseOrderGameId OrderGameId { get; private set; }

        private string MatchKeyword { get; set; }

        private IMPTGameType()
        { }

        public static readonly IMPTGameType IMPTLive = new IMPTGameType()
        {
            Value = "IMPTLive",
            MatchKeyword = "Live",
            //OrderGameId = MiseOrderGameId.IMPTLIVE, 未開放
            Sort = 1
        };

        public static readonly IMPTGameType IMPTSlot = new IMPTGameType()
        {
            Value = "IMPTSlot",
            MatchKeyword = "Slot",
            //OrderGameId = MiseOrderGameId.IMPT, 未開放
            Sort = 2
        };

        public static IMPTGameType GetIMPTGameType(string gameType)
        {
            IMPTGameType imptGameType = GetAll()
                .Where(w => gameType.IndexOf(w.MatchKeyword, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .FirstOrDefault();

            if (imptGameType != null)
            {
                return imptGameType;
            }

            return IMPTSlot;
        }
    }
}