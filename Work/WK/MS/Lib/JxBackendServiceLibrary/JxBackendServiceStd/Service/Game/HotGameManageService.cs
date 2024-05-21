using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.Game
{
    public class HotGameManageService : BaseBackSideService, IHotGameManageService, IHotGameManageReadService
    {
        private readonly FrontsideMenuTypeSetting _hotGameType = FrontsideMenuTypeSetting.Hot;

        private readonly Lazy<IBoolSelectListItemsService> _boolSelectListItemsService;

        private readonly Lazy<IFrontsideMenuRep> _frontsideMenuRep;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.HotGameManage;

        private readonly string _hotGamesOssDirectoryPath = $"BackSideWeb/Upload/HotGames/";

        private readonly Lazy<IProcessObjectStorageService> _processObjectStorageService;

        private readonly Lazy<IOSSSettingService> _ossSettingService;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        public HotGameManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _boolSelectListItemsService = ResolveJxBackendService<IBoolSelectListItemsService>();
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
            _frontsideMenuService = ResolveJxBackendService<IFrontsideMenuService>();
            _processObjectStorageService = ResolveJxBackendService<IProcessObjectStorageService>();
            _ossSettingService = ResolveJxBackendService<IOSSSettingService>();
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application, Merchant);
        }

        public List<JxBackendSelectListItem> GetProductSelectListItems()
        {
            return _platformProductService.Value.GetContractSelectListItems(hasBlankOption: true, isSupportHotGame: true);
        }

        public List<JxBackendSelectListItem> GetActionSelectListItems()
        {
            return _boolSelectListItemsService.Value.GetActionSelectListItems();
        }

        public PagedResultModel<HotGameManageModel> GetPagedModel(HotGameManageQueryParam param)
        {
            PagedResultModel<FrontsideMenu> pagedModel = _frontsideMenuRep.Value.GetPagedFrontsideMenu(new QueryFrontsideMenuParam
            {
                MenuName = param.GameName,
                TypeValue = _hotGameType.Value.ToString(),
                PageNo = param.PageNo,
                PageSize = param.PageSize,
            });

            return pagedModel.CastByJson<PagedResultModel<HotGameManageModel>>();
        }

        public FrontsideMenu GetSingle(int no)
        {
            return _frontsideMenuRep.Value.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = no });
        }

        public BaseReturnModel Create(HotGameManageCreateParam param)
        {
            BaseReturnModel validGameCodeResult = IsGameCodeValid(param.ProductCode, param.GameCode);

            if (!validGameCodeResult.IsSuccess)
            {
                return validGameCodeResult;
            }

            FrontsideMenu existedUniqueKey = _frontsideMenuRep.Value.GetByUniqueKey(param.ProductCode, param.GameCode, param.RemoteCode);

            if (existedUniqueKey != null)
            {
                return new BaseReturnModel(ReturnCode.DataIsExist);
            }

            var newModel = new FrontsideMenu
            {
                Type = _hotGameType.Value,
                MenuName = param.MenuName,
                ProductCode = param.ProductCode,
                GameCode = param.GameCode,
                RemoteCode = param.RemoteCode,
                IsActive = param.IsActive,
                Sort = param.Sort.Value,
                AppSort = 0,
            };

            if (param.ImageFile != null)
            {
                newModel.ImageUrl = UploadHotGamesOriginAndAESToOSS(param.ImageFile);
                //newModel.ImageUrl = UploadToOSS(param.ImageFile, _hotGamesOssDirectoryPath);
            }

            BaseReturnDataModel<long> createResult = _frontsideMenuRep.Value.CreateByProcedure(newModel);

            if (!createResult.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, new FrontsideMenu()),
                ActTypes.Insert);

            CreateOperationLog(PermissionElement.Insert, param.MenuName, compareContent);
            ForceRefreshFrontsideMenus();

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Update(HotGameManageUpdateParam param)
        {
            BaseReturnModel validGameCodeResult = IsGameCodeValid(param.ProductCode, param.GameCode);

            if (!validGameCodeResult.IsSuccess)
            {
                return validGameCodeResult;
            }

            FrontsideMenu originModel = _frontsideMenuRep.Value.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = param.No });

            if (!IsHotGame(originModel))
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            if (param.ProductCode != originModel.ProductCode ||
                param.GameCode != originModel.GameCode ||
                param.RemoteCode != originModel.RemoteCode) //如果code都沒改就不檢查
            {
                FrontsideMenu existedUniqueKey = _frontsideMenuRep.Value.GetByUniqueKey(param.ProductCode, param.GameCode, param.RemoteCode);

                if (existedUniqueKey != null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsExist);
                }
            }

            FrontsideMenu newModel = originModel.CloneByJson();
            newModel.MenuName = param.MenuName;
            newModel.ProductCode = param.ProductCode;
            newModel.GameCode = param.GameCode;
            newModel.RemoteCode = param.RemoteCode;
            newModel.IsActive = param.IsActive;
            newModel.Sort = param.Sort.Value;

            bool isUploadNewImage = param.ImageFile != null;

            if (isUploadNewImage)
            {
                newModel.ImageUrl = UploadHotGamesOriginAndAESToOSS(param.ImageFile);

                //newModel.ImageUrl = UploadToOSS(param.ImageFile, _hotGamesOssDirectoryPath);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, originModel),
                ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            bool updateResult = _frontsideMenuRep.Value.UpdateByProcedure(newModel);

            if (!updateResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Edit, newModel.MenuName, compareContent);

            if (isUploadNewImage)
            {
                _processObjectStorageService.Value.DeleteOriginAndAESImage(_ossSettingService.Value.GetOSSClientSetting(), originModel.ImageUrl);
            }

            ForceRefreshFrontsideMenus();

            return new BaseReturnModel(ReturnCode.Success);
        }

        public List<JxBackendSelectListItem> GetHotGameSubGames(PlatformProduct product)
        {
            return ThirdPartySubGameCodes.GetSelectListItems(product, hasBlankOption: false, isSubGameOptionOfHotGameVislble: true);
        }

        public BaseReturnModel Delete(int no)
        {
            FrontsideMenu model = _frontsideMenuRep.Value.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = no });

            if (!IsHotGame(model))
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            bool deleteResult = _frontsideMenuRep.Value.DeleteByProcedure(model);

            if (!deleteResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Delete, model.MenuName);
            _processObjectStorageService.Value.DeleteOriginAndAESImage(_ossSettingService.Value.GetOSSClientSetting(), model.ImageUrl);
            ForceRefreshFrontsideMenus();

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string UploadHotGamesOriginAndAESToOSS(IFormFile imageFile)
        {
            string imageUrl = string.Empty;

            var updateImageOSSParams = new List<UpdateImageOSSParam>()
                {
                    new UpdateImageOSSParam
                    {
                        KeyID = Guid.NewGuid().ToString(),
                        Application = EnvLoginUser.Application,
                        ImageFileName = imageFile.FileName,
                        ImageBytes = imageFile.ToBytes(),
                        UploadOSSRemotePath = _hotGamesOssDirectoryPath,
                    }
                };

            Action<int, string> uploadCallback = (index, fullFileName) =>
            {
                if (!fullFileName.IsNullOrEmpty())
                {
                    if (index == 0)
                    {
                        imageUrl = fullFileName;
                    }
                }
            };

            _processObjectStorageService.Value.UploadOriginAndAESToImageOSS(
                _ossSettingService.Value.GetOSSClientSetting(),
                updateImageOSSParams,
                uploadCallback,
                out List<string> deleteFullObjectNames);

            return imageUrl;
        }

        private List<RecordCompareParam> GetRecordCompareParams(FrontsideMenu newModel, FrontsideMenu originModel)
            => new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.HotGameName,
                    OriginValue = originModel.MenuName,
                    NewValue = newModel.MenuName,
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.ThirdPartyOwnership,
                    OriginValue = PlatformProduct.GetName(originModel.ProductCode),
                    NewValue = PlatformProduct.GetName(newModel.ProductCode),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.GameCategory,
                    OriginValue = originModel.GameCode,
                    NewValue = newModel.GameCode,
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.GameCode,
                    OriginValue = originModel.RemoteCode,
                    NewValue = newModel.RemoteCode,
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.Sort,
                    OriginValue = originModel.Sort.ToString(),
                    NewValue = newModel.Sort.ToString(),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.IsActiveStatus,
                    OriginValue = originModel.IsActive.GetActionText(),
                    NewValue = newModel.IsActive.GetActionText(),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.UpdateGameImage,
                    OriginValue = originModel.ImageUrl,
                    NewValue = newModel.ImageUrl,
                    IsVisibleCompareValue = false
                },
            };

        private bool IsHotGame(FrontsideMenu menu)
        {
            return menu != null && menu.Type == _hotGameType;
        }

        private void CreateOperationLog(string actionName, string roleName)
        {
            CreateOperationLog(actionName, roleName, memo: string.Empty);
        }

        private void CreateOperationLog(string actionName, string gameName, string memo)
        {
            string content = string.Format(BWOperationLogElement.HotGameManageMessage,
                actionName,
                gameName,
                memo);

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = _permissionKey,
                Content = content
            });
        }

        private BaseReturnModel IsGameCodeValid(string productCode, string gameCode)
        {
            if (!_frontsideMenuRep.Value.IsFrontsideMainMenuExists(new FrontSideMainMenu() { ProductCode = productCode, GameCode = gameCode }))
            {
                return new BaseReturnModel($"找不到對應的主選單(productCode={productCode},gameCode={gameCode}");
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private void ForceRefreshFrontsideMenus()
        {
            _frontsideMenuService.Value.ForceRefreshFrontsideMenus();
        }
    }
}