using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Param.Game;

namespace JxBackendService.Model.ViewModel.Game
{
    public class LiveGameManageModel : BaseGameManageModel, ILiveGameManageGridRowSetDefaultParam
    {
        public int LotteryId { get; set; }

        public string LotteryType { get; set; }

        public string Url { get; set; }

        public string ApiUrl { get; set; }

        public decimal FrameRatio { get; set; }

        public int Style { get; set; }

        public bool IsCountdown { get; set; }

        public int Duration { get; set; }

        public bool IsH5 { get; set; }

        public bool IsFollow { get; set; }

        public int LiveGameManageDataTypeValue { get; set; }

        public string LiveGameManageDataTypeName { get; set; }

        public int TabType { get; set; }

        public string ProductCode { get; set; }

        public string GameCode { get; set; }

        public string RemoteCode { get; set; }

        public string IsCountdownText { get; set; }

        public string IsH5Text { get; set; }

        public string IsFollowText { get; set; }

        public string FrameRatioText { get; set; }

        public string StyleText { get; set; }

        public string DurationText { get; set; }
    }
}