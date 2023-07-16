using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.ThirdParty.IMSport
{
    public class IMSportBetStatus : BaseStringValueModel<IMSportBetStatus>
    {
        public BetResultType BetResultType { get; private set; }

        private IMSportBetStatus() { }

        public static readonly IMSportBetStatus Win = new IMSportBetStatus()
        {
            Value = "1",
            BetResultType = BetResultType.Win,
        };

        public static readonly IMSportBetStatus Lose = new IMSportBetStatus()
        {
            Value = "2",
            BetResultType = BetResultType.Lose,
        };

        public static readonly IMSportBetStatus Draw = new IMSportBetStatus()
        {
            Value = "3",
            BetResultType = BetResultType.Draw,
        };

        public static readonly IMSportBetStatus HalfWin = new IMSportBetStatus()
        {
            Value = "4",
            BetResultType = BetResultType.HalfWin,
        };

        public static readonly IMSportBetStatus HalfLose = new IMSportBetStatus()
        {
            Value = "5",
            BetResultType = BetResultType.HalfLose,
        };
    }

    public class IMSportWagerType : BaseStringValueModel<IMSportWagerType>
    {
        public WagerType WagerType { get; private set; }

        private IMSportWagerType() { }

        public static readonly IMSportWagerType Single = new IMSportWagerType()
        {
            Value = "Single",
            WagerType = WagerType.Single,
        };

        public static readonly IMSportWagerType Combo = new IMSportWagerType()
        {
            Value = "Combo",
            WagerType = WagerType.Combo,
        };
    }
}
