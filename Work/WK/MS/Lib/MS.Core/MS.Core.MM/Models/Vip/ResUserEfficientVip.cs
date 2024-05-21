using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.Vip
{
    public class ResUserEfficientVip
    {
        public int VipId { get; set; }
        public string Name { get; set; } = null!;

        public int UserId { get; set; }
        public VipType Type { get; set; }
        public DateTime? EffectiveTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}