using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.MiseOrder;

namespace SLPolyGame.Web.MSSeal.Models.Interface
{
    public interface IGameIdInfo
    {
        string Type { get; set; }

        string SubType { get; set; }

        string GameId { get; set; }

        string GameName { get; set; }
    }

    public static class GameIdInfoExtensions
    {
        public static void SetByGameId(this IGameIdInfo gameIdInfo, MiseOrderGameId miseOrderGameId)
        {
            gameIdInfo.Type = miseOrderGameId.OrderSubType.OrderType.Value;
            gameIdInfo.SubType = miseOrderGameId.OrderSubType.Value;
            gameIdInfo.GameId = miseOrderGameId.Value;

            if (gameIdInfo.GameName.IsNullOrEmpty())
            {
                gameIdInfo.GameName = miseOrderGameId.Name;
            }
        }
    }
}