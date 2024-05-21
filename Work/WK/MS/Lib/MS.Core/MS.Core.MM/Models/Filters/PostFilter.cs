using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.Filters
{
    public class PostFilter
    {
        public IEnumerable<int> UserIds { get; set; }
        public ReviewStatus? Status { get; set; }
    }
}
