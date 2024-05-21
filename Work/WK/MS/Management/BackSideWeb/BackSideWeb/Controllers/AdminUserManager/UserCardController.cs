using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.MMModel.Extensions;
using System.ComponentModel;

namespace BackSideWeb.Controllers.AdminUserManager
{
    public class UserCardController : BaseSearchGridController<AdminUserManagerUserCardsListParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminUserManager/userCardSearchParam.min.js",
            "business/adminUserManager/userCardSearchService.min.js"
        };

        protected override string ClientServiceName => "userCardSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.UserCard;

        public List<SelectListItem> GetCardTypeItems()
        {
            var cardTypeItems = new List<SelectListItem> { new SelectListItem(CommonElement.All, null) { Selected = true } };
            var cardTypeDic = EnumExtension.GetSelectListItemDic<DescriptionAttribute, VipType>();
            cardTypeItems.AddRange(cardTypeDic.Select(x => new SelectListItem { Text = x.Key, Value = ((byte)x.Value).ToString() }));
            return cardTypeItems;
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
            var viewModel = new UserCardViewModel
            {
                CardTypeItems = GetCardTypeItems(),
                PayTypeItems = GetPayTypeItems()
            };
            return View(viewModel);
        }

        public override ActionResult GetGridViewResult(AdminUserManagerUserCardsListParam searchParam)
        {
            searchParam.EndDate = searchParam.EndDate.AddDays(1);
            var result = MMClientApi.PostApi<AdminUserManagerUserCardsListParam, AdminUserManagerUserCardsList>("AdminUserManager", "UserCards", searchParam);
            return PartialView(result);
        }
    }
}