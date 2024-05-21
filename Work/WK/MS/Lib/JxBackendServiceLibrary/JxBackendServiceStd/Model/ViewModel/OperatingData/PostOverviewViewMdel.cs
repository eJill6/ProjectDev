using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ViewModel.OperatingData
{
    public class PostOverviewViewMdel
    {
        /// <summary>
        /// 展示中帖子总数
        /// </summary>
        public int TotalPost { get; set; }

        /// <summary>
        /// 广场展示中
        /// </summary>
        public int SquareDisplayed { get; set; }

        /// <summary>
        /// 广场审核中
        /// </summary>
        public int SquareUnderReview { get; set; }

        /// <summary>
        /// 广场未通过
        /// </summary>
        public int SquareNotApproved { get; set; }

        /// <summary>
        /// 广场被解锁次数
        /// </summary>
        public int SquareUnlockedCount { get; set; }

        /// <summary>
        /// 寻芳阁展示中
        /// </summary>
        public int AgencyDisplayed { get; set; }

        /// <summary>
        /// 寻芳阁审核中
        /// </summary>
        public int AgencyUnderReview { get; set; }

        /// <summary>
        /// 寻芳阁未通过
        /// </summary>
        public int AgencyNotApproved { get; set; }

        /// <summary>
        /// 寻芳阁被解锁次数
        /// </summary>
        public int AgencyUnlockedCount { get; set; }

        /// <summary>
        /// 官方展示中
        /// </summary>
        public int OfficialDisplayed { get; set; }

        /// <summary>
        /// 官方审核中
        /// </summary>
        public int OfficialUnderReview { get; set; }

        /// <summary>
        /// 官方未通过
        /// </summary>
        public int OfficialNotApproved { get; set; }

        /// <summary>
        /// 官方被预约次数
        /// </summary>
        public int OfficialReservedCount { get; set; }

        /// <summary>
        /// 官方被预约取消次数
        /// </summary>
        public int OfficialReserveCancelCount { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.ActivePostsCount), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string TotalPostText => TotalPost.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareDisplayed), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string SquareDisplayedText => SquareDisplayed.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareUnderReview), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string SquareUnderReviewText => SquareUnderReview.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareNotApproved), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string SquareNotApprovedText => SquareNotApproved.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareUnlocksCount), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string SquareUnlockedCountText => SquareUnlockedCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyDisplayed), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string AgencyDisplayedText => AgencyDisplayed.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyUnderReview), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string AgencyUnderReviewText => AgencyUnderReview.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyNotApproved), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string AgencyNotApprovedText => AgencyNotApproved.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyUnlocksCount), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string AgencyUnlockedCountText => AgencyUnlockedCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialDisplayed), ResourceType = typeof(DisplayElement), Sort = 10)]
        public string OfficialDisplayedText => OfficialDisplayed.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialUnderReview), ResourceType = typeof(DisplayElement), Sort = 11)]
        public string OfficialUnderReviewText => OfficialUnderReview.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialNotApproved), ResourceType = typeof(DisplayElement), Sort = 12)]
        public string OfficialNotApprovedText => OfficialNotApproved.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialReservationsCount), ResourceType = typeof(DisplayElement), Sort = 13)]
        public string OfficialReservedCountText => OfficialReservedCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialReserveCancelCount), ResourceType = typeof(DisplayElement), Sort = 14)]
        public string OfficialReserveCancelCountText => OfficialReserveCancelCount.ToString("N0");
    }
}