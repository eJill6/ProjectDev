using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Param.PublishRecord;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models;
using BackSideWeb.Models.ViewModel.PublishRecord;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminUserManager;
using NuGet.Packaging;
using MS.Core.MMModel.Models.User.Enums;
using System.Text;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using Newtonsoft.Json;
using BackSideWeb.Models.Enums;
using MS.Core.Models.Models;
using BackSideWeb.Filters;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.MessageQueue;
using MS.Core.MM.Models.AdminPost;
using System.ComponentModel;

namespace BackSideWeb.Controllers.PostRecord
{
    /// <summary>
    /// 官方发帖记录控制器
    /// </summary>
    public class OfficialPublishRecordController : BaseCRUDController<PublishRecordParam, AdminPostDetailInputModel, ExaminePostData>
    {
        public OfficialPublishRecordController()
        {
        }

        protected override string[] PageJavaScripts => new string[]
        {
            "business/publishRecord/officialPublishRecordSearchParam.min.js",
            "business/publishRecord/officialPublishRecordSearchService.min.js"
        };

        protected override string ClientServiceName => "officialPublishRecordSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.OfficialPostRecord;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.OfficialPublishPostRecord;

        public override ActionResult GetGridViewResult(PublishRecordParam searchParam)
        {
            AdminPostListParam param = new AdminPostListParam();
            param.Status = (MS.Core.MMModel.Models.Post.Enums.ReviewStatus?)searchParam.Status;
            param.PageSize = searchParam.PageSize;
            param.BeginDate = searchParam.BeginDate;
            param.EndDate = searchParam.EndDate.Date.AddDays(1);
            param.Title = searchParam.Title;
            param.PageNo = searchParam.PageNo;
            param.PostId = searchParam.PostId;
            param.SortType = searchParam.SortType;
            param.UserId = searchParam.UserId;
            param.DateTimeType = searchParam.DateTimeType;
            param.PostType = MS.Core.MMModel.Models.Post.Enums.PostType.Official;
            param.UserIdentity=searchParam.UserIdentity;

            ViewBag.HasEditPermission = HasPermission(PermissionKeys.PostRecord, AuthorityTypes.Edit);
            PagedResultModel<AdminOfficialPostList> pagePublishRecordVmModel = new PagedResultModel<AdminOfficialPostList>();
            string controller = "AdminOfficialPost";
            string action = "List";
            string parameStr = JsonConvert.SerializeObject(param);
            var result = MMClientApi.PostApi<AdminOfficialPostList>(controller, action, parameStr);
            pagePublishRecordVmModel.PageSize = 10;
            if (result != null && result.IsSuccess)
            {
                if (result.DataModel != null)
                {
                    pagePublishRecordVmModel.ResultList = result.DataModel.Data.ToList();
                    pagePublishRecordVmModel.TotalPageCount = result.DataModel.TotalPage;
                    pagePublishRecordVmModel.PageSize = result.DataModel.PageSize;
                    pagePublishRecordVmModel.PageNo = result.DataModel.Page;
                    pagePublishRecordVmModel.TotalCount = result.DataModel.TotalCount;
                }
            }
            pagePublishRecordVmModel.ResultList = pagePublishRecordVmModel.ResultList.Select(item =>
            {
                if (item.CreateTime == item.UpdateTime)
                {
                    item.UpdateTime = null;
                }
                return item;
            }).ToList();
            return PartialView(pagePublishRecordVmModel);
        }

