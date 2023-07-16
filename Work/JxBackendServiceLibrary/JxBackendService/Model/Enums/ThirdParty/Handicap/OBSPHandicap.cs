using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.ThirdParty.Handicap
{
    public class OBSPHandicap : BaseStringValueModel<OBSPHandicap>
    {
        public PlatformHandicap PlatformHandicap { get; private set; }

        private OBSPHandicap(string value)
        {
            Value = value;
        }

        /// <summary>香港</summary>
        public static readonly OBSPHandicap HK = new OBSPHandicap("HK")
        {
            PlatformHandicap = PlatformHandicap.HongKong
        };

        /// <summary>歐洲</summary>
        public static readonly OBSPHandicap EU = new OBSPHandicap("EU")
        {
            PlatformHandicap = PlatformHandicap.Europe
        };
    }
}
