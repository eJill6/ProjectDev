using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.BotBet;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class LiveBotController : BaseCRUDController<AnchorInfoParam, LiveBotInput, LiveBotInput>
    {
        private readonly Lazy<IBotParameterRep> _rep;

        public LiveBotController()
        {
            _rep = DependencyUtil.ResolveJxBackendService<IBotParameterRep>(EnvLoginUser, DbConnectionTypes.Master);
        }

        protected override string[] PageJavaScripts => new string[]
        {
          "business/botBet/liveBotService.min.js"
        };

        protected override string ClientServiceName => "liveBotService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.LiveBot;

        public override ActionResult Index()
        {
            var viewModel = new AnchorInfoContextViewModel
            {
                BotGroupItems = MMSelectListItem.GetEnumItemsDefaultNull<BotGroup>(SelectEnum.All),
            };
            return View(viewModel);
        }

        public override ActionResult GetGridViewResult(AnchorInfoParam searchParam)
        {
            PagedResultModel<AnchorInfoContextViewModel> model = new PagedResultModel<AnchorInfoContextViewModel>();
            var result = _rep.Value.GetAnchorInfoContext(searchParam);
            if (result != null)
            {
                model = MMHelpers.MapPagedResultModel<AnchorInfoContext, AnchorInfoContextViewModel>(result);
            }
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.LiveBot;

        protected override IActionResult GetInsertView()
        {
            var model = new AnchorInfoContextViewModel
            {
                BotGroupItems = MMSelectListItem.GetEnumItems<BotGroup>()
            };
            return GetEditView(model);
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            AnchorInfoContext response = _rep.Value.GetAnchorInfoContextDetail(Convert.ToInt64(keyContent));
            AnchorInfoContextViewModel model = new AnchorInfoContextViewModel
            {
                BotGroupItems = MMSelectListItem.GetEnumItems<BotGroup>(),
                Id = response.Id,
                GroupId = response.GroupId,
                OriginalId = response.Id
            };
            return GetEditView(model);
        }

        protected override BaseReturnModel DoInsert(LiveBotInput model)
        {
            string errorMsg = ValidateInput(model, Operation.Insert);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            var result = _rep.Value.CreateAnchorInfoContext(model);
            if (!result)
                return new BaseReturnModel(ReturnCode.SystemError);

            #region 日志记录

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                        {
                            Title = "直播间ID",
                            NewValue = model.Id.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "组别",
                            NewValue = model.GroupIdText
                        }
                }, ActTypes.Insert);

            CreateOperationLog(string.Format("{0}: {1}", PermissionElement.Insert, compareContent), _permissionKey);

            #endregion 日志记录

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoUpdate(LiveBotInput model)
        {
            string errorMsg = ValidateInput(model, Operation.Update);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            AnchorInfoContext response = _rep.Value.GetAnchorInfoContextDetail(model.OriginalId);
            AnchorInfoContextViewModel source = new AnchorInfoContextViewModel
            {
                BotGroupItems = MMSelectListItem.GetEnumItems<BotGroup>(),
                Id = response.Id,
                GroupId = response.GroupId,
                OriginalId = response.Id
            };

            if (model.GroupId != source.GroupId)
            {
                var result = _rep.Value.UpdateAnchorInfoContext(model);
                if (!result)
                    return new BaseReturnModel(ReturnCode.SystemError);

                #region 日志记录

                if (source != null)
                {
                    string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = "编辑",
                            IsLogTitleValue = true
                        },
                        new RecordCompareParam
                        {
                            Title = "直播间ID",
                            IsLogTitleValue = true,
                            OriginValue = source.Id.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "组别",
                            OriginValue = source.GroupIdText,
                            NewValue = model.GroupIdText
                        }
                    }, ActTypes.Update);

                    if (string.IsNullOrWhiteSpace(compareContent))
                    {
                        return new BaseReturnModel(ReturnCode.Success);
                    }
                    CreateOperationLog(compareContent, _permissionKey);
                }

                #endregion 日志记录
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            var result = _rep.Value.DeleteAnchorInfoContext(keyContent);
            if (!result)
                return new BaseReturnModel(ReturnCode.SystemError);

            #region 日志记录

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = "直播间",
                            NewValue = keyContent
                        }
                    }, ActTypes.Delete);

            CreateOperationLog(string.Format("{0}: {1}", PermissionElement.Delete, compareContent), _permissionKey);

            #endregion 日志记录

            return new BaseReturnModel(ReturnCode.Success);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        public string ValidateInput(AnchorInfoContext model, Operation type)
        {
            if (model.Id <= 0)
                return "直播间ID请填入大于0之正整数";

            if (type == Operation.Insert && _rep.Value.IsExistAnchorInfoContext(model.Id))
            {
                return "直播间ID不可重复";
            }

            return string.Empty;
        }
    }
}