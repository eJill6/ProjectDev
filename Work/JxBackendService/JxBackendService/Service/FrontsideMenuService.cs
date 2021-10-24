using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.DownloadFile;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendService.Service.Cache;
using JxBackendService.Service.HomeGameMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service
{
    public class FrontsideMenuService : BaseService, IFrontsideMenuService
    {
        private readonly IFrontsideMenuRep _frontsideMenuRep;
        private readonly IJxCacheService _jxCacheService;
        private readonly IOperationLogService _operationLogService;

        public FrontsideMenuService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
            _jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
            _operationLogService = ResolveJxBackendService<IOperationLogService>();
        }

        public List<FrontsideMenuModel> GetFrontsideAllMenu(bool isForceRefresh = false)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.FrontsideMenuGrouped,
                IsForceRefresh = isForceRefresh
            };

            return _jxCacheService.GetCache(searchCacheParam, () =>
            {
                IEnumerable<FrontsideMenu> frontsideMenus = _frontsideMenuRep.GetAll().Where(w => PlatformProduct.GetSingle(w.ProductCode) != null);
                frontsideMenus = frontsideMenus.OrderBy(x => x.Type).ThenBy(x => x.Sort);

                List<FrontsideMenuModel> result = new List<FrontsideMenuModel>();
                //前台只取第三方的
                List<FrontsideMenuTypes> frontsideMenuTypes = FrontsideMenuTypes.GetAllThirdPartyGameMenu();

                foreach (FrontsideMenuTypes menuType in frontsideMenuTypes)
                {
                    FrontsideMenuModel frontsideMenu = new FrontsideMenuModel
                    {
                        Type = menuType.Value,
                        MenuItems = frontsideMenus.Where(x => x.Type == menuType.Value).ToList()
                    };

                    result.Add(frontsideMenu);
                }

                return result;
            });
        }

        public List<GameMenuViewModel> GetGameMenusForApi(string imageDomainName, List<GameMenuRecommendItemViewModel> recommendLotteryItems)
        {
            return _jxCacheService.GetCache(CacheKey.GameMenusForApi, () =>
            {
                //發現若DB語法先上線後，會跟先前Web一樣入口出現但無法進入的問題故加上Where判斷程式使否與DB內容匹配才顯示
                IOrderedEnumerable<FrontsideMenu> frontsideMenuRepResult = _frontsideMenuRep
                .GetAll()
                .Where(w => PlatformProduct.GetSingle(w.ProductCode) != null)
                .OrderBy(x => x.Type).ThenBy(x => x.AppSort);

                List<GameMenuViewModel> result = new List<GameMenuViewModel>();
                List<FrontsideMenuTypes> frontsideMenuTypes = FrontsideMenuTypes.GetAll();

                foreach (var menuType in frontsideMenuTypes)
                {
                    GameMenuViewModel frontsideMenu = new GameMenuViewModel
                    {
                        MenuName = menuType.AppDisplayName
                    };

                    if (menuType == FrontsideMenuTypes.Lottery)
                    {
                        frontsideMenu.EntranceItems = new List<GameMenuEntranceItemViewModel>()
                        {
                            new GameMenuEntranceItemViewModel()
                            {
                                ProductCode = PlatformProduct.Lottery.Value,
                                IsActive = true,
                                Title = CommonElement.LotteryLobbyGameEntranceTitle,
                                SubTitle = CommonElement.LotteryLobbyGameEntranceSubTitle,
                                LogoImageUrl = StringUtil.ToFullFileUrl
                                (
                                    imageDomainName ,
                                    DownloadFilePathTypes.GetControlSizeImagePath(DownloadFilePathTypes.GameMenuEntranceItemLogo, PlatformProduct.Lottery.Value,
                                                                                  ImageSizeTypes.Large)
                                ),
                                AdvertisingImageUrl = StringUtil.ToFullFileUrl
                                (
                                    imageDomainName ,
                                    DownloadFilePathTypes.GetControlSizeImagePath(DownloadFilePathTypes.GameMenuEntranceItemAdvertising, PlatformProduct.Lottery.Value,
                                                                                  ImageSizeTypes.Large)
                                ),
                            }
                        };

                        frontsideMenu.RecommendItems = recommendLotteryItems;
                    }
                    else
                    {
                        var frontsideMenuItems = frontsideMenuRepResult.Where(x => x.Type == menuType.Value).ToList();
                        int itemTotalCount = frontsideMenuItems.Count();

                        frontsideMenu.EntranceItems = frontsideMenuItems
                            .Select(x => new GameMenuEntranceItemViewModel()
                            {
                                Title = x.MenuName,
                                SubTitle = x.EngName,
                                ProductCode = x.ProductCode,
                                SubGameCode = x.GameCode,
                                IsActive = x.Active,
                                LogoImageUrl = StringUtil.ToFullFileUrl
                                (
                                    imageDomainName,
                                    DownloadFilePathTypes.GetControlSizeImagePath(DownloadFilePathTypes.GameMenuEntranceItemLogo,
                                                                                  (x.GameCode.IsNullOrEmpty()) ? x.ProductCode : x.GameCode,
                                                                                  GetGameMenuItemImageSize(frontsideMenuItems.IndexOf(x), itemTotalCount))
                                ),
                                AdvertisingImageUrl = StringUtil.ToFullFileUrl
                                (
                                    imageDomainName,
                                    DownloadFilePathTypes.GetControlSizeImagePath(DownloadFilePathTypes.GameMenuEntranceItemAdvertising,
                                                                                  (x.GameCode.IsNullOrEmpty()) ? x.ProductCode : x.GameCode,
                                                                                  GetGameMenuItemImageSize(frontsideMenuItems.IndexOf(x), itemTotalCount))
                                ),
                                LoadingImageUrl = StringUtil.ToFullFileUrl
                                (
                                    imageDomainName,
                                    DownloadFilePathTypes.GetBaseImagePath(DownloadFilePathTypes.GameMenuEntranceItemLoading,
                                                                           (x.GameCode.IsNullOrEmpty()) ? x.ProductCode : x.GameCode)
                                ),
                            }).ToList();

                        frontsideMenu.RecommendItems = new List<GameMenuRecommendItemViewModel>();
                    }

                    result.Add(frontsideMenu);
                }

                return result;
            });
        }

        public List<FrontsideMenu> GetAllMenu()
        {
            return _jxCacheService.GetCache<List<FrontsideMenu>>(CacheKey.FrontsideMenuList, () =>
            {
                var menu = _frontsideMenuRep.GetAll().OrderBy(x => x.Type).ThenBy(x => x.Sort);
                return menu.ToList();
            });
        }

        public List<FrontsideMenu> GetActiveFrontsideMenu()
        {
            return _jxCacheService.GetCache(CacheKey.ActiveFrontsideMenuList, () =>
            {
                return _frontsideMenuRep.GetActiveFrontsideMenu().OrderBy(x => x.Type)
                .ThenBy(x => x.Sort).ToList();
            });
        }

        public FrontsideMenu GetSingle(string productCode)
        {
            return GetAllMenu().FirstOrDefault(x => x.ProductCode == productCode);
        }

        public FrontsideMenu GetSingle(string productCode, string gameCode)
        {
            return GetAllMenu().FirstOrDefault(x => x.ProductCode == productCode && x.GameCode == gameCode);
        }

        #region 後台功能使用
        public List<FrontsideMenuTypes> GetAllThirdPartyGameMenuType()
        {
            return FrontsideMenuTypes.GetAllThirdPartyGameMenu();
        }

        /// <summary>
        /// 後台頁面的開關設定值
        /// </summary>
        public List<JxBackendSelectListItem> GetFrontsideMenuRadioTagSetting()
        {
            List<JxBackendSelectListItem> selectListItems = new List<JxBackendSelectListItem>();

            selectListItems.Add(new JxBackendSelectListItem()
            {
                Value = Convert.ToInt16(true).ToString(),
                Text = SelectedItemBooleanTypes.FrontsideMenuActive.GetItemName(true)
            });
            selectListItems.Add(new JxBackendSelectListItem()
            {
                Value = Convert.ToInt16(false).ToString(),
                Text = SelectedItemBooleanTypes.FrontsideMenuActive.GetItemName(false)
            });

            return selectListItems;
        }

        public List<FrontsideMenu> GetFrontsideMenuByType(int frontsideMenuType)
        {
            bool isForceRefresh = true;

            // 呼叫既有查詢全部,並強刷cache
            List<FrontsideMenuModel> frontsideMenuModels = GetFrontsideAllMenu(isForceRefresh);

            // 抽取需要的部分
            List<FrontsideMenu> result = frontsideMenuModels
                                            .Where(m => m.Type == frontsideMenuType)
                                            .Select(m => m.MenuItems)
                                            .FirstOrDefault();

            return result;
        }

        public BaseReturnModel UpdateFrontsideMenuByType(int frontsideMenuType, List<FrontsideMenu> modifyFrontSideMenuList)
        {
            if (modifyFrontSideMenuList.Where(w => w.Sort > GlobalVariables.MaxSortSerialLimit || w.AppSort > GlobalVariables.MaxSortSerialLimit).Any())
            {
                throw new ArgumentOutOfRangeException();
            }

            List<FrontsideMenu> sourceList = GetFrontsideMenuByType(frontsideMenuType);

            foreach (FrontsideMenu modifyFrontSideMenu in modifyFrontSideMenuList)
            {
                FrontsideMenu source = sourceList.Where(w => w.No == modifyFrontSideMenu.No).SingleOrDefault();

                if (source != null
                    && (source.Active != modifyFrontSideMenu.Active
                    || source.Sort != modifyFrontSideMenu.Sort
                    || source.AppSort != modifyFrontSideMenu.AppSort))
                {
                    //複製一份
                    FrontsideMenu newData = JsonUtil.CloneByJson(source);

                    //帶入異動的資料
                    newData.Active = modifyFrontSideMenu.Active;
                    newData.Sort = modifyFrontSideMenu.Sort;
                    newData.AppSort = modifyFrontSideMenu.AppSort;

                    //異動資料的使用者
                    newData.UpdateUser = EnvLoginUser.LoginUser.UserName;

                    if (_frontsideMenuRep.UpdateByProcedure(newData)) //呼叫SP
                    {
                        //組操作日誌內容
                        string operationLogContent = string.Format("{0}, {1}", OperationLogContentElement.ProductSetting, source.MenuName);

                        if (source.Active != newData.Active)
                        {
                            string modifyContent = string.Format(MessageElement.ModifyData, // {0}:{1} -> {2}
                                CommonElement.Status,
                                source.Active ? CommonElement.Open : CommonElement.CloseDown,
                                newData.Active ? CommonElement.Open : CommonElement.CloseDown);

                            operationLogContent = string.Format("{0}, {1}", operationLogContent, modifyContent);
                        }

                        if (source.Sort != newData.Sort || source.AppSort != newData.AppSort)
                        {
                            string modifyContent = string.Format(MessageElement.ModifyData, // {0}:{1} -> {2}
                                CommonElement.Sort,
                                source.Sort.ToString(),
                                newData.Sort.ToString());

                            operationLogContent = string.Format("{0}, {1}", operationLogContent, modifyContent);
                        }

                        //新增操作紀錄
                        BaseReturnDataModel<long> saveOperationLogResult = _operationLogService.InsertModifySystemOperationLog(
                            new Model.Param.User.InsertModifySystemOperationLogParam()
                            {
                                Category = JxOperationLogCategory.SystemSettings,
                                Content = operationLogContent
                            }
                        );

                        if (!saveOperationLogResult.IsSuccess)
                        {
                            return saveOperationLogResult;
                        }
                    }
                    else
                    {
                        return new BaseReturnModel(ReturnCode.OperationFailed);
                    }
                }
            }

            //以上執行完畢以後回傳成功
            return new BaseReturnModel(new SuccessMessage(MessageElement.SaveSuccess));
        }
        #endregion

        /// <summary>
        /// 取的當前圖片要顯示的圖片大小
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="totalItemCount"></param>
        /// <returns></returns>
        private string GetGameMenuItemImageSize(int itemIndex, int totalItemCount)
        {
            //一個頁面最大的項目數量
            int maxItemCountForOnePage = 4;

            //只要總數量大於 一個頁面最大的項目數量 全部都給小圖
            if (totalItemCount >= maxItemCountForOnePage)
            {
                return ImageSizeTypes.Small;
            }

            //剩下只有三種情況
            //總數量1、2頁面 => 都是大圖
            //總數量3頁面 => 第1張大圖、第2、3張小圖

            switch (totalItemCount)
            {
                case 0:
                    return ImageSizeTypes.Small;
                case 1:
                case 2:
                    return ImageSizeTypes.Large;
                case 3:

                    int pageItemIndex = (itemIndex % maxItemCountForOnePage);
                    if (pageItemIndex == 0)
                    {
                        return ImageSizeTypes.Large;
                    }
                    else
                    {
                        return ImageSizeTypes.Small;
                    }
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
