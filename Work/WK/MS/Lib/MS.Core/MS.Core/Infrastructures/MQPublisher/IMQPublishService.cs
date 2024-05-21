using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.Infrastructures.MQPublisher
{
    public interface IMQPublishService
    {
        /// <summary>
        /// 删除房间
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="roomId">房间id（对方用户id）</param>
        /// <returns></returns>
        Task DeleteChatMessages(int userId, int roomId);
    }
}
