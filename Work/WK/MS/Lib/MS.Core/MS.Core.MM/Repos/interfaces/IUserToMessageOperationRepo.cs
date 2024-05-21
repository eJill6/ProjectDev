using MS.Core.MM.Models.Entities.MessageUserRead;
using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IUserToMessageOperationRepo
    {
        Task<IEnumerable<MMUserToMessageOperation>> GetMessageListByUserId(int? userId);
        Task<int> GetMessageCount(int userId, MessageType type);
        /// <summary>
        /// 新增用户已读消息记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<bool> InsertMessage(IEnumerable<MMUserToMessageOperation> operations);
        /// <summary>
        /// 新增修改用户消息记录状态为已读
        /// </summary>
        /// <param name="operations"></param>
        /// <returns></returns>
        Task<bool> InsertAndUpdateMessage(IEnumerable<MMUserToMessageOperation> InsertOperations, IEnumerable<MMUserToMessageOperation> UpdateOperations);
    }
}
