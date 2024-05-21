using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.SystemSettings;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models
{
    public class MMGlobalSettings
    {
        /// <summary>
        /// 解鎖基本價
        /// </summary>
        public static readonly decimal BaseUnlockAmount = 100M;

        /// <summary>
        /// 应入账时间=>解锁时间+120小时
        /// </summary>
        public static int BaseDispatchHours => 120;

        /// <summary>
        /// 解鎖基本價
        /// </summary>
        public static readonly List<PostTypeUnlockAmount> BaseUnlockAmountSetting = new List<PostTypeUnlockAmount>()
        {
            new PostTypeUnlockAmount() {PostType = PostType.Square, UnlockAmount = 100},
            new PostTypeUnlockAmount() {PostType = PostType.Agency, UnlockAmount = 200},
        };
    }
}