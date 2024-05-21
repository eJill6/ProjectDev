using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.Filters
{
    public class VipWelfareFilter
    {
        public VIPWelfareTypeEnum? Type { get; set; }
        public IEnumerable<VipType>? VipTypes { get; set; }
    }
}
