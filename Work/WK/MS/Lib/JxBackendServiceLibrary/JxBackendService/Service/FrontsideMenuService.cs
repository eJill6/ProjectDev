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
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service
{
    public class FrontsideMenuService : BaseService, IFrontsideMenuService
    {
        private readonly IFrontsideMenuRep _frontsideMenuRep;

        private readonly IFrontsideMenuTypeRep _frontsideMenuTypeRep;

        private readonly IFrontsideMenuTypeService _frontsideMenuTypeService;

        private readonly IJxCacheService _jxCacheService;

        private readonly IPlatformProductService _platformProductService;

        public FrontsideMenuService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
            _frontsideMenuTypeRep = ResolveJxBackendService<IFrontsideMenuTypeRep>();
            _frontsideMenuTypeService = ResolveJxBackendService<IFrontsideMenuTypeService>();
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
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
            List<FrontsideMenu> frontsideMenus = _frontsideMenuRep.GetActiveFrontsideMenu().OrderBy(o => o.Sort).ToList();
            var frontsideMenuMap = new Dictionary<int, List<FrontsideMenu>>();

            //先把資料分組
            foreach (FrontsideMenu frontsideMenu in frontsideMenus)
            {
                frontsideMenuMap.AddNX(frontsideMenu.Type, new List<FrontsideMenu>());
                List<FrontsideMenu> menus = frontsideMenuMap[frontsideMenu.Type];
                menus.Add(frontsideMenu);
            }

            List<FrontsideMenuType> frontsideMenuTypes = _frontsideMenuTypeRep.GetAll();

            IEnumerable<FrontsideMenuTypeSetting> frontsideMenuTypeSettings = _frontsideMenuTypeService
                .GetAllAndSortByDbValues(frontsideMenuTypes)
                .Where(w => frontsideMenuMap.ContainsKey(w.Value));

            foreach (FrontsideMenuTypeSetting frontsideMenuTypeSetting in frontsideMenuTypeSettings)
            {
                int menuTypeValue = frontsideMenuTypeSetting.Value;
                List<FrontsideMenu> allMenus = frontsideMenuMap[menuTypeValue];

                PagedFrontsideProductMenu pagedFrontsideProductMenu = CreatePagedFrontsideProductMenu(menuTypeValue);
                var notFoundMenus = new List<FrontsideMenu>();

                var httpWebRequestUtilService = ResolveJxBackendService<IHttpWebRequestUtilService>();

                foreach (FrontsideMenu menu in allMenus)
                {
                    if (frontsideMenuTypeSetting.IsThirdPartySubGame)
                    {
                        pagedFrontsideProductMenu.FrontsideProductMenus.Add(new FrontsideProductMenu()
                        {
                            ProductCode = menu.ProductCode,
                            GameCode = menu.GameCode,
                            RemoteCode = menu.RemoteCode,
                            Title = menu.MenuName,
                            FullImageUrl = httpWebRequestUtilService.CombineUrl(SharedAppSettings.BucketCdnDomain, menu.ImageUrl),
                            IsRedirectUrl = false,
                        });
                    }
                    else
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

                        if (product == null || setting == null)
                        {
                            notFoundMenus.Add(menu);

                            continue;
                        }

                        string url = menu.Url;
                        bool isRedirectUrl = true;

                        if (menu.Url.IsNullOrEmpty())
                        {
                            url = $"/GameCenter/Index?productCode={product.Value}";
                            isRedirectUrl = false;
                        }

                        pagedFrontsideProductMenu.FrontsideProductMenus.Add(new FrontsideProductMenu()
                        {
                            ProductCode = product.Value,
                            ProductName = setting.Name,
                            Url = url,
                            Title = setting.Name,
                            IsRedirectUrl = isRedirectUrl,
                            GameCode = menu.GameCode,
                            CardCssClass = setting.CardCssClass,
                            IsHideHeaderWithFullScreen = PlatformProduct.GetSingle(product.Value).ProductType.IsHideHeaderWithFullScreen
                        });
                    }
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
                    CardOutCssClass = s.CardOutCssClass
                })
                .ToList();

            return frontsideMenuViewModel;
        }

        private PagedFrontsideProductMenu CreatePagedFrontsideProductMenu(int menuTypeValue)
        {
            return new PagedFrontsideProductMenu()
            {
                MenuTypeValue = menuTypeValue
            };
        }
    }
}