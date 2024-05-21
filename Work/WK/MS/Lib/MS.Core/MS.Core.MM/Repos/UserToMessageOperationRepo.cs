using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.MessageUserRead;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Repos
{
    public class UserToMessageOperationRepo : BaseInlodbRepository<MMUserToMessageOperation>, IUserToMessageOperationRepo
    {
        public UserToMessageOperationRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }
        /// <summary>
        /// 根据用户ID获取用户的消息操作记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<MMUserToMessageOperation>> GetMessageListByUserId(int? userId)
        {
            var component = WriteDb.QueryTable<MMUserToMessageOperation>();
            if(userId.HasValue)
            {
                component.Where(c => c.UserId == userId);
            }
            return component.QueryAsync();
        }

        public async Task<int> GetMessageCount(int userId, MessageType type)
        {
            var component = ReadDb.QueryTable<MMUserToMessageOperation>();
            return await component.Where(c => c.UserId == userId && c.MessageType==type).CountAsync();
        }

        /// <summary>
        /// 用户记录新增
        /// </summary>
        /// <param name="operations"></param>
        /// <returns></returns>
        public async Task<bool> InsertMessage(IEnumerable<MMUserToMessageOperation> operations)
        {
            try
            {
                await WriteDb.Insert<MMUserToMessageOperation>(operations).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增用户消息记录出错");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 新增修改用户的消息记录
        /// </summary>
        /// <param name="operations"></param>
        /// <returns></returns>
        public async Task<bool> InsertAndUpdateMessage(IEnumerable<MMUserToMessageOperation> InsertOperations, IEnumerable<MMUserToMessageOperation> UpdateOperations)
        {
            try
            {
                await WriteDb.Insert(InsertOperations).Update<MMUserToMessageOperation>(UpdateOperations).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "更新用户消息记录出错");
                return false;
            }
            return true;
        }

    }
}
