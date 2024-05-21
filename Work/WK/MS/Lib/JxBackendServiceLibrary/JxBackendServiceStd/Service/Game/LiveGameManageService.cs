using Flurl;
using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Param.Game;
using JxBackendService.Interface.Repository;
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
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace JxBackendService.Service.Game
{
    public class LiveGameManageService : BaseBackSideService, ILiveGameManageService, ILiveGameManageReadService
    {
        private readonly Lazy<ILiveGameManageRep> _liveGameManageRep;

        private readonly string _ossDirectoryPath = $"BackSideWeb/Upload/LiveGames/";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.LiveGameManage;

        private readonly Lazy<IProcessObjectStorageService> _processObjectStorageService;

        private readonly Lazy<IOSSSettingService> _ossSettingService;

        public LiveGameManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(
            envLoginUser, dbConnectionType)
        {
            _liveGameManageRep = ResolveJxBackendService<ILiveGameManageRep>();
            _processObjectStorageService = ResolveJxBackendService<IProcessObjectStorageService>();
            _ossSettingService = ResolveJxBackendService<IOSSSettingService>();
        }

        public PagedResultModel<LiveGameManageModel> GetPagedModel(LiveGameManageQueryParam param)
        {
            PagedResultModel<LiveGameManage> pagedModel = _liveGameManageRep.Value.GetPagedAll(param);
            var pagedResultModel = pagedModel.CastByJson<PagedResultModel<LiveGameManageModel>>();

            pagedResultModel.ResultList.ForEach(f =>
            {
                ISearchLiveGameManageDataType modelDataType = new BaseLiveGameManageParam
                {
                    ProductCode = f.ProductCode.ToNonNullString(),
                    LotteryId = f.LotteryId,
                    RemoteCode = f.RemoteCode.ToNonNullString(),
                    GameCode = f.GameCode.ToNonNullString(),
                };

                f.LiveGameManageDataTypeValue = LiveGameManageDataType.GetSingle(modelDataType).Value;
                f.LiveGameManageDataTypeName = LiveGameManageDataType.GetName(f.LiveGameManageDataTypeValue);
                f.DurationText = f.Duration.ToString();
                f.StyleText = f.Style.ToString();
                f.FrameRatioText = f.FrameRatio.ToString();
                f.IsCountdownText = f.IsCountdown.GetYesNoText();
                f.IsH5Text = f.IsH5.GetYesNoText();
                f.IsFollowText = f.IsFollow.GetActionName();

                SetDefaultValue(f);
            });

            return pagedResultModel;
        }

        public IEnumerable<LiveGameManage> GetAll()
        {
            foreach (LiveGameManage liveGameManage in _liveGameManageRep.Value.GetAll())
            {
                liveGameManage.ImageUrl = !liveGameManage.ImageUrl.IsNullOrEmpty()
                    ? Url.Combine(SharedAppSettings.BucketCdnDomain, liveGameManage.ImageUrl)
                    : string.Empty;

                yield return liveGameManage;
            }
        }

        public LiveGameManage GetDetail(int no)
        {
            return _liveGameManageRep.Value.GetDetail(no);
        }

        public BaseReturnModel Create(LiveGameManageCreateParam createParam)
        {
            BaseReturnModel validResult = Validate(createParam);

            if (!validResult.IsSuccess)
            {
                return validResult;
            }

            var insertModel = createParam.CastByJson<LiveGameManage>();

            insertModel.LotteryId = createParam.LotteryId;
            insertModel.Url = createParam.Url.ToNonNullString();
            insertModel.ApiUrl = createParam.ApiUrl.ToNonNullString();
            insertModel.ProductCode = createParam.ProductCode.ToNonNullString();
            insertModel.GameCode = createParam.GameCode.ToNonNullString();
            insertModel.RemoteCode = createParam.RemoteCode.ToNonNullString();

            if (createParam.ImageFile != null)
            {
                insertModel.ImageUrl = UploadLiveGameAESToOSS(createParam.ImageFile);
            }

            BaseReturnDataModel<long> insertResult = _liveGameManageRep.Value.CreateByProcedure(insertModel);

            if (!insertResult.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(createParam, new LiveGameManage()),
                ActTypes.Insert);

            CreateOperationLog(PermissionElement.Insert, createParam.LotteryType, compareContent, isUpdateImage: false);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Update(LiveGameManageUpdateParam updateParam)
        {
            BaseReturnModel validResult = Validate(updateParam);

            if (!validResult.IsSuccess)
            {
                return validResult;
            }

            LiveGameManage originModel = _liveGameManageRep.Value.GetDetail(updateParam.No);
            bool isUploadNewImage = updateParam.ImageFile != null;

            if (isUploadNewImage)
            {
                updateParam.ImageUrl = UploadLiveGameAESToOSS(updateParam.ImageFile);
            }
            else
            {
                updateParam.ImageUrl = originModel.ImageUrl;
            }

            string compareContent =
                GetOperationCompareContent(GetRecordCompareParams(updateParam, originModel), ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            var targetLiveGameManage = updateParam.CastByJson<LiveGameManage>();
            targetLiveGameManage.Url = updateParam.Url.ToNonNullString();
            targetLiveGameManage.ApiUrl = updateParam.ApiUrl.ToNonNullString();

            bool updateResult = _liveGameManageRep.Value.UpdateByProcedure(targetLiveGameManage);

            if (!updateResult)
            {
                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            CreateOperationLog(PermissionElement.Edit, updateParam.LotteryType, compareContent, isUploadNewImage);

            if (isUploadNewImage && !originModel.ImageUrl.IsNullOrEmpty())
            {
                _processObjectStorageService.Value.DeleteImageObject(_ossSettingService.Value.GetOSSClientSetting(), originModel.ImageUrl);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Delete(int no)
        {
            LiveGameManage model = _liveGameManageRep.Value.GetSingleByKey(InlodbType.Inlodb, new LiveGameManage { No = no });

            bool deleteResult = _liveGameManageRep.Value.DeleteByProcedure(model);

            if (!deleteResult)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Delete, model.LotteryType, memo: string.Empty, isUpdateImage: false);
            _processObjectStorageService.Value.DeleteImageObject(_ossSettingService.Value.GetOSSClientSetting(), model.ImageUrl);

            return new BaseReturnModel(ReturnCode.Success);
        }

        private BaseReturnModel Validate(BaseLiveGameManageParam param)
        {
            if (param.LotteryType.IsNullOrEmpty())
            {
                return new BaseReturnModel(string.Format(MessageElement.FieldIsNotAllowEmpty, DisplayElement.Name));
            }

            var gameDataType = LiveGameManageDataType.GetSingle(param.LiveGameManageDataTypeValue);

            if (gameDataType == LiveGameManageDataType.GameCenter || gameDataType == LiveGameManageDataType.DirectPlay)
            {
                if (!RequiredUtil.IsValidRequired(param.ProductCode))
                {
                    return new BaseReturnModel(string.Format(MessageElement.FieldIsNotAllowEmpty, DisplayElement.ThirdPartyOwnership));
                }

                if (gameDataType == LiveGameManageDataType.DirectPlay)
                {
                    if (!RequiredUtil.IsValidRequired(param.RemoteCode))
                    {
                        return new BaseReturnModel(string.Format(MessageElement.FieldIsNotAllowEmpty, DisplayElement.GameCode));
                    }
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string UploadLiveGameAESToOSS(IFormFile imageFile)
        {
            string imageUrl = string.Empty;

            string extRaw = Path.GetExtension(imageFile.FileName);
            string ext = extRaw.ToLower();
            string fileName = imageFile.FileName.Replace(ext, ".aes");
            var tool = new AESImageTool();
            byte[] aesBytes = tool.AesEncryptionBytes(imageFile.ToBytes());

            var updateImageOSSParams = new List<UpdateImageOSSParam>()
                {
                    new UpdateImageOSSParam
                    {
                        KeyID = Guid.NewGuid().ToString(),
                        Application = EnvLoginUser.Application,
                        ImageFileName = fileName,
                        ImageBytes = aesBytes,
                        UploadOSSRemotePath = _ossDirectoryPath,
                    }
                };

            Action<int, string> uploadCallback = (index, fullFileName) =>
            {
                if (!fullFileName.IsNullOrEmpty())
                {
                    imageUrl = fullFileName;
                }
            };

            _processObjectStorageService.Value.UploadToOss(
                _ossSettingService.Value.GetOSSClientSetting(),
                updateImageOSSParams,
                uploadCallback,
                out List<string> deleteFullObjectNames);

            return imageUrl;
        }

        private void CreateOperationLog(string actionName, string gameName, string memo, bool isUpdateImage)
        {
            if (isUpdateImage)
            {
                memo = string.Join(", ", memo, DisplayElement.UpdateGameImage);
            }

            string content = string.Format(BWOperationLogElement.LiveGameManageMessage,
                actionName,
                gameName,
                memo);

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = _permissionKey,
                Content = content
            });
        }

        private List<RecordCompareParam> GetRecordCompareParams(BaseLiveGameManageParam newModel, LiveGameManage originModel)
        {
            ISearchLiveGameManageDataType originModelDataType = new BaseLiveGameManageParam
            {
                ProductCode = originModel.ProductCode.ToNonNullString(),
                LotteryId = originModel.LotteryId,
                RemoteCode = originModel.RemoteCode.ToNonNullString(),
                GameCode = originModel.GameCode.ToNonNullString(),
            };

            var records = new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Name,
                    OriginValue = originModel.LotteryType,
                    NewValue = newModel.LotteryType,
                },
                new RecordCompareParam
                {
                    Title = "类型",
                    OriginValue = LiveGameManageDataType.GetName(originModelDataType),
                    NewValue = LiveGameManageDataType.GetName(newModel.LiveGameManageDataTypeValue),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.GameImage,
                    OriginValue = originModel.ImageUrl,
                    NewValue = newModel.ImageUrl,
                    IsVisibleCompareValue = false,
                },
            };

            if (newModel.LiveGameManageDataTypeValue == LiveGameManageDataType.MiseLottery)
            {
                records.AddRange(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.GameID,
                        OriginValue = originModel.LotteryId.ToNonNullString(),
                        NewValue = newModel.LotteryId.ToNonNullString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "URL",
                        OriginValue = originModel.Url.ToNonNullString(),
                        NewValue = newModel.Url.ToNonNullString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "API URL",
                        OriginValue = originModel.ApiUrl.ToNonNullString(),
                        NewValue = newModel.ApiUrl.ToNonNullString(),
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.FrameRatio,
                        OriginValue = originModel.FrameRatio.ToString(),
                        NewValue = newModel.FrameRatio.ToString(),
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Style,
                        OriginValue = originModel.Style.ToString(),
                        NewValue = newModel.Style.ToString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "倒计时",
                        OriginValue = originModel.IsCountdown.GetYesNoText(),
                        NewValue = newModel.IsCountdown.GetYesNoText(),
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Duration,
                        OriginValue = originModel.Duration.ToString(),
                        NewValue = newModel.Duration.ToString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "保持状态 H5用",
                        OriginValue = originModel.IsH5.GetYesNoText(),
                        NewValue = newModel.IsH5.GetYesNoText(),
                    },
                    new RecordCompareParam
                    {
                        Title = "跟投",
                        OriginValue = originModel.IsFollow.GetActionText(),
                        NewValue = newModel.IsFollow.GetActionText(),
                    },
                });
            }

            if (newModel.LiveGameManageDataTypeValue == LiveGameManageDataType.GameCenter ||
                newModel.LiveGameManageDataTypeValue == LiveGameManageDataType.DirectPlay)
            {
                records.AddRange(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.ThirdPartyOwnership,
                        OriginValue = originModel.ProductCode.ToNonNullString(),
                        NewValue = newModel.ProductCode.ToNonNullString(),
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.GameCategory,
                        OriginValue = originModel.GameCode.ToNonNullString(),
                        NewValue = newModel.GameCode.ToNonNullString(),
                    },
                });

                if (newModel.LiveGameManageDataTypeValue == LiveGameManageDataType.DirectPlay)
                {
                    records.Add(new RecordCompareParam
                    {
                        Title = DisplayElement.GameCode,
                        OriginValue = originModel.RemoteCode,
                        NewValue = newModel.RemoteCode,
                    });
                }
            }

            records.AddRange(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Sort,
                    OriginValue = originModel.Sort.ToString(),
                    NewValue = newModel.Sort.ToNonNullString(),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.IsActiveStatus,
                    OriginValue = originModel.IsActive.GetActionText(),
                    NewValue = newModel.IsActive.GetActionText(),
                },
            });

            return records;
        }

        public void SetDefaultValue(ILiveGameManageSetDefaultParam model)
        {
            if (model.LiveGameManageDataTypeValue == LiveGameManageDataType.GameCenter ||
                model.LiveGameManageDataTypeValue == LiveGameManageDataType.DirectPlay)
            {
                model.LotteryId = 0;
                model.Url = string.Empty;
                model.ApiUrl = string.Empty;
                model.FrameRatio = 0;
                model.Style = 0;
                model.IsCountdown = false;
                model.Duration = 0;
                model.IsH5 = false;
                model.IsFollow = false;

                if (model is ILiveGameManageGridRowSetDefaultParam)
                {
                    var gridRowSetDefaultParam = model as ILiveGameManageGridRowSetDefaultParam;
                    gridRowSetDefaultParam.DurationText = string.Empty;
                    gridRowSetDefaultParam.StyleText = string.Empty;
                    gridRowSetDefaultParam.FrameRatioText = string.Empty;
                    gridRowSetDefaultParam.IsCountdownText = string.Empty;
                    gridRowSetDefaultParam.IsH5Text = string.Empty;
                    gridRowSetDefaultParam.IsFollowText = string.Empty;
                }
            }
            else if (model.LiveGameManageDataTypeValue == LiveGameManageDataType.MiseLottery)
            {
                model.ProductCode = string.Empty;
                model.GameCode = string.Empty;
                model.RemoteCode = string.Empty;
            }
        }
    }
}