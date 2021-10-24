using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public abstract class BaseTPGameMoneyInfo
    {        
        public decimal Amount { get; set; }
        
        public string OrderID { get; set; }
        
        public DateTime OrderTime { get; set; }
        
        public string Handle { get; set; }
        
        public DateTime? HandTime { get; set; }
        
        public int UserID { get; set; }
        
        public string UserName { get; set; }
        
        public short Status { get; set; }
        
        public string Memo { get; set; }

        public abstract string GetMoneyID();

        public abstract string GetPrimaryKeyColumnName();
    }
}
