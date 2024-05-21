using Amazon.Auth.AccessControlPolicy;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using BackSideWeb.Models.ViewModel.MM;
using BackSideWeb.Models.ViewModel.PublishRecord;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Demo;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Packaging.Core;
using System.Linq;

namespace BackSideWeb.Controllers
{
    public class HomeAnnouncementController : BaseCRUDController<QueryHomeAnnouncementParam, HomeAnnouncementInputModel, HomeAnnouncementViewModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publicitySettings/homeAnnouncementSearchService.min.js",
            "business/publicitySettings/homeAnnouncementEditSingleRowService.min.js"
        };

        protected override string ClientServiceName => "homeAnnouncementSearchService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.HomeAnnouncement;

        public override ActionResult GetGridViewResult(QueryHomeAnnouncementParam searchParam)
        {
            var result = MMClientApi.GetApi<QueryHomeAnnouncementModel>("HomeAnnouncement", "List");
            var datas = result.Datas.Where(a => a.Type == searchParam.Type);
            if (searchParam.Type == 1)
            {
                if (!string.IsNullOrWhiteSpace(searchParam.Title))
                {
                    datas = datas.Where(a => a.Title != null && a.Title.Contains(searchParam.Title.Trim()));
                }
                if (searchParam.BeginDate != null && searchParam.EndDate != null)
                {
                    if (searchParam.BeginDate.HasValue && searchParam.EndDate.HasValue)
                    {
                        datas = datas.Where(a => a.CreateDate >= searchParam.BeginDate && a.CreateDate <= searchParam.EndDate.Value.AddDays(1));
                    }
                }
                if (searchParam.IsActive != null)
                {
                    datas = datas.Where(a => a.IsActive == searchParam.IsActive);
                }
            }
            result.Datas = datas.OrderBy(a => a.Weight).ToList();
            PagedResultModel<QueryHomeAnnouncementModel> model = new PagedResultModel<QueryHomeAnnouncementModel>();

            // 遍历 result.Datas 中的每个对象
            foreach (var item in result.Datas)
            {
                if (item.HomeContent != null)
                {
                    // 将 HomeContent 转换为纯文本
                    string plainText = MMHelpers.StripHtmlTags(item.HomeContent);

                    item.HomeContent = plainText;
                }
            }
            model.ResultList = result.Datas;
            model.PageSize = searchParam.PageSize;
            model.TotalCount = result.Datas.Count;
            model.PageNo = searchParam.PageNo;

            ViewBag.isAnnouncement = searchParam.Type == 1 ? true : false;
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.HomeAnnouncement;

        protected override IActionResult GetInsertView()
        {
            HomeAnnouncementInputModel model = new HomeAnnouncementInputModel();

            model.Items = new List<SelectListItem>()
            {
                new SelectListItem(){
                    Text="隐藏",
                    Value="false",
                    Selected=true,
                },
                new SelectListItem()
                {
                    Text="显示",
                    Value="true",
                }
            };

            return OpenInsertView(model);
        }

        private IActionResult OpenInsertView<T>(T model)
        {
            SetPageActType(ActTypes.Insert);
            SetPageTitle("新增");
            return View("AddInsert", model);
        }

        private IActionResult GetEditView(HomeAnnouncementViewModel model)
        {
            //此範例為新增與修改共用同一個view
            return View("Edit", model);
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var response = MMClientApi.GetSingleApi<MMHomeAnnouncementBs>("HomeAnnouncement", "Detail", keyContent);
            var model = new HomeAnnouncementViewModel
            {
                Items = MMSelectListItem.GetIsActiveItems(),
                Data = response.Datas,
                SelectOption = Convert.ToInt16(response.Datas.IsActive)
            };
            ViewBag.HomeContent = response.Datas.HomeContent;
            return View("Edit", model);
        }

        protected override BaseReturnModel DoInsert(HomeAnnouncementInputModel insertModel)
        {
            if (insertModel.MMHomeAnnouncementBs.Weight == null)
            {
                return new BaseReturnModel("排序不可为空");
            }
            if (insertModel.MMHomeAnnouncementBs.Weight > 9999)
            {
                return new BaseReturnModel("排序仅可填入0~9999");
            }
            if (string.IsNullOrWhiteSpace(insertModel.MMHomeAnnouncementBs.Title))
            {
                return new BaseReturnModel("标题不可为空");
            }
            if (insertModel.MMHomeAnnouncementBs.Title.Length > 21)
            {
                return new BaseReturnModel("标题上限21字");
            }
            if (string.IsNullOrWhiteSpace(insertModel.MMHomeAnnouncementBs.HomeContent))
            {
                return new BaseReturnModel("内文不可为空");
            }
            string homeContent = MMHelpers.HtmlToString(insertModel.MMHomeAnnouncementBs.HomeContent);
            if (homeContent.Length > 500)
            {
                return new BaseReturnModel("首页公告内容上限500字");
            }

            if ((insertModel.MMHomeAnnouncementBs.StartTime != null && insertModel.MMHomeAnnouncementBs.EndTime != null)
            && !MMValidate.IsCorrectStartDateToEndDate(insertModel.MMHomeAnnouncementBs.StartTime, insertModel.MMHomeAnnouncementBs.EndTime))
            {
                return new BaseReturnModel("开始时间不可大于结束时间");
            }

            string parame = JsonConvert.SerializeObject(insertModel.MMHomeAnnouncementBs);
            var result = MMClientApi.PostObjectApi("HomeAnnouncement", "Create", parame);
            if (result.IsSuccess)
            {
                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "操作成功"
                };
            }
            else
            {
                if (result.Code == "E30001")
                {
                    return new BaseReturnModel("权重重复，请再确认");
                }
                return new BaseReturnModel("新增失败");
            }
        }

        protected override BaseReturnModel DoUpdate(HomeAnnouncementViewModel model)
        {
            model.Data.HomeContent = model.Data.HomeContent == null ? string.Empty : model.Data.HomeContent;
            model.Data.RedirectUrl = model.Data.RedirectUrl == null ? string.Empty : model.Data.RedirectUrl;
            string errorMsg = ValidateInput(model);

            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            var source = MMClientApi.GetSingleApi<MMHomeAnnouncementBs>("HomeAnnouncement", "Detail", model.Data.Id.ToString());

            var result = MMClientApi.PostApi("HomeAnnouncement", "Update", new MMHomeAnnouncementBs
            {
                Id = model.Data.Id,
                HomeContent = model.Data.HomeContent,
                RedirectUrl = "",
                Operator = ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName,
                IsActive = Convert.ToBoolean(model.SelectOption)
            });
            if (result.IsSuccess == false)
            {
                return new BaseReturnModel(result.Code + " " + result.Message);
            }

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Text,
                    OriginValue = source.Datas?.HomeContent,
                    NewValue = model.Data?.HomeContent,
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.Show,
                    OriginValue = source.Datas?.IsActive==true?"显示":"隐藏",
                    NewValue = model.SelectOption==1 ?"显示":"隐藏",
                },
            }, ActTypes.Update);

            if (string.IsNullOrWhiteSpace(compareContent))
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            CreateOperationLog(compareContent, _permissionKey);

            return new BaseReturnModel();
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            var result = MMClientApi.PostApiWithParams("HomeAnnouncement", "Delete", new Dictionary<string, string>
            {
                {"id",keyContent }
            });

            return new BaseReturnModel(result.Message);
        }

        public static string ValidateInput(HomeAnnouncementViewModel model)
        {
            string result = string.Empty;
            if (model.Data.Weight == null && model.Data.Type != 2)
            {
                result = "排序不可为空";
            }
            if (string.IsNullOrWhiteSpace(model.Data.Title) && model.Data.Type != 2)
            {
                result = "标题不可为空";
            }
            if (model.Data.StartTime > model.Data.EndTime && model.Data.Type != 2)
            {
                result = "开始时间不可大于结束时间";
            }
            string homeContent = MMHelpers.HtmlToString(model.Data.HomeContent);
            if (homeContent.Length > 500)
            {
                result = "首页公告内容上限500字";
            }
            if (homeContent.Length <= 0)
            {
                result = "内文不可为空";
            }
            return result;
        }

        public IActionResult HomeAnnouncementEdit(string keyContent)
        {
            SetLayout(LayoutType.EditSingleRow);
            SetPageActType(ActTypes.Update);
            SetPageTitle("编辑");
            ViewBag.SubmitUrl = "DoHomeAnnouncementEdit";
            ViewBag.ClientPopupWindowServiceName = "homeAnnouncementEditSingleRowService";

            var response = MMClientApi.GetSingleApi<MMHomeAnnouncementBs>("HomeAnnouncement", "Detail", keyContent);
            var model = new HomeAnnouncementViewModel
            {
                Items = MMSelectListItem.GetIsActiveItems(),
                Data = response.Datas,
                SelectOption = Convert.ToInt16(response.Datas.IsActive)
            };
            ViewBag.HomeContent = response.Datas.HomeContent;
            return View(model);
        }

        [HttpPost]
        public BaseReturnModel DoHomeAnnouncementEdit(HomeAnnouncementViewModel model)
        {
            string errorMsg = ValidateInput(model);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            var result = MMClientApi.PostApi("HomeAnnouncement", "Update", new MMHomeAnnouncementBs
            {
                Id = model.Data.Id,
                Title = model.Data.Title,
                StartTime = model.Data.StartTime,
                EndTime = model.Data.EndTime,
                HomeContent = model.Data.HomeContent,
                RedirectUrl = "",
                Operator = ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName,
                IsActive = Convert.ToBoolean(model.SelectOption),
                Weight = model.Data.Weight,
                Type = model.Data.Type,
            });
            if (result.IsSuccess == false)
            {
                return new BaseReturnModel(result.Message);
            }
            return new BaseReturnModel();
        }
    }
}