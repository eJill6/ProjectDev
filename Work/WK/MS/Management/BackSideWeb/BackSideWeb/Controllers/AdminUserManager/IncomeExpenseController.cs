using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using System.ComponentModel;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class IncomeExpenseController : BaseSearchGridController<SearchIncomeExpensesParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/incomeExpenseSearchParam.min.js",
            "business/adminUserManager/incomeExpenseSearchService.min.js"
        };
       

        protected override string ClientServiceName => "incomeExpenseSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.IncomeExpense;

        public List<SelectListItem> GetPayActionItems()
        {
            var payActionItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var payActionDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, AdminIncomeExpensesCategory>();
            payActionDic.Remove("购买会员卡");
            payActionItems.AddRange(payActionDic.Select(x => new SelectListItem { Text = x.Key, Value = ((int)x.Value).ToString() }));
            return payActionItems;
        }

		public List<SelectListItem> GetPostModuleItems()
        {
            var postModuleItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var postModuleDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, PostType>();
            postModuleItems.AddRange(postModuleDic.Select(x => new SelectListItem { Text = x.Key, Value = ((byte)x.Value).ToString() }));
            return postModuleItems;
        }

        public List<SelectListItem> GetPayTypeItems()
        {
            var payTypeItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var payTypeDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, IncomeExpensePayType>();
            payTypeItems.AddRange(payTypeDic.Select(x => new SelectListItem { Text = x.Key, Value = ((byte)x.Value).ToString() }));
            return payTypeItems;
        }

        public override ActionResult Index()
        {
            var viewModel = new IncomeExpenseViewModel
            {
                PayActionItems = GetPayActionItems(),
                PostModuleItems = GetPostModuleItems(),
                PayTypeItems = GetPayTypeItems(),
                IdentityTypeItems = GetIdentityItems()
            };
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

        public override ActionResult GetGridViewResult(SearchIncomeExpensesParam searchParam)
		{
            searchParam.EndDate = searchParam.EndDate.AddDays(1);
            var result = MMClientApi.PostApi<SearchIncomeExpensesParam, AdminUserManagerOfficialIncomeExpensesList>("AdminUserManager", "IncomeExpenses", searchParam);
            if (result != null)
            {
                var model = new PagedResultModel<AdminUserManagerOfficialIncomeExpensesList>()
                {
                    PageNo = result.PageNo,
                    TotalCount = result.TotalCount,
                    PageSize = result.PageSize,
                    TotalPageCount = result.TotalPage,
                    ResultList = result.Data.ToList(),
                };
                return PartialView(model);
            }else
                return PartialView(null);

        }
    }
}