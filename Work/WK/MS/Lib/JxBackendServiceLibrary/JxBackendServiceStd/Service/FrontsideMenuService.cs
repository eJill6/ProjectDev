using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.Menu;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service
{
    public class FrontsideMenuService : BaseService, IFrontsideMenuService
    {
        private readonly Lazy<IFrontsideMenuRep> _frontsideMenuRep;

        private readonly Lazy<IFrontsideMenuTypeService> _frontsideMenuTypeService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private readonly Lazy<IHttpWebRequestUtilService> _httpWebRequestUtilService;

        public FrontsideMenuService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
            _frontsideMenuTypeService = ResolveJxBackendService<IFrontsideMenuTypeService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _httpWebRequestUtilService = ResolveJxBackendService<IHttpWebRequestUtilService>();
        }

        public List<FrontsideMenu> GetActiveFrontsideMenus(bool IsForceRefresh = false)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.ActiveFrontsideMenu,
                IsForceRefresh = IsForceRefresh
            };

            return _jxCacheService.Value.GetCache(searchCacheParam, () =>
            {
                return _frontsideMenuRep.Value.GetActiveFrontsideMenu();
            });
        }

        public List<MenuInnerInfo> GetMenuInnerInfos()
        {
            WebGameCenterViewModel viewModel = GetWebGameCenterViewModel();
            var frontsideProductMenus = new List<FrontsideProductMenu>();

            foreach (WebMenuTypeViewModel menuTypeViewModel in viewModel.WebMenuTypeViewModels)
            {
                foreach (FrontsideProductMenu frontsideProductMenu in menuTypeViewModel.FrontsideProductMenus)
                {
                    if (frontsideProductMenu.MenuInnerIcon != null)
                    {
                        frontsideProductMenus.Add(frontsideProductMenu);
                    }
                }
            }

            List<MenuInnerInfo> menuInnerIcons = frontsideProductMenus
                .Select(s =>
                {
                    string remoteCode = s.RemoteCode;
                    var thirdPartySubGameCode = ThirdPartySubGameCodes.GetSingle(s.GameCode);

                    if (thirdPartySubGameCode != null)
                    {
                        remoteCode = thirdPartySubGameCode.RemoteGameCode;
                    }

                    var menuInnerInfo = new MenuInnerInfo()
                    {
                        ProductCode = s.ProductCode,
                        GameCode = s.GameCode,
                        RemoteCode = remoteCode,
                        IsMaintaining = s.IsMaintaining,
                        IconInfo = s.MenuInnerIcon,
                        Title = s.Title
                    };

                    return menuInnerInfo;
                })
                .OrderBy(o =>
                {
                    var thirdPartySubGameCode = ThirdPartySubGameCodes.GetSingle(o.GameCode);

                    if (thirdPartySubGameCode == null)
                    {
                        return int.MaxValue;
                    }

                    return thirdPartySubGameCode.Sort;
                })
                .ToList();

            return menuInnerIcons;
        }

        public WebGameCenterViewModel GetWebGameCenterViewModel()
        {
            return CreateWebGameCenterViewModel();
        }

        private WebGameCenterViewModel CreateWebGameCenterViewModel()
        {
            var webGameCenterViewModel = new WebGameCenterViewModel();
            List<FrontsideMenu> frontsideMenus = _frontsideMenuRep.Value.GetAll().OrderBy(o => o.Sort).ToList();
            var frontsideMenuMap = new Dictionary<int, List<FrontsideMenu>>();

            //先把資料分組
            foreach (FrontsideMenu frontsideMenu in frontsideMenus)
            {
                frontsideMenuMap.AddNX(frontsideMenu.Type, new List<FrontsideMenu>());
                List<FrontsideMenu> menus = frontsideMenuMap[frontsideMenu.Type];
                menus.Add(frontsideMenu);
            }

            IEnumerable<FrontsideMenuTypeSetting> frontsideMenuTypeSettings = _frontsideMenuTypeService.Value
                .GetAll()
                .Where(w => frontsideMenuMap.ContainsKey(w.Value));

            foreach (FrontsideMenuTypeSetting frontsideMenuTypeSetting in frontsideMenuTypeSettings)
            {
                string iconFileName = frontsideMenuTypeSetting.IconFileName;
                string aesIconFileName = frontsideMenuTypeSetting.IconFileName.ConvertToAESExtension();

                var webMenuTypeViewModel = new WebMenuTypeViewModel()
                {
                    MenuTypeValue = frontsideMenuTypeSetting.Value,
                    MenuTypeName = frontsideMenuTypeSetting.Name,
                    ColsInRow = frontsideMenuTypeSetting.ColsInRow,
                    IconFileName = iconFileName,
                    AESIconFileName = aesIconFileName,
                    CardOutCssClass = frontsideMenuTypeSetting.CardOutCssClass,
                    MaintainanceCssClass = frontsideMenuTypeSetting.MaintainanceCssClass,
                };

                List<FrontsideMenu> allMenus = frontsideMenuMap[webMenuTypeViewModel.MenuTypeValue];
                var notFoundMenus = new List<FrontsideMenu>();
                List<GameLobbyType> allGameLobbyTypes = GameLobbyType.GetAll();

                foreach (FrontsideMenu menu in allMenus)
                {
                    FrontsideProductMenu frontsideProductMenu;

                    if (frontsideMenuTypeSetting.IsThirdPartySubGame)
                    {
                        if (!menu.IsActive)
                        {
                            continue;
                        }

                        //熱門遊戲對應的ProductCode遊戲
                        FrontsideMenu productCodeMenu = frontsideMenus.FirstOrDefault(f =>
                            f.RemoteCode.IsNullOrEmpty() &&
                            f.ProductCode == menu.ProductCode &&
                            f.GameCode == menu.GameCode);

                        if (productCodeMenu != null && !productCodeMenu.IsActive)
                        {
                            continue;
                        }

                        string imageUrl = menu.ImageUrl;
                        string aesImageUrl = menu.ImageUrl.ConvertToAESExtension();

                        frontsideProductMenu = new FrontsideProductMenu()
                        {
                            ProductCode = menu.ProductCode,
                            GameCode = menu.GameCode,
                            RemoteCode = menu.RemoteCode,
                            Title = menu.MenuName,
                            FullImageUrl = _httpWebRequestUtilService.Value.CombineUrl(SharedAppSettings.BucketCdnDomain, imageUrl),
                            AESFullImageUrl = _httpWebRequestUtilService.Value.CombineUrl(SharedAppSettings.BucketAESCdnDomain, aesImageUrl),
                            GameLobbyTypeValue = null,
                        };
                    }
                    else
                    {
                        FrontsideMenuSetting setting = GetFrontsideMenuSetting(menu);

                        if (setting == null)
                        {
                            notFoundMenus.Add(menu);

                            continue;
                        }

                        PlatformProduct product = setting.Product;

                        string url = menu.Url;
                        string gameLobbyTypeValue = null;

                        if (menu.Url.IsNullOrEmpty())
                        {
                            url = $"/GameCenter/Index?productCode={product.Value}";
                        }
                        else
                        {
                            GameLobbyType gameLobbyType = allGameLobbyTypes.SingleOrDefault(w => w.Product == product && w.SubGameCode == menu.GameCode);

                            if (gameLobbyType != null)
                            {
                                gameLobbyTypeValue = gameLobbyType.Value;
                            }
                        }

                        string cardImageName = setting.CardImageName;
                        string aesCardImageName = setting.CardImageName.ConvertToAESExtension();

                        frontsideProductMenu = new FrontsideProductMenu()
                        {
                            ProductCode = product.Value,
                            GameLobbyUrl = url,
                            Title = setting.Name,
                            GameLobbyTypeValue = gameLobbyTypeValue,
                            GameCode = menu.GameCode,
                            CardCssClass = setting.CardCssClass,
                            CardImageName = cardImageName,
                            AESCardImageName = aesCardImageName,
                            MenuInnerIcon = setting.MenuInnerIcon,
                            IsHideHeaderWithFullScreen = product.ProductType.IsHideHeaderWithFullScreen
                        };

                        //App希望能帶出原生彩票RemoteCode，這邊利用enum帶出
                        ThirdPartySubGameCodes thirdPartySubGameCode = ThirdPartySubGameCodes.GetSingle(menu.GameCode);

                        if (thirdPartySubGameCode != null && !thirdPartySubGameCode.RemoteGameCode.IsNullOrEmpty())
                        {
                            frontsideProductMenu.RemoteCode = thirdPartySubGameCode.RemoteGameCode;
                        }
                    }

                    frontsideProductMenu.IsMaintaining = !menu.IsActive;
                    webMenuTypeViewModel.FrontsideProductMenus.Add(frontsideProductMenu);
                }

                if (notFoundMenus.Any())
                {
                    ErrorMsgUtil.ErrorHandle(
                        new InvalidProgramException("请确认PlatformProduct、FrontsideMenuSetting设定, " +
                            $"{notFoundMenus.Select(s => new { s.ProductCode, s.GameCode }).ToJsonString()} ")
                        , EnvLoginUser);
                }

                webGameCenterViewModel.WebMenuTypeViewModels.Add(webMenuTypeViewModel);
            }

            return webGameCenterViewModel;
        }

        private FrontsideMenuSetting GetFrontsideMenuSetting(FrontsideMenu menu)
        {
            ThirdPartySubGameCodes thirdPartySubGameCode = ThirdPartySubGameCodes.GetSingle(menu.GameCode);

            PlatformProduct product = _platformProductService.Value.GetSingle(menu.ProductCode);
            FrontsideMenuSetting setting = null;

            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                if (product != null)
                {
                    setting = FrontsideMenuSetting.GetSingle(product, thirdPartySubGameCode);
                }
            });

            return setting;
        }

        public PagedResultModel<QueryFrontsideMenuModel> GetPagedFrontsideMenu(QueryFrontsideMenuParam queryParam)
        {
            PagedResultModel<FrontsideMenu> datas = _frontsideMenuRep.Value.GetPagedFrontsideMenu(queryParam);
            var models = datas.CastByJson<PagedResultModel<QueryFrontsideMenuModel>>();

            return models;
        }

        public List<GameCenterManageDetail> GetAllByProduct(PlatformProduct product)
        {
            if (product == null)
            {
                return new List<GameCenterManageDetail>();
            }

            List<FrontsideMenu> menus = _frontsideMenuRep.Value.GetGameCenterMenusByProduct(product.Value);

            return ConvertToGameCenterManageModel<GameCenterManageDetail>(menus);
        }

        public List<GameCenterManageModel> GetModelsByType(FrontsideMenuTypeSetting frontsideMenuTypeSetting)
        {
            List<FrontsideMenu> menus = _frontsideMenuRep.Value.GetAllByType(frontsideMenuTypeSetting.Value);

            return ConvertToGameCenterManageModel<GameCenterManageModel>(menus);
        }

        public string GetNameFromSetting(FrontsideMenu menu)
        {
            FrontsideMenuSetting setting = GetFrontsideMenuSetting(menu);

            if (setting != null)
            {
                return setting.Name;
            }

            return menu.MenuName;
        }

        public List<JxBackendSelectListItem> GetTypes()
        {
            return FrontsideMenuTypeSetting.GetSelectListItems();
        }

        public void ForceRefreshFrontsideMenus()
        {
            GetActiveFrontsideMenus(IsForceRefresh: true);
            _jxCacheService.Value.RemoveCache(CacheKey.MobileApiGameLobbyMenu);
        }

        private List<T> ConvertToGameCenterManageModel<T>(List<FrontsideMenu> menus) where T : GameCenterManageModel, new()
        {
            return menus.Select(menu =>
            {
                var model = new T()
                {
                    No = menu.No,
                    Sort = menu.Sort,
                    IsActive = menu.IsActive,
                    MenuName = GetNameFromSetting(menu)
                };

                if (model is GameCenterManageDetail)
                {
                    var liveSubGameMenu = model as GameCenterManageDetail;
                    liveSubGameMenu.ProductCode = menu.ProductCode;
                    liveSubGameMenu.GameCode = menu.GameCode;
                    liveSubGameMenu.RemoteCode = menu.RemoteCode;
                }

                return model;
            }).ToList();
        }
    }
}