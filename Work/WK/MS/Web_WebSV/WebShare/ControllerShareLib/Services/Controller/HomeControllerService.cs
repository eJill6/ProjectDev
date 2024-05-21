using ControllerShareLib.Helpers;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game.Menu;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Menu;
using SLPolyGame.Web.Interface;

namespace ControllerShareLib.Service.Controller
{
    public class HomeControllerService : IHomeControllerService
    {
        private static readonly string s_lobbyIconPrefixPath = "/images/gamelobby";

        private static readonly string s_innerIconPrefixPath = "/images/inner";

        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public HomeControllerService()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
            var environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        public WebGameCenterViewModel GetWebGameLobbyMenu(bool isUseRequestHost)
        {
            WebGameCenterViewModel webGameCenterViewModel = _slPolyGameWebSVService.Value.GetWebGameCenterViewModel().GetAwaiterAndResult();

            foreach (WebMenuTypeViewModel webMenuTypeViewModel in webGameCenterViewModel.WebMenuTypeViewModels)
            {
                webMenuTypeViewModel.IconUrl = WebResourceHelper.Content(
                    resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_lobbyIconPrefixPath}/{webMenuTypeViewModel.IconFileName}",
                    isAppendVersion: true,
                    isUseRequestHost);

                webMenuTypeViewModel.AESIconUrl = WebResourceHelper.Content(
                    resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_lobbyIconPrefixPath}/{webMenuTypeViewModel.AESIconFileName}",
                    isAppendVersion: true,
                    isUseRequestHost);
            }

            return webGameCenterViewModel;
        }

        public MobileApiGameCenterViewModel GetMobileApiGameLobbyMenu(bool isUseRequestHost, bool isForceRefresh)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.MobileApiGameLobbyMenu,
                IsForceRefresh = isForceRefresh
            };

            MobileApiGameCenterViewModel mobileApiGameCenterViewModel = _jxCacheService.Value.GetCache(
                searchCacheParam,
                () => CreateMobileApiGameCenterViewModel(isUseRequestHost));

            return mobileApiGameCenterViewModel;
        }

        public List<MobileApiMenuInnerInfo> GetMobileApiMenuInnerInfos(bool isUseRequestHost)
        {
            List<MenuInnerInfo> menuInnerInfos = _slPolyGameWebSVService.Value.GetMenuInnerInfos().ConfigureAwait(false).GetAwaiter().GetResult();
            var mobileApiMenuInnerInfos = new List<MobileApiMenuInnerInfo>();

            foreach (MenuInnerInfo menuInnerInfo in menuInnerInfos)
            {
                var mobileApiMenuInnerInfo = menuInnerInfo.CastByJson<MobileApiMenuInnerInfo>();

                mobileApiMenuInnerInfo.FullDefaultImageUrl = WebResourceHelper.Content(
                    resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_innerIconPrefixPath}/{menuInnerInfo.IconInfo.DefaultImageName}",
                    isAppendVersion: true,
                    isUseRequestHost);

                mobileApiMenuInnerInfo.AESFullDefaultImageUrl = WebResourceHelper.Content(
                    resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_innerIconPrefixPath}/{menuInnerInfo.IconInfo.AESDefaultImageName}",
                    isAppendVersion: true,
                    isUseRequestHost);

                mobileApiMenuInnerInfo.FullFocusImageUrl = WebResourceHelper.Content(
                    resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_innerIconPrefixPath}/{menuInnerInfo.IconInfo.FocusImageName}",
                    isAppendVersion: true,
                    isUseRequestHost);

                mobileApiMenuInnerInfo.AESFullFocusImageUrl = WebResourceHelper.Content(
                    resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_innerIconPrefixPath}/{menuInnerInfo.IconInfo.AESFocusImageName}",
                    isAppendVersion: true,
                    isUseRequestHost);

                mobileApiMenuInnerInfos.Add(mobileApiMenuInnerInfo);
            }

            return mobileApiMenuInnerInfos;
        }

        private MobileApiGameCenterViewModel CreateMobileApiGameCenterViewModel(bool isUseRequestHost)
        {
            var mobileApiGameCenterViewModel = new MobileApiGameCenterViewModel()
            {
                MobileApiMenuTypeViewModels = new List<MobileApiMenuTypeViewModel>()
            };

            WebGameCenterViewModel webGameCenterViewModel = GetWebGameLobbyMenu(isUseRequestHost);

            foreach (WebMenuTypeViewModel viewModel in webGameCenterViewModel.WebMenuTypeViewModels)
            {
                var mobileApiMenuViewModel = new MobileApiMenuTypeViewModel()
                {
                    MenuTypeValue = viewModel.MenuTypeValue,
                    MenuTypeName = viewModel.MenuTypeName,
                    ColsInRow = viewModel.ColsInRow,
                    IconUrl = viewModel.IconUrl,
                    AESIconUrl = viewModel.AESIconUrl,
                };

                mobileApiGameCenterViewModel.MobileApiMenuTypeViewModels.Add(mobileApiMenuViewModel);

                foreach (FrontsideProductMenu frontsideProductMenu in viewModel.FrontsideProductMenus)
                {
                    var mobileApiProductMenu = frontsideProductMenu.CastByJson<MobileApiProductMenu>();

                    if (mobileApiProductMenu.FullImageUrl.IsNullOrEmpty() && !frontsideProductMenu.CardImageName.IsNullOrEmpty())
                    {
                        mobileApiProductMenu.FullImageUrl = WebResourceHelper.Content(
                            resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_lobbyIconPrefixPath}/{frontsideProductMenu.CardImageName}",
                            isAppendVersion: true,
                            isUseRequestHost);

                        mobileApiProductMenu.AESFullImageUrl = WebResourceHelper.Content(
                            resourceUrl: $"{GlobalCacheHelper.RazorShareContentPath}{s_lobbyIconPrefixPath}/{frontsideProductMenu.AESCardImageName}",
                            isAppendVersion: true,
                            isUseRequestHost);
                    }

                    mobileApiMenuViewModel.MobileApiProductMenus.Add(mobileApiProductMenu);
                }
            }

            return mobileApiGameCenterViewModel;
        }
    }
}