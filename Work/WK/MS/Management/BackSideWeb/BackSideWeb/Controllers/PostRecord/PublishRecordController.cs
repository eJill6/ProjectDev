using Amazon.Runtime.Internal.Transform;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Param.PublishRecord;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel.PublishRecord;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Enums.MM;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.SystemSettings;
using JxBackendService.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using MS.Core.MMModel.Models.Media.Enums;

namespace BackSideWeb.Controllers.PostRecord
{
    /// <summary>
    /// 发表记录控制器
    /// </summary>
    public class PublishRecordController : BaseCRUDController<PublishRecordParam, AdminPostDetailInputModel, ExaminePostData>
    {
        private readonly Lazy<IUserInfoRelatedService> _userInfoRelatedService;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.PublishPostRecord;

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostRecord;

        private readonly IOptionsMonitor<List<ProvinceInfo>> _provinceInfo;

        public PublishRecordController(IOptionsMonitor<List<ProvinceInfo>> provinceInfo)
        {
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Slave);
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave);
            _provinceInfo = provinceInfo;
        }

        protected override string[] PageJavaScripts => new string[]
        {
            "business/publishRecord/publishRecordSearchParam.min.js",
            "business/publishRecord/publishRecordSearchService.min.js"
        };

        protected override string ClientServiceName => "publishRecordSearchService";

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
            param.PostType = (MS.Core.MMModel.Models.Post.Enums.PostType?)searchParam.PostType;

            ViewBag.HasEditPermission = HasPermission(PermissionKeys.PostRecord, AuthorityTypes.Edit);
            PagedResultModel<AdminPostList> pagePublishRecordVmModel = new PagedResultModel<AdminPostList>();
            string controller = "AdminPost";
            string action = "List";
            string parameStr = JsonConvert.SerializeObject(param);
            var result = MMClientApi.PostApi<AdminPostList>(controller, action, parameStr);
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
            string postid = HttpContext.Request.Query["postid"];

            PublishRecordOptionViewModel optionSelected = new PublishRecordOptionViewModel();
            optionSelected.PostRegionalListItem = MMSelectListItem.GetEnumItemsDefaultNull<SquareXFG>(SelectEnum.All);
            //    new List<SelectListItem>()
            //{
            //     new SelectListItem(){
            //        Text="全部",
            //        Value=null,
            //        Selected=true,
            //    },
            //    new SelectListItem()
            //    {
            //        Text="广场",
            //        Value="1",
            //    },
            //    new SelectListItem()
            //    {
            //        Text="寻芳阁",
            //        Value="2",
            //    }
            //};
            bool isSelected = string.IsNullOrWhiteSpace(postid);

            if (isSelected)
            {
                optionSelected.PostStatus = MMSelectListItem.GetEnumItemsDefaultNull<PostStatusEnum>(SelectEnum.All, 0);
            }
            else
            {
                optionSelected.PostStatus = MMSelectListItem.GetEnumItemsDefaultNull<PostStatusEnum>(SelectEnum.All);
            }
            //optionSelected.PostStatus = new List<SelectListItem>()
            //{
            //    new SelectListItem()
            //    {
            //        Text = "审核中",
            //        Value = "0",
            //        Selected = isSelected,
            //    },
            //    new SelectListItem()
            //    {
            //        Text = "全部",
            //        Value = null,
            //        Selected = !isSelected,
            //    },

            //    new SelectListItem()
            //    {
            //        Text = "展示中",
            //        Value = "1",
            //    },
            //    new SelectListItem()
            //    {
            //        Text = "未通过",
            //        Value = "2",
            //    }
            //};

            optionSelected.TimeType = MMSelectListItem.GetEnumItems<TimeTypeEnum>();

            if (!string.IsNullOrWhiteSpace(postid))
            {
                ViewBag.PostId = postid;
            }
            return View(optionSelected);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        private IActionResult OpenInsertView<T>(T model)
        {
            return View("AddInsert", model);
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

            if (userinfo.DataModel == null)
            {
                return new BaseReturnModel("此用户ID不存在!");
            }
            //编辑不验证的部分
            if (string.IsNullOrWhiteSpace(insertModel.PostId))
            {
                //广场只有觅老板不能发帖
                if (insertModel.PostType == (int)MS.Core.MMModel.Models.Post.Enums.PostType.Square && (userinfo.DataModel.UserIdentity == IdentityType.Boss || userinfo.DataModel.UserIdentity == IdentityType.SuperBoss))
                {
                    return new BaseReturnModel("此会员身份无法发布指定区域的帖子");
                }
                //寻芳阁只能觅老板 觅经纪发帖
                if (insertModel.PostType == (int)MS.Core.MMModel.Models.Post.Enums.PostType.Agency && !(userinfo.DataModel.UserIdentity == IdentityType.Agent || userinfo.DataModel.UserIdentity == IdentityType.Boss || userinfo.DataModel.UserIdentity == IdentityType.SuperBoss))
                {
                    return new BaseReturnModel("此会员身份无法发布指定区域的帖子");
                }
                if (string.IsNullOrWhiteSpace(userinfo.DataModel.VipCards) && userinfo.DataModel.UserIdentity == IdentityType.General)
                {
                    return new BaseReturnModel("此会员ID不具备会员卡，且不具备身份");
                }
                if (userinfo.DataModel.PostRemain < 1 &&
                    userinfo.DataModel.UserIdentity == IdentityType.Agent &&
                    insertModel.PostType == (int)MS.Core.MMModel.Models.Post.Enums.PostType.Square)
                {
                    return new BaseReturnModel("此会员ID剩余发帖次数不足");
                }
                if (userinfo.DataModel.PostRemain < 1 &&
                    (userinfo.DataModel.UserIdentity == IdentityType.Boss || userinfo.DataModel.UserIdentity==IdentityType.SuperBoss) &&
                   insertModel.PostType == (int)MS.Core.MMModel.Models.Post.Enums.PostType.Agency)
                {
                    return new BaseReturnModel("此会员ID剩余发帖次数不足");
                }
            }
            if (insertModel.PostType == 0)
            {
                return new BaseReturnModel("请选择帖子分区!");
            }
            if (insertModel.MessageId == 0)
            {
                return new BaseReturnModel("请选择信息类型!");
            }
            if (insertModel.IsApply == true && (insertModel.ApplyAmount == null || insertModel.ApplyAmount == 0))
            {
                return new BaseReturnModel("请选择预设价格或者选择会员申请调价!");
            }
            if (string.IsNullOrWhiteSpace(insertModel.Title))
            {
                return new BaseReturnModel("帖子标题不可为空!");
            }
            if (insertModel.AreaCode == null || insertModel.AreaCode == "00")
            {
                return new BaseReturnModel("请选所在地区!");
            }
            if (insertModel.Quantity == null)
            {
                return new BaseReturnModel("请填写数量!");
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
            if (insertModel.LowPrice == null)
            {
                return new BaseReturnModel("请输入最低的服务价格");
            }
            if (insertModel.LowPrice < 100)
            {
                return new BaseReturnModel("最低价格不得低于100");
            }
            if (insertModel.HighPrice == null)
            {
                return new BaseReturnModel("请输入最高的服务价格");
            }
            if (insertModel.LowPrice > insertModel.HighPrice)
            {
                return new BaseReturnModel("最低服务价格必须小于最高服务价格");
            }
            if (insertModel.Address == null)
            {
                return new BaseReturnModel("详细地址不能为空!");
            }
            if (string.IsNullOrEmpty(insertModel.WeChat) && string.IsNullOrEmpty(insertModel.QQ) && string.IsNullOrEmpty(insertModel.Phone))
            {
                return new BaseReturnModel("联系方式请至少填写一项!");
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
            addPostModel.PostType = insertModel.PostType;
            addPostModel.MessageId = insertModel.MessageId;
            addPostModel.ApplyAmount = insertModel.ApplyAmount;
            addPostModel.Title = insertModel.Title;
            addPostModel.AreaCode = insertModel.AreaCode;
            addPostModel.Quantity = insertModel.Quantity;
            addPostModel.Age = insertModel.Age;
            addPostModel.Height = insertModel.Height;
            addPostModel.Cup = insertModel.Cup;
            addPostModel.BusinessHours = insertModel.BusinessHours;
            //服务项目
            insertModel.ServiceIdStr = insertModel.ServiceIdStr.Substring(0, insertModel.ServiceIdStr.Length - 1);
            addPostModel.ServiceIds = new int[insertModel.ServiceIdStr.Split(',').Length];
            for (int i = 0; i < insertModel.ServiceIdStr.Split(',').Length; i++)
            {
                addPostModel.ServiceIds[i] = int.Parse(insertModel.ServiceIdStr.Split(',')[i]);
            }
            addPostModel.LowPrice = insertModel.LowPrice;
            addPostModel.HighPrice = insertModel.HighPrice;
            addPostModel.Address = insertModel.Address;

            List<ContactInfo> contactInfos = new List<ContactInfo>();
            if (!string.IsNullOrWhiteSpace(insertModel.WeChat))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.Weixin;
                contactInfo.Contact = insertModel.WeChat;
                contactInfos.Add(contactInfo);
            }
            if (!string.IsNullOrWhiteSpace(insertModel.QQ))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.QQ;
                contactInfo.Contact = insertModel.QQ;
                contactInfos.Add(contactInfo);
            }
            if (!string.IsNullOrWhiteSpace(insertModel.Phone))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.Phone;
                contactInfo.Contact = insertModel.Phone;
                contactInfos.Add(contactInfo);
            }
            addPostModel.ContactInfos = contactInfos.ToArray();

            addPostModel.ServiceDescribe = insertModel.ServiceDescribe;
            addPostModel.PhotoIds = (insertModel.PhotoIdsStr?.TrimEnd(',') ?? "").Split(',');
            addPostModel.VideoIds = (insertModel.VideoIdsStr?.TrimEnd(',') ?? "").Split(',');

            addPostModel.UserId = insertModel.UserId;

            #endregion 数据字段处理

            #region 数据提交

            string parame = JsonConvert.SerializeObject(addPostModel);
            var result = MMClientApi.PostObjectApi("Square", $"AddPost", parame);
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
                return new BaseReturnModel("新增失败");
            }

            #endregion 数据提交
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

        public ApiResult<QueryPostWeightModel> GetPostWeightData()
        {
            var result = MMClientApi.GetApi<QueryPostWeightModel>("PostWeight", "List");
            return result;
        }

        protected override BaseReturnModel DoUpdate(ExaminePostData updateModel)
        {
            #region 数据验证逻辑

            if (string.IsNullOrEmpty(updateModel.Title))
            {
                return new BaseReturnModel("帖子标题不可为空");
            }
            if (string.IsNullOrEmpty(updateModel.BusinessHours))
            {
                return new BaseReturnModel("营业时间不可为空");
            }
            if (updateModel.BusinessHours.Length > 15)
            {
                return new BaseReturnModel("营业时间上限15字");
            }
            if (string.IsNullOrEmpty(updateModel.Address))
            {
                return new BaseReturnModel("详细地址不可为空");
            }
            if (string.IsNullOrEmpty(updateModel.QQ) &&
                string.IsNullOrEmpty(updateModel.Phone) &&
                string.IsNullOrEmpty(updateModel.Weixin))
            {
                return new BaseReturnModel("联系方式请至少填写一项");
            }
            if (string.IsNullOrEmpty(updateModel.ServiceDescribe))
            {
                return new BaseReturnModel("服务描述不可为空");
            }
            if (updateModel.IsHomePost && updateModel.Weight == null)
            {
                return new BaseReturnModel("权重不可为空");
            }
            if (updateModel.IsHomePost && updateModel.Weight <= 0)
            {
                return new BaseReturnModel("权重最小值为1");
            }
            if (updateModel.Weight == null)
            {
                updateModel.Weight = 1;
            }

            if (updateModel.UnlockBaseCount == null)
            {
                return new BaseReturnModel("解锁基础值不可为空");
            }
            if (updateModel.UnlockBaseCount <= 0 && updateModel.UnlockBaseCount > 999999)
            {
                return new BaseReturnModel("解锁基础值请输入0 ~ 999999间正整数");
            }
            if (updateModel.ViewBaseCount == null)
            {
                return new BaseReturnModel("观看基础值不可为空");
            }
            if (updateModel.ViewBaseCount <= 0 && updateModel.ViewBaseCount > 999999)
            {
                return new BaseReturnModel("观看基础值请输入0 ~ 999999间正整数");
            }
            if (updateModel.PostStatus != 1 && updateModel.PostStatus != 2)
            {
                return new BaseReturnModel("请选择审核状态");
            }
            if (!string.IsNullOrEmpty(updateModel.Memo) && updateModel.Memo.Length > 500)
            {
                return new BaseReturnModel("未通过原因不能超过500字符!");
            }

            if (updateModel.PostType == MS.Core.MMModel.Models.Post.Enums.PostType.Agency)
            {
                updateModel.IsFeatured = false;
            }

            #endregion 数据验证逻辑

            #region 联系方式

            List<ContactInfo> contactInfos = new List<ContactInfo>();
            if (!string.IsNullOrWhiteSpace(updateModel.Weixin))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.Weixin;
                contactInfo.Contact = updateModel.Weixin;
                contactInfos.Add(contactInfo);
            }
            if (!string.IsNullOrWhiteSpace(updateModel.QQ))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.QQ;
                contactInfo.Contact = updateModel.QQ;
                contactInfos.Add(contactInfo);
            }
            if (!string.IsNullOrWhiteSpace(updateModel.Phone))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.Phone;
                contactInfo.Contact = updateModel.Phone;
                contactInfos.Add(contactInfo);
            }
            updateModel.ContactInfos = JsonUtil.ToJsonString(contactInfos);

            #endregion 联系方式

            var source = GetAdminPostDetail(updateModel.PostId);
            if (updateModel.IsHomePost && !source.Data.IsHomePost)
            {
                var postWeight = GetPostWeightData();
                var listCount = postWeight.Datas?.Where(r => r.PostType == updateModel.PostType).ToList().Count();
                if (listCount >= 50)
                {
                    return new BaseReturnModel($"首页贴【{updateModel.PostType.GetDescription()}】达设定上限50笔");
                }
            }
            string parame = JsonConvert.SerializeObject(updateModel);
            var result = MMClientApi.PostObjectApi("AdminPost", $"Edit/{updateModel.PostId}", parame);
            if (result.IsSuccess)
            {
                #region 日志记录

                if (source != null)
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
                            Title = DisplayElement.PriceAdjustment,
                            OriginValue = source.Data.ApplyAdjustPrice == true ? "是" : "否",
                            NewValue = updateModel.IsApplyAdjustPrice == true ? "是" : "否"
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
                            Title = "详细地址",
                            OriginValue = source.Data.Address,
                            NewValue = updateModel.Address
                        },
                        new RecordCompareParam
                        {
                            Title = "QQ",
                            OriginValue = source.Data.UnlockInfo.ContactInfos?.Where(a=>a.ContactType==ContactType.QQ).SingleOrDefault()?.Contact,
                            NewValue = updateModel.QQ
                        },
                        new RecordCompareParam
                        {
                            Title = "微信",
                            OriginValue = source.Data.UnlockInfo.ContactInfos?.Where(a=>a.ContactType==ContactType.Weixin).SingleOrDefault()?.Contact,
                            NewValue = updateModel.Weixin
                        },
                        new RecordCompareParam
                        {
                            Title = "Phone",
                            OriginValue = source.Data.UnlockInfo.ContactInfos?.Where(a=>a.ContactType==ContactType.Phone).SingleOrDefault()?.Contact,
                            NewValue = updateModel.Phone
                        },
                        new RecordCompareParam
                        {
                            Title = "服务描述",
                            OriginValue = source.Data.ServiceDescribe,
                            NewValue = updateModel.ServiceDescribe
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.Featured,
                            OriginValue = source.Data.IsFeatured==true?"开":"关",
                            NewValue = updateModel.IsFeatured==true?"开":"关"
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
                            Title = DisplayElement.Review,
                            OriginValue = source.Data.Status.GetDescription(),
                            NewValue = ((JxBackendService.Model.Enums.ReviewStatus)updateModel.PostStatus).GetDescription()
                        },
                        new RecordCompareParam
                        {
                            Title = "解锁基础值",
                            OriginValue = source.Data.UnlockBaseCount.ToString(),
                            NewValue = updateModel.UnlockBaseCount.ToString()
                        },
                             new RecordCompareParam
                        {
                            Title = "观看基础值",
                            OriginValue = source.Data.ViewBaseCount.ToString(),
                            NewValue = updateModel.ViewBaseCount.ToString()
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

        private ApiResponse<AdminPostDetail> GetAdminPostDetail(string keyContent)
        {
            ApiResponse<AdminPostDetail> aRequestResult = MMClientApi.GetApiSingle<AdminPostDetail>("AdminPost", "Detail", keyContent);

            // 获取预设价格信息
            decimal unlockAmount = MMGlobalSettings.BaseUnlockAmountSetting
                .SingleOrDefault(a => a.PostType == (MS.Core.MMModel.Models.Post.Enums.PostType)Enum.Parse(typeof(MS.Core.MMModel.Models.Post.Enums.PostType), aRequestResult.Data.PostType.ToString()))
                .UnlockAmount;

            aRequestResult.Data.DefaultPricing = unlockAmount;

            return aRequestResult;
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            SetPageTitle($"帖子审核");
            AdminPostDetail adminPostDetail = GetAdminPostDetail(keyContent).Data;
            return GetEditView(adminPostDetail);
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            AdminPostDetail adminPostDetail = GetAdminPostDetail(keyContent).Data;
            return View(adminPostDetail);
        }

        protected override IActionResult GetInsertView()
        {
            AdminPostDetailInputModel model = SelectListData();

            return OpenInsertView(model);
        }

        [HttpGet]
        public ApiResponse<List<CityInfo>> GetCityInfoData(string provinceId)
        {
            ApiResponse<List<CityInfo>> response = new ApiResponse<List<CityInfo>>();
            if (!string.IsNullOrWhiteSpace(provinceId))
            {
                List<CityInfo> result = new List<CityInfo>();
                result.Add(new CityInfo() { Name = "请选择", Code = "00" });
                if (!provinceId.Equals("00"))
                {
                    List<CityInfo> cityInfos = AppHelper.ReadAppSetting<CityInfo>("CityData");
                    result = cityInfos.Where(s => s.Province.Equals(provinceId)).ToList();
                    response.Data = result;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = result;
                    response.IsSuccess = true;
                }
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }

        /// <summary>
        /// 帖子分区/信息类型/预设价格联动
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<OptionItem> GetPostTypeData(int postType)
        {
            ApiResult<OptionItem> response = new ApiResult<OptionItem>();
            if (postType != 0)
            {
                //获取信息类型
                Dictionary<string, string> dicMessageTypeParamer = new Dictionary<string, string>();
                dicMessageTypeParamer.Add("postType", postType.ToString());
                var messageType = MMClientApi.GetApiByDictionary<OptionItem>("Settings", "MessageType", dicMessageTypeParamer).Datas?.Select(s => new OptionItem() { Key = s.Key, Value = s.Value, Type = "MessageType" });

                //获取服务项目
                var services = MMClientApi.GetApiByDictionary<OptionItem>("Settings", "Services", dicMessageTypeParamer).Datas?.Select(s => new OptionItem() { Key = s.Key, Value = s.Value, Type = "Services" });

                //获取调价信息
                var priceResult = MMClientApi.GetApiByDictionary<OptionItem>("Settings", "Price", dicMessageTypeParamer).Datas?.Select(s => new OptionItem() { Key = s.Key, Value = s.Value, Type = "Price" }); ;

                //获取预设价格信息
                var unlockAmount = MMGlobalSettings.BaseUnlockAmountSetting.Where(a => a.PostType == (MS.Core.MMModel.Models.Post.Enums.PostType)Enum.Parse(typeof(MS.Core.MMModel.Models.Post.Enums.PostType), postType.ToString())).Select(s => new OptionItem() { Key = (int)s.PostType, Value = s.UnlockAmount.ToString(), Type = "UnlockAmount" });

                response.IsSuccess = true;
                response.Datas = (List<OptionItem>?)services.Concat(messageType).Concat(priceResult).Concat(unlockAmount).ToList();
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }

        [HttpPost]
        public BaseReturnModel BatchReview(string[] postIds, string type)
        {
            if (postIds.Length == 0)
            {
                return new BaseReturnModel("未选择帖子，请再确认");
            }
            if (postIds.Length > 50)
            {
                return new BaseReturnModel("批量审核最大同时50笔,请再确认");
            }
            AdminPostBatchData updateModel = new AdminPostBatchData();
            if (type == "display")
            {
                updateModel.PostStatus = 1;
            }
            else if (type == "failed")
            {
                updateModel.PostStatus = 2;
            }

            updateModel.PostIds = string.Join(",", postIds);
            string parame = JsonConvert.SerializeObject(updateModel);

            var result = MMClientApi.PostObjectApi("AdminPost", $"BatchReview", parame);
            if (!result.IsSuccess)
            {
                return new BaseReturnModel()
                {
                    IsSuccess = false,
                    Code = ReturnCode.SystemError.ToString(),
                    Message = $"批量审核失败"
                };
            }

            string originValue = $"发帖ID：{updateModel.PostIds}{((MS.Core.MMModel.Models.Post.Enums.ReviewStatus)updateModel.PostStatus).GetDescription()}";
            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = "批量审核",
                    OriginValue = originValue,
                    IsLogTitleValue = true,
                },
            }, ActTypes.Update);

            if (string.IsNullOrWhiteSpace(compareContent))
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

        /// <summary>
        /// 照片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse<MediaViewModel> PhotoUpload(string photoBaseStr)
        {
            if (!string.IsNullOrWhiteSpace(photoBaseStr))
            {
                //获取图片后缀
                string suffix = photoBaseStr.Substring(photoBaseStr.IndexOf('/') + 1, photoBaseStr.IndexOf(';') - photoBaseStr.IndexOf('/') - 1);
                photoBaseStr = photoBaseStr.Substring(photoBaseStr.IndexOf(',') + 1);

                byte[] bytes = Convert.FromBase64String(photoBaseStr);

                if (bytes.Length > 5 * 1024 * 1024)
                {
                    // 图片大小超过5MB的处理逻辑
                    return new ApiResponse<MediaViewModel>() { IsSuccess = false, Message = "上传的图片大小不能超过5MB" };
                }

                SaveMediaToOssParamForClient saveMediaToOssParamForClient = new SaveMediaToOssParamForClient();
                saveMediaToOssParamForClient.FileName = $"photoAgentPost_{DateTime.Now.ToString("yyyyMMddHHmmssss")}.{suffix}";
                saveMediaToOssParamForClient.SourceType = SourceType.Post;
                saveMediaToOssParamForClient.MediaType = 0;
                saveMediaToOssParamForClient.Bytes = Convert.FromBase64String(photoBaseStr);
                string parame = JsonConvert.SerializeObject(saveMediaToOssParamForClient);
                ApiResponse<MediaViewModel> result = MMClientApi.PostRequest<MediaViewModel>("Media", "Create", parame);

                return result;
            }
            else
            {
                return new ApiResponse<MediaViewModel>() { IsSuccess = false };
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

        public IActionResult PostEdit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("编辑");
            ViewBag.SubmitUrl = "DoPostEdit";
            ViewBag.ClientPopupWindowServiceName = "publishEditSingleRowService";

            ApiResponse<AdminPostDetail> apiResponse = GetAdminPostDetail(keyContent);

            ViewBag.postDetail = apiResponse.Data;
            AdminPostDetailInputModel model = SelectListData();

            var citydata = AppHelper.ReadAppSetting<CityInfo>("CityData").Where(a => a.Name == apiResponse.Data.AreaCode).SingleOrDefault();
            var provincedata = model.ProvinceItem?.Where(a => a.Value == citydata?.Province).SingleOrDefault();
            ViewBag.cityCode = citydata?.Code;
            ViewBag.provinceCode = provincedata?.Value;

            return View(model);
        }

        [HttpPost]
        public BaseReturnModel DoPostEdit(AdminPostDetailInputModel insertModel)
        {
            var validateResult = ValidateData(insertModel);
            if (validateResult != null &&
                !validateResult.IsSuccess)
            {
                return validateResult;
            }

            if (string.IsNullOrWhiteSpace(insertModel.PostId))
            {
                return new BaseReturnModel("帖子ID不能为空！");
            }

            #region 数据字段处理

            ExaminePostData addPostModel = new ExaminePostData();
            addPostModel.PostId = insertModel.PostId;
            addPostModel.IsApplyAdjustPrice = insertModel.IsApply.Value;
            addPostModel.IsFeatured = insertModel.IsFeatured;
            addPostModel.Weight = insertModel.Weight;
            addPostModel.IsHomePost = insertModel.IsHomePost;
            addPostModel.PostType = (MS.Core.MMModel.Models.Post.Enums.PostType)insertModel.PostType;
            addPostModel.MessageId = insertModel.MessageId;
            addPostModel.ApplyAmount = insertModel.ApplyAmount;

            addPostModel.Title = insertModel.Title;
            addPostModel.AreaCode = insertModel.AreaCode;
            addPostModel.Quantity = insertModel.Quantity;
            addPostModel.Age = insertModel.Age;
            addPostModel.Height = insertModel.Height;
            addPostModel.Cup = insertModel.Cup;
            addPostModel.BusinessHours = insertModel.BusinessHours;
            //服务项目
            addPostModel.ServiceIds = insertModel.ServiceIdStr;
            addPostModel.LowPrice = insertModel.LowPrice;
            addPostModel.HighPrice = insertModel.HighPrice;
            addPostModel.Address = insertModel.Address;
            addPostModel.IsCertified = insertModel.IsCertified;
            

            List<ContactInfo> contactInfos = new List<ContactInfo>();
            if (!string.IsNullOrWhiteSpace(insertModel.WeChat))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.Weixin;
                contactInfo.Contact = insertModel.WeChat;
                contactInfos.Add(contactInfo);
            }
            if (!string.IsNullOrWhiteSpace(insertModel.QQ))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.QQ;
                contactInfo.Contact = insertModel.QQ;
                contactInfos.Add(contactInfo);
            }
            if (!string.IsNullOrWhiteSpace(insertModel.Phone))
            {
                ContactInfo contactInfo = new ContactInfo();
                contactInfo.ContactType = ContactType.Phone;
                contactInfo.Contact = insertModel.Phone;
                contactInfos.Add(contactInfo);
            }
            addPostModel.ContactInfos = JsonUtil.ToJsonString(contactInfos); ;

            addPostModel.ServiceDescribe = insertModel.ServiceDescribe;
            addPostModel.PhotoIds = insertModel.PhotoIdsStr;
            addPostModel.VideoIds = insertModel.VideoIdsStr;

            addPostModel.UserId = insertModel.UserId;

            #endregion 数据字段处理

            #region 数据提交

            string parame = JsonConvert.SerializeObject(addPostModel);
            var result = MMClientApi.PostObjectApi("AdminPost", $"EditPost/{insertModel.PostId}", parame);

            if (result.IsSuccess)
            {
                //if (insertModel.Status == MS.Core.MMModel.Models.Post.Enums.ReviewStatus.Approval)
                //{
                //    //修改前数据
                //    var source = GetAdminPostDetail(insertModel.PostId);
                //    ExaminePostData updateModel = new ExaminePostData();
                //    updateModel.Title = source.Data.Title;
                //    updateModel.Address = source.Data.Address;
                //    updateModel.BusinessHours = source.Data.BusinessHours;
                //    updateModel.ServiceDescribe = source.Data.ServiceDescribe;
                //    updateModel.IsHomePost = source.Data.IsHomePost;
                //    updateModel.Weight = source.Data.Weight;
                //    updateModel.PostStatus = (int)MS.Core.MMModel.Models.Post.Enums.ReviewStatus.Approval;

                //    string updataJson = JsonConvert.SerializeObject(updateModel);
                //    var editResult = MMClientApi.PostObjectApi("AdminOfficialPost", $"Edit/{insertModel.PostId}", updataJson);
                //    if (editResult.IsSuccess)
                //    {
                //        return new BaseReturnModel()
                //        {
                //            IsSuccess = true,
                //            Message = "操作成功"
                //        };
                //    }
                //    return new BaseReturnModel("修改失败!");
                //}
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

        /// <summary>
        /// 下拉框初始数据
        /// </summary>
        /// <returns></returns>
        private AdminPostDetailInputModel SelectListData()
        {
            AdminPostDetailInputModel model = new AdminPostDetailInputModel();

            #region 帖子分区

            model.PostTypeListItem = MMSelectListItem.GetEnumItemsDefaultNull<SquareXFG>(SelectEnum.Choose);
            //    new List<SelectListItem>()
            //{
            //    new SelectListItem(){
            //        Text="请选择",
            //        Value="0",
            //        Selected=true,
            //    },
            //    new SelectListItem()
            //    {
            //        Text="广场",
            //        Value="1",
            //    },
            //    new SelectListItem()
            //    {
            //        Text="寻芳阁",
            //        Value="2",
            //    }
            //};

            #endregion 帖子分区

            #region 获取消息类型下拉列表框

            //model.MessageTypeItem = new List<SelectListItem>();
            //model.MessageTypeItem.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            //Dictionary<string, string> dicMessageTypeParamer = new Dictionary<string, string>();
            //dicMessageTypeParamer.Add("postType", ((int)MS.Core.MMModel.Models.Post.Enums.PostType.Agency).ToString());
            //ApiResult<OptionItem> messageTypeResult = MMClientApi.GetApiByDictionary<OptionItem>("Settings", "MessageType", dicMessageTypeParamer);
            //if (messageTypeResult.IsSuccess && messageTypeResult.Datas?.Count > 0)
            //{
            //    model.MessageTypeItem.AddRange(messageTypeResult.Datas.Select(p => new SelectListItem() { Text = p.Value, Value = p.Key.ToString() }).ToList());
            //}

            #endregion 获取消息类型下拉列表框

            #region 获取价格设定下拉列表框

            model.ApplyAmountItem = new List<SelectListItem>();
            model.ApplyAmountItem.Add(new SelectListItem() { Text = "请选择", Value = null });
            Dictionary<string, string> dicParamer = new Dictionary<string, string>();
            dicParamer.Add("postType", ((int)MS.Core.MMModel.Models.Post.Enums.PostType.Agency).ToString());
            ApiResult<OptionItem> priceResult = MMClientApi.GetApiByDictionary<OptionItem>("Settings", "Price", dicParamer);
            if (priceResult.IsSuccess && priceResult.Datas?.Count > 0)
            {
                model.ApplyAmountItem.AddRange(priceResult.Datas.Select(p => new SelectListItem() { Text = p.Value, Value = p.Key.ToString() }).ToList());
            }

            #endregion 获取价格设定下拉列表框

            #region 获取服务项目复选框

            model.ServiceItem = new List<OptionItem>();
            Dictionary<string, string> dicServiceItemParamer = new Dictionary<string, string>();
            dicServiceItemParamer.Add("postType", ((int)MS.Core.MMModel.Models.Post.Enums.PostType.Agency).ToString());
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
    }
}