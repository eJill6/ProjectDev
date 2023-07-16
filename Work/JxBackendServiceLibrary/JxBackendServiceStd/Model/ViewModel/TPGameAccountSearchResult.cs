using JxBackendService.Common.Util;
using System.Collections.Generic;

namespace JxBackendService.Model.ViewModel
{
    public class UserAccountSearchResult
    {
        public List<TPGameAccountSearchResult> TPGameAccountSearchResults { get; set; } = new List<TPGameAccountSearchResult>();

        public PlatformAccountSearchResult PlatformAccountSearchResult { get; set; }
    }

    public class TPGameAccountSearchResult
    {
        public int LocalUserID { get; set; }

        public string TPGameProductCode { get; set; }

        public string TPGameProductName { get; set; }

        public string TPGameAccount { get; set; }

        public decimal TPGameAvailableScore { get; set; }

        public string TPGameAvailableScoreText => TPGameAvailableScore.ToCurrency();

        public decimal TPGameFreezeScore { get; set; }

        public string TPGameFreezeScoreText => TPGameFreezeScore.ToCurrency();

        public int Sort { get; set; }
    }

    public class PlatformAccountSearchResult
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public decimal AvailableScore { get; set; }

        public string AvailableScoreText => AvailableScore.ToCurrency(true);
    }
}