using JxBackendService.Model.Enums;

namespace ControllerShareLib.Models.Game.GameLobby
{
    public class GameTabTypeValue : BaseIntValueModel<GameTabTypeValue>
    {
        public static readonly GameTabTypeValue All = new GameTabTypeValue { Value = 1, };

        public static readonly GameTabTypeValue Hot = new GameTabTypeValue { Value = 2, };

        public static readonly GameTabTypeValue Favorite = new GameTabTypeValue { Value = 3, };
    }
}