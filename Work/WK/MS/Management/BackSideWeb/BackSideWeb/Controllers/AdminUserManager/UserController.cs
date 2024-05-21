using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.AdminUserManager;
using BackSideWeb.Models.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Vip.Enums;
using System.ComponentModel;
using BackSideWeb.Models;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.BackSideWeb;
using Newtonsoft.Json;

using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Common;
using BackSideWeb.Models.ViewModel.PublishRecord;
using JxBackendService.Model.ReturnModel;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;
using JxBackendService.Repository.Extensions;
using System.Globalization;
using System.Drawing;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class UserController : BaseSearchGridController<AdminUserManagerUsersListParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/userSearchParam.min.js",
            "business/adminUserManager/userSearchService.min.js"
        };

        protected override string ClientServiceName => "userSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.User;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.User;

        public List<SelectListItem> GetCardTypeItems()
        {
            var cardTypeItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var cardTypeDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, VipType>();
            cardTypeItems.AddRange(cardTypeDic.Select(x => new SelectListItem { Text = x.Key, Value = ((byte)x.Value).ToString() }));
            return cardTypeItems;
        }

        public List<SelectListItem> GetOpeningStatusItems() =>
            new List<SelectListItem>
            {
                new SelectListItem(CommonElement.All, null) { Selected = true },
                new SelectListItem("ON", "true"),
                new SelectListItem("OFF", "false")
            };

        public List<SelectListItem> GetIdentityItems()
        {
            var identityItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var identityDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, IdentityType>();
            identityItems.AddRange(identityDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            List<string> valuesToRemove = new List<string> { "觅女郎", "星觅官" };
            identityItems.RemoveAll(item => valuesToRemove.Contains(item.Text));
            return identityItems;
        }

        public List<SelectListItem> GetIdentityItemsWithoutAll()
        {
            var identityItems = new List<SelectListItem>();
            var identityDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, IdentityType>();
            identityItems.AddRange(identityDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            List<string> valuesToRemove = new List<string> { "觅女郎", "星觅官" };
            identityItems.RemoveAll(item => valuesToRemove.Contains(item.Text));
            return identityItems;
        }

        public override ActionResult Index()
        {
            var viewModel = new UserViewModel
            {
                CardTypeItems = GetCardTypeItems(),
                IdentityTypeItems = GetIdentityItems(),
                OpeningStatusItems = GetOpeningStatusItems()
            };
            ViewBag.IsAutoSearchAfterPageLoaded = false;
            return View(viewModel);
        }

        public override ActionResult GetGridViewResult(AdminUserManagerUsersListParam searchParam)
        {
            var result = MMClientApi.PostApi<AdminUserManagerUsersListParam, AdminUserManagerUsersList>("AdminUserManager", "Users", searchParam);
            return PartialView(result);
        }

        public IActionResult Detail(string keyContent)
        {
            SetLayout(LayoutType.Base);
            var userDetail = MMClientApi.GetSingleApi<AdminUserManagerUsersDetail>("AdminUserManager", "UserDetail", keyContent);
            return View(userDetail.Datas);
        }

        public IActionResult IdentityEdit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("身份编辑");
            ViewBag.SubmitUrl = "IdentityApply";
            ViewBag.ClientPopupWindowServiceName = "userEditSingleRowService";
            ViewBag.IdentityItems = GetIdentityItemsWithoutAll();
            var result = MMClientApi.GetSingleApi<UserBossApplyInfoModel>("AdminUserManager", "UserBossInfoAndIdentityApplyInfo", keyContent);
            if (result.IsSuccess)
            {
                return View(new AdminUserManagerIdentityApplyParam
                {
                    UserId = result.Datas.UserId,
                    EarnestMoney = result.Datas.EarnestMoney,
                    ApplyIdentity = result.Datas.ApplyIdentity,
                    ExtraPostCount = 0,
                    ContactApp = result.Datas.ContactApp,
                    Contact = result.Datas.Contact,
                    Memo = result.Datas.Memo,
                    PlatformSharing = result.Datas.PlatformSharing
                });
            }
            else
                return View(null);
        }

        public IActionResult EarnestMoneyEdit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("调整保证金");
            ViewBag.SubmitUrl = "EarnestMoneyApply";
            ViewBag.ClientPopupWindowServiceName = "userEditSingleRowService";

            var userId = Convert.ToInt32(keyContent.Split('|')[0]);
            var originalEarnestMoney = Convert.ToDecimal(keyContent.Split('|')[1]);
            var param = new AdminUserManagerEarnestMoneyChangeParam
            {
                UserId = userId,
                OriginalEarnestMoney = originalEarnestMoney,
                EarnestMoney = 0
            };

            ViewBag.EarnestMoneyHis = MMClientApi.PostRequest<List<UserEarnestMoneyData>>("AdminUserManager", "UserEarnestMoneyList", JsonConvert.SerializeObject(new { userId = userId })).Data;
            return View(param);
        }

        public JsonResult IdentityApply(AdminUserManagerIdentityApplyParam param)
        {
            var source = MMClientApi.GetSingleApi<AdminUserManagerUsersDetail>("AdminUserManager", "UserDetail", param.UserId.ToString());

            param.ExtraPostCount = param.ExtraPostCount ?? 0;
            param.Memo = param.Memo ?? "";
            param.ContactApp = param.ContactApp ?? "QQ";

            if (string.IsNullOrWhiteSpace(param.Contact))
            {
                return new JsonResult(new { IsSuccess = false, Message = "QQ号码不可为空" });
            }
            if (param.Contact.Length > 15)
            {
                return new JsonResult(new { IsSuccess = false, Message = "QQ号码长度不能超过15" });
            }

            if (param.Memo.Length > 500)
            {
                return new JsonResult(new { IsSuccess = false, Message = "备注长度不能超过500" });
            }

            if(param.ApplyIdentity==(int)IdentityType.SuperBoss)
            {
                if(!param.PlatformSharing.HasValue)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "平台拆账比不可为空" });
                }
                if (!param.PlatformSharing.HasValue|| param.PlatformSharing < 0 || param.PlatformSharing > 100)
                {
                    return new JsonResult(new { IsSuccess = false, Message = "平台拆账比仅允许填写 0 ~ 100 正整数" });
                }
            }

            var result = MMClientApi.PostApi2("AdminUserManager", "IdentityApply", param);

            var newrce = MMClientApi.GetSingleApi<AdminUserManagerUsersDetail>("AdminUserManager", "UserDetail", param.UserId.ToString());
            if (result.IsSuccess && source.IsSuccess && newrce.IsSuccess)
            {
                if (param.EarnestMoney != source.Datas.EarnestMoney)
                {
                    MMClientApi.PostApi2("AdminUserManager", "EarnestMoneyAudit", new AdminUserManagerEarnestMoneyChangeParam
                    {
                        UserId = param.UserId,
                        EarnestMoney = param.EarnestMoney,
                        ExamineMan = (EnvLoginUser.LoginUser as BackSideWebUser).UserName,
                        OriginalEarnestMoney = source.Datas.EarnestMoney
                    });
                }

                #region 日志记录

                var sourceData = source.Datas;
                var newData = newrce.Datas;

                var recordList = new List<RecordCompareParam>() {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.UserID,
                        OriginValue = param.UserId.ToString(),
                        IsLogTitleValue = true
                    },
                    new RecordCompareParam
                    {
                        Title ="身份信息",
                        OriginValue = sourceData?.UserIdentityText,
                        NewValue =newData?.UserIdentityText
                    },
                    new RecordCompareParam
                    {
                        Title = "通讯账号",
                        OriginValue = sourceData?.Contact,
                        NewValue = newData?.Contact
                    },
                    new RecordCompareParam
                    {
                        Title = "保证金",
                        OriginValue = sourceData?.EarnestMoney.ToString(),
                        NewValue = newData?.EarnestMoney.ToString(),
                    },
                    new RecordCompareParam
                    {
                        Title = "发帖次数",
                        OriginValue = sourceData?.PostRemain.ToString(),
                        NewValue = newData?.PostRemain.ToString()
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

        public JsonResult EarnestMoneyApply(AdminUserManagerEarnestMoneyChangeParam param)
        {
            if (param.EarnestMoney == 0)
                return new JsonResult(new { IsSuccess = false, Message = "调整保证金金额不可为0" });

            param.ExamineMan = (EnvLoginUser.LoginUser as BackSideWebUser).UserName;
            var result = MMClientApi.PostApi2("AdminUserManager", "EarnestMoneyAudit", param);
            return new JsonResult(new { IsSuccess = result.IsSuccess, Message = result.Message });
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
            userBossParam.ShopAvatarSource = userDetail.ShopAvatarSource;
            userBossParam.BusinessPhotoSource = userDetail.BusinessPhotoSource;
            userBossParam.BusinessDateStart = string.IsNullOrWhiteSpace(userDetail.BusinessDate) ? "" : userDetail.BusinessDate.Split("至")[0];
            userBossParam.BusinessDateEnd = string.IsNullOrWhiteSpace(userDetail.BusinessDate) ? "" : userDetail.BusinessDate.Split("至")[1];
            userBossParam.BusinessHourStart = string.IsNullOrWhiteSpace(userDetail.BusinessHour) ? "" : userDetail.BusinessHour.Split("-")[0];
            userBossParam.BusinessHourEnd = string.IsNullOrWhiteSpace(userDetail.BusinessHour) ? "" : userDetail.BusinessHour.Split("-")[1];
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

        [HttpPost]
        public BaseReturnModel DoStoreEdit(AdminUserBossParam insertModel)
        {
            if (string.IsNullOrWhiteSpace(insertModel.ShopName))
            {
                return new BaseReturnModel("店铺名称不可为空");
            }
            if (insertModel.ShopName.Length > 7)
            {
                return new BaseReturnModel("店铺名称，最多7个字");
            }
            if (insertModel.ShopYears == null)
            {
                return new BaseReturnModel("店龄不可为空");
            }
            if (insertModel.ShopYears > 99 || insertModel.ShopYears < 0)
            {
                return new BaseReturnModel("店龄请输入0 ~ 99间正整数");
            }
            if (insertModel.Girls == null)
            {
                return new BaseReturnModel("妹子数量不可为空");
            }
            if (insertModel.Girls > 99999 || insertModel.Girls < 0)
            {
                return new BaseReturnModel("妹子数量请输入0 ~ 99999间正整数");
            }
            if (insertModel.DealOrder == null)
            {
                return new BaseReturnModel("成交订单不可为空");
            }
            if (insertModel.DealOrder > 99999 || insertModel.DealOrder < 0)
            {
                return new BaseReturnModel("成交订单请输入0 ~ 99999间正整数");
            }
            if (insertModel.SelfPopularity == null)
            {
                return new BaseReturnModel("自评人气不可为空");
            }
            if (insertModel.SelfPopularity > 99999 || insertModel.SelfPopularity < 0)
            {
                return new BaseReturnModel("自评人气请输入0 ~ 99999间正整数");
            }
            if (string.IsNullOrEmpty(insertModel.Introduction))
            {
                return new BaseReturnModel("介绍不可为空");
            }
            if (insertModel.Introduction.Length > 17 || string.IsNullOrWhiteSpace(insertModel.Introduction))
            {
                return new BaseReturnModel("介绍最多17个字");
            }

            //int businessDateStart = dayOptions[insertModel.BusinessDateStart];
            //int businessDateEnd = dayOptions[insertModel.BusinessDateEnd];

            //int businessHourStart = timeOptions[insertModel.BusinessHourStart];
            //int businessHourEnd = timeOptions[insertModel.BusinessHourEnd];

            //if (businessDateStart < businessDateEnd)
            //{
            //    return new BaseReturnModel("营业时段开始不得小于结束，请再确认");
            //}

            //if (businessHourStart < businessHourEnd)
            //{
            //    return new BaseReturnModel("营业时间开始不得小于结束，请再确认");
            //}
            if (string.IsNullOrEmpty(insertModel.ShopAvatar) || insertModel.ShopAvatar.Split(",").Length < 1)
            {
                return new BaseReturnModel("店铺头像不可为空");
            }

            insertModel.BusinessDate = $"{insertModel.BusinessDateStart}至{insertModel.BusinessDateEnd}";
            insertModel.BusinessHour = $"{insertModel.BusinessHourStart}-{insertModel.BusinessHourEnd}";
            string parame = JsonConvert.SerializeObject(insertModel);
            var result = MMClientApi.PostObjectApi("AdminUserManager", "StoreEdit", parame);
            if (result.IsSuccess)
            {
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

        /// <summary>
        /// 照片上传
        /// </summary>
        /// <param name="photoBaseStr"></param>
        /// <param name="type">0封面,1商家照片</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse<MediaViewModel> PhotoUpload(string photoBaseStr, int type)
        {
            if (!string.IsNullOrWhiteSpace(photoBaseStr))
            {
                //获取图片后缀
                string suffix = photoBaseStr.Substring(photoBaseStr.IndexOf('/') + 1, photoBaseStr.IndexOf(';') - photoBaseStr.IndexOf('/') - 1);
                photoBaseStr = photoBaseStr.Substring(photoBaseStr.IndexOf(',') + 1);

                byte[] bytes = Convert.FromBase64String(photoBaseStr);

                if (bytes.Length > 1 * 1024 * 1024)
                {
                    // 图片大小超过1MB的处理逻辑
                    return new ApiResponse<MediaViewModel>() { IsSuccess = false, Message = "上传的图片大小不能超过1MB" };
                }
                if (suffix != "png" && suffix != "jpg" && suffix != "jpeg")
                {
                    // 图片格式不是PNG或JPG的处理逻辑
                    return new ApiResponse<MediaViewModel>() { IsSuccess = false, Message = "只能上传PNG和JPG格式的图片" };
                }

                SaveMediaToOssParamForClient saveMediaToOssParamForClient = new SaveMediaToOssParamForClient();
                saveMediaToOssParamForClient.FileName = $"photoAgentPost_{DateTime.Now.ToString("yyyyMMddHHmmssss")}.{suffix}";
                saveMediaToOssParamForClient.SourceType = type == 0 ? SourceType.BossApply : SourceType.BusinessPhoto;
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
    }
}