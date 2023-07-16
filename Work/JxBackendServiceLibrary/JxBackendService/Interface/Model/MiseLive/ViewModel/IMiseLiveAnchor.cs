namespace JxBackendService.Interface.Model.MiseLive.ViewModel
{
    public interface IMiseLiveAnchor
    {
        int AccountStatus { get; set; }

        int AnchorId { get; set; }

        string Avatar { get; set; }

        string Description { get; set; }

        string EndTime { get; set; }

        int EntranceStatus { get; set; }

        string FansNum { get; set; }

        int FollowLive { get; set; }

        int GameStateLive { get; set; }

        int GameTypeIdLive { get; set; }

        int LiveStatus { get; set; }

        string NickName { get; set; }

        int OnlineLive { get; set; }

        int ShowStatus { get; set; }

        string SmallAvatar { get; set; }

        string StartTime { get; set; }

        string UserName { get; set; }

        decimal WinRangeLive { get; set; }
    }
}