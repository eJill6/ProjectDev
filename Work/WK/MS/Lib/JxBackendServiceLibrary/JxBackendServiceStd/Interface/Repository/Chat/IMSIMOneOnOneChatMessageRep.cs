using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Chat;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Chat
{
    public interface IMSIMOneOnOneChatMessageRep : IBaseDbRepository<MSIMOneOnOneChatMessage>
    {
        List<MSIMOneOnOneChatMessage> GetRoomMessages(int ownerUserId, QueryRoomMessageParam queryMessageParam, int fetchCount);

        BaseReturnModel SaveOneOnOneChatMessage(ProSaveOneOnOneChatMessageParam proSaveOneOnOneChatMessageParam);

        void DeleteChatMessages(string deleteChatMessageKeysJson);

        List<MSIMOneOnOneChatMessageKey> GetOneOnOneChatMessageKeys(QueryOneOnOneMessageParam queryParam, int fetchCount);
    }
}