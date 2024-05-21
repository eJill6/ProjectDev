using MS.Core.Extensions;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using System.Diagnostics.CodeAnalysis;

namespace MS.Core.MM.Extensions
{
    public static class UserVipExtension
    {
        /// <summary>
        /// 有效會員
        /// </summary>
        /// <param name="vip"></param>
        /// <returns></returns>
        public static bool IsEffective([NotNull]this MMUserVip vip)
        {
            return vip.EffectiveTime.AddDays(vip.ExtendDay) > DateTimeExtension.GetCurrentTime();
        }
    }
}
