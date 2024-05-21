using Microsoft.Extensions.Logging;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Chat;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Chat;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Services;

namespace MS.Core.MM.Repos
{
    public class MSIMOneOnOneChatMessageService : BaseService, IMSIMOneOnOneChatMessageService
    {
        private readonly IMSIMOneOnOneChatMessageRepo _repo = null;
        private readonly IMediaService _media = null;
        public MSIMOneOnOneChatMessageService(IEnumerable<IMediaService> medias,
            IMSIMOneOnOneChatMessageRepo repo,
            ILogger logger) : base(logger)
        {
            _repo = repo;
            _media = medias.FirstOrDefault(x => x.SourceType == SourceType.PrivateMessage
                && x.Type == MMModel.Models.Media.Enums.MediaType.Image);
        }

        public async Task<MSIMOneOnOneChatMessage[]> GetRoomMessages(int ownerUserId, QueryRoomMessageParam queryMessageParam, int fetchCount)
        {
            var result = await _repo.GetRoomMessages(ownerUserId, queryMessageParam, fetchCount);
            var ids = result.Data.Where(x => x.MessageType == (int)MMModel.Models.Chat.Enum.MessageType.Image).Select(x => x.Message).ToArray();
            var medias = new MediaInfo[0];
            if (ids.Any())
            {
                medias = (await _media.GetByIds(SourceType.PrivateMessage, ids)).DataModel;
            }
            return result.Data.Select(x =>
            {
                if (x.MessageType == (int)MMModel.Models.Chat.Enum.MessageType.Image)
                {
                    var image = medias.FirstOrDefault(i => i.Id == x.Message);
                    var url = string.Empty;
                    if (image != null)
                    {
                        url = _media.GetFullMediaUrl(
                                        new MMMedia()
                                        {
                                            FileUrl = image.FileUrl
                                        },
                                        postType: PostType.Square,
                                        isThumbnail: true).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                    x.Message = url;
                }
                return x;
            }).OrderBy(x => x.MessageID).ToArray();
        }
    }
}
