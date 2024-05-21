using MS.Core.Extensions;
using MS.Core.MM.Extensions;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Vip;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Vip.Enums;
using System.ComponentModel.DataAnnotations;

namespace MS.Core.MM.Models.User
{
    public class UserInfoData
    {
        public UserInfoData(MMUserInfo userInfo, MMVipType[] vipTypes)
        {
            VipTypes = vipTypes;
            UserInfo = userInfo;
        }

        public UserInfoData(MMUserInfo userInfo, MMVipType[] vipTypes, UserVipInfo[]? vips, MMVipWelfare[]? vipWelfares)
        {
            VipTypes = vipTypes;
            var vipTypeDic = vipTypes?.ToDictionary(e => e.Id, e => e);

            UserInfo = userInfo;

            Vips = vips;

            VipWelfares = vipWelfares;
        }

        public MMVipType[] VipTypes { get; }
        public MMUserInfo UserInfo { get; }

        public UserVipInfo[]? Vips { get; }

        /// <summary>
        /// 當前使用者的福利
        /// </summary>
        public MMVipWelfare[]? VipWelfares { get; }

        public UserVipInfo? CurrentVip => Vips?.OrderByDescending(e => e.Priority).First();

        public bool IsVip => Vips.IsNotEmpty();

        /// <summary>
        /// 當前使用者的免費解鎖次數
        /// </summary>
        public int FreeUnlock => (int?)VipWelfares?.Where(e => e.Type == VIPWelfareTypeEnum.FreeUnlock).Select(e => e.Value).FirstOrDefault() ?? 0;

        public bool HasFreeUnlockAuth(PostType postType) =>
            VipWelfares?
            .Where(e => e.Type == VIPWelfareTypeEnum.FreeUnlockAuth)
            .Any(e => e.Category == postType.ConvertToVIPWelfareCategory() && e.Value == 1M) ?? false;

        /// <summary>
        /// 有折扣給折扣(0.8、0.7)，沒折扣就給 1
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public decimal Discount(PostType postType) => VipWelfares?
            .Where(e => e.Category == postType.ConvertToVIPWelfareCategory())
            .Where(e => e.Type == VIPWelfareTypeEnum.Discount).FirstOrDefault()?.Value ?? 1;

        /// <summary>
        /// 發贴上限（包含会员的一次）
        /// </summary>
        public int PublishLimit => (IsVip ? 1 : 0) + UserInfo.ExtraPostCount;

    }
}