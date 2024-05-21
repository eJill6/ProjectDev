using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Chat;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Chat;
using MS.Core.MMModel.Models.Chat.Enum;
using MS.Core.Models.Models;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class MSIMOneOnOneChatMessageRepo : BaseInlodbRepository<MSIMOneOnOneChatMessage>, IMSIMOneOnOneChatMessageRepo
    {
        public MSIMOneOnOneChatMessageRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger, IMediaRepo media) : base(setting, provider, logger)
        {
        }

        public async Task<PageResultModel<MSIMOneOnOneChatMessage>> GetRoomMessages(int ownerUserId, QueryRoomMessageParam queryMessageParam, int fetchCount)
        {
            if (!queryMessageParam.LastMessageID.HasValue)
            {
                queryMessageParam.LastMessageID = long.MaxValue;
                queryMessageParam.SearchDirectionTypeValue = (int)SearchDirectionType.Backward;
            }

            var roomId = Convert.ToInt32(queryMessageParam.RoomID);
            var component = WriteDb.QueryTable<MSIMOneOnOneChatMessage>()
                .Where(x => x.OwnerUserID == ownerUserId
                        && x.DialogueUserID == roomId);

            if (queryMessageParam.SearchDirectionTypeValue == (int)SearchDirectionType.Forward)
            {
                component.Where(x => x.MessageID > queryMessageParam.LastMessageID);
                component.OrderBy(x => x.MessageID);
            }
            else
            {
                component.Where(x => x.MessageID < queryMessageParam.LastMessageID);
                component.OrderByDescending(x => x.MessageID);
            }

            return await component.QueryPageResultAsync(new PaginationModel()
            {
                PageSize = fetchCount,
            });
        }
    }
}
