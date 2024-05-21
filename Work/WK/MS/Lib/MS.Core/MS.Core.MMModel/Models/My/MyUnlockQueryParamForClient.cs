using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.My
{
    public class MyUnlockQueryParamForClient : PageParam
    {
        /// <inheritdoc cref="PostType"/>
        public PostType PostType { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
