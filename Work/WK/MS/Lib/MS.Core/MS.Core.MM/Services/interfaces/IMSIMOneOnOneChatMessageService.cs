using MS.Core.MM.Models.Chat;
using MS.Core.MMModel.Models.Chat;

namespace MS.Core.MM.Services.interfaces
{
    public interface IMSIMOneOnOneChatMessageService
    {
        Task<MSIMOneOnOneChatMessage[]> GetRoomMessages(int ownerUserId, QueryRoomMessageParam queryMessageParam, int fetchCount);
    }
}
