using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.ViewModel.Game;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class MailSettingController : BaseCRUDController<BasePagingRequestParam, MMAdvertisingContentBs, QueryAdvertisingContentModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publicitySettings/mailSettingSearchService.min.js"
        };

        protected override string ClientServiceName => "mailSettingSearchService";

        //private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.MailSetting;

        public override ActionResult GetGridViewResult(BasePagingRequestParam searchParam)
        {
            var result = MMClientApi.GetApi<QueryAdvertisingContentModel>("AdvertisingContent", "GetMailSetting");
            PagedResultModel<QueryAdvertisingContentModel> model = new PagedResultModel<QueryAdvertisingContentModel>();
            model.ResultList = result.Datas;
            model.PageSize = searchParam.PageSize;
            model.TotalCount = result.Datas.Count;
            model.PageNo = searchParam.PageNo;
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.MailSetting;

        protected override IActionResult GetInsertView()
        {
            throw new NotImplementedException();
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var response = MMClientApi.GetSingleApi<QueryAdvertisingContentModel>("AdvertisingContent", "Get", keyContent);
            return View("Edit", response.Datas);
        }

        protected override BaseReturnModel DoInsert(MMAdvertisingContentBs insertModel)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnModel DoUpdate(QueryAdvertisingContentModel model)
        {
            if (model.AdvertisingType == 5 && model.AdvertisingContent.Length > 50)
            {
                return new BaseReturnModel("下载提示文字的最大长度不允许超过50个汉字!");
            }
            if (model.AdvertisingType == 6 && model.AdvertisingContent.Length > 1000)
            {
                return new BaseReturnModel("下载的URL的最大长度不允许超过1000个字符!");
            }

            var source = MMClientApi.GetSingleApi<MMAdvertisingContentBs>("AdvertisingContent", "Get", model.Id.ToString());
            var result = MMClientApi.PostApi("AdvertisingContent", "Update", model);

            if (result.IsSuccess)
            {
                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = model.AdvertisingText,
                            OriginValue = source.Datas?.AdvertisingContent,
                            NewValue = model.AdvertisingContent
                        },
                    }, ActTypes.Update);

                    //if (!string.IsNullOrWhiteSpace(compareContent))
                    //{
                    //    CreateOperationLog(compareContent, _permissionKey);
                    //}
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

        protected override string? GetDeleteActionUrl()
        {
            return null;
        }

        protected override string? GetInsertViewUrl()
        {
            return null;
        }
    }
}