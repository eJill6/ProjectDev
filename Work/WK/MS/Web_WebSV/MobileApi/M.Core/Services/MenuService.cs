using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game.Menu;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Menu;
using M.Core.Interface.Services;
using M.Core.Models;

namespace M.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly Lazy<ILotteryService> _lotteryService;

        private readonly Lazy<ILotterySpaService> _spaService;

        private readonly Lazy<IHomeControllerService> _homeControllerService;

        public MenuService()
        {
            _lotteryService = DependencyUtil.ResolveService<ILotteryService>();
            _spaService = DependencyUtil.ResolveService<ILotterySpaService>();
            _homeControllerService = DependencyUtil.ResolveService<IHomeControllerService>();
        }

        public IEnumerable<LotteryInfoResponse> GetLotteryMenus()
        {
            List<LiveGameManage> liveGameManageInfos = _lotteryService.Value.GetLiveGameManageInfos().ToList();
            RemoveByInactiveLotteryInfos(liveGameManageInfos);
            liveGameManageInfos.RemoveAll(r => r.LotteryId == 0);

            return liveGameManageInfos
                .Select(l => ConvertToLotteryInfoResponse(l));
        }

        public List<LiveGameTypeAndMenu> GetLiveGameTypeAndMenus()
        {
            List<LiveGameManage> liveGameManages = _lotteryService.Value.GetLiveGameManageInfos().ToList();
            RemoveByInactiveLotteryInfos(liveGameManages);
            Dictionary<string, MobileApiProductMenu> productMenuMap = GetKeyMapAndRemoveByProductMenus(liveGameManages);

            var settingMap = new Dictionary<FrontsideMenuTypeSetting, List<LiveGameMenuViewModel>>();

            foreach (LiveGameManage liveGameManage in liveGameManages)
            {
                LiveGameMenuViewModel liveGameMenuViewModel;

                if (liveGameManage.LotteryId > 0)
                {
                    liveGameMenuViewModel = new LiveGameMenuViewModel()
                    {
                        LotteryInfo = ConvertToLotteryInfoResponse(liveGameManage)
                    };

                    liveGameMenuViewModel.SetMenuSource(LiveMenuSource.LotteryInfo);
                }
                else
                {
                    //判斷對應的遊戲開關是否有開啟
                    string mapKey = CreateHomeMenuKey(liveGameManage.ProductCode, liveGameManage.GameCode);

                    if (!productMenuMap.TryGetValue(mapKey, out MobileApiProductMenu mobileApiProductMenu))
                    {
                        continue;
                    }

                    var mobileApiProductAESMenu = new MobileApiProductAESMenu()
                    {
                        Title = mobileApiProductMenu.Title,
                        ProductCode = liveGameManage.ProductCode,
                        GameCode = liveGameManage.GameCode,
                        RemoteCode = liveGameManage.RemoteCode,
                        AESFullImageUrl = liveGameManage.ImageUrl
                    };

                    if (liveGameManage.RemoteCode.IsNullOrEmpty())
                    {
                        mobileApiProductAESMenu.RemoteCode = mobileApiProductMenu.RemoteCode;
                        mobileApiProductAESMenu.GameLobbyTypeValue = mobileApiProductMenu.GameLobbyTypeValue;
                    }

                    liveGameMenuViewModel = new LiveGameMenuViewModel()
                    {
                        MobileApiProductMenu = mobileApiProductAESMenu
                    };

                    liveGameMenuViewModel.SetMenuSource(LiveMenuSource.GameCenter);
                }

                //加上分類資料與排序
                FrontsideMenuTypeSetting frontsideMenuTypeSetting = FrontsideMenuTypeSetting.GetSingle(liveGameManage.TabType);

                if (frontsideMenuTypeSetting == null)
                {
                    continue;
                }

                if (!settingMap.TryGetValue(frontsideMenuTypeSetting, out List<LiveGameMenuViewModel> liveGameMenus))
                {
                    liveGameMenus = new List<LiveGameMenuViewModel>();
                    settingMap.Add(frontsideMenuTypeSetting, liveGameMenus);
                }

                liveGameMenus.Add(liveGameMenuViewModel);
            }

            List<LiveGameTypeAndMenu> liveGameTypeAndMenus = settingMap.OrderBy(o => o.Key.Sort)
                .Select(s => new LiveGameTypeAndMenu()
                {
                    MenuType = new BasicMenuType()
                    {
                        MenuTypeValue = s.Key.Value,
                        MenuTypeName = s.Key.Name
                    },
                    LiveGameMenuViewModels = s.Value
                })
                .ToList();

            return liveGameTypeAndMenus;
        }

        private void RemoveByInactiveLotteryInfos(List<LiveGameManage> liveGameManageInfos)
        {
            HashSet<int> lotteryIdSet = _spaService.Value.GetAllLotteryInfo().Select(s => s.LotteryID).ToHashSet();
            liveGameManageInfos.RemoveAll(r => r.LotteryId > 0 && !lotteryIdSet.Contains(r.LotteryId));
        }

        private Dictionary<string, MobileApiProductMenu> GetKeyMapAndRemoveByProductMenus(List<LiveGameManage> liveGameManageInfos)
        {
            List<MobileApiProductMenu> activeMobileApiProductMenus = GetActiveMobileApiProductMenus();
            Dictionary<string, MobileApiProductMenu> productMenuMap = activeMobileApiProductMenus.ToDictionary(d => CreateHomeMenuKey(d.ProductCode, d.GameCode));

            liveGameManageInfos.RemoveAll(r => r.LotteryId == 0 && !productMenuMap.ContainsKey(CreateHomeMenuKey(r.ProductCode, r.GameCode)));

            return productMenuMap;
        }

        private List<MobileApiProductMenu> GetActiveMobileApiProductMenus()
        {
            var allActiveMobileApiProductMenus = new List<MobileApiProductMenu>();
            MobileApiGameCenterViewModel mobileApiGameCenterViewModel = _homeControllerService.Value.GetMobileApiGameLobbyMenu(isUseRequestHost: true, isForceRefresh: false);
            var nonHotMenuTypeViewModel = mobileApiGameCenterViewModel.MobileApiMenuTypeViewModels.Where(w => w.MenuTypeValue != FrontsideMenuTypeSetting.Hot);

            foreach (MobileApiMenuTypeViewModel? menuTypeViewModel in nonHotMenuTypeViewModel)
            {
                IEnumerable<MobileApiProductMenu> activeMobileApiProductMenus = menuTypeViewModel.MobileApiProductMenus.Where(w => !w.IsMaintaining);
                allActiveMobileApiProductMenus.AddRange(activeMobileApiProductMenus);
            }

            return allActiveMobileApiProductMenus;
        }

        private string CreateHomeMenuKey(string productCode, string gameCode) => $"{productCode}:{gameCode}";

        private LotteryInfoResponse ConvertToLotteryInfoResponse(LiveGameManage liveGameManage)
        {
            return new LotteryInfoResponse()
            {
                LotteryId = liveGameManage.LotteryId,
                ImageUrl = liveGameManage.ImageUrl
            };
        }
    }
}