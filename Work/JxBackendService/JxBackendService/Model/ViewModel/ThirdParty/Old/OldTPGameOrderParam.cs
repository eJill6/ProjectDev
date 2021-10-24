using JxBackendService.Model.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty.Old
{
    public class OldTPGameOrderParam<ApiParamType>
    {
        public ApiParamType ApiParam { get; set; }
        
        public BaseTPGameMoneyInfo TPGameMoneyInfo { get; set; }
    }
}
