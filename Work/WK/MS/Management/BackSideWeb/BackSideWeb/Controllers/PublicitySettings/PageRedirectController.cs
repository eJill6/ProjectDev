using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.Game;
using BackSideWeb.Models.ViewModel;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.AdminUserManager;

namespace BackSideWeb.Controllers
{
    public class PageRedirectController : BaseCRUDController<QueryPageRedirectParam, MMPageRedirectBs, PageRedirectUpdateViewModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publicitySettings/pageRedirectSearchService.min.js"
        };

        protected override string ClientServiceName => "pageRedirectSearchService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.PageRedirect;

        public override ActionResult GetGridViewResult(QueryPageRedirectParam searchParam)
        {
            var result = MMClientApi.GetApi<QueryPageRedirectModel>("AdminPageRedirect", "GetAll");
            int skipCount = (searchParam.PageNo - 1) * searchParam.PageSize;
            var pageItems = result.Datas.Skip(skipCount).Take(searchParam.PageSize);
            PagedResultModel<QueryPageRedirectModel> model = new PagedResultModel<QueryPageRedirectModel>();
            model.ResultList = new List<QueryPageRedirectModel>();
            foreach (var item in pageItems)
            {
                model.ResultList.Add(item);
            }
            model.PageSize = searchParam.PageSize;
            model.TotalCount = result.Datas.Count;
            model.PageNo = searchParam.PageNo;
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PageRedirect;

        protected override IActionResult GetInsertView()
        {
            throw new NotImplementedException();
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var pageRedirectResult = MMClientApi.GetSingleApi<MMPageRedirectBs>("AdminPageRedirect", "Get", keyContent);
            var listItems = MMSelectListItem.GetRedirectTypeItems();
            foreach (var item in listItems)
            {
                item.Selected = item.Value == Convert.ToInt16(pageRedirectResult.Datas.Type).ToString();
            }

            var viewModel = new PageRedirectUpdateViewModel
            {
                MMPageRedirectBs = pageRedirectResult.Datas,
                RedirectTypeItems = listItems,
                RedirectTypeSelectOption = Convert.ToInt16(pageRedirectResult.Datas.Type)
            };
            ViewBag.HomeContent = pageRedirectResult.Datas;
            return GetEditView(viewModel);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        protected override string? GetDeleteActionUrl()
        {
            return null;
        }

        protected override BaseReturnModel DoInsert(MMPageRedirectBs insertModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private BaseReturnDataModel<AdminUserManagerUsersDetail> GetUsersInfo(string userId)
        {
            BaseReturnDataModel<AdminUserManagerUsersDetail> baseReturnData = MMClientApi.GetApiSingleBaseReturn<AdminUserManagerUsersDetail>("AdminUserManager", "UserDetail", userId);
            return baseReturnData;
        }

        protected override BaseReturnModel DoUpdate(PageRedirectUpdateViewModel updateModel)
        {
            var pageRedirect = updateModel.MMPageRedirectBs;
            var redirectType = updateModel.RedirectTypeSelectOption;
            if (redirectType == null)
            {
                return new BaseReturnModel("请选择转导页面");
            }

            if ((redirectType.Value == 4 || redirectType.Value == 5 || redirectType.Value == 6) && string.IsNullOrWhiteSpace(pageRedirect.RefId))
            {
                return new BaseReturnModel("编号不可为空");
            }

            var source = MMClientApi.GetSingleApi<MMPageRedirectBs>("AdminPageRedirect", "Get", pageRedirect.Id.ToString()).Datas;

            var result = MMClientApi.PostApi("AdminPageRedirect", "Update", new MMPageRedirectBs
            {
                Id = pageRedirect.Id,
                Title = pageRedirect.Title ?? "",
                Type = (JxBackendService.Model.Enums.MM.RedirectType?)updateModel.RedirectTypeSelectOption,
                RefId = pageRedirect.RefId,
                CreateUser = ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName,
            });

            if (result.IsSuccess)
            {
                #region 日志记录

                if (source.Title == updateModel.MMPageRedirectBs.Title &&
                source.TypeText == updateModel.RedirectTypeText &&
                source.RefId == updateModel.MMPageRedirectBs.RefId)
                    return new BaseReturnModel(ReturnCode.Success);
                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = "转导名称",
                            OriginValue = source.RedirectName,
                            IsLogTitleValue = true
                        },
                        new RecordCompareParam
                        {
                            Title = "标题",
                            OriginValue = source.Title,
                            NewValue = updateModel.MMPageRedirectBs.Title
                        },
                        new RecordCompareParam
                        {
                            Title = "转导页面",
                            OriginValue = source.TypeText,
                            NewValue = updateModel.RedirectTypeText
                        },
                        new RecordCompareParam
                        {
                            Title = "编号",
                            OriginValue = source.RefId,
                            NewValue = updateModel.MMPageRedirectBs.RefId
                        }
                    }, ActTypes.Update);

                    if (string.IsNullOrWhiteSpace(compareContent))
                    {
                        return new BaseReturnModel(ReturnCode.Success);
                    }
                    CreateOperationLog(compareContent, _permissionKey);
                }

                #endregion 日志记录

                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                if (result.Code == "D10101")
                {
                    return new BaseReturnModel("无此帖子ID");
                }
                else if (result.Code == "E00035")
                {
                    return new BaseReturnModel("此Apply ID非觅老板 or 超觅老板");
                }
                return new BaseReturnModel(result.Message);
            }
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            throw new NotImplementedException();
        }

        protected override string? GetInsertViewUrl()
        {
            return null;
        }
    }
}