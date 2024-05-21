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
using TencentCloud.Asr.V20190614.Models;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class PostContactUpdate : BaseSearchGridController<AdminPostContactUpdateParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/storeManageSearchParam.min.js",
            "business/adminUserManager/storeManageSearchService.min.js"
        };

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.PostContactUpdate;
        protected override string ClientServiceName => "storeManageSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostContactUpdate;

        public override ActionResult Index()
        {
            return View();
        }

        public override ActionResult GetGridViewResult(AdminPostContactUpdateParam searchParam)
        {
            return View();
        }

        public IActionResult Detail(string keyContent)
        {
            return View();
        }

        public BaseReturnModel DoPostContactUpdate(AdminPostContactUpdateParam updateData)
        {
            if (!updateData.UserId.HasValue)
            {
                return new BaseReturnModel("会员ID不可为空");
            }
            if (string.IsNullOrWhiteSpace(updateData.Phone) && string.IsNullOrWhiteSpace(updateData.QQ) && string.IsNullOrWhiteSpace(updateData.WeChat))
            {
                return new BaseReturnModel("联系方式请至少填写一项");
            }

            var result = MMClientApi.PostApi("AdminPost", "PostContactUpdate", updateData);
            if (result.IsSuccess)
            {
                #region 日志记录
                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = "批量修改会员ID",
                            OriginValue = updateData.UserId.ToString(),
                            IsLogTitleValue = true
                        },
                        new RecordCompareParam
                        {
                            Title = "所属帖子微信号",
                            OriginValue = "",
                            NewValue = updateData.WeChat
                        },
                        new RecordCompareParam
                        {
                            Title = "所属帖子QQ号",
                            OriginValue = "",
                            NewValue = updateData.QQ
                        },
                        new RecordCompareParam
                        {
                            Title = "所属帖子手机号",
                            OriginValue = "",
                            NewValue = updateData.Phone
                        }
                    }, ActTypes.Update);

                if (string.IsNullOrWhiteSpace(compareContent))
                {
                    return new BaseReturnModel()
                    {
                        IsSuccess = true,
                        Message = "批量修改完成"
                    };
                }
                CreateOperationLog(compareContent, _permissionKey);
                #endregion

                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "批量修改完成"
                };
            }
            else
            {
                if (result.Code == "E00029")
                {
                    return new BaseReturnModel("无此会员ID");
                }
                return new BaseReturnModel("批量修改失败");
            }
        }
    }
}