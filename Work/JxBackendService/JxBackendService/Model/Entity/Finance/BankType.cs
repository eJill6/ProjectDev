using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Finance
{
    public class BaseBankType
    {
        public int BankTypeID { get; set; }

        public string BankTypeName { get; set; }
    }

    public class BankType : BaseBankType
    {
        public int MoneyInType { get; set; }

        public bool Visible { get; set; }
    }
}
