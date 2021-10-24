using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.VIP
{
    public class VIPPointsChangeLogModel : BaseVIPChangeLogModel
    {
        public int ChangeType { get; set; }

        public string ChangeTypeName { get; set; }

        public decimal OldAccumulatePoints { get; set; }

        public decimal NewAccumulatePoints { get; set; }

        public decimal ChangePoints { get; set; }

        public decimal BetMoney { get; set; }
    }
}
