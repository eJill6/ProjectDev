using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class HotGameManageService : BaseBackSideService, IHotGameManageService, IHotGameManageReadService
    {
        private readonly FrontsideMenuTypeSetting _hotGameType = FrontsideMenuTypeSetting.Hot;
        private readonly string _gameCode = string.Empty; // 熱門遊戲GameCode固定為空
        private readonly IBoolSelectListItemsService _boolSelectListItemsService;
        private readonly IFrontsideMenuRep _frontsideMenuRep;
        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.HotGameManage;
        private readonly string _ossDirectoryPath = $"BackSideWeb/Upload/HotGames";

        public HotGameManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _boolSelectListItemsService = ResolveJxBackendService<IBoolSelectListItemsService>();
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
        }

        public List<JxBackendSelectListItem> GetProductSelectListItems()
        {
            List<PlatformProduct> supportHotGamePlatformProducts = PlatformProduct.GetAll().Where(p => p.IsSupportHotGame).ToList();
            return PlatformProduct.GetSelectListItems(supportHotGamePlatformProducts, hasBlankOption: true);
        }

        public List<JxBackendSelectListItem> GetActionSelectListItems()
        {
            return _boolSelectListItemsService.GetActionSelectListItems();
        }

        public PagedResultModel<HotGameManageModel> GetPagedModel(HotGameManageQueryParam param)
        {
            PagedResultModel<FrontsideMenu> pagedModel = _frontsideMenuRep.GetPagedFrontsideMenu(new QueryFrontsideMenuParam
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
            return _frontsideMenuRep.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = no });
        }

        public BaseReturnModel Create(HotGameManageCreateParam param)
        {
            FrontsideMenu existedUniqueKey = _frontsideMenuRep.GetByUniqueKey(param.ProductCode, _gameCode, param.RemoteCode);

            if (existedUniqueKey != null)
            {
                return new BaseReturnModel(ReturnCode.DataIsExist);
            }

            FrontsideMenu newModel = new FrontsideMenu
            {
                Type = _hotGameType.Value,
                MenuName = param.MenuName,
                ProductCode = param.ProductCode,
                GameCode = _gameCode,
                RemoteCode = param.RemoteCode,
                IsActive = param.IsActive,
                Sort = param.Sort.Value,
                AppSort = 0,
            };

            if (param.ImageFile != null)
            {
                newModel.ImageUrl = UploadToOSS(param.ImageFile, _ossDirectoryPath);
            }

            BaseReturnDataModel<long> createResult = _frontsideMenuRep.CreateByProcedure(newModel);

            if (!createResult.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, new FrontsideMenu()), 
                ActTypes.Insert);

            CreateOperationLog(PermissionElement.Insert, param.MenuName, compareContent);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Update(HotGameManageUpdateParam param)
        {
            FrontsideMenu originModel = _frontsideMenuRep.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = param.No });

            if (!IsHotGame(originModel))
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            if (param.ProductCode != originModel.ProductCode || param.RemoteCode != originModel.RemoteCode) //如果code都沒改就不檢查
            {
                FrontsideMenu existedUniqueKey = _frontsideMenuRep.GetByUniqueKey(param.ProductCode, _gameCode, param.RemoteCode);

                if (existedUniqueKey != null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsExist);
                }
            }

            FrontsideMenu newModel = originModel.CloneByJson();
            newModel.MenuName = param.MenuName;
            newModel.ProductCode = param.ProductCode;
            newModel.RemoteCode = param.RemoteCode;
            newModel.IsActive = param.IsActive;
            newModel.Sort = param.Sort.Value;

            bool isUploadNewImage = param.ImageFile != null;

            if (isUploadNewImage)
            {
                newModel.ImageUrl = UploadToOSS(param.ImageFile, _ossDirectoryPath);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, originModel), 
                ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            bool updateResult = _frontsideMenuRep.UpdateByProcedure(newModel);

            if (!updateResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Edit, newModel.MenuName, compareContent);

            if (isUploadNewImage)
            {
                OssService.DeleteObject(originModel.ImageUrl);
            }

            return new BaseReturnModel(ReturnCode.Success);
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

        public BaseReturnModel Delete(int no)
        {
            FrontsideMenu model = _frontsideMenuRep.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = no });

            if (!IsHotGame(model))
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            bool deleteResult = _frontsideMenuRep.DeleteByProcedure(model);

            if (!deleteResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Delete, model.MenuName);
            OssService.DeleteObject(model.ImageUrl);

            return new BaseReturnModel(ReturnCode.Success);
        }

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
    }
}