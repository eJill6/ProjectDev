using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class UserScore
    {
        public decimal AvailableScores { get; set; }
        public decimal FreezeScores { get; set; }
    }

    public class TPGameTransferReturnResult
    {
        public UserScore RemoteUserScore { get; set; }

        public string ApiResult { get; set; }
    }
}
