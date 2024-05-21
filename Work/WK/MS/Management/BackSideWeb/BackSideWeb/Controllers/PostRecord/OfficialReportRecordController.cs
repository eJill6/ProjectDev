using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Param.PublishRecord;
using BackSideWeb.Models;
using BackSideWeb.Models.ViewModel.PublishRecord;
using JxBackendService.Model.Entity.PublishRecord;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminReport;
using Newtonsoft.Json;
using MS.Core.MMModel.Models.AdminPost;
using JxBackendService.Resource.Element;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.BackSideWeb;
using MS.Core.MMModel.Extensions;
using BackSideWeb.Models.Enums;

namespace BackSideWeb.Controllers.PostRecord
{
    public class OfficialReportRecordController : BaseCRUDController<MMPostReportParam, MMPostReportModel, ExamineReportData>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publishRecord/officialReportRecordSearchParam.min.js",
            "business/publishRecord/officialReportRecordSearchService.min.js"
        };

        protected override string ClientServiceName => "officialReportRecordSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.OfficialReportRecord;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.OfficialReportRecord;

        public OfficialReportRecordController()
        {
        }

        public override ActionResult GetGridViewResult(MMPostReportParam requestParam)
        {
            AdminReportListParam param = new AdminReportListParam();

            param.PageSize = requestParam.PageSize;

            param.Page = requestParam.PageNo;

            param.Id = requestParam.Id;
            param.PostId = requestParam.PostId;
            param.UserId = requestParam.UserId;
            param.PostType = MS.Core.MMModel.Models.Post.Enums.PostType.Official;
            param.ReportType = (MS.Core.MMModel.Models.Post.Enums.ReportType?)requestParam.ReportType;
            param.Status = (MS.Core.MMModel.Models.Post.Enums.ReviewStatus?)requestParam.Status;
            param.BeginDate = requestParam.BeginDate;
            param.EndDate = requestParam.EndDate.Date.AddDays(1);

            ViewBag.HasEditPermission = HasPermission(PermissionKeys.ReportRecord, AuthorityTypes.Edit);
            requestParam.PageNo = requestParam.PageNo;
            PagedResultModel<AdminReportList> pagePublishRecordVmModel = new PagedResultModel<AdminReportList>();
            string controller = "AdminReport";
            string action = "List";
            string parame = JsonConvert.SerializeObject(param);
            var result = MMClientApi.PostApi<AdminReportList>(controller, action, parame);
            pagePublishRecordVmModel.PageSize = 10;
            if (result != null && result.IsSuccess)
            {
                if (result.DataModel != null)
                {
                    pagePublishRecordVmModel.ResultList = result.DataModel.Data.ToList();
                    pagePublishRecordVmModel.TotalPageCount = result.DataModel.TotalPage;
                    pagePublishRecordVmModel.PageSize = result.DataModel.PageSize;
                    pagePublishRecordVmModel.PageNo = result.DataModel.Page;
                    pagePublishRecordVmModel.TotalCount = result.DataModel.TotalCount;
                }
            }
            return PartialView(pagePublishRecordVmModel);
        }

        public override ActionResult Index()
        {
            ReportRecordOptionViewModel optionViewModel = this.GetSelectListItemData();

            string postid = HttpContext.Request.Query["postid"];
            string userid = HttpContext.Request.Query["userId"];
            if (!string.IsNullOrWhiteSpace(postid) || !string.IsNullOrWhiteSpace(userid))
            {
                ViewBag.PostId = postid;
                ViewBag.Userid = userid;
            }
            return View(optionViewModel);
        }

        private ReportRecordOptionViewModel GetSelectListItemData()
        {
            ReportRecordOptionViewModel optionViewModel = new ReportRecordOptionViewModel();

            optionViewModel.PostRegionalListItem = MMSelectListItem.GetPostTypeItems();
            optionViewModel.ReportStatusListItem = MMSelectListItem.GetPostStatusItems().Select(item =>
            {
                if (item.Text == "展示中")
                {
                    item.Text = "通过";
                }
                return item;
            }).ToList();
            // 0：騙子、1：廣告騷擾、2：貨不對版、3：無效聯絡方式
            optionViewModel.ReportTypeListItem = MMSelectListItem.GetReportTypeItems();
            return optionViewModel;
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnModel DoInsert(MMPostReportModel insertModel)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnModel DoUpdate(ExamineReportData updateModel)
        {
            if (updateModel.Status == 2 && string.IsNullOrWhiteSpace(updateModel.Memo))
            {
                return new BaseReturnModel(string.Format(MessageElement.FieldIsNotAllowEmpty, DisplayElement.Memo));
            }
            //修改前数据
            var source = MMClientApi.GetApiSingle<AdminReportDetail>("AdminReport", "Detail", updateModel.Id);
            if (source.IsSuccess && (int)source.Data.Status == 0 && updateModel.Status == 0)
            {
                return new BaseReturnModel()
                {
                    IsSuccess = false
                };
            }
            string parame = JsonConvert.SerializeObject(updateModel);
            var result = MMClientApi.PostObjectApi("AdminOfficialReport", $"Edit/{updateModel.Id}", parame);
            if (result.IsSuccess)
            {
                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = DisplayElement.ComplaintID,
                            OriginValue = source.Data.ReportId,
                            IsLogTitleValue = true,
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.Review,
                            OriginValue = source.Data.Status.GetDescription(),
                            NewValue =((ReviewStatus)updateModel.Status).GetDescription()
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.ReasonForFailure,
                            OriginValue = source.Data.Memo,
                            NewValue = updateModel.Memo
                        },
                    }, ActTypes.Update);

                    if (string.IsNullOrWhiteSpace(compareContent))
                    {
                        return new BaseReturnModel(ReturnCode.Success);
                    }
                    CreateOperationLog(compareContent, _permissionKey);
                }

                return new BaseReturnModel()
                {
                    IsSuccess = true,
                    Message = "操作成功"
                };
            }
            else
            {
                return new BaseReturnModel()
                { IsSuccess = false, Message = "操作失败!" };
            }
        }

        protected override IActionResult GetInsertView()
        {
            throw new NotImplementedException();
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            SetPageTitle($"投诉审核");
            ApiResponse<AdminReportDetail> aRequestResult = MMClientApi.GetApiSingle<AdminReportDetail>("AdminReport", "Detail", keyContent);
            return GetEditView(aRequestResult.Data);
        }

        private IActionResult GetEditView<T>(T model)
        {
            //此範例為新增與修改共用同一個view
            return View("Edit", model);
        }

        protected override string? GetInsertViewUrl()
        {
            return null;
        }
    }
}