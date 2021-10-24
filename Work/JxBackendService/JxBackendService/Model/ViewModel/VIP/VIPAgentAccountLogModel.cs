using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.VIP
{
    public class VIPAgentAccountLogModel : BaseVIPChangeLogModel
    {
        public string ChangeType { get; set; }

        public string ChangeTypeName { get; set; }

        public decimal ChangeAvailableScores { get; set; }

        public decimal OldAvailableScores { get; set; }

        public decimal NewAvailableScores { get; set; }

        public decimal ChangeFreezeScores { get; set; }

        public decimal OldFreezeScores { get; set; }

        public decimal NewFreezeScores { get; set; }
    }
}
