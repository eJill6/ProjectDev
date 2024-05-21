using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.Param;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.User.Enums;
using Newtonsoft.Json;
using System.ComponentModel;

namespace BackSideWeb.Controllers
{
    public class AdminBookingController : BaseCRUDController<QueryAdminBookingParam, AdminBookingInputModel, AdminBookingInputModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminBooking/adminBookingSearchService.min.js",
            "business/adminBooking/adminBookingSearchParam.min.js",
        };

        protected override string ClientServiceName => "adminBookingSearchService";

        public override ActionResult Index()
        {
            SetPageTitleByPermission();

            var viewModel = new AdminBookingViewModel
            {
                PaymentTypeItems = MMSelectListItem.GetPaymentTypeItems(),
                TimeTypeItems = MMSelectListItem.GetTimeTypeItems_Booking(),
                OrderStatusItems = MMSelectListItem.GetOrderStatusItems(),
                BookingStatusItems = MMSelectListItem.GetBookingStatusItems(),
                IdentityTypeItems= GetIdentityItems()
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
            List<string> valuesToRemove = new List<string> { "觅女郎", "星觅官" };
            identityItems.RemoveAll(item => valuesToRemove.Contains(item.Text));
            return identityItems;
        }
        public override ActionResult GetGridViewResult(QueryAdminBookingParam searchParam)
        {
            PagedResultModel<QueryAdminBookingViewModel> pageAdminBookingModel = new PagedResultModel<QueryAdminBookingViewModel>();
            searchParam.EndDate = searchParam.EndDate.AddDays(1);
            string controller = "AdminBooking";
            string action = "List";
            string parame = JsonConvert.SerializeObject(searchParam);
            pageAdminBookingModel.PageSize = searchParam.PageSize;
            var result = MMClientApi.PostApi<QueryAdminBookingViewModel>(controller, action, parame);
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

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.AdminBooking;

        protected override IActionResult GetInsertView()
        {
            return View("Insert", new AdminBookingInputModel());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            //pass view model to view here
            return GetEditView(new MMAdminBookingBs());
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        protected override BaseReturnModel DoInsert(AdminBookingInputModel insertModel)
        {
            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoUpdate(AdminBookingInputModel updateModel)
        {
            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
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