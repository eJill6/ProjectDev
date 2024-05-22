using M.Core.Models;

namespace M.Core.Interface.Services
{
    public interface IMenuService
    {
        List<LiveGameTypeAndMenu> GetLiveGameTypeAndMenus();

        IEnumerable<LotteryInfoResponse> GetLotteryMenus();
    }
}