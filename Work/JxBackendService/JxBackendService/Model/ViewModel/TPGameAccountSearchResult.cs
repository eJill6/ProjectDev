using System.Collections.Generic;

namespace JxBackendService.Model.ViewModel
{
    public class UserAccountSearchReault
    {
        public List<TPGameAccountSearchResult> TPGameAccountSearchResults { get; set; } = new List<TPGameAccountSearchResult>();
        public PlatformAccountSearchResult PlatformAccountSearchResult { get; set; }
    }

    public class TPGameAccountSearchResult
    {
        public int LocalUserID { get; set; }
        public string LocalAccount { get; set; }
        public string TPGameProductCode { get; set; }
        public string TPGameProductName { get; set; }
        public string TPGameAccount { get; set; }
        public string TPGameAvailableScoreText { get; set; }
        public int Sort { get; set; }
    }

    public class PlatformAccountSearchResult
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string AvailableScoreText { get; set; }
    }
}
