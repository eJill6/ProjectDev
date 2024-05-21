using MS.Core.MM.Models.Chat;
using MS.Core.MMModel.Models.Chat;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IMSIMOneOnOneChatMessageRepo
    {
        Task<PageResultModel<MSIMOneOnOneChatMessage>> GetRoomMessages(int ownerUserId, QueryRoomMessageParam queryMessageParam, int fetchCount);
    }
}
