using JxBackendService.Interface.Model.MiseLive.ViewModel;

namespace JxBackendService.Model.ViewModel.ThirdParty.MiseLive
{
    public class MiseLiveAnchor : IMiseLiveAnchor
    {
        public int AccountStatus { get; set; }

        public int AnchorId { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }

        public string EndTime { get; set; }

        public int EntranceStatus { get; set; }

        public string FansNum { get; set; }

        public int FollowLive { get; set; }

        public int GameStateLive { get; set; }

        public int GameTypeIdLive { get; set; }

        public int LiveStatus { get; set; }

        public string NickName { get; set; }

        public int OnlineLive { get; set; }

        public int ShowStatus { get; set; }

        public string SmallAvatar { get; set; }

        public string StartTime { get; set; }

        public string UserName { get; set; }

        public decimal WinRangeLive { get; set; }
    }
}