using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Param.PublishRecord;
using BackSideWeb.Models;
using BackSideWeb.Models.ViewModel.PublishRecord;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Extensions;
using Newtonsoft.Json;
using BackSideWeb.Models.Enums;

namespace BackSideWeb.Controllers.PostRecord
{
    public class EvaluateRecordController : BaseCRUDController<EvaluateRecordParam, AdminCommentDetail, ExaminePostCommentData>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/publishRecord/evaluateRecordSearchParam.min.js",
            "business/publishRecord/evaluateRecordSearchService.min.js"
        };

        protected override string ClientServiceName => "evaluateRecordSearchService";

        // private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.EvaluateRecord;

        public override ActionResult GetGridViewResult(EvaluateRecordParam requestParam)
        {
            AdminCommentListParam param = new AdminCommentListParam();

            param.PageSize = requestParam.PageSize;
            param.PageNo = requestParam.PageNo;

            param.Id = requestParam.Id;
            param.PostId = requestParam.PostId;
            param.UserId = requestParam.UserId;
            param.PostType = (MS.Core.MMModel.Models.Post.Enums.PostType?)requestParam.PostType;
            param.Status = (MS.Core.MMModel.Models.Post.Enums.ReviewStatus?)requestParam.Status;
            param.DateTimeType = requestParam.DateTimeType;
            param.BeginDate = requestParam.BeginDate;
            param.EndDate = requestParam.EndDate.Date.AddDays(1);

            ViewBag.HasEditPermission = HasPermission(PermissionKeys.EvaluateRecord, AuthorityTypes.Edit);
            requestParam.PageNo = requestParam.PageNo;
            PagedResultModel<AdminCommentList> pagePublishRecordVmModel = new PagedResultModel<AdminCommentList>();
            string controller = "AdminComment";
            string action = "List";
            string parame = JsonConvert.SerializeObject(param);
            var result = MMClientApi.PostApi<AdminCommentList>(controller, action, parame);
            pagePublishRecordVmModel.PageSize = 10;
            if (result != null && result.IsSuccess)
            {
                if (result.DataModel != null)
                {
                    pagePublishRecordVmModel.ResultList = result.DataModel.Data.ToList();
                    pagePublishRecordVmModel.TotalPageCount = result.DataModel.TotalPage;
                    pagePublishRecordVmModel.PageSize = result.DataModel.PageSize;
                    pagePublishRecordVmModel.PageNo = result.DataModel.PageNo;
                    pagePublishRecordVmModel.TotalCount = result.DataModel.TotalCount;
                }
            }
            return PartialView(pagePublishRecordVmModel);
        }

        public override ActionResult Index()
        {
            EvaluateRecordOptionViewModel optionViewModel = this.GetSelectListItemData();

            return View(optionViewModel);
        }

        /// <summary>
        /// 获取下拉列表选择框
        /// </summary>
        /// <returns></returns>
        private EvaluateRecordOptionViewModel GetSelectListItemData()
        {
            EvaluateRecordOptionViewModel optionViewModel = new EvaluateRecordOptionViewModel();
            optionViewModel.PostRegionalListItem = new List<SelectListItem>()
            {
                 new SelectListItem(){
                    Text="全部",
                    Value=null,
                    Selected=true,
                },
                new SelectListItem()
                {
                    Text="广场",
                    Value="1",
                },
                new SelectListItem()
                {
                    Text="寻芳阁",
                    Value="2",
                },
                new SelectListItem()
                {
                    Text="官方",
                    Value="3",
                }
            };
            optionViewModel.CommentStatusListItem = MMSelectListItem.GetEnumItemsDefaultNull<MS.Core.MMModel.Models.Post.Enums.ReviewStatus>(SelectEnum.All);
            optionViewModel.TimeTypeListItem = MMSelectListItem.GetEnumItems<TimeTypeEnum>();
            return optionViewModel;
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnModel DoInsert(AdminCommentDetail insertModel)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnModel DoUpdate(ExaminePostCommentData updateModel)
        {
            var source = MMClientApi.GetApiSingle<AdminCommentDetail>("AdminComment", "Detail", updateModel.Id);

            string parame = JsonConvert.SerializeObject(updateModel);
            var result = MMClientApi.PostObjectApi("AdminComment", $"Edit/{updateModel.Id}", parame);
            if (result.IsSuccess)
            {
                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = DisplayElement.EvaluationID,
                            OriginValue = source.Data.CommentId,
                            IsLogTitleValue = true,
                        },
                        new RecordCompareParam
                        {
                            Title = DisplayElement.Review,
                            OriginValue = source.Data.Status.GetDescription(),
                            NewValue = ((ReviewStatus)updateModel.Status).GetDescription()
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
                    //CreateOperationLog(compareContent, _permissionKey);
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
            return GetEditView(new AdminCommentDetail());
        }

        private IActionResult GetEditView<T>(T model)
        {
            //此範例為新增與修改共用同一個view
            return View("Edit", model);
        }

        //protected override PermissionKeys GetPermissionKey() => PermissionKeys.EvaluateRecord;
        protected override PermissionKeys GetPermissionKey()
        {
            return PermissionKeys.EvaluateRecord;
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            SetPageTitle($"评论审核");
            ApiResponse<AdminCommentDetail> aRequestResult = MMClientApi.GetApiSingle<AdminCommentDetail>("AdminComment", "Detail", keyContent);
            return GetEditView<AdminCommentDetail>(aRequestResult.Data);
        }

        protected override string? GetInsertViewUrl()
        {
            return null;
        }
    }
}