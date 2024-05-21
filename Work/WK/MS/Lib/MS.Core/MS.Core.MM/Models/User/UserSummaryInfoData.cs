using MS.Core.Extensions;
using MS.Core.MM.Extensions;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.User
{
    public class UserSummaryInfoData : UserInfoData
    {
        public UserSummaryInfoData(UserInfoData userInfoData, UserSummaryModel[] userSummaries, int postTotalCount)
            : base(userInfoData.UserInfo, userInfoData.VipTypes, userInfoData.Vips, userInfoData.VipWelfares)
        {
            UserSummaris = userSummaries;

            TotalSummaris = UserSummaris
                .GroupBy(x => x.Type)
                .ToDictionary(x => x.Key, x => x.SumOrDefault(f => f.Amount));
            //改版后台新加总发帖数 不包括官方贴
            PostTotalCount = Math.Abs(postTotalCount);

            UserSummaryDic = UserSummaris.ToDictionary(e => (e.Category, e.Type), e => e.Amount);
        }

        /// <summary>
        /// UserSummaris
        /// </summary>
        public UserSummaryModel[] UserSummaris { get; set; } = null!;

        public Dictionary<UserSummaryTypeEnum, decimal> TotalSummaris { get; }

        private Dictionary<(UserSummaryCategoryEnum, UserSummaryTypeEnum), decimal> UserSummaryDic { get; }

        /// <summary>
        /// 改版后台新加总发帖数 不包括官方贴
        /// </summary>
        public int PostTotalCount { get; set; }

        /// <summary>
        /// 使用免費解鎖次數
        /// </summary>
        public int UseFreeUnlock => (int?)UserSummaris
            .Where(e => e.Category == CurrentVip?.VipType.ConvertToUserSummaryCategory())
            .Where(e => e.Type == UserSummaryTypeEnum.FreeUnlock)
            .FirstOrDefault()?.Amount ?? 0;

        /// <summary>
        /// 剩餘免費解鎖次數
        /// </summary>
        public int RemainingFreeUnlock => FreeUnlock - UseFreeUnlock;

        /// <summary>
        /// 贴子免費解鎖次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostFreeUnlockCount(PostType postType) => HasFreeUnlockAuth(postType) ? RemainingFreeUnlock : 0;

        /// <summary>
        /// 剩餘發贴次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int RemainingUnlock => Math.Max(PublishLimit - TotalPostSendTimes, 0);

        /// <summary>
        /// PostType 發贴次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostTypeSendTimes(PostType postType)
            => (int)UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.Send));

        /// <summary>
        /// 總發贴次數
        /// </summary>
        public int TotalPostSendTimes => PostTotalCount;

        /// <summary>
        /// 區收益
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public decimal PostTypeIncome(PostType postType)
            => UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.Income));

        /// <summary>
        /// 區解鎖次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostTypeUnLock(PostType postType)
            => (int)UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.UnLock));

        /// <summary>
        /// 區被解鎖次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostTypeUnlocked(PostType postType)
            => (int)UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.Unlocked));

        /// <summary>
        /// 區評論次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostTypeComment(PostType postType)
            => (int)UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.Comment));

        /// <summary>
        /// 累積收益
        /// </summary>
        public decimal TotalIncome => TotalSummaris.GetValueOrDefault(UserSummaryTypeEnum.Income);

        /// <summary>
        /// 區預約次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostTypeAppointmentCount(PostType postType)
            => (int)UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.BookingCount));

        /// <summary>
        /// 區被預約次數
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int PostTypeAppointmentedCount(PostType postType)
            => (int)UserSummaryDic.GetValueOrDefault((postType.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.BookedCount));

        /// <summary>
        /// 收藏数
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int FavoritesCount(UserFavoriteCategoryEnum userSummaryCategoryEnum)
            => (int)UserSummaryDic.GetValueOrDefault((userSummaryCategoryEnum.ConvertToUserSummaryCategory(), UserSummaryTypeEnum.FavoriteCount));
    }
}