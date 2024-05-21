using MS.Core.MMModel.Models.My.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.My
{
    /// <summary>
    /// 我的發贴查詢
    /// </summary>
    public class MyPostQueryParamForClient : PageParam
    {
        /// <inheritdoc cref="Post.Enums.PostType"/>
        public PostType PostType { get; set; }

        /// <inheritdoc cref="ReviewStatus"/>
        public ReviewStatus? PostStatus { get; set; }

        /// <inheritdoc cref="MyPostSortType"/>
        public MyPostSortType SortType { get; set; }
    }
}
