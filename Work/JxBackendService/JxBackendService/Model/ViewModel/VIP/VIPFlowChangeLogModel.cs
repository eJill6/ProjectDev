using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.VIP
{
    public class VIPFlowChangeLogModel : BaseVIPChangeLogModel
    {
        public string ChangeType { get; set; }

        public decimal OldFlowAccountAmount { get; set; }

        public decimal NewFlowAccountAmount { get; set; }

        public decimal ChangeFlowAmount { get; set; }

        public decimal Multiple { get; set; }

        public string ChangeTypeName { get; set; }
    }
}
