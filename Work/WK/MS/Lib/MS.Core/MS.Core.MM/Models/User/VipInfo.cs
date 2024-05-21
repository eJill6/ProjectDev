using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.User
{
    public interface IVipTypeInfo
    {
        /// <summary>
        /// 名稱
        /// </summary>
        string TypeName { get; set; }
        /// <summary>
        /// 優先度
        /// </summary>
        int Priority { get; set; }
    }
    public class VipInfo : MMVip, IVipTypeInfo
    {
        public VipInfo(MMVip vip, MMVipType vipType)
        {
            Id = vip.Id;
            Name = vip.Name;
            Type = vip.Type;
            Price = vip.Price;
            Status = vip.Status;
            Memo = vip.Memo;
            CreateUser = vip.CreateUser;
            CreateTime = vip.CreateTime;
            UpdateUser = vip.UpdateUser;
            UpdateTime = vip.UpdateTime;
            Days = vip.Days;
            TypeName = vipType.Name;
            Priority = vipType.Priority;
        }
        /// <summary>
        /// 名稱
        /// </summary>
        public string TypeName { get; set; } = string.Empty;
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; set; }
    }
}
