using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.ThirdParty.Handicap
{
    public class SabaSportHandicap : BaseStringValueModel<SabaSportHandicap>
    {
        public PlatformHandicap PlatformHandicap { get; private set; }

        private SabaSportHandicap(string value)
        {
            Value = value;
        }

        /// <summary>馬來</summary>
        public static readonly SabaSportHandicap Malaysia = new SabaSportHandicap("1")
        {
            PlatformHandicap = PlatformHandicap.Malaysia
        };

        /// <summary>中國</summary>
        public static readonly SabaSportHandicap China = new SabaSportHandicap("2")
        {
            PlatformHandicap = PlatformHandicap.China
        };

        /// <summary>歐洲</summary>
        public static readonly SabaSportHandicap Europe = new SabaSportHandicap("3")
        {
            PlatformHandicap = PlatformHandicap.Europe
        };

        /// <summary>印尼</summary>
        public static readonly SabaSportHandicap Indonesia = new SabaSportHandicap("4")
        {
            PlatformHandicap = PlatformHandicap.Indonesia
        };

        /// <summary>美國</summary>
        public static readonly SabaSportHandicap America = new SabaSportHandicap("5")
        {
            PlatformHandicap = PlatformHandicap.America
        };
    }
}