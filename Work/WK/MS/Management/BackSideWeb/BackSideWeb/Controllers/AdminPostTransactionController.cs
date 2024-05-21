using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BackSideWeb.Controllers
{
    public class AdminPostTransactionController : BaseCRUDController<QueryAdminPostTransactionParam, AdminPostTransactionInputModel, AdminPostTransactionInputModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminPostTransaction/adminPostTransactionSearchService.js",
            "business/adminPostTransaction/adminPostTransactionSearchParam.min.js"
        };

        protected override string ClientServiceName => "adminPostTransactionSearchService";

        public override ActionResult Index()
        {
            SetPageTitleByPermission();

            var viewModel = new AdminPostTransactionViewModel
            {
                PostTypeItems = MMSelectListItem.GetEnumItemsDefaultNull<SquareXFG>(SelectEnum.All),
                UnlockMethodItems = MMSelectListItem.GetUnlockMethodItems()
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

        public override ActionResult GetGridViewResult(QueryAdminPostTransactionParam searchParam)
        {
            //QueryAdminPostTransactionModel
            //QueryAdminPostTransactionViewModel
            ViewBag.HasEditPermission = HasPermission(PermissionKeys.AdminPostTransaction, AuthorityTypes.Edit);
            PagedResultModel<QueryAdminPostTransactionViewModel> pagePublishRecordVmModel = new PagedResultModel<QueryAdminPostTransactionViewModel>();
            if (searchParam.PostType == 0)
            {
                searchParam.PostType = null;
            }
            searchParam.EndDate = searchParam.EndDate.AddDays(1);
            string controller = "AdminPostTransaction";
            string action = "List";
            string parame = JsonConvert.SerializeObject(searchParam);
            pagePublishRecordVmModel.PageSize = searchParam.PageSize;
            var result = MMClientApi.PostApi<QueryAdminPostTransactionViewModel>(controller, action, parame);
            if (result != null && result.IsSuccess)
            {
                pagePublishRecordVmModel.ResultList = result.DataModel.Data.ToList();
                pagePublishRecordVmModel.TotalPageCount = result.DataModel.TotalPage;
                pagePublishRecordVmModel.PageSize = result.DataModel.PageSize;
                pagePublishRecordVmModel.PageNo = result.DataModel.PageNo;
                pagePublishRecordVmModel.TotalCount = result.DataModel.TotalCount;
            }
            return PartialView(pagePublishRecordVmModel);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.AdminPostTransaction;

        protected override IActionResult GetInsertView()
        {
            AdminPostTransactionInputModel model = new AdminPostTransactionInputModel()
            {
                PostTypeListItem = MMSelectListItem.GetPostTypeItems(),
                OptionTypeListItem = MMSelectListItem.GetOptionTypeItems(),
                IsActiveListItem = MMSelectListItem.GetIsActiveItems(),
            };
            return View("Insert", model);
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            //pass view model to view here
            return GetEditView(new MMAdminPostTransactionBs());
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        protected override BaseReturnModel DoInsert(AdminPostTransactionInputModel insertModel)
        {
            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoUpdate(AdminPostTransactionInputModel updateModel)
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