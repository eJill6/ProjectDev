using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class TPGameMoneySettingViewModel
    {
        public List<JxBackendSelectListItem> ProductItems { get; set; }

        public List<JxBackendSelectListItem> SearchTransferTypeItems { get; set; }

        public List<JxBackendSelectListItem> MoneyInOrderStatusItems { get; set; }

        public List<JxBackendSelectListItem> MoneyOutOrderStatusItems { get; set; }
    }
}
