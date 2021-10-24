using System;

namespace JxBackendService.Model.ViewModel.Game
{
    public class PlatformUserProfitloss
    {
        public string DisplayProfitLossType { get; set; }

        public string UserName { get; set; }

        public string DisplayMoney { get; set; } = "0";

        public string DisplayProfitLossMoney { get; set; } = "0";

        public string Memo { get; set; }

        public string DisplayProfitLossTime { get; set; }
    }    
}
