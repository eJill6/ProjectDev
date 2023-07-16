using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
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
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service
{
    public class FrontsideMenuService : BaseService, IFrontsideMenuService
    {
        private readonly IFrontsideMenuRep _frontsideMenuRep;

        private readonly IFrontsideMenuTypeService _frontsideMenuTypeSettingService;

        private readonly IJxCacheService _jxCacheService;

        private readonly IPlatformProductService _platformProductService;

        private readonly IHttpWebRequestUtilService _httpWebRequestUtilService;

        public FrontsideMenuService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
            _frontsideMenuTypeSettingService = ResolveJxBackendService<IFrontsideMenuTypeService>();
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _httpWebRequestUtilService = ResolveJxBackendService<IHttpWebRequestUtilService>();
        }

        public List<FrontsideMenu> GetActiveFrontsideMenus()
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.ActiveFrontsideMenu,
            };

            return _jxCacheService.GetCache(searchCacheParam, () =>
            {
                return _frontsideMenuRep.GetActiveFrontsideMenu();
            });
        }

        public FrontsideMenuViewModel GetFrontsideMenuViewModel()
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.FrontsideMenuViewModel,
            };

            return _jxCacheService.GetCache(searchCacheParam, CreateFrontsideMenuViewModel);
        }

        private FrontsideMenuViewModel CreateFrontsideMenuViewModel()
        {
            var frontsideMenuViewModel = new FrontsideMenuViewModel();
            List<FrontsideMenu> frontsideMenus = _frontsideMenuRep.GetAll().OrderBy(o => o.Sort).ToList();
            var frontsideMenuMap = new Dictionary<int, List<FrontsideMenu>>();

            //先把資料分組
            foreach (FrontsideMenu frontsideMenu in frontsideMenus)
            {
                frontsideMenuMap.AddNX(frontsideMenu.Type, new List<FrontsideMenu>());
                List<FrontsideMenu> menus = frontsideMenuMap[frontsideMenu.Type];
                menus.Add(frontsideMenu);
            }

            IEnumerable<FrontsideMenuTypeSetting> frontsideMenuTypeSettings = _frontsideMenuTypeSettingService
                .GetAll().Where(w => frontsideMenuMap.ContainsKey(w.Value));

            foreach (FrontsideMenuTypeSetting frontsideMenuTypeSetting in frontsideMenuTypeSettings)
            {
                int menuTypeValue = frontsideMenuTypeSetting.Value;
                List<FrontsideMenu> allMenus = frontsideMenuMap[menuTypeValue];

                PagedFrontsideProductMenu pagedFrontsideProductMenu = CreatePagedFrontsideProductMenu(menuTypeValue);
                var notFoundMenus = new List<FrontsideMenu>();

                foreach (FrontsideMenu menu in allMenus)
                {
                    FrontsideProductMenu frontsideProductMenu;

                    if (frontsideMenuTypeSetting.IsThirdPartySubGame)
                    {
                        if (!menu.IsActive)
                        {
                            continue;
                        }

                        frontsideProductMenu = new FrontsideProductMenu()
                        {
                            ProductCode = menu.ProductCode,
                            GameCode = menu.GameCode,
                            RemoteCode = menu.RemoteCode,
                            Title = menu.MenuName,
                            FullImageUrl = _httpWebRequestUtilService.CombineUrl(SharedAppSettings.BucketCdnDomain, menu.ImageUrl),
                            IsRedirectUrl = false,
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
                        bool isRedirectUrl = true;

                        if (menu.Url.IsNullOrEmpty())
                        {
                            url = $"/GameCenter/Index?productCode={product.Value}";
                            isRedirectUrl = false;
                        }

                        frontsideProductMenu = new FrontsideProductMenu()
                        {
                            ProductCode = product.Value,
                            ProductName = setting.Name,
                            Url = url,
                            Title = setting.Name,
                            IsRedirectUrl = isRedirectUrl,
                            GameCode = menu.GameCode,
                            CardCssClass = setting.CardCssClass,
                            IsHideHeaderWithFullScreen = product.ProductType.IsHideHeaderWithFullScreen
                        };
                    }

                    frontsideProductMenu.IsMaintaining = !menu.IsActive;
                    pagedFrontsideProductMenu.FrontsideProductMenus.Add(frontsideProductMenu);
                }

                if (notFoundMenus.Any())
                {
                    ErrorMsgUtil.ErrorHandle(
                        new InvalidProgramException("请确认PlatformProduct、FrontsideMenuSetting设定, " +
                            $"{notFoundMenus.Select(s => new { s.ProductCode, s.GameCode }).ToJsonString()} ")
                        , EnvLoginUser);
                }

                frontsideMenuViewModel.PagedFrontsideProductMenus.Add(pagedFrontsideProductMenu);
            }

            frontsideMenuViewModel.FrontsideMenuTypes = frontsideMenuTypeSettings
                .Select(s => new FrontsideMenuTypeViewModel()
                {
                    MenuTypeValue = s.Value,
                    MenuTypeName = s.Name,
                    ColsInRow = s.ColsInRow,
                    IconFileName = s.IconFileName,
                    CardOutCssClass = s.CardOutCssClass,
                    MaintainanceCssClass = s.MaintainanceCssClass,
                }).ToList();

            return frontsideMenuViewModel;
        }

        private FrontsideMenuSetting GetFrontsideMenuSetting(FrontsideMenu menu)
        {
            ThirdPartySubGameCodes thirdPartySubGameCode = ThirdPartySubGameCodes.GetSingle(menu.GameCode);

            PlatformProduct product = _platformProductService.GetSingle(menu.ProductCode);
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

        private PagedFrontsideProductMenu CreatePagedFrontsideProductMenu(int menuTypeValue)
        {
            return new PagedFrontsideProductMenu()
            {
                MenuTypeValue = menuTypeValue
            };
        }

        public PagedResultModel<QueryFrontsideMenuModel> GetPagedFrontsideMenu(QueryFrontsideMenuParam queryParam)
        {
            PagedResultModel<FrontsideMenu> datas = _frontsideMenuRep.GetPagedFrontsideMenu(queryParam);
            var models = datas.CastByJson<PagedResultModel<QueryFrontsideMenuModel>>();

            return models;
        }

        public List<FrontsideMenu> GetAllByType(int type)
        {
            return _frontsideMenuRep.GetAllByType(type);
        }

        public List<GameCenterManageModel> GetModelsByType(int type)
        {
            List<FrontsideMenu> menus = GetAllByType(type);

            return menus.Select(menu => new GameCenterManageModel()
            {
                No = menu.No,
                Sort = menu.Sort,
                IsActive = menu.IsActive,
                MenuName = GetNameFromSetting(menu),
            }).ToList();
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
    }
}