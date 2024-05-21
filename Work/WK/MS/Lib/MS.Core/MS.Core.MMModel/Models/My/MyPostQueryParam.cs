using MS.Core.MMModel.Models.My.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.My
{
    /// <summary>
    /// 我的發贴查詢
    /// </summary>
    public class MyPostQueryParam : MyPostQueryParamForClient
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }
    }
}
