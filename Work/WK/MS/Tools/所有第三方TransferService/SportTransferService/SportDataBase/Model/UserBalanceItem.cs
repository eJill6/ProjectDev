using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportDataBase.Model
{
    public class UserBalanceItem
    {
        public string vendor_member_id { get; set; }
        public Nullable<decimal> balance { get; set; }
        public Nullable<decimal> outstanding { get; set; }
        public Nullable<int> currency { get; set; }
        public int error_code { get; set; }
    }
}
