using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.SabaSport
{
    public class SabaSportPlatformTypes : BaseStringValueModel<SabaSportPlatformTypes>
    {
        private SabaSportPlatformTypes() { }

        /// <summary>桌機</summary>
        public static SabaSportPlatformTypes PC = new SabaSportPlatformTypes()
        {
            Value = "1"
        };

        /// <summary>手機H5</summary>
        public static SabaSportPlatformTypes MobileH5 = new SabaSportPlatformTypes()
        {
            Value = "2"
        };

        /// <summary>手機純文字</summary>
        public static SabaSportPlatformTypes MobileOnlyText = new SabaSportPlatformTypes()
        {
            Value = "3"
        };
    }
}