        public override ActionResult Index()
        {
            PublishRecordOptionViewModel optionSelected = new PublishRecordOptionViewModel();
            optionSelected.PostStatus = MMSelectListItem.GetEnumItemsDefaultNull<PostStatusEnum>(SelectEnum.All,0);
            //optionSelected.PostStatus = new List<SelectListItem>()
            //{
            //    new SelectListItem()
            //    {
            //        Text="审核中",
            //        Value="0",
            //        Selected=true,
            //    },
            //    new SelectListItem(){
            //        Text="全部",
            //        Value=null,
            //    },
            //    new SelectListItem()
            //    {
            //        Text="展示中",
            //        Value="1",
            //    },
            //    new SelectListItem()
            //    {
            //        Text="未通过",
            //        Value="2",
            //    }
            //};
            optionSelected.TimeType = MMSelectListItem.GetReviewTimeTypeItems();

            optionSelected.IdentityTypeItems = GetIdentityItems();

            return View(optionSelected);
        }
        public List<SelectListItem> GetIdentityItems()
        {
            var identityItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var identityDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, IdentityType>();
            identityItems.AddRange(identityDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            List<string> valuesToRemove = new List<string> { "觅女郎", "星觅官", "一般", "觅经纪" };
            identityItems.RemoveAll(item => valuesToRemove.Contains(item.Text));
            return identityItems;
        }
        private IActionResult GetEditView<T>(T model)
        {
            //此範例為新增與修改共用同一個view
            return View("Edit", model);
        }

        private IActionResult OpenInsertView<T>(T model)
        {
            return View("AddInsert", model);
        }

        /// <summary>
        /// 套餐服务校验
        /// </summary>
        /// <param name="insertModel"></param>
        /// <returns></returns>
        public BaseReturnModel ValidateCombos(AdminPostDetailInputModel insertModel)
        {
            int[] comboNumbers = { 1, 2, 3 };

            foreach (int comboNumber in comboNumbers)
            {
                string comboName = (string)typeof(AdminPostDetailInputModel).GetProperty($"ComboName{comboNumber}")?.GetValue(insertModel);
                decimal? comboPrice = (decimal?)typeof(AdminPostDetailInputModel).GetProperty($"ComboPrice{comboNumber}")?.GetValue(insertModel);
                string comboService = (string)typeof(AdminPostDetailInputModel).GetProperty($"ComboService{comboNumber}")?.GetValue(insertModel);

                if (!string.IsNullOrEmpty(comboName) || comboPrice.HasValue || !string.IsNullOrEmpty(comboService))
                {
                    if (string.IsNullOrEmpty(comboName))
                    {
                        return new BaseReturnModel(string.Format(MessageElement.ComboNameNotNull, comboNumber));
                    }

                    if (!comboPrice.HasValue)
                    {
                        return new BaseReturnModel(string.Format(MessageElement.ComboPriceNotNull, comboNumber));
                    }
                    bool isGreaterThan100 = comboPrice >= 100 && comboPrice == Math.Floor(comboPrice.Value) && comboPrice < 99999;
                    if (!isGreaterThan100)
                    {
                        return new BaseReturnModel(string.Format(MessageElement.ComboPricePricePositiveInteger, comboNumber));
                    }

                    if (string.IsNullOrEmpty(comboService))
                    {
                        return new BaseReturnModel(string.Format(MessageElement.ComboTimeAndTimes_CannotBeEmpty, comboNumber));
                    }
                }
            }

            return null;
        }

        private BaseReturnModel ValidateData(AdminPostDetailInputModel insertModel)
        {
            #region 数据验证逻辑

            if (insertModel.UserId == null || !insertModel.UserId.HasValue)
            {
                return new BaseReturnModel("请填写所属的用户ID!");
            }

            var userinfo = GetUsersInfo(insertModel.UserId.Value.ToString());
            if (!userinfo.IsSuccess || userinfo.DataModel == null)
            {
                return new BaseReturnModel("无此会员ID，请再次确认");
            }
            //官方只能觅老板发帖
            if (userinfo.DataModel.UserIdentity != IdentityType.Boss && userinfo.DataModel.UserIdentity != IdentityType.SuperBoss)
            {
                return new BaseReturnModel("此会员身份无法发布指定区域的帖子");
            }

            if (userinfo.DataModel == null)
            {
                return new BaseReturnModel("此用户ID不存在!");
            }
            if (userinfo.DataModel.PostRemain < 1 && userinfo.DataModel.UserIdentity != IdentityType.Boss && userinfo.DataModel.UserIdentity != IdentityType.SuperBoss)
            {
                return new BaseReturnModel("此会员ID剩余发帖次数不足");
            }
            if (string.IsNullOrWhiteSpace(insertModel.Title))
            {
                return new BaseReturnModel("帖子标题不可为空!");
            }
            if (insertModel.AreaCode == null || insertModel.AreaCode == "00")
            {
                return new BaseReturnModel("请选择所在地区!");
            }
            if (insertModel.Age == 0)
            {
                return new BaseReturnModel("请选择年龄!");
            }
            if (insertModel.Height == 0)
            {
                return new BaseReturnModel("请选择身高!");
            }
            if (insertModel.Cup == 0)
            {
                return new BaseReturnModel("请选择罩杯!");
            }
            if (string.IsNullOrEmpty(insertModel.BusinessHours))
            {
                return new BaseReturnModel("请输入营业时间!");
            }
            if (insertModel.BusinessHours.Length > 15)
            {
                return new BaseReturnModel("营业时间上限15字");
            }
            if (string.IsNullOrEmpty(insertModel.ServiceIdStr) || insertModel.ServiceIdStr.Split(',').Length == 0)
            {
                return new BaseReturnModel("服务项目至少选择一项");
            }
            //服务价格请至少填写一组
            if (string.IsNullOrEmpty(insertModel.ComboName1)
                && string.IsNullOrEmpty(insertModel.ComboName2)
                && string.IsNullOrEmpty(insertModel.ComboName3)
                && !insertModel.ComboPrice1.HasValue
                && !insertModel.ComboPrice2.HasValue
                && !insertModel.ComboPrice3.HasValue
                && string.IsNullOrEmpty(insertModel.ComboService1)
                && string.IsNullOrEmpty(insertModel.ComboService2)
                && string.IsNullOrEmpty(insertModel.ComboService3))
            {
                return new BaseReturnModel(MessageElement.ComboFillInAtLeastOneSet);
            }
            var validate = ValidateCombos(insertModel);
            if (validate != null)
            {
                return validate;
            }

            if (insertModel.ServiceDescribe == null)
            {
                return new BaseReturnModel("请输入服务描述!");
            }
            if (string.IsNullOrEmpty(insertModel.PhotoIdsStr) || insertModel.PhotoIdsStr.Split(",").Length < 1)
            {
                return new BaseReturnModel("请至少上传一张照片!");
            }
            return new BaseReturnModel()
            {
                IsSuccess = true,
                Message = "验证通过"
            };

            #endregion 数据验证逻辑
        }

        protected override BaseReturnModel DoInsert(AdminPostDetailInputModel insertModel)
        {
            var validateResult = ValidateData(insertModel);
            if (validateResult != null && !validateResult.IsSuccess)
            {
                return validateResult;
            }

            #region 数据字段处理

            AdminPostInsertData addPostModel = new AdminPostInsertData();
            addPostModel.UserId = insertModel.UserId;
            addPostModel.PostType = (int)MS.Core.MMModel.Models.Post.Enums.PostType.Official;
            addPostModel.Title = insertModel.Title;
            addPostModel.AreaCode = insertModel.AreaCode;
            addPostModel.Age = insertModel.Age;
            addPostModel.Height = insertModel.Height;
            addPostModel.Cup = insertModel.Cup;
            addPostModel.BusinessHours = insertModel.BusinessHours;
            //服务项目
            insertModel.ServiceIdStr = insertModel.ServiceIdStr?.Substring(0, insertModel.ServiceIdStr.Length - 1);
            addPostModel.ServiceIds = new int[insertModel.ServiceIdStr.Split(',').Length];
            for (int i = 0; i < insertModel.ServiceIdStr.Split(',').Length; i++)
            {
                addPostModel.ServiceIds[i] = int.Parse(insertModel.ServiceIdStr.Split(',')[i]);
            }
            //服务价格
            List<AdminComboData> comboDataList = new List<AdminComboData>();
            string[] comboNames = { insertModel.ComboName1, insertModel.ComboName2, insertModel.ComboName3 };
            string[] comboServices = { insertModel.ComboService1, insertModel.ComboService2, insertModel.ComboService3 };
            decimal?[] comboPrices = { insertModel.ComboPrice1, insertModel.ComboPrice2, insertModel.ComboPrice3 };

            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(comboNames[i]) && !string.IsNullOrEmpty(comboServices[i]) && comboPrices[i].HasValue)
                {
                    comboDataList.Add(new AdminComboData { ComboName = comboNames[i], ComboPrice = comboPrices[i].Value, Service = comboServices[i] });
                }
            }
            addPostModel.Combo = comboDataList.ToArray();
            addPostModel.ServiceDescribe = insertModel.ServiceDescribe;
            addPostModel.PhotoIds = insertModel.PhotoIdsStr?.Substring(0, insertModel.PhotoIdsStr.Length - 1).Split(',')?.ToArray();
            addPostModel.VideoIds = insertModel.VideoIdsStr?.Substring(0, insertModel.VideoIdsStr.Length - 1).Split(',')?.ToArray();

