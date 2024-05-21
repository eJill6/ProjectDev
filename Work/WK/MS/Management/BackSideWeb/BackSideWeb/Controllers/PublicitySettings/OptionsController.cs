using Amazon.Auth.AccessControlPolicy;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using Castle.Core.Internal;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.BackSideWeb;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models.Post.Enums;

namespace BackSideWeb.Controllers
{
    public class OptionsController : BaseCRUDController<QueryOptionsParam, OptionsInputModel, OptionsInputModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publicitySettings/optionsSearchService.min.js"
        };

        protected override string ClientServiceName => "optionsSearchService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.Options;

        public override ActionResult Index()
        {
            base.Index();

            var viewModel = new OptionsViewModel
            {
                //移除掉体验選項
                PostTypeItems = MMSelectListItem.GetEnumItems<PostType>(SelectEnum.All).Where(x=>x.Value!="4").ToList(),
                OptionTypeItems = MMSelectListItem.GetOptionTypeItems(true)
            };
            return View(viewModel);
        }

        public override ActionResult GetGridViewResult(QueryOptionsParam searchParam)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (searchParam.PostType != 0)
            {
                parms.Add("PostType", searchParam.PostType.ToString());
            }

            if (searchParam.OptionType != 0)
            {
                parms.Add("OptionType", searchParam.OptionType.ToString());
            }
            var result = MMClientApi.GetApiWithParams<QueryOptionsModel>("Options", "GetOptionsByPostTypeAndOptionType", parms);
            int skipCount = (searchParam.PageNo - 1) * searchParam.PageSize;
            var pageItems = result.Datas.Skip(skipCount).Take(searchParam.PageSize);
            PagedResultModel<QueryOptionsModel> model = new PagedResultModel<QueryOptionsModel>();
            model.ResultList = pageItems.ToList();
            model.PageSize = searchParam.PageSize;
            model.TotalCount = result.Datas.Count;
            model.PageNo = searchParam.PageNo;
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.Options;

        protected override IActionResult GetInsertView()
        {
            OptionsInputModel model = new OptionsInputModel()
            {
                PostTypeListItem = MMSelectListItem.GetEnumItems<PostType>(SelectEnum.All).Where(x => x.Value != "4").ToList(),
                OptionTypeListItem = MMSelectListItem.GetOptionTypeItems(),
                IsActiveListItem = MMSelectListItem.GetIsActiveItems(),
            };
            return View("Insert", model);
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var optionsBsResult = MMClientApi.GetSingleApi<MMOptionsBs>("Options", "GetOptionsByOptionId", keyContent);
            var listItems = MMSelectListItem.GetIsActiveItems();
            foreach (var item in listItems)
            {
                item.Selected = item.Value == Convert.ToInt16(optionsBsResult.Datas.IsActive).ToString();
            }

            var viewModel = new OptionsInputModel
            {
                MMOptionsBs = optionsBsResult.Datas,
                IsActiveListItem = listItems,
                IsActiveSelected = Convert.ToInt16(optionsBsResult.Datas.IsActive)
            };

            return GetEditView(viewModel);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        protected override BaseReturnModel DoInsert(OptionsInputModel insertModel)
        {
            var model = insertModel.MMOptionsBs;
            string errorMsg = ValidateOptionContent(model.OptionContent, model.OptionType);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return new BaseReturnModel(errorMsg);
            }
            var result = MMClientApi.PostApi("Options", "Create", new MMOptionsBs
            {
                OptionContent = model.OptionContent,
                OptionType = model.OptionType,
                PostType = model.PostType,
                CreateDate = DateTime.Now,
                CreateUser = ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName,
                IsActive = Convert.ToBoolean(insertModel.IsActiveSelected)
            });

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Insert,
                    IsLogTitleValue = true
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.OptioncOntent,
                    OriginValue = model.OptionContent,
                    IsLogTitleValue = true
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            CreateOperationLog(compareContent, _permissionKey);

            return new BaseReturnModel(result.Message);
        }

        protected override BaseReturnModel DoUpdate(OptionsInputModel updateModel)
        {
            var model = updateModel.MMOptionsBs;
            string errorMsg = ValidateOptionContent(model.OptionContent, model.OptionType);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return new BaseReturnModel(errorMsg);
            }

            var source = MMClientApi.GetSingleApi<MMOptionsBs>("Options", "GetOptionsByOptionId", updateModel.MMOptionsBs.OptionId.ToString());

            var result = MMClientApi.PostApi("Options", "Update", new MMOptionsBs
            {
                OptionId = model.OptionId,
                OptionContent = model.OptionContent,
                IsActive = Convert.ToBoolean(updateModel.IsActiveSelected),
                OptionType= model.OptionType,
                PostType=model.PostType,
            });

            if (source != null)
            {
                string optionType = MMSelectListItem.GetOptionTypeItems().Where(a => a.Value == source.Datas?.OptionType.ToString()).SingleOrDefault().Text;

                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.FillInTheField,
                        OriginValue = optionType,
                        IsLogTitleValue=true
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Text,
                        OriginValue = source.Datas?.OptionContent,
                        NewValue = model.OptionContent
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Show,
                        OriginValue = source.Datas?.IsActive==true?"显示":"隐藏",
                        NewValue = Convert.ToBoolean(updateModel.IsActiveSelected)==true?"显示":"隐藏"
                    },
                }, ActTypes.Update);

                if (string.IsNullOrWhiteSpace(compareContent))
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }
                CreateOperationLog(compareContent, _permissionKey);
            }

            return new BaseReturnModel(result.Message);
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            var source = MMClientApi.GetSingleApi<MMOptionsBs>("Options", "GetOptionsByOptionId", keyContent);

            var result = MMClientApi.PostApi("Options", "Delete", keyContent);

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Delete,
                    IsLogTitleValue = true
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.OptioncOntent,
                    OriginValue = source.Datas?.OptionContent,
                    IsLogTitleValue = true
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            CreateOperationLog(compareContent, _permissionKey);

            return new BaseReturnModel(result.Message);
        }

        public static string ValidateOptionContent(string optionContent, int optionType)
        {
            if (!MMValidate.IsValidLength(optionContent, 1, 100))
            {
                return "内文长度为 1 ~ 100 字之间请再确认";
            }

            if (optionType == 2 && !MMValidate.IsNumeric(optionContent))
            {
                return "请输入数字";
            }

            if (!MMValidate.IsChineseOrEnglishOrNumber(optionContent))
            {
                return "仅接受中、英、数";
            }

            return string.Empty;
        }
    }
}