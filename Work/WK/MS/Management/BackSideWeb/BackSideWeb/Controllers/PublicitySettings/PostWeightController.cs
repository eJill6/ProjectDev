using Amazon.Auth.AccessControlPolicy;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using BackSideWeb.Models.ViewModel.PublishRecord;
using Castle.Core.Internal;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.User.Enums;
using Newtonsoft.Json;

namespace BackSideWeb.Controllers
{
    public class PostWeightController : BaseCRUDController<QueryPostWeightParam, MMPostWeightBs, MMPostWeightBs>
    {
        private const int _maxPostWeight = 9999;
        private const int _variable = 5;

        protected override string[] PageJavaScripts => new string[]
        {
                    "business/publicitySettings/postWeightSearchService.min.js"
        };

        protected override string ClientServiceName => "postWeightSearchService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.PostWeight;

        public ApiResult<QueryPostWeightModel> GetData()
        {
            var result = MMClientApi.GetApi<QueryPostWeightModel>("PostWeight", "List");
            return result;
        }

        public ApiResult<QueryGoldStoreModel> GetGoldStoreData(int type)
        {
            var result = MMClientApi.GetApi<QueryGoldStoreModel>("GoldStore", "List");
            result.Datas = result.Datas.Where(a => a.Type == type).ToList();
            return result;
        }

        public override ActionResult GetGridViewResult(QueryPostWeightParam searchParam)
        {
            var result = GetData();
            PagedResultModel<QueryPostWeightModel> model = new PagedResultModel<QueryPostWeightModel>();
            var resultList = result.Datas.Where(r => r.PostType == searchParam.PostType).ToList();
            model.ResultList = resultList;
            model.TotalCount = resultList.Count;
            model.PageSize = searchParam.PageSize;
            model.PageNo = searchParam.PageNo;
            return PartialView(model);
        }

        public IActionResult EditGoldStore()
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("金牌店铺");
            ViewBag.SubmitUrl = "DoEditGoldStore";
            ViewBag.ClientPopupWindowServiceName = "postWeightEditSingleRowService";

            ApiResult<QueryGoldStoreModel> storeDatas = GetGoldStoreData(2);
            ViewBag.StoreDatas = storeDatas.Datas;

            return View();
        }

        public IActionResult EditGoldStoreRecommend()
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("官方店铺推荐");
            ViewBag.SubmitUrl = "DoEditGoldStore";
            ViewBag.ClientPopupWindowServiceName = "postWeightEditSingleRowService";

            ApiResult<QueryGoldStoreModel> storeDatas = GetGoldStoreData(1);
            ViewBag.StoreDatas = storeDatas.Datas;

            return View();
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
        public BaseReturnModel DoEditGoldStore(GoldStoreInputModel editModel)
        {
            List<MMGoldStoreBs> editData = new List<MMGoldStoreBs>();
            var userIds = new HashSet<int>();

            for (int i = 0; i < 3; i++)
            {
                int? userId = null;
                if (editModel.TopUserIds.Any())
                {
                    if (editModel.TopUserIds.Length <= i || editModel.TopUserIds[i] == null)
                    {
                        userId = null;
                    }
                    else
                    {
                        userId = Convert.ToInt32(editModel.TopUserIds[i]);
                        var userinfo = GetUsersInfo(userId.ToString());
                        if (!userinfo.IsSuccess || userinfo.DataModel == null)
                        {
                            return new BaseReturnModel($"查无 NO.{i + 1} 会员ID，请再确认");
                        }
                        if (userIds.Contains(userId.Value))
                        {
                            return new BaseReturnModel($"NO.{i + 1}会员ID重复，请再确认");
                        }
                        else
                        {
                            userIds.Add(userId.Value);
                        }

                        if (userinfo.DataModel.UserIdentity != IdentityType.Boss && userinfo.DataModel.UserIdentity != IdentityType.SuperBoss)
                        {
                            return new BaseReturnModel($"NO.{i + 1} 非觅老板 or 超觅老板请再确认");
                        }
                    }
                }

                editData.Add(new MMGoldStoreBs()
                {
                    Top = i + 1,
                    UserId = userId,
                    Type = editModel.Type
                });
            }
            string parame = JsonConvert.SerializeObject(editData);
            var result = MMClientApi.PostObjectApi("GoldStore", $"Update", parame);
            if (!result.IsSuccess)
            {
                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "操作失败"
                };
            }
            return new BaseReturnModel()
            {
                IsSuccess = true,
                Message = "操作成功"
            };
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostWeight;

        protected override IActionResult GetInsertView()
        {
            string maxPostWeight = string.Empty;
            try
            {
                maxPostWeight = GetData().Datas.Max(x => Convert.ToInt32(x.Weight)).ToString();
                if (Convert.ToInt32(maxPostWeight) + _variable > _maxPostWeight)
                    maxPostWeight = _maxPostWeight.ToString();
                else
                    maxPostWeight = (Convert.ToInt32(maxPostWeight) + 5).ToString();
            }
            catch (Exception ex)
            {
                maxPostWeight = string.Empty;
            }
            return GetEditView(new MMPostWeightBs() { Weight = maxPostWeight });
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var response = MMClientApi.GetSingleApi<MMPostWeightBs>("PostWeight", "Detail", keyContent);
            return GetEditView(response.Datas);
        }

        protected override BaseReturnModel DoInsert(MMPostWeightBs model)
        {
            string errorMsg = ValidateInput(model);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);
            model.Operator = ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName;
            model.CreateTime = DateTime.Now;
            var result = MMClientApi.PostApi("PostWeight", "Create", model);
            string errorMessage = GetErrorMessageFromResult(result);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new BaseReturnModel(errorMessage);
            }

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Insert,
                    IsLogTitleValue = true
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.PostId,
                    OriginValue = model.PostId,
                    IsLogTitleValue = true
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            CreateOperationLog(compareContent, _permissionKey);

            return new BaseReturnModel();
        }

        protected override BaseReturnModel DoUpdate(MMPostWeightBs model)
        {
            string errorMsg = ValidateInput(model);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            var source = MMClientApi.GetSingleApi<MMPostWeightBs>("PostWeight", "Detail", model.Id.ToString());

            model.Operator = ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName;
            var result = MMClientApi.PostApi("PostWeight", "Update", model);
            string errorMessage = GetErrorMessageFromResult(result);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new BaseReturnModel(errorMessage);
            }

            if (source != null)
            {
                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.PostId,
                        OriginValue = source.Datas?.PostId,
                        IsLogTitleValue = true,
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Weights,
                        OriginValue = source.Datas?.Weight,
                        NewValue = model.Weight
                    },
                }, ActTypes.Update);

                if (string.IsNullOrWhiteSpace(compareContent))
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }
                CreateOperationLog(compareContent, _permissionKey);
            }

            return new BaseReturnModel();
        }

        private string GetErrorMessageFromResult(ApiResult<MMPostWeightBs> result)
        {
            if (result.IsSuccess == false)
            {
                if (result.Code == "D10101")
                {
                    return "无此帖子，请再次确认";
                }
                else if (result.Code == "D10102")
                {
                    return "帖子重复，请再次确认";
                }
                else if (result.Code == "D10111")
                {
                    return "首页帖【广场】达设定上限50笔";
                }
                else if (result.Code == "D10112")
                {
                    return "首页帖【寻芳阁】达设定上限50笔";
                }
                else if (result.Code == "D10113")
                {
                    return "首页帖【官方】达设定上限50笔";
                }
                //else if (result.Code == "D10103")
                //{
                //    return "权重重复，请再次确认";
                //}
                else
                {
                    return $"失敗。{result.Message}";
                }
            }
            return string.Empty;
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            var source = MMClientApi.GetSingleApi<MMPostWeightBs>("PostWeight", "Detail", keyContent);

            var result = MMClientApi.PostApi("PostWeight", "Delete", keyContent);

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Delete,
                    IsLogTitleValue = true
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.PostId,
                    OriginValue = source.Datas?.PostId,
                    IsLogTitleValue = true
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            CreateOperationLog(compareContent, _permissionKey);

            return new BaseReturnModel(result.Message);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        [HttpPost]
        public BaseReturnModel BatchReview(string[] postIds)
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

            updateModel.PostIds = string.Join(",", postIds);
            string parame = JsonConvert.SerializeObject(updateModel);

            var result = MMClientApi.PostObjectApi("PostWeight", $"PostWeightBatchRemove", parame);
            if (!result.IsSuccess)
            {
                return new BaseReturnModel()
                {
                    IsSuccess = false,
                    Code = ReturnCode.SystemError.ToString(),
                    Message = $"批量移除失败"
                };
            }

            //string originValue = $"发帖ID：{updateModel.PostIds}{((MS.Core.MMModel.Models.Post.Enums.ReviewStatus)updateModel.PostStatus).GetDescription()}";
            string originValue = $"发帖ID：{updateModel.PostIds}";
            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = "批量移除",
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

        public static string ValidateInput(MMPostWeightBs model)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(model.PostId))
                return "帖子ID不可为空";
            if (model.Weight == null)
                return "权重不可为空";
            if (!MMValidate.IsNumeric(model.PostId))
                return "仅允许输入数字";
            if (!MMValidate.IsExistIntRange(model.Weight, 1, 9999))
                return "权重仅允许数字 1 ~ 9999";
            return result;
        }
    }
}