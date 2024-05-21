using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.Enums;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.User.Enums;
using Newtonsoft.Json;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class StoreManageController : BaseSearchGridController<AdminStoreManageParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/storeManageSearchParam.min.js",
            "business/adminUserManager/storeManageSearchService.min.js"
        };

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.StoreManage;
        protected override string ClientServiceName => "storeManageSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.StoreManage;

        public List<SelectListItem> GetIsOpenStatusItems() =>
      new List<SelectListItem>
      {
                new SelectListItem(CommonElement.All, null) { Selected = true },
                new SelectListItem("开启", "1"),
                new SelectListItem("关闭", "0")
      };

        public override ActionResult Index()
        {
            ViewBag.IsOpenStatusItems = GetIsOpenStatusItems();
            return View();
        }

        public override ActionResult GetGridViewResult(AdminStoreManageParam searchParam)
        {
            var result = MMClientApi.PostApi<AdminStoreManageParam, AdminStoreManageList>("AdminBossStore", "StoreList", searchParam);
            if (result != null)
            {
                var model = new PagedResultModel<AdminStoreManageList>()
                {
                    PageNo = result.PageNo,
                    TotalCount = result.TotalCount,
                    PageSize = result.PageSize,
                    TotalPageCount = result.TotalPage,
                    ResultList = result.Data.ToList(),
                };
                return PartialView(model);
            }
            else
                return PartialView(null);
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            var userDetail = MMClientApi.GetSingleApi<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", keyContent);
            return View(userDetail.Datas);
        }

        public IActionResult StoreEdit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("编辑店铺");
            ViewBag.SubmitUrl = "DoStoreEdit";
            ViewBag.ClientPopupWindowServiceName = "userEditSingleRowService";

            var userDetail = MMClientApi.GetSingleApi<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", keyContent).Datas;

			AdminUserBossParam userBossParam = new AdminUserBossParam();
			userBossParam.ApplyId = userDetail.ApplyId;
            userBossParam.ApplyIdentity= (int)userDetail.ApplyIdentity;
            userBossParam.ApplyIdentityText = userDetail.ApplyIdentityText;
			userBossParam.BossId = userDetail.BossId;
			userBossParam.UserId = userDetail.UserId;
			userBossParam.ShopName = userDetail.ShopName;
			userBossParam.ShopYears = userDetail.ShopYears;
			userBossParam.Girls = userDetail.Girls;
			userBossParam.DealOrder = userDetail.DealOrder;
			userBossParam.SelfPopularity = userDetail.SelfPopularity;
			userBossParam.Introduction = userDetail.Introduction;
			userBossParam.BusinessDate = userDetail.BusinessDate;
			userBossParam.BusinessHour = userDetail.BusinessHour;
			userBossParam.IsOpen = userDetail.IsOpen;
			userBossParam.ShopAvatarSource = userDetail.ShopAvatarSource;
			userBossParam.BusinessPhotoSource = userDetail.BusinessPhotoSource;
			userBossParam.BusinessDateStart = string.IsNullOrWhiteSpace(userDetail.BusinessDate) ? "" : userDetail.BusinessDate.Split("至")[0];
			userBossParam.BusinessDateEnd = string.IsNullOrWhiteSpace(userDetail.BusinessDate) ? "" : userDetail.BusinessDate.Split("至")[1];
			userBossParam.BusinessHourStart = string.IsNullOrWhiteSpace(userDetail.BusinessHour) ? "" : userDetail.BusinessHour.Split("-")[0];
			userBossParam.BusinessHourEnd = string.IsNullOrWhiteSpace(userDetail.BusinessHour) ? "" : userDetail.BusinessHour.Split("-")[1];
			userBossParam.TelegramGroupId = userDetail.TelegramGroupId;
			return View(userBossParam);
		}

        private Dictionary<string, int> timeOptions = new Dictionary<string, int>
{
    { "01:00", 1 },
    { "02:00", 2 },
    { "03:00", 3 },
    { "04:00", 4 },
    { "05:00", 5 },
    { "06:00", 6 },
    { "07:00", 7 },
    { "08:00", 8 },
    { "09:00", 9 },
    { "10:00", 10 },
    { "11:00", 11 },
    { "12:00", 12 },
    { "13:00", 13 },
    { "14:00", 14 },
    { "15:00", 15 },
    { "16:00", 16 },
    { "17:00", 17 },
    { "18:00", 18 },
    { "19:00", 19 },
    { "20:00", 20 },
    { "21:00", 21 },
    { "22:00", 22 },
    { "23:00", 23 },
    { "24:00", 24 }
};

        private Dictionary<string, int> dayOptions = new Dictionary<string, int>
{
    { "周一", 1 },
    { "周二", 2 },
    { "周三", 3 },
    { "周四", 4 },
    { "周五", 5 },
    { "周六", 6 },
    { "周日", 7 }
};

        public BaseReturnModel DoStoreEdit(AdminUserBossParam param)
        {
            if (string.IsNullOrWhiteSpace(param.ShopName))
            {
                return new BaseReturnModel("店铺名称不可为空");
            }
            if (param.ShopName.Length > 7)
            {
                return new BaseReturnModel("店铺名称，最多7个字");
            }
            if (param.ShopYears == null)
            {
                return new BaseReturnModel("店龄不可为空");
            }
            if (param.ShopYears > 99 || param.ShopYears < 0)
            {
                return new BaseReturnModel("店龄请输入0 ~ 99间正整数");
            }
            if (param.Girls == null)
            {
                return new BaseReturnModel("妹子数量不可为空");
            }
            if (param.Girls > 99999 || param.Girls < 0)
            {
                return new BaseReturnModel("妹子数量请输入0 ~ 99999间正整数");
            }
            if (param.DealOrder == null)
            {
                return new BaseReturnModel("成交订单不可为空");
            }
            if (param.DealOrder > 99999 || param.DealOrder < 0)
            {
                return new BaseReturnModel("成交订单请输入0 ~ 99999间正整数");
            }
            if (param.SelfPopularity == null)
            {
                return new BaseReturnModel("自评人气不可为空");
            }
            if (param.SelfPopularity > 99999 || param.SelfPopularity < 0)
            {
                return new BaseReturnModel("自评人气请输入0 ~ 99999间正整数");
            }
            if (string.IsNullOrEmpty(param.Introduction))
            {
                return new BaseReturnModel("介绍不可为空");
            }
            if (param.Introduction.Length > 17 || string.IsNullOrWhiteSpace(param.Introduction))
            {
                return new BaseReturnModel("介绍最多17个字");
            }

            //int businessDateStart = dayOptions[param.BusinessDateStart];
            //int businessDateEnd = dayOptions[param.BusinessDateEnd];

            //int businessHourStart = timeOptions[param.BusinessHourStart];
            //int businessHourEnd = timeOptions[param.BusinessHourEnd];

            //if (businessDateStart < businessDateEnd)
            //{
            //    return new BaseReturnModel("营业时段开始不得小于结束，请再确认");
            //}

            //if (businessHourStart < businessHourEnd)
            //{
            //    return new BaseReturnModel("营业时间开始不得小于结束，请再确认");
            //}
            if (string.IsNullOrEmpty(param.ShopAvatar) || param.ShopAvatar.Split(",").Length < 1)
            {
                return new BaseReturnModel("店铺头像不可为空");
            }

            var source = MMClientApi.GetSingleApi<AdminUserManagerIdentityApplyList>("AdminUserManager", "IdentityApplyDetail", param.UserId.ToString()).Datas;

            param.BusinessDate = $"{param.BusinessDateStart}至{param.BusinessDateEnd}";
            param.BusinessHour = $"{param.BusinessHourStart}-{param.BusinessHourEnd}";

            string parame = JsonConvert.SerializeObject(param);
            var result = MMClientApi.PostObjectApi("AdminUserManager", "StoreEdit", parame);
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
                                Title = "店龄",
                                OriginValue = source.ShopYears.ToString(),
                                NewValue = param.ShopYears.ToString()
                            },
                            new RecordCompareParam
                            {
                                Title = "妹子数量",
                                OriginValue = source.Girls.ToString(),
                                NewValue = param.Girls.ToString()
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
                                Title = "介绍",
                                OriginValue = source.Introduction,
                                NewValue = param.Introduction
                            },
                            new RecordCompareParam
                            {
                                Title = "营业时段",
                                OriginValue = source.BusinessDate,
                                NewValue = param.BusinessDate
                            },
                            new RecordCompareParam
                            {
                                Title = "营业时间",
                                OriginValue = source.BusinessHour,
                                NewValue = param.BusinessHour
                            },
                            new RecordCompareParam
                            {
                                Title = "TG_Chat ID",
                                OriginValue = source.TelegramGroupId,
                                NewValue = param.TelegramGroupId
                            },
                            new RecordCompareParam
                            {
                                Title = "店铺状态",
                                OriginValue = source.IsOpenText,
                                NewValue = param.IsOpenText
                            }
                        }, ActTypes.Update);

                    if (string.IsNullOrWhiteSpace(compareContent))
                    {
                        return new BaseReturnModel()
                        {
                            IsSuccess = true,
                            Message = "编辑成功"
                        };
                    }
                    CreateOperationLog(compareContent, _permissionKey);
                }
                #endregion 日志记录

                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "编辑成功"
                };
            }
            else
            {
                return new BaseReturnModel("编辑失败");
            }
        }
    }
}