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

namespace BackSideWeb.Controllers
{
    public class AdvertisingContentController : BaseCRUDController<QueryAdvertisingContentParam, MMAdvertisingContentBs, AdvertisingContentUpdateViewModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publicitySettings/advertisingContentSearchService.min.js"
        };

        protected override string ClientServiceName => "advertisingContentSearchService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.AdvertisingContent;

        public override ActionResult GetGridViewResult(QueryAdvertisingContentParam searchParam)
        {
            var result = MMClientApi.GetApi<QueryAdvertisingContentModel>("AdvertisingContent", "GetAll");
            int skipCount = (searchParam.PageNo - 1) * searchParam.PageSize;
            var pageItems = result.Datas.Where(d=>d.AdvertisingType!=5 && d.AdvertisingType != 6).Skip(skipCount).Take(searchParam.PageSize);
            PagedResultModel<QueryAdvertisingContentModel> model = new PagedResultModel<QueryAdvertisingContentModel>();
            model.ResultList = new List<QueryAdvertisingContentModel>();
            foreach (var item in pageItems)
            {
                string plainText = item.AdvertisingContent != null ? MMHelpers.StripHtmlTags(item.AdvertisingContent) : string.Empty;
                item.AdvertisingContent = plainText;
                model.ResultList.Add(item);
            }
            model.PageSize = searchParam.PageSize;
            model.TotalCount = result.Datas.Count;
            model.PageNo = searchParam.PageNo;
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.AdvertisingContent;

        protected override IActionResult GetInsertView()
        {
            throw new NotImplementedException();
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var advertisingContentResult = MMClientApi.GetSingleApi<MMAdvertisingContentBs>("AdvertisingContent", "Get", keyContent);
            var listItems = new List<SelectListItem>
            {
              new SelectListItem("隐藏", "0") ,
              new SelectListItem("显示", "1")
            };
            foreach (var item in listItems)
            {
                item.Selected = item.Value == Convert.ToInt16(advertisingContentResult.Datas.IsActive).ToString();
            }

            var viewModel = new AdvertisingContentUpdateViewModel
            {
                MMAdvertisingContentBs = advertisingContentResult.Datas,
                Items = listItems,
                SelectOption = Convert.ToInt16(advertisingContentResult.Datas.IsActive)
            };
            ViewBag.HomeContent = advertisingContentResult.Datas.AdvertisingContent;
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

        protected override BaseReturnModel DoInsert(MMAdvertisingContentBs insertModel)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnModel DoUpdate(AdvertisingContentUpdateViewModel updateModel)
        {
            var advertisingContent = updateModel.MMAdvertisingContentBs;
            string content = string.Empty;
            var source = MMClientApi.GetSingleApi<MMAdvertisingContentBs>("AdvertisingContent", "Get", advertisingContent.Id.ToString());

            string advertisingContentString = MMHelpers.HtmlToString(advertisingContent.AdvertisingContent);
            int min = 1, max = 4000;
            if (!MMValidate.IsValidLength(advertisingContentString, min, max))
            {
                return new BaseReturnModel($"内文长度为 {min} ~ {max} 字之间请再确认");
            }
            if(updateModel.MMAdvertisingContentBs.AdvertisingType==3 || updateModel.MMAdvertisingContentBs.AdvertisingType ==7 || updateModel.MMAdvertisingContentBs.AdvertisingType ==8|| updateModel.MMAdvertisingContentBs.AdvertisingType == 9)
            {
                content = advertisingContentString;
            }
            else
            {
                content = advertisingContent.AdvertisingContent;
            }
            var result = MMClientApi.PostApi("AdvertisingContent", "Update", new MMAdvertisingContentBs
            {
                Id = advertisingContent.Id,
                AdvertisingContent = content,
                IsActive = updateModel.SelectOption == 1
            });

            if (result.IsSuccess)
            {
                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Text,
                    OriginValue = source.Datas?.AdvertisingContent,
                    NewValue = content
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.Show,
                    OriginValue = source.Datas?.IsActive == true ? "显示" : "隐藏",
                    NewValue = updateModel.SelectOption == 1 ? "显示" : "隐藏",
                },
            }, ActTypes.Update);

                    if (!string.IsNullOrWhiteSpace(compareContent))
                    {
                        CreateOperationLog(compareContent, _permissionKey);
                    }
                }
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
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