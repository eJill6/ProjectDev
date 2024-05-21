using Amazon.Auth.AccessControlPolicy;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.AdminBooking;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.User.Enums;
using Newtonsoft.Json;
using System.ComponentModel;

namespace BackSideWeb.Controllers
{
    public class AdminBookingRefundController : BaseCRUDController<QueryAdminBookingRefundParam, AdminBookingRefundInputModel, AdminBookingRefundInputModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminBookingRefund/adminBookingRefundSearchService.min.js",
            "business/adminBookingRefund/adminBookingRefundSearchParam.min.js",
        };

        protected override string ClientServiceName => "adminBookingRefundSearchService";
        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.AdminBookingRefund;

        public override ActionResult Index()
        {
            SetPageTitleByPermission();

            var viewModel = new AdminBookingRefundViewModel
            {
                PaymentTypeItems = MMSelectListItem.GetBookingPaymentTypeItems(),
                ApplyReasonItems = MMSelectListItem.GetApplyReasonItems(),
                IdentityTypeItems = GetIdentityItems()
            };
            string postid = HttpContext.Request.Query["postid"];
            string userid = HttpContext.Request.Query["userId"];
            if (!string.IsNullOrWhiteSpace(postid) || !string.IsNullOrWhiteSpace(userid))
            {
                ViewBag.PostId = postid;
                ViewBag.Userid = userid;
            }
            return View(viewModel);
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
        public override ActionResult GetGridViewResult(QueryAdminBookingRefundParam searchParam)
        {
            PagedResultModel<QueryAdminBookingRefundModel> pageAdminBookingModel = new PagedResultModel<QueryAdminBookingRefundModel>();
            searchParam.EndDate = searchParam.EndDate.AddDays(1);
            string controller = "AdminBooking";
            string action = "RefundApplyList";
            string parame = JsonConvert.SerializeObject(searchParam);
            pageAdminBookingModel.PageSize = searchParam.PageSize;
            var result = MMClientApi.PostApi<QueryAdminBookingRefundModel>(controller, action, parame);
            if (result != null && result.IsSuccess)
            {
                pageAdminBookingModel.ResultList = result.DataModel.Data.ToList();
                pageAdminBookingModel.TotalPageCount = result.DataModel.TotalPage;
                pageAdminBookingModel.PageSize = result.DataModel.PageSize;
                pageAdminBookingModel.PageNo = result.DataModel.PageNo;
                pageAdminBookingModel.TotalCount = result.DataModel.TotalCount;
            }
            return PartialView(pageAdminBookingModel);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.AdminBookingRefund;

        protected override IActionResult GetInsertView()
        {
            return View("Insert", new AdminBookingInputModel());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var adminIncomeDetail = MMClientApi.GetSingleApi<AdminBookingRefundDetail>("AdminBooking", "RefundDetail", keyContent);
            return GetEditView(adminIncomeDetail.Datas);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        protected override BaseReturnModel DoInsert(AdminBookingRefundInputModel insertModel)
        {
            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoUpdate(AdminBookingRefundInputModel updateModel)
        {
            if (updateModel.Status != 1 && updateModel.Status != 2)
            {
                return new BaseReturnModel("请选择审核状态");
            }
            if (updateModel.Status == 2 && string.IsNullOrWhiteSpace(updateModel.Memo))
            {
                return new BaseReturnModel("未通过理由未填写");
            }

            var source = MMClientApi.GetSingleApi<AdminBookingRefundDetail>("AdminBooking", "RefundDetail", updateModel.RefundId);

            var result = MMClientApi.PostApi2("AdminBooking", "RefundAudit", new AdminBookingRefundInputModel
            {
                RefundId = updateModel.RefundId,
                Status = updateModel.Status,
                Memo = updateModel.Memo,
                ExamineMan = (EnvLoginUser.LoginUser as BackSideWebUser).UserName
            });

            if (!result.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.GetSingle(result.Code));
            }

            #region 日志记录

            var sourceData = source.Datas;

            var recordList = new List<RecordCompareParam>() {
                    new RecordCompareParam
                    {
                        Title = "预约申请单",
                        OriginValue = sourceData?.RefundId.ToString(),
                        IsLogTitleValue = true
                    },
                    new RecordCompareParam
                    {
                        Title ="审核",
                        OriginValue = sourceData?.StatusText,
                        NewValue =GetStatusText(updateModel.Status),
                    },
                    new RecordCompareParam
                    {
                        Title = "审核原因",
                        OriginValue = sourceData?.Memo,
                        NewValue = updateModel?.Memo
                    },
                };

            string compareContent = GetOperationCompareContent(recordList, ActTypes.Update);

            CreateOperationLog(compareContent, _permissionKey);

            #endregion 日志记录

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string GetStatusText(int Status)
        {
            switch (Status)
            {
                case 0:
                    return "審核中";

                case 1:
                    return "通過";

                case 2:
                    return "不通過";
            }
            return "-";
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override string? GetInsertViewUrl()
        {
            return null;
        }
    }
}