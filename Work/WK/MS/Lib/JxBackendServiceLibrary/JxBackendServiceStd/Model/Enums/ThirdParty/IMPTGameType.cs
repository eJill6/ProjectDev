using JxBackendService.Model.Enums.MiseOrder;
using System;
using System.Linq;

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
            OrderGameId = MiseOrderGameId.IMPTLIVE,
            Sort = 1
        };

        public static readonly IMPTGameType IMPTSlot = new IMPTGameType()
        {
            Value = "IMPTSlot",
            MatchKeyword = "Slot",
            OrderGameId = MiseOrderGameId.IMPT,
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