            #endregion 数据字段处理

            #region 数据提交

            string parame = JsonConvert.SerializeObject(addPostModel);
            var result = MMClientApi.PostObjectApi("OfficialPost", $"AddPost", parame);
            if (result.IsSuccess)
            {
                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Insert,
                        IsLogTitleValue = true
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.PublishPostRecord,
                        OriginValue = addPostModel.Title,
                        IsLogTitleValue = true
                    },
                }, ActTypes.Update);

                if (string.IsNullOrEmpty(compareContent))
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                CreateOperationLog(compareContent, _permissionKey);

                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "操作成功"
                };
            }
            else
            {
                return new BaseReturnModel("新增失败!");
            }

            #endregion 数据提交
        }

        public ApiResult<QueryPostWeightModel> GetPostWeightData()
        {
            var result = MMClientApi.GetApi<QueryPostWeightModel>("PostWeight", "List");
            return result;
        }

        protected override BaseReturnModel DoUpdate(ExaminePostData updateModel)
        {
            //修改前数据
            var source = GetAdminOfficialPostDetail(updateModel.PostId);

            #region 数据校验

            updateModel.Title = source.Data.Title;
            //if (string.IsNullOrWhiteSpace(updateModel.Title))
            //{
            //    return new BaseReturnModel("帖子标题不可为空");
            //}
            updateModel.BusinessHours = source.Data.BusinessHours;
            //if (string.IsNullOrWhiteSpace(updateModel.BusinessHours))
            //{
            //    return new BaseReturnModel("营业时间不可为空");
            //}

            //if (updateModel.BusinessHours.Length > 10)
            //{
            //    return new BaseReturnModel("营业时间上限10字");
            //}
            updateModel.ServiceDescribe = source.Data.ServiceDescribe;
            //if (string.IsNullOrWhiteSpace(updateModel.ServiceDescribe))
            //{
            //    return new BaseReturnModel("服务描述不可为空");
            //}

            if (updateModel.PostStatus != 1 && updateModel.PostStatus != 2)
            {
                return new BaseReturnModel("请选择审核状态");
            }
            if (updateModel.PostStatus == 2)
            {
                updateModel.IsHomePost = source.Data.IsHomePost;
                updateModel.Weight = source.Data.Weight;
                updateModel.ViewBaseCount = source.Data.ViewBaseCount;
            }
            else
            {
                if (updateModel.ViewBaseCount == null)
                {
                    return new BaseReturnModel("观看基础值不可为空");
                }
                if (updateModel.ViewBaseCount <= 0 && updateModel.ViewBaseCount > 999999)
                {
                    return new BaseReturnModel("观看基础值请输入0 ~ 999999间正整数");
                }
            }

            #endregion 数据校验

            string parame = JsonConvert.SerializeObject(updateModel);

            if (updateModel.IsHomePost && !source.Data.IsHomePost)
            {
                var postWeight = GetPostWeightData();
                var listCount = postWeight.Datas?.Where(r => r.PostType == updateModel.PostType).ToList().Count();
                if (listCount >= 50)
                {
                    return new BaseReturnModel($"首页贴【{updateModel.PostType.GetDescription()}】达设定上限50笔");
                }
            }

            var result = MMClientApi.PostObjectApi("AdminOfficialPost", $"Edit/{updateModel.PostId}", parame);
            if (result.IsSuccess)
            {
                #region 日志记录

                if (source.IsSuccess && source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = DisplayElement.PostId,
                            OriginValue = source.Data.PostId,
                            IsLogTitleValue = true,
                        },
                        new RecordCompareParam
                        {
                            Title = "信息标题",
                            OriginValue = source.Data.Title,
                            NewValue = updateModel.Title
                        },
                        new RecordCompareParam
                        {
                            Title = "营业时间",
                            OriginValue = source.Data.BusinessHours,
                            NewValue = updateModel.BusinessHours
                        },
                        new RecordCompareParam
                        {
                            Title = "服务描述",
                            OriginValue = source.Data.ServiceDescribe,
                            NewValue = updateModel.ServiceDescribe
                        },
                        new RecordCompareParam
                        {
                            Title = "首页贴",
                            OriginValue = source.Data.IsHomePost==true?"开":"关",
                            NewValue = updateModel.IsHomePost==true?"开":"关"
                        },
                        new RecordCompareParam
                        {
                            Title = "权重",
                            OriginValue = source.Data.Weight.ToString(),
                            NewValue = updateModel.Weight.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "观看基础值",
                            OriginValue = source.Data.ViewBaseCount.ToString(),
                            NewValue = updateModel.ViewBaseCount.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.Review,
                            OriginValue = source.Data.Status.GetDescription(),
                            NewValue = ((JxBackendService.Model.Enums.ReviewStatus)updateModel.PostStatus).GetDescription()
                        },
                         new RecordCompareParam
                        {
                            Title = DisplayElement.ReasonForFailure,
                            OriginValue = source.Data.Memo,
                            NewValue = updateModel.Memo
                        },
                    }, ActTypes.Update);

                    if (string.IsNullOrWhiteSpace(compareContent))
                    {
                        return new BaseReturnModel(ReturnCode.Success);
                    }
                    CreateOperationLog(compareContent, _permissionKey);
                }

                #endregion 日志记录

                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "操作成功"
                };
            }
            else
            {
                return new BaseReturnModel()
                { IsSuccess = false, Message = "操作失败!" };
            }
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            throw new NotImplementedException();
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            SetPageTitle($"帖子审核");
            ApiResponse<AdminOfficialPostDetail> aRequestResult = GetAdminOfficialPostDetail(keyContent);

            return GetEditView(aRequestResult.Data);
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            ApiResponse<AdminOfficialPostDetail> aRequestResult = GetAdminOfficialPostDetail(keyContent);
            return View(aRequestResult.Data);
        }

        private ApiResponse<AdminOfficialPostDetail> GetAdminOfficialPostDetail(string keyContent)
        {
            ApiResponse<AdminOfficialPostDetail> aRequestResult = MMClientApi.GetApiSingle<AdminOfficialPostDetail>("AdminOfficialPost", "Detail", keyContent);
            return aRequestResult;
        }

        protected override IActionResult GetInsertView()
        {
            AdminPostDetailInputModel model = GetPostDetailInput();

            return OpenInsertView(model);
        }

        protected AdminPostDetailInputModel GetPostDetailInput()
        {
            AdminPostDetailInputModel model = new AdminPostDetailInputModel();

            #region 获取服务项目复选框

            model.ServiceItem = new List<OptionItem>();
            Dictionary<string, string> dicServiceItemParamer = new Dictionary<string, string>();
            dicServiceItemParamer.Add("postType", ((int)MS.Core.MMModel.Models.Post.Enums.PostType.Official).ToString());
            ApiResult<OptionItem> serviceItemParamerResult = MMClientApi.GetApiByDictionary<OptionItem>("Settings", "Services", dicServiceItemParamer);
            if (serviceItemParamerResult.IsSuccess && serviceItemParamerResult.Datas?.Count > 0)
            {
                model.ServiceItem.AddRange(serviceItemParamerResult.Datas);
            }

            #endregion 获取服务项目复选框

            #region 获取省份下拉列表框

            model.ProvinceItem = new List<SelectListItem>();
            model.ProvinceItem.Add(new SelectListItem() { Text = "请选择", Value = "00" });

            List<ProvinceInfo> provinceInfos = AppHelper.ReadAppSetting<ProvinceInfo>("ProvinceData");
            if (provinceInfos != null && provinceInfos.Count > 0)
            {
                model.ProvinceItem.AddRange(provinceInfos.Select(p => new SelectListItem() { Text = p.Name, Value = p.Province }));
            }

            #endregion 获取省份下拉列表框

            #region 获取年龄下拉列表框

            model.AgeItem = new List<SelectListItem>();
            model.AgeItem.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            model.AgeItem.AddRange(Enum.GetValues(typeof(AgeDefined)).Cast<AgeDefined>().Select(p => new SelectListItem
            {
                Value = ((int)p).ToString(),
                Text = p.GetDescription()
            }).ToList());

            #endregion 获取年龄下拉列表框

            #region 获取身高下拉列表框

            model.HeightItem = new List<SelectListItem>();
            model.HeightItem.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            model.HeightItem.AddRange(Enum.GetValues(typeof(BodyHeightDefined)).Cast<BodyHeightDefined>().Select(p => new SelectListItem
            {
                Value = ((int)p).ToString(),
                Text = p.GetDescription()
            }).ToList());

            #endregion 获取身高下拉列表框

            #region 获取罩杯下拉列表框

            model.CupItem = new List<SelectListItem>();
            model.CupItem.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            model.CupItem.AddRange(Enum.GetValues(typeof(CupDefined)).Cast<CupDefined>().Select(p => new SelectListItem
            {
                Value = ((int)p).ToString(),
                Text = p.GetDescription()
            }).ToList());

            #endregion 获取罩杯下拉列表框

            return model;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private BaseReturnDataModel<AdminUserManagerUsersDetail> GetUsersInfo(string userId)
        {
            BaseReturnDataModel<AdminUserManagerUsersDetail> baseReturnData = MMClientApi.GetApiSingleBaseReturn<AdminUserManagerUsersDetail>("AdminUserManager", "UserDetail", userId);
            return baseReturnData;
        }

        [HttpPost]
        public ApiResponse<MediaViewModel> SplitUpload(AdminPostDetailInputModel insertModel)
        {
            try
            {
                int currentChunk = Convert.ToInt32(base.Request.Form["currentChunk"]);
                if (currentChunk == 0)
                {
                    var validateResult = ValidateData(insertModel);
                    if (validateResult != null && !validateResult.IsSuccess)
                    {
                        return new ApiResponse<MediaViewModel>
                        {
                            IsSuccess = false,
                            Message = validateResult.Message
                        };
                    }
                }

                IFormFile file = base.Request.Form.Files[0];

                int totalChunks = Convert.ToInt32(base.Request.Form["totalChunks"]);
                if (file != null && file.Length > 0)
                {
                    string suffix = Path.GetExtension(file.FileName).Trim('.');
                    byte[] bytes;
                    using (Stream stream = file.OpenReadStream())
                    {
                        using MemoryStream memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                    if (bytes.Length > 1048576 && totalChunks <= 50)
                    {
                        return new ApiResponse<MediaViewModel>
                        {
                            IsSuccess = false,
                            Message = "视频格式或档案大小不符"
                        };
                    }
                    SaveMediaToOssParamForClient saveMediaToOssParamForClient = new SaveMediaToOssParamForClient();
                    saveMediaToOssParamForClient.FileName = "videoAgentPost_" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".mp4";
                    saveMediaToOssParamForClient.SourceType = SourceType.Post;
                    saveMediaToOssParamForClient.MediaType = 1;
                    saveMediaToOssParamForClient.Bytes = bytes.ToArray();
                    string parame = JsonConvert.SerializeObject(saveMediaToOssParamForClient);
                    ApiResponse<string> chunkUrl = MMClientApi.PostRequest("Media", "SplitUpload", JsonConvert.SerializeObject(saveMediaToOssParamForClient));
                    if (!chunkUrl.IsSuccess)
                    {
                        return new ApiResponse<MediaViewModel>
                        {
                            IsSuccess = false,
                            Message = "上传失败：" + chunkUrl.Message
                        };
                    }
                    ApiResponse<MediaViewModel> result = new ApiResponse<MediaViewModel>();
                    result.IsSuccess = true;
                    result.Data = new MediaViewModel
                    {
                        FullMediaUrl = chunkUrl.Data
                    };
                    return result;
                }
                return new ApiResponse<MediaViewModel>
                {
                    IsSuccess = false
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MediaViewModel>
                {
                    IsSuccess = false,
                    Message = "上传失败：" + ex.Message
                };
            }
        }

        [HttpPost]
        public ApiResponse<MediaViewModel> MergeUpload()
        {
            try
            {
                string paths = base.Request.Form["paths"].ToString();
                MergeUpload mergeUpload = new MergeUpload();
                if (paths.Length > 0)
                {
                    mergeUpload.Suffix = "videoAgentPost_" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".mp4";
                    mergeUpload.SourceType = SourceType.Post;
                    mergeUpload.MediaType = MediaType.Video;
                    mergeUpload.Paths = (from s in paths.Split(',')
                                         where !string.IsNullOrEmpty(s)
                                         select s).ToArray();
                    string parame = JsonConvert.SerializeObject(mergeUpload);
                    return MMClientApi.PostRequest<MediaViewModel>("Media", "MergeUpload", parame);
                }
                return new ApiResponse<MediaViewModel>
                {
                    IsSuccess = false
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MediaViewModel>
                {
                    IsSuccess = false,
                    Message = "上传失败：" + ex.Message
                };
            }
        }

        /// <summary>
        /// 取得上傳網址及資料
        /// </summary>
        /// <returns>上傳網址及資料</returns>
        [HttpGet]
        public ApiSingleResult<VideoUrlModel> GetUploadVideoUrl()
        {
            var result = MMClientApi.GetSingleApi<VideoUrlModel>("Media", "GetUploadVideoUrl");
            return result;
        }

        [HttpPost]
        public JsonResult EditLock(string postId, bool status)
        {
            AdminOfficialPostEditLockData officialPostEditLockData = new AdminOfficialPostEditLockData();
            officialPostEditLockData.LockStatus = status;
            string parame = JsonConvert.SerializeObject(officialPostEditLockData);
            var result = MMClientApi.PostObjectApi("AdminOfficialPost", $"EditLock/{postId}", parame);

            return Json(result);
        }

        public IActionResult OfficialPostEdit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("帖子编辑");
            ViewBag.SubmitUrl = "DoOfficialPostEdit";
            ViewBag.ClientPopupWindowServiceName = "officialPublishEditSingleRowService";

            ApiResponse<AdminOfficialPostDetail> aRequestResult = GetAdminOfficialPostDetail(keyContent);

            ViewBag.officialPostDetail = aRequestResult.Data;
            AdminPostDetailInputModel model = GetPostDetailInput();

            var citydata = AppHelper.ReadAppSetting<CityInfo>("CityData").Where(a => a.Name == aRequestResult.Data.AreaCode).SingleOrDefault();
            var provincedata = model.ProvinceItem?.Where(a => a.Value == citydata?.Province).SingleOrDefault();
            ViewBag.cityCode = citydata?.Code;
            ViewBag.provinceCode = provincedata?.Value;

            return View(model);
        }

        [HttpPost]
        public BaseReturnModel DoOfficialPostEdit(AdminPostDetailInputModel insertModel)
        {
            var validateResult = ValidateData(insertModel);
            if (validateResult != null && !validateResult.IsSuccess)
            {
                return validateResult;
            }

            if (string.IsNullOrWhiteSpace(insertModel.PostId))
            {
                return new BaseReturnModel("帖子ID不能为空！");
            }

            #region 数据字段处理

            AdminPostInsertData addPostModel = new AdminPostInsertData();
            addPostModel.UserId = insertModel.UserId;
            addPostModel.PostType = (int)MS.Core.MMModel.Models.Post.Enums.PostType.Official;
            addPostModel.Title = insertModel.Title;
            addPostModel.AreaCode = insertModel.AreaCode;
            addPostModel.Age = insertModel.Age;
            addPostModel.Height = insertModel.Height;
            addPostModel.Cup = insertModel.Cup;
            addPostModel.BusinessHours = insertModel.BusinessHours;

            //服务项目
            insertModel.ServiceIdStr = insertModel.ServiceIdStr?.Substring(0, insertModel.ServiceIdStr.Length - 1);
            addPostModel.ServiceIds = new int[insertModel.ServiceIdStr.Split(',').Length];
            for (int i = 0; i < insertModel.ServiceIdStr.Split(',').Length; i++)
            {
                addPostModel.ServiceIds[i] = int.Parse(insertModel.ServiceIdStr.Split(',')[i]);
            }
            //服务价格
            List<AdminComboData> comboDataList = new List<AdminComboData>();
            string[] comboNames = { insertModel.ComboName1, insertModel.ComboName2, insertModel.ComboName3 };
            string[] comboServices = { insertModel.ComboService1, insertModel.ComboService2, insertModel.ComboService3 };
            decimal?[] comboPrices = { insertModel.ComboPrice1, insertModel.ComboPrice2, insertModel.ComboPrice3 };

            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(comboNames[i]) && !string.IsNullOrEmpty(comboServices[i]) && comboPrices[i].HasValue)
                {
                    comboDataList.Add(new AdminComboData { ComboName = comboNames[i], ComboPrice = comboPrices[i].Value, Service = comboServices[i] });
                }
            }
            addPostModel.Combo = comboDataList.ToArray();
            addPostModel.ServiceDescribe = insertModel.ServiceDescribe;
            addPostModel.PhotoIds = insertModel.PhotoIdsStr?.Substring(0, insertModel.PhotoIdsStr.Length - 1).Split(',')?.ToArray();
            addPostModel.VideoIds = insertModel.VideoIdsStr?.Substring(0, insertModel.VideoIdsStr.Length - 1).Split(',')?.ToArray();

            #endregion 数据字段处理

            #region 数据提交

            string parame = JsonConvert.SerializeObject(addPostModel);
            var result = MMClientApi.PostObjectApi("OfficialPost", $"EditPost/{insertModel.PostId}", parame);
            if (result.IsSuccess)
            {
                if (insertModel.Status == MS.Core.MMModel.Models.Post.Enums.ReviewStatus.Approval)
                {
                    //修改前数据
                    var source = GetAdminOfficialPostDetail(insertModel.PostId);
                    ExaminePostData updateModel = new ExaminePostData();
                    updateModel.Title = source.Data.Title;
                    updateModel.BusinessHours = source.Data.BusinessHours;
                    updateModel.ServiceDescribe = source.Data.ServiceDescribe;
                    updateModel.IsHomePost = source.Data.IsHomePost;
                    updateModel.Weight = source.Data.Weight;
                    updateModel.ViewBaseCount = source.Data.ViewBaseCount;
                    updateModel.PostStatus = (int)MS.Core.MMModel.Models.Post.Enums.ReviewStatus.UnderReview;

                    string updataJson = JsonConvert.SerializeObject(updateModel);
                    var editResult = MMClientApi.PostObjectApi("AdminOfficialPost", $"Edit/{insertModel.PostId}", updataJson);
                    if (editResult.IsSuccess)
                    {
                        return new BaseReturnModel()
                        {
                            IsSuccess = true,
                            Message = "操作成功"
                        };
                    }
                    return new BaseReturnModel("修改失败!");
                }
                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "操作成功"
                };
            }
            else
            {
                return new BaseReturnModel("修改失败!");
            }

            #endregion 数据提交
        }

        [HttpPost]
        public JsonResult ModifyDeleteStatus(string postId, int? userid, int? isDelete)
        {
            var OfficialPostDelete = new
            {
                PostIds = new string[] { postId },
                UserId = userid,
                IsDelete = isDelete
            };
            string parame = JsonConvert.SerializeObject(OfficialPostDelete);
            var result = MMClientApi.PostObjectApi("AdminOfficialPost", $"DeleteOfficialPost", parame);
            return Json(result);
        }
    }
}