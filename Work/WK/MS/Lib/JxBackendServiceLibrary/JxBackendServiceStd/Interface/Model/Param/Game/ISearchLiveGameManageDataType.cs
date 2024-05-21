namespace JxBackendService.Interface.Model.Param.Game
{
    public interface ISearchLiveGameManageDataType
    {
        int LotteryId { get; set; }

        string ProductCode { get; set; }

        string GameCode { get; set; }

        string RemoteCode { get; set; }
    }

    public interface ILiveGameManageSetDefaultParam : ISearchLiveGameManageDataType
    {
        int LiveGameManageDataTypeValue { get; set; }

        string Url { get; set; }

        string ApiUrl { get; set; }

        decimal FrameRatio { get; set; }

        int Style { get; set; }

        bool IsCountdown { get; set; }

        int Duration { get; set; }

        bool IsH5 { get; set; }

        bool IsFollow { get; set; }
    }

    public interface ILiveGameManageGridRowSetDefaultParam : ILiveGameManageSetDefaultParam
    {
        string IsFollowText { get; set; }

        string IsCountdownText { get; set; }

        string IsH5Text { get; set; }

        string FrameRatioText { get; set; }

        string StyleText { get; set; }

        string DurationText { get; set; }
    }
}