using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class UserPointExpenseModel
    {
        public PostType PostType { get; set; }
        public decimal Point { get; set; }
        public VipType VipType { get; set; }
    }

}
