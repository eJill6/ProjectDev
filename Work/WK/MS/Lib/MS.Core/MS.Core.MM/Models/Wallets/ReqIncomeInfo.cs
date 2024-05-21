using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.Wallets
{
    public class ReqIncomeInfo
    {
        public PostType? PostType { get; set; }
        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int PageNo { get; set; }
    }
}