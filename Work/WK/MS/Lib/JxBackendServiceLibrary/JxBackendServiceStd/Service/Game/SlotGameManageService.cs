using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class SlotGameManageService : BaseBackSideService, ISlotGameManageService, ISlotGameManageReadService
    {
        private readonly Lazy<IBoolSelectListItemsService> _boolSelectListItemsService;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        private readonly Lazy<IGameLobbyListRep> _gameLobbyListRep;

        private readonly Lazy<IProcessObjectStorageService> _processObjectStorageService;

        private readonly Lazy<IOSSSettingService> _ossSettingService;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.SlotGameManage;

        public SlotGameManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _boolSelectListItemsService = ResolveJxBackendService<IBoolSelectListItemsService>();
            _frontsideMenuService = ResolveJxBackendService<IFrontsideMenuService>();
            _gameLobbyListRep = ResolveJxBackendService<IGameLobbyListRep>();
            _processObjectStorageService = ResolveJxBackendService<IProcessObjectStorageService>();
            _ossSettingService = ResolveJxBackendService<IOSSSettingService>();
        }

        public List<JxBackendSelectListItem> GetProductSelectListItems(string defaultText)
        {
            List<JxBackendSelectListItem> selectList = GetThirdPartyCodeNameMap()
                .Select(entry => new JxBackendSelectListItem(entry.Key, entry.Value)).ToList();

            selectList.AddBlankOption(hasBlankOption: true, defaultValue: string.Empty, defaultText);

            return selectList;
        }

        private Dictionary<string, string> GetThirdPartyCodeNameMap()
        {
            return GameLobbyType.GetAll()
                .ToDictionary(t => t.Value, t => _frontsideMenuService.Value.GetNameFromSetting(
                    new FrontsideMenu
                    {
                        ProductCode = t.Product.Value,
                        GameCode = t.SubGameCode
                    }));
        }

        public List<JxBackendSelectListItem> GetActionSelectListItems()
        {
            return _boolSelectListItemsService.Value.GetActionSelectListItems();
        }

        public PagedResultModel<SlotGameManageModel> GetPagedModel(SlotGameManageQueryParam param)
        {
            PagedResultModel<GameLobbyList> pagedEntity = _gameLobbyListRep.Value.GetPagedModel(param);
            PagedResultModel<SlotGameManageModel> pagedModel = pagedEntity.CastByJson<PagedResultModel<SlotGameManageModel>>();

            pagedModel.ResultList = pagedEntity.ResultList.Select(entity => ConvertEntityToViewModel(entity)).ToList();

            return pagedModel;
        }

        public BaseSlotGameManageParam GetManageParam(int no)
        {
            GameLobbyList entity = _gameLobbyListRep.Value.GetSingleByKey(InlodbType.Inlodb, new GameLobbyList { No = no });

            return new BaseSlotGameManageParam
            {
                No = entity.No,
                GameName = entity.ChineseName,
                Sort = entity.Sort,
                IsActive = entity.IsActive,
                IsHot = entity.IsHot,
                GameCode = entity.WebGameCode,
                ThirdPartyCode = entity.ThirtyPartyCode,
                ImageUrl = entity.ImageUrl,
            };
        }

        public BaseReturnModel Create(SlotGameManageCreateParam param)
        {
            GameLobbyList existedUniqueKey = _gameLobbyListRep.Value.GetByCodes(param.ThirdPartyCode, param.GameCode);

            if (existedUniqueKey != null)
            {
                return new BaseReturnModel(ReturnCode.DataIsExist);
            }

            var newModel = new GameLobbyList
            {
                ThirtyPartyCode = param.ThirdPartyCode,
                ChineseName = param.GameName,
                WebGameCode = param.GameCode,
                MobileGameCode = param.GameCode,
                IsActive = param.IsActive,
                IsHot = param.IsHot,
                Sort = param.Sort.Value,
            };

            newModel.ImageUrl = UploadHotGamesOriginAndAESToOSS(param.ImageFile.FileName, param.ImageFile.ToBytes(), SlotGamesOssDirectoryPath(param.ThirdPartyCode));
            //newModel.ImageUrl = UploadToOSS(param.ImageFile, OssDirectoryPath(param.ThirdPartyCode));

            BaseReturnDataModel<long> createResult = _gameLobbyListRep.Value.CreateByProcedure(newModel);

            if (!createResult.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, new GameLobbyList()),
                ActTypes.Insert);

            CreateOperationLog(PermissionElement.Insert, param.GameName, compareContent);
            ForceRefreshFrontsideMenus();

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Update(SlotGameManageUpdateParam param)
        {
            GameLobbyList originModel = _gameLobbyListRep.Value.GetSingleByKey(InlodbType.Inlodb, new GameLobbyList { No = param.No });

            if (originModel == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            if (param.ThirdPartyCode != originModel.ThirtyPartyCode || param.GameCode != originModel.WebGameCode) //如果code都沒改就不檢查
            {
                GameLobbyList existedUniqueKey = _gameLobbyListRep.Value.GetByCodes(param.ThirdPartyCode, param.GameCode);

                if (existedUniqueKey != null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsExist);
                }
            }

            GameLobbyList newModel = originModel.CloneByJson();
            newModel.ChineseName = param.GameName;
            newModel.ThirtyPartyCode = param.ThirdPartyCode;
            newModel.WebGameCode = param.GameCode;
            newModel.MobileGameCode = param.GameCode;
            newModel.IsActive = param.IsActive;
            newModel.IsHot = param.IsHot;
            newModel.Sort = param.Sort.Value;

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, originModel),
                ActTypes.Update);

            bool isUploadNewImage = param.ImageFile != null;
            string newOssDirectoryPath = SlotGamesOssDirectoryPath(param.ThirdPartyCode);
            bool isReuploadImage = !isUploadNewImage && !newOssDirectoryPath.Equals(SlotGamesOssDirectoryPath(originModel.ThirtyPartyCode));

            if (isUploadNewImage)
            {
                newModel.ImageUrl = UploadHotGamesOriginAndAESToOSS(param.ImageFile.FileName, param.ImageFile.ToBytes(), newOssDirectoryPath);
                //newModel.ImageUrl = UploadToOSS(param.ImageFile, newOssDirectoryPath);
            }
            else if (isReuploadImage)
            {
                string originFileName = originModel.ImageUrl;
                byte[] originImageBytes = _processObjectStorageService.Value.GetImageObject(_ossSettingService.Value.GetOSSClientSetting(), originFileName);
                //byte[] originImageBytes = OssService.GetObject(originFileName);

                newModel.ImageUrl = UploadHotGamesOriginAndAESToOSS(originFileName, originImageBytes, newOssDirectoryPath);
                //newModel.ImageUrl = UploadToOSS(originImageBytes, originFileName, newOssDirectoryPath);
            }

            if (compareContent.IsNullOrEmpty() && !isUploadNewImage && !isReuploadImage)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            bool updateResult = _gameLobbyListRep.Value.UpdateByProcedure(newModel);

            if (!updateResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Edit, newModel.ChineseName, compareContent, isUploadNewImage);

            if (isUploadNewImage || isReuploadImage)
            {
                //OssService.DeleteObject(originModel.ImageUrl);
                _processObjectStorageService.Value.DeleteOriginAndAESImage(_ossSettingService.Value.GetOSSClientSetting(), originModel.ImageUrl);
            }

            ForceRefreshFrontsideMenus();

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Delete(int no)
        {
            GameLobbyList model = _gameLobbyListRep.Value.GetSingleByKey(InlodbType.Inlodb, new GameLobbyList { No = no });

            if (model == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            bool deleteResult = _gameLobbyListRep.Value.DeleteByProcedure(model);

            if (!deleteResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Delete, model.ChineseName);

            //OssService.DeleteObject(model.ImageUrl);
            _processObjectStorageService.Value.DeleteOriginAndAESImage(_ossSettingService.Value.GetOSSClientSetting(), model.ImageUrl);
            ForceRefreshFrontsideMenus();

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string SlotGamesOssDirectoryPath(string thirdPartyCode) => $"BackSideWeb/Upload/SlotGames/{thirdPartyCode}/";

        private string UploadHotGamesOriginAndAESToOSS(string imageFileName, byte[] imageBytes, string uploadOSSRemotePath)
        {
            string imageUrl = string.Empty;

            var updateImageOSSParams = new List<UpdateImageOSSParam>()
            {
                new UpdateImageOSSParam
                {
                    KeyID = Guid.NewGuid().ToString(),
                    Application = EnvLoginUser.Application,
                    ImageFileName = imageFileName,
                    ImageBytes = imageBytes,
                    UploadOSSRemotePath = uploadOSSRemotePath,
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

        private SlotGameManageModel ConvertEntityToViewModel(GameLobbyList entity)
        {
            return new SlotGameManageModel
            {
                No = entity.No,
                MenuName = entity.ChineseName,
                Sort = entity.Sort,
                IsActive = entity.IsActive,
                IsHot = entity.IsHot,
                GameCode = entity.WebGameCode,
                ThirdPartyCode = entity.ThirtyPartyCode,
                ImageUrl = entity.ImageUrl,
                ProductName = GetThirdPartyCodeNameMap().TryGetValue(entity.ThirtyPartyCode),
            };
        }

        private List<RecordCompareParam> GetRecordCompareParams(GameLobbyList newModel, GameLobbyList originModel)
            => new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.SlotGameName,
                    OriginValue = originModel.ChineseName,
                    NewValue = newModel.ChineseName,
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.ThirdPartyOwnership,
                    OriginValue = GetThirdPartyCodeNameMap().TryGetValue(originModel.ThirtyPartyCode.ToNonNullString()),
                    NewValue = GetThirdPartyCodeNameMap().TryGetValue(newModel.ThirtyPartyCode.ToNonNullString()),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.GameCode,
                    OriginValue = originModel.WebGameCode,
                    NewValue = newModel.WebGameCode,
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
                    Title = DisplayElement.RecommendGame,
                    OriginValue = GetRecommendText(originModel.IsHot),
                    NewValue = GetRecommendText(newModel.IsHot),
                }
            };

        private string GetRecommendText(bool isRecommend)
        {
            return _boolSelectListItemsService.Value.GetRecommendSelectListItems()
                .Single(selectListItem => selectListItem.Value == isRecommend.ToString())
                .Text;
        }

        private void CreateOperationLog(string actionName, string roleName)
        {
            CreateOperationLog(actionName, roleName, memo: string.Empty);
        }

        private void CreateOperationLog(string actionName, string roleName, string memo)
        {
            CreateOperationLog(actionName, roleName, memo, isUpdateImage: false);
        }

        private void CreateOperationLog(string actionName, string gameName, string memo, bool isUpdateImage)
        {
            if (isUpdateImage)
            {
                memo = string.Join(", ", memo, DisplayElement.UpdateGameImage);
            }

            string content = string.Format(BWOperationLogElement.SlotGameManageMessage,
                actionName,
                gameName,
                memo);

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = _permissionKey,
                Content = content
            });
        }

        private void ForceRefreshFrontsideMenus()
        {
            _frontsideMenuService.Value.ForceRefreshFrontsideMenus();
        }
    }
}