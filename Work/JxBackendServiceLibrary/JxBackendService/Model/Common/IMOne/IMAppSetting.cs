using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Common.IMOne
{
    public class IMAppSetting : BaseIMOneAppSetting
    {
        public IMAppSetting() { }

        public override string ProductWallet => "401";

        public override string GameCode => "ESPORTSBULL";
    }
}
