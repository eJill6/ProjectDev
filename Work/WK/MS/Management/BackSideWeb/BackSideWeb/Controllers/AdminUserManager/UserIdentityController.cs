using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.AdminUserManager;
using Newtonsoft.Json;
using JxBackendService.Model.ReturnModel;
using BackSideWeb.Models.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.MMModel.Extensions;
using ReviewStatus = MS.Core.MMModel.Models.Post.Enums.ReviewStatus;
using JxBackendService.Resource.Element;
using System.ComponentModel;
using JxBackendService.Model.ViewModel.Telegram;
using Amazon.Auth.AccessControlPolicy;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Param.User;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class UserIdentityController : BaseSearchGridController<AdminUserManagerIdentityApplyListParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/userIdentityAuditSearchParam.min.js",
            "business/adminUserManager/userIdentityAuditSearchService.min.js"
        };

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.UserIdentity;
        protected override string ClientServiceName => "userIdentityAuditSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.UserIdentity;

        public List<SelectListItem> GetIdentityItems()
        {
            var identityItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var identityDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, IdentityType>();
            identityItems.AddRange(identityDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            return identityItems;
        }

        public List<SelectListItem> GetIdentityItemsWithoutAll()
        {
            var identityItems = new List<SelectListItem>();
            var identityDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, IdentityType>();
            identityItems.AddRange(identityDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            return identityItems;
        }

        public List<SelectListItem> GetAuditStatusItems()
        {
            var statusItems = new List<SelectListItem>();
            var statusDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, ReviewStatus>();
            statusItems.AddRange(statusDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            return statusItems;
        }

        public override ActionResult Index()
        {
            var viewModel = new UserIdentityViewModel
            {
                IdentityTypeItems = GetIdentityItems(),
                AuditStatusItems = GetAuditStatusItems()
            };
            return View(viewModel);
        }

        public override ActionResult GetGridViewResult(AdminUserManagerIdentityApplyListParam searchParam)
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
            var result = MMClientApi.PostApi<AdminUserManagerIdentityApplyListParam, AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyRecord", searchParam);

            return PartialView(result);
        }

        public IActionResult Edit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);

            ViewBag.SubmitUrl = "Audit";
            SetPageTitle("身份审核");
            ViewBag.ClientPopupWindowServiceName = "userEditSingleRowService";
            var result = MMClientApi.GetSingleApi<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", keyContent);
           
            ViewBag.IdentityItems = GetIdentityItemsWithoutAll().Where(c => c.Value == ((int)IdentityType.Boss).ToString() || c.Value == ((int)IdentityType.SuperBoss).ToString());
            ViewBag.AuditStatusItems = GetAuditStatusItems();
            ViewBag.UserName = (EnvLoginUser.LoginUser as BackSideWebUser).UserName;

            return View(result.Datas);
        }

        public JsonResult Audit(AdminUserManagerIdentityAuditParam param)
        {
            var source = MMClientApi.GetApiSingle<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", param.ApplyId);
            param.Memo = param.Memo ?? string.Empty;

            if ((source.Data.ApplyIdentity == IdentityType.Boss || source.Data.ApplyIdentity==IdentityType.SuperBoss) && param.ViewBaseCount == null)
            {
                return new JsonResult(new { IsSuccess = false, Message = "店铺观看基础值不可为空" });
            }
            if (param.Status == 1)
            {
                if (param.EarnestMoney == null)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "缴交保证金不可为空" });
                }
                if (param.EarnestMoney.Value < 0)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "保证金不能为小于0" });
                }
                if (param.ExtraPostCount == null)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "发帖次数不可为空" });
                }
                if (param.ExtraPostCount < 0)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "发帖次数不能为小于0" });
                }
            }
            else
            {
                param.EarnestMoney = source.Data.EarnestMoney;
                param.ExtraPostCount = source.Data.ExtraPostCount;
            }

            if (param.Status != 1 && param.Status != 2)
            {
                return new JsonResult(new { IsSuccess = false, Message = "请选择审核状态" });
            }
            if ((source.Data.ApplyIdentity != IdentityType.Boss || source.Data.ApplyIdentity != IdentityType.SuperBoss) && param.Status != 1 )
            {
                param.ViewBaseCount = 0;
            }

            if (param.ApplyIdentity == (int)IdentityType.SuperBoss)
            {
                if (!param.PlatformSharing.HasValue)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "平台拆账比不可为空" });
                }

                if (!param.PlatformSharing.HasValue || param.PlatformSharing < 0 || param.PlatformSharing > 100)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "平台拆账比仅允许填写 0 ~ 100 正整数" });
                }
            }

            


            var result = MMClientApi.PostApi2("AdminUserManager", "IdentityAudit", param);
            var newResult = MMClientApi.GetApiSingle<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", param.ApplyId);
            if (result.IsSuccess && source.IsSuccess && newResult.IsSuccess)
            {
                #region 日志记录

                var sourceData = source.Data;
                var newData = newResult.Data;

                var recordList = new List<RecordCompareParam>() {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.UserID,
                        OriginValue = sourceData.UserId.ToString(),
                        IsLogTitleValue = true
                    },
                    new RecordCompareParam
                    {
                        Title ="身份信息",
                        OriginValue = sourceData.OriginalIdentityText,
                        NewValue =newData.OriginalIdentityText
                    },
                    new RecordCompareParam
                    {
                        Title = "审核状态",
                        OriginValue = sourceData.StatusText,
                        NewValue = newData.StatusText
                    },
                    new RecordCompareParam
                    {
                        Title = "店铺观看基础值",
                        OriginValue = sourceData.ViewBaseCount.ToString(),
                        NewValue = newData.ViewBaseCount.ToString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "保证金",
                        OriginValue = sourceData.EarnestMoney.ToString(),
                        NewValue = newData.EarnestMoney.ToString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "发帖次数",
                        OriginValue = sourceData.ExtraPostCount.ToString(),
                        NewValue = newData.ExtraPostCount.ToString()
                    },
                    new RecordCompareParam
                    {
                        Title = "身份备注",
                        OriginValue = sourceData.Memo,
                        NewValue = newData.Memo
                    },
                };

                string compareContent = GetOperationCompareContent(recordList, ActTypes.Update);

                CreateOperationLog(compareContent, _permissionKey);

                #endregion 日志记录
            }
            return new JsonResult(new { IsSuccess = result.IsSuccess, Message = result.Message });
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            var userDetail = MMClientApi.GetSingleApi<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", keyContent);
            return View(userDetail.Datas);
        }
    }
}