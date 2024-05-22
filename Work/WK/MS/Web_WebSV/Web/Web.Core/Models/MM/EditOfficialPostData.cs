using MS.Core.MMModel.Models.Post;

namespace Web.Core.Models.MM
{
    public class EditOfficialPostData : OfficialPostDataForClient
    {
        /// <summary>
        /// 帖子id
        /// </summary>
        public string PostId { get; set; }
    }
}