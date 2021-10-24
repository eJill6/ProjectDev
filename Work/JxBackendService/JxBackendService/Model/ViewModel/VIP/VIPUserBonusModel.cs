using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.VIP
{
    public class VIPUserBonusModel
    {
        public string Username { get; set; }

        public string VIPLevel { get; set; }

        public string BonusType { get; set; }

        public decimal BonusMoney { get; set; }

        public string Memo { get; set; }

        public string ReceivedStatus { get; set; }

        public DateTime? ReceivedDate { get; set; }
    }
}
