﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Common.IMOne
{
    public class IMeBETAppSetting : BaseIMOneAppSetting
    {
        public IMeBETAppSetting() { }

        public override string ProductWallet => "201";

        public override string GameCode => "imlive20000";
    }
}
