using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Model.Banner;
using MS.Core.MM.Model.Entities.Banner;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MS.Core.MM.Models.Entities.MessageUserRead;

namespace MS.Core.MM.Repos
{
    public class HomeAnnouncementRepo : BaseInlodbRepository, IHomeAnnouncementRepo
    {
        public HomeAnnouncementRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<IEnumerable<MMHomeAnnouncement>> Get()
        {
            return (await ReadDb.QueryTable<MMHomeAnnouncement>()
                   .QueryAsync()).ToArray();
        }

        public async Task<DBResult> Update(MMHomeAnnouncement param)
        {
            string sql = @"UPDATE MMHomeAnnouncement
                           SET [ModifyDate] = GETDATE(),
                            [Title] = @Title,
                            [StartTime] = @StartTime,
                            [EndTime] = @EndTime,
                           [HomeContent] = @HomeContent,
                           [RedirectUrl] = @RedirectUrl,
                           [Operator]=@Operator,
                           [IsActive]=@IsActive,
                           [Weight] = @Weight
                           WHERE [Id] = @Id;";
            await WriteDb.AddExecuteSQL(sql, param).SaveChangesAsync();
            DBResult result = new DBResult(ReturnCode.Success);
            return result;
        }

        public async Task<DBResult> Create(MMHomeAnnouncement param)
        {
            await WriteDb.Insert(param).SaveChangesAsync();
            return new DBResult(ReturnCode.Success);
        }

        /// <inheritdoc/>
        public async Task Delete(int id)
        {
            await WriteDb.Delete(new MMHomeAnnouncement()
            {
                Id = id,
            }).SaveChangesAsync();
        }

        /// <summary>
        /// 查询权重值是否重复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<bool> HasDuplicateSort(MMHomeAnnouncement param)
        {
            var query = (await ReadDb.QueryTable<MMHomeAnnouncement>()
                .Where(x => x.Weight == param.Weight && x.Type == param.Type)
                .QueryAsync()).FirstOrDefault();
            if (query != null && query.Id != param.Id)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 根据不同的类型获取到不同的公告
        /// </summary>
        /// <param name="announcementType"></param>
        /// <returns></returns>
        public Task<PageResultModel<MMHomeAnnouncement>> GetAnnouncementPageAsync(MyMessageQueryParamForClient param)
        {
            var component = ReadDb.QueryTable<MMHomeAnnouncement>();

            return component.Where(c => c.Type== (int)AnnouncementType.Announcement && c.IsActive==true && c.EndTime>DateTime.Now ).OrderByDescending(c => c.Id).QueryPageResultAsync(param);
        }
        public Task<IEnumerable<MMHomeAnnouncement>> GetAllAnnouncement()
        {
            var component = ReadDb.QueryTable<MMHomeAnnouncement>();
            return component.Where(c => c.Type == (int)AnnouncementType.Announcement).QueryAsync();
        }

        public Task<int> GetGetAnnouncementCount()
        {
            var component = ReadDb.QueryTable<MMHomeAnnouncement>();
            return component.Where(c => c.Type == (int)AnnouncementType.Announcement).CountAsync();
        }
        /// <summary>
        /// 获取用户的未读消息数
        /// </summary>
        /// <returns></returns>
        public async Task<UserUnreadMessage> GetUnreadCount(int userId)
        {
            ///获取全部公告的ID集合
            var allAnnouncementIds = (await ReadDb.QueryTable<MMHomeAnnouncement>().Where(c => c.Type == (int)AnnouncementType.Announcement && c.IsActive == true && c.EndTime > DateTime.Now).QueryAsync()).Select(c=>c.Id.ToString());
            var allComplaintIds = (await ReadDb.QueryTable<MMPostReport>().Where(e => e.ComplainantUserId == userId && e.Memo != null && e.Memo != "" && e.Status == 2).QueryAsync()).Select(c=>c.ReportId);
           
            ///根据用户ID获取全部消息ID集合
            var totalMessageIds = (await ReadDb.QueryTable<MMUserToMessageOperation>()
                .Where(c => c.UserId == userId).QueryAsync())
                .Select(c => new { MessageId = c.MessageId, MessageType = c.MessageType,IsRed=c.IsRead,IsDelete=c.IsDelete });


            var redAnnouncementIds = totalMessageIds
                                         .Where(c => c.MessageType == MessageType.Announcement && c.MessageId != "")
                                         .ToList()
                                         .Select(c=>c.MessageId);
            var redComplaintIds = totalMessageIds
                                         .Where(c => c.MessageType == MessageType.ComplaintPost && c.MessageId != "")
                                         .ToList()
                                         .Select(c => c.MessageId);

            var deleteComplaintIds = totalMessageIds
                             .Where(c => c.MessageType == MessageType.ComplaintPost && c.MessageId != "" && c.IsDelete)
                             .ToList()
                             .Select(c => c.MessageId);

            ///获取未读
            int announcementUnreadCount = allAnnouncementIds.Where(c=> !redAnnouncementIds.Contains(c)).Count();
            int complaintUnreadCount = allComplaintIds.Where(c => !redComplaintIds.Contains(c)).Count();

            //获取已读
            int announcementReadCount = allAnnouncementIds.Where(c => redAnnouncementIds.Contains(c)).Count();
            int complaintReadCount = allComplaintIds.Where(c => redComplaintIds.Contains(c)).Count();

            ////获取投诉是否被删除的消息数
            //int complanintDeleteCount = allComplaintIds.Where(c => deleteComplaintIds.Contains(c)).Count();

            //获取全部的未读消息数
            int total = (announcementUnreadCount + complaintUnreadCount);

            return new UserUnreadMessage()
            {
                //未读
                AnnouncementUnreadCount = announcementUnreadCount < 0 ? 0 : announcementUnreadCount,
                ComplaintUnreadCount = complaintUnreadCount < 0 ? 0 : complaintUnreadCount,

                //已读
                AnnouncementReadCount = announcementReadCount < 0 ? 0 : announcementReadCount,
                ComplaintReadCount= complaintReadCount<0?0: complaintReadCount,

                //所有
                AnnouncementCount= allAnnouncementIds.Count(),
                ComplaintCount= allComplaintIds.Except(deleteComplaintIds).Count(),

                TotalUnreadCount = total < 0 ? 0 : total,
            };

        }


        public Task<MMHomeAnnouncement> GetAnnouncementById(int id)
        {
            return ReadDb.QueryTable<MMHomeAnnouncement>().Where(c=>c.Id==id).QueryFirstAsync();
        }

    }
}