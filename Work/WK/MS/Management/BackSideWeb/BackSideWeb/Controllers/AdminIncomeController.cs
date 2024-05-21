using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.ViewModel;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Extensions;
using Newtonsoft.Json;
using JxBackendService.Model.ViewModel.Telegram;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.Param;
using BackSideWeb.Models.ViewModel.OperatingData;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.Models.Models;
using MS.Core.MMModel.Models.AdminUserManager;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.User.Enums;
using System.ComponentModel;

namespace BackSideWeb.Controllers
{
    public class AdminIncomeController : BaseCRUDController<QueryAdminIncomeParam, AdminIncomeInputModel, AdminIncomeInputModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/adminIncome/adminIncomeSearchService.min.js",
            "business/adminIncome/adminIncomeService.min.js",
            "business/adminIncome/adminIncomeSearchParam.min.js"
        };

        protected override string ClientServiceName => "adminIncomeService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.AdminIncome;

        public override ActionResult Index()
        {
            SetPageTitleByPermission();
            var viewModel = new AdminIncomeViewModel
            {
                LockedStateItems = MMSelectListItem.GetLockedStateItems(),
                IncomeStatementStatusItems = MMSelectListItem.GetIncomeStatementStatusItems(),
                PostTypeItems = MMSelectListItem.GetPostTypeItems(true),
                TimeTypeItems = MMSelectListItem.GetTimeTypeItems(),
                DiamondStatusItems = MMSelectListItem.GetDiamondStatusItems(),
                BookingPaymentTypeItems = MMSelectListItem.GetBookingPaymentTypeItems(),
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
        public override ActionResult GetGridViewResult(QueryAdminIncomeParam searchParam)
        {
            ViewBag.HasEditPermission = HasPermission(PermissionKeys.AdminIncome, AuthorityTypes.Edit);
            if (searchParam.PostType == 0)
            {
                searchParam.PostType = null;
            }
            searchParam.EndDate = searchParam.EndDate.AddDays(1);

            var result = MMClientApi.PostApi<QueryAdminIncomeParam, AdminIncomeList>("AdminIncome", "List", searchParam);
            if (result != null)
            {
                var model = new PagedResultModel<AdminIncomeList>()
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

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.AdminIncome;

        protected override IActionResult GetInsertView()
        {
            return GetEditView(new AdminIncomeInputModel());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            ApiSingleResult<AdminIncomeDetail> result = MMClientApi.GetSingleApi<AdminIncomeDetail>("AdminIncome", "Detail", keyContent);
            if (result.Datas.Category == IncomeExpenseCategoryEnum.Official)
            {
                return GetBookingEditView(result.Datas);
            }
            else
            {
                return GetEditView(result.Datas);
            }
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        private IActionResult GetBookingEditView<T>(T model)
        {
            return View("BookingEdit", model);
        }

        protected override BaseReturnModel DoInsert(AdminIncomeInputModel insertModel)
        {
            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        public IActionResult Detail(string keyContent)
        {
            var adminIncomeDetail = MMClientApi.GetSingleApi<AdminIncomeDetail>("AdminIncome", "Detail", keyContent);
            SetPageTitle($"解锁单详情 {keyContent}");
            SetLayout(LayoutType.Base);
            return View(adminIncomeDetail.Datas);
        }

        public IActionResult BookingDetail(string keyContent)
        {
            var adminIncomeBookingDetail = MMClientApi.GetSingleApi<AdminIncomeBookingDetail>("AdminIncome", "BookingDetail", keyContent);
            SetPageTitle($"预约单详情 {keyContent}");
            SetLayout(LayoutType.Base);
            return View(adminIncomeBookingDetail.Datas);
        }

        protected override BaseReturnModel DoUpdate(AdminIncomeInputModel updateModel)
        {
            var source = MMClientApi.GetSingleApi<AdminIncomeDetail>("AdminIncome", "Detail", updateModel.IncomeId);

            var result = MMClientApi.PostApi("AdminIncome", "Edit/" + updateModel.IncomeId, new
            {
                Id = updateModel.Id,
                Status = updateModel.Status,
                Memo = updateModel.Memo,
                ExamineMan = (EnvLoginUser.LoginUser as BackSideWebUser).UserName
            });

            if (!result.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.GetSingle(result.Code));
            }

            if (source != null)
            {
                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = DisplayElement.IncomeSlipID,
                            OriginValue = source.Datas?.Id,
                            IsLogTitleValue = true
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.Review,
                            OriginValue = source.Datas?.Status.GetDescription(),
                            NewValue = updateModel.Status.GetDescription()
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.Memo,
                            OriginValue = source.Datas?.Memo,
                            NewValue = updateModel.Memo
                        },
                    }, ActTypes.Update);

                if (string.IsNullOrWhiteSpace(compareContent))
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }
                CreateOperationLog(compareContent, _permissionKey);
            }
            return new BaseReturnModel();
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override string? GetInsertViewUrl()
        {
            return null;
        }
    }
}