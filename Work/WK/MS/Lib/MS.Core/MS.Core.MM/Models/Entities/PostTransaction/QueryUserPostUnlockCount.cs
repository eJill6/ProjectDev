using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class QueryUserPostUnlockCount
    {
        public PostType PostType { get; set; }
        public int Count { get; set; }
    }
}
