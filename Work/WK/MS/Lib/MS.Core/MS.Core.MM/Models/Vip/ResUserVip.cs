using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.Vip
{
    public class ResUserVip
    {
        /// <summary>
        /// VipType(致富銀卡、致富金卡)
        /// </summary>
        public VipType Type { get; set; }
        /// <summary>
        /// TypeName
        /// </summary>
        public string Name { get; set; } = null!;
    }
}