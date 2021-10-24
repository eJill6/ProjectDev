using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Merchant.CTS
{
    public class ApolloCTSSetting : ApolloService.JXSetting
    {
        public ApolloCTSSetting(string apolloPostUrl, string apollokey) : base(apolloPostUrl, apollokey)
        { 
        
        }

        public override string MerchantNO => "11010157682";
    }
}