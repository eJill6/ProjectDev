using JxBackendService.Model.MiseLive.Response;

namespace JxBackendService.Model.ViewModel.RecycleBalance
{
    public class MiseAndTPGameBalance
    {
        public string UserID { get; set; }

        public MiseLiveBalance MiseLiveBalance { get; set; }

        public UserAccountSearchResult UserAccountSearchResult { get; set; } = new UserAccountSearchResult();
    }
}