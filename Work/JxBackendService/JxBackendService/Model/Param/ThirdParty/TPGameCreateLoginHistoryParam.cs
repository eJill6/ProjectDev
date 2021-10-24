using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class TPGameCreateLoginHistoryParam
    {
        public int Type{get;set;}
        public int UserID{get;set;}
        public string UserName{get;set;}
        public JxIpInformation Ipinformation { get; set; }
    }
}
