using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGameMoneyInInfo : BaseTPGameMoneyInfo
    {
        public string MoneyInID { get; set; }
        public override string GetMoneyID() => MoneyInID;

        public override string GetPrimaryKeyColumnName() => nameof(MoneyInID);
    }
}
