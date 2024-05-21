using MS.Core.MM.Model.Banner;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Models.Entities.MessageUserRead;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IHomeAnnouncementRepo
    {
        Task<IEnumerable<MMHomeAnnouncement>> Get();

        Task<DBResult> Update(MMHomeAnnouncement param);

        Task<PageResultModel<MMHomeAnnouncement>> GetAnnouncementPageAsync(MyMessageQueryParamForClient param);
        Task<IEnumerable<MMHomeAnnouncement>> GetAllAnnouncement();
        /// <summary>
        /// 根据ID查询单个公告详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MMHomeAnnouncement> GetAnnouncementById(int id);
        /// <summary>
        /// 获取用户未读消息数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserUnreadMessage> GetUnreadCount(int userId);
        Task<int> GetGetAnnouncementCount();

        Task<DBResult> Create(MMHomeAnnouncement param);

        Task<bool> HasDuplicateSort(MMHomeAnnouncement param);

        Task Delete(int id);
    }
}