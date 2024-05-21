using AutoMapper;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;

namespace MMService.Infrastructure.AutoMapper
{
    /// <summary>
    /// 贴子相關mapper
    /// </summary>
    public class MMPostModelProfile : Profile
    {
        /// <summary>
        /// 贴子相關mapper profile 
        /// </summary>
        public MMPostModelProfile()
        {
            // 官方贴資料
            CreateMap<OfficialPostData, ReqOfficialPostData>();

            // 官方贴搜尋參數
            CreateMap<OfficialPostSearchParam, ReqOfficialPostSearchParam>();

            // 官方贴搜尋參數
            CreateMap<OfficialCommentData, ReqOfficialCommentData>();

            // 官方贴刪除資料
            CreateMap<OfficialPostDelete, ReqOfficialPostDelete>();
            
            // 官方上架帖子
            CreateMap<OfficialPostShelfOfficial, ReqSetShelfOfficialPost>();
        }
    }
}