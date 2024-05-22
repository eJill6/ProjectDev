using ControllerShareLib.Models.Game.Menu;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Menu;

namespace M.Core.Models
{
    public class LiveGameTypeAndMenu
    {
        public BasicMenuType MenuType { get; set; }

        public List<LiveGameMenuViewModel> LiveGameMenuViewModels { get; set; }
    }


    public class LiveGameMenuViewModel
    {
        /// <summary>直播遊戲選單類型 1:原生彩票, 2: 遊戲大廳選單</summary>
        public int MenuSource { get; private set; }

        public LotteryInfoResponse? LotteryInfo { get; set; }

        public MobileApiProductAESMenu? MobileApiProductMenu { get; set; }

        public void SetMenuSource(LiveMenuSource menuSource) => MenuSource = menuSource.Value;
    }

    public class LiveMenuSource : BaseIntValueModel<LiveMenuSource>
    {
        private LiveMenuSource()
        { }

        public static readonly LiveMenuSource LotteryInfo = new LiveMenuSource() { Value = 1 };

        public static readonly LiveMenuSource GameCenter = new LiveMenuSource() { Value = 2 };
    }
}