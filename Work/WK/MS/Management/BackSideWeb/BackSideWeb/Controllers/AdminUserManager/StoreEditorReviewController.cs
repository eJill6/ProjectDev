using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.Enums;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.User.Enums;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class StoreEditorReviewController : BaseSearchGridController<AdminBossShopListParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/storeEditorReviewSearchParam.min.js",
            "business/adminUserManager/storeEditorReviewSearchService.min.js"
        };

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.StoreEditorReview;
        protected override string ClientServiceName => "storeEditorReviewSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.StoreEditorReview;

        public override ActionResult Index()
        {
            return View();
        }

        public override ActionResult GetGridViewResult(AdminBossShopListParam searchParam)
        {
            if (searchParam.BeginDate == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "申请开始时间不能为空" });
            }
            if (searchParam.EndDate == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "申请结束时间不能为空" });
            }
            searchParam.EndDate = searchParam.EndDate.Value.AddDays(1);
            var result = MMClientApi.PostApi<AdminBossShopListParam, AdminBossShopList>("AdminBossStore", "StoreEditorList", searchParam);

            return PartialView(result);
        }

        public IActionResult Edit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);

            ViewBag.SubmitUrl = "Audit";
            SetPageTitle("审核");
            ViewBag.ClientPopupWindowServiceName = "storeEditorReviewEditSingleRowService";

            var resultEdit = MMClientApi.GetSingleApi<AdminBossShopList>("AdminBossStore", "BossShopDetail", keyContent).Datas;
            if (resultEdit == null)
            {
                return PartialView(null);
            }
            var resultCurrent = MMClientApi.GetSingleApi<AdminBossShopList>("AdminBossStore", "CurrentBossDetail", resultEdit.Id).Datas;
            ViewBag.UserName = (EnvLoginUser.LoginUser as BackSideWebUser).UserName;
            ViewBag.resultCurrent = resultCurrent;

            return View(resultEdit);
        }

        public JsonResult Audit(AdminUserBossParam param)
        {
            #region 参数校验

            if (string.IsNullOrWhiteSpace(param.ShopName))
            {
                return new JsonResult(new { IsSuccess = false, Message = "店铺名称不可为空" });
            }
            if (param.ShopName.Length > 7)
            {
                return new JsonResult(new { IsSuccess = false, Message = "输入您的店铺名称，最多7个字" });
            }
            if (string.IsNullOrWhiteSpace(param.Introduction))
            {
                return new JsonResult(new { IsSuccess = false, Message = "店铺介绍不可为空" });
            }
            if (param.Introduction.Length > 17)
            {
                return new JsonResult(new { IsSuccess = false, Message = "一句话概述你的店铺，最多17个字" });
            }
            if (param.Girls == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "妹子数量不可为空" });
            }
            if (param.Girls < 0 || param.Girls > 99999)
            {
                return new JsonResult(new { IsSuccess = false, Message = "妹子数量请输入0 ~ 99999间正整数" });
            }
            if (string.IsNullOrWhiteSpace(param.ContactInfo))
            {
                return new JsonResult(new { IsSuccess = false, Message = "QQ号码不可为空" });
            }
            if (param.ContactInfo.Length > 15)
            {
                return new JsonResult(new { IsSuccess = false, Message = "输入您的QQ号码，最多15个字" });
            }
            if (param.DealOrder == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "成交订单不可为空" });
            }
            if (param.DealOrder < 0 || param.DealOrder > 99999)
            {
                return new JsonResult(new { IsSuccess = false, Message = "成交订单请输入0 ~ 99999间正整数" });
            }
            if (param.SelfPopularity == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "自评人气不可为空" });
            }
            if (param.SelfPopularity < 0 || param.SelfPopularity > 99999)
            {
                return new JsonResult(new { IsSuccess = false, Message = "自评人气请输入0 ~ 99999间正整数" });
            }
            if (param.ShopYears == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "店龄不可为空" });
            }
            if (param.ShopYears < 0 || param.ShopYears > 99)
            {
                return new JsonResult(new { IsSuccess = false, Message = "店龄请输入0 ~ 99间正整数" });
            }

            #endregion 参数校验

            var source = MMClientApi.GetSingleApi<AdminBossShopList>("AdminBossStore", "BossShopDetail", param.Id).Datas;

            if (source.ShopName == param.ShopName &&
                source.ShopYears == param.ShopYears &&
                source.Girls == param.Girls &&
                source.DealOrder == param.DealOrder &&
                source.SelfPopularity == param.SelfPopularity &&
                source.Introduction == param.Introduction &&
                source.ContactInfo == param.ContactInfo &&
                (int)source.Status == param.Status &&
                (source.Memo ?? "") == (param.Memo ?? "")
                )
                return new JsonResult(new { IsSuccess = true, Message = "提交成功" });

            var result = MMClientApi.PostApi2("AdminBossStore", "BossShopAudit", param);

            if (result.IsSuccess)
            {
                #region 日志记录
                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = "会员ID",
                            OriginValue = param.UserId.ToString(),
                            IsLogTitleValue = true
                        },
                        new RecordCompareParam
                        {
                            Title = "店铺名称",
                            OriginValue = source.ShopName,
                            NewValue = param.ShopName
                        },
                        new RecordCompareParam
                        {
                            Title = "店铺介绍",
                            OriginValue = source.Introduction,
                            NewValue = param.Introduction
                        },
                        new RecordCompareParam
                        {
                            Title = "妹子数量",
                            OriginValue = source.Girls.ToString(),
                            NewValue = param.Girls.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "QQ号码",
                            OriginValue = source.ContactInfo,
                            NewValue = param.ContactInfo
                        },
                        new RecordCompareParam
                        {
                            Title = "成交订单",
                            OriginValue = source.DealOrder.ToString(),
                            NewValue = param.DealOrder.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "自评人气",
                            OriginValue = source.SelfPopularity.ToString(),
                            NewValue = param.SelfPopularity.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "店龄",
                            OriginValue = source.ShopYears.ToString(),
                            NewValue = param.ShopYears.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "审核",
                            OriginValue = source.StatusText.ToString(),
                            NewValue = param.StatusText.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "备注",
                            OriginValue = source.Memo ?? "",
                            NewValue = param.Memo ?? ""
                        }
                    }, ActTypes.Update);

                    if (string.IsNullOrWhiteSpace(compareContent))
                    {
                        return new JsonResult(new { IsSuccess = result.IsSuccess, Message = result.Message });
                    }
                    CreateOperationLog(compareContent, _permissionKey);
                }
                #endregion 日志记录
            }

            return new JsonResult(new { IsSuccess = result.IsSuccess, Message = result.Message });
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            var resultEdit = MMClientApi.GetSingleApi<AdminBossShopList>("AdminBossStore", "BossShopDetail", keyContent).Datas;
            var resultCurrent = MMClientApi.GetSingleApi<AdminBossShopList>("AdminBossStore", "CurrentBossDetail", resultEdit.Id).Datas;
            ViewBag.resultCurrent = resultCurrent;

            return View(resultEdit);
        }
    }
}