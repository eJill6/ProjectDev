using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Chat
{
    public interface IMSIMLastMessageInfoRep : IBaseDbRepository<MSIMLastMessageInfo>
    {
        BaseReturnModel ClearUnreadCount(int ownerUserId, string roomId);

        List<MSIMLastMessageInfo> GetMSIMLastMessageInfos(QueryLastMessagesParam queryLastMessagesParam, int fetchCount);

        bool HasUnreadMessage(int ownerUserId);

        void DeleteLastMessages(string deleteLastMessageKeysJson);

        List<MSIMLastMessageKey> GetLastMessageKeys(string messageIDsJson);
    }
}