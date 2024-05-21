using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Param.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class LiveGameManageController : BaseCRUDController<LiveGameManageQueryParam, LiveGameManageCreateParam, LiveGameManageUpdateParam>
    {
        private readonly Lazy<ILiveGameManageService> _liveGameManageService;

        private readonly Lazy<ILiveGameManageReadService> _liveGameManageReadService;

        private readonly Lazy<IBoolSelectListItemsService> _boolSelectListItemsService;

        private readonly Lazy<IFrontsideMenuTypeService> _frontsideMenuTypeService;

        protected override string ClientServiceName => "imgConvertToAesCRUDService";

        protected override string ClientEditSingleRowServiceName => "liveGameEditSingleRowService";

        protected override string[] PageJavaScripts => new string[]
        {
            "base/crud/imgConvertToAesCRUDService.min.js"
        };

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.LiveGameManage;

        public LiveGameManageController()
        {
            _liveGameManageService = DependencyUtil.ResolveJxBackendService<ILiveGameManageService>(EnvLoginUser, DbConnectionTypes.Master);
            _liveGameManageReadService = DependencyUtil.ResolveJxBackendService<ILiveGameManageReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _boolSelectListItemsService = DependencyUtil.ResolveService<IBoolSelectListItemsService>();
            _frontsideMenuTypeService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuTypeService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        public override ActionResult Index()
        {
            SetIsActiveSelectListItems(needAll: true);
            SetLiveGameTabTypesSelectListItems(hasBlankOption: true);

            return base.Index();
        }

        protected override BaseReturnModel DoInsert(LiveGameManageCreateParam insertModel)
        {
            string errorMsg = Validate(insertModel);

            if (!errorMsg.IsNullOrEmpty())
            {
                return new BaseReturnModel(errorMsg);
            }

            SetDefaultValue(insertModel);

            return _liveGameManageService.Value.Create(insertModel);
        }

        protected override BaseReturnModel DoUpdate(LiveGameManageUpdateParam updateModel)
        {
            string errorMsg = Validate(updateModel);

            if (!errorMsg.IsNullOrEmpty())
            {
                return new BaseReturnModel(errorMsg);
            }

            SetDefaultValue(updateModel);

            return _liveGameManageService.Value.Update(updateModel);
        }

        protected override BaseReturnModel DoDelete(string keyContent)
            => _liveGameManageService.Value.Delete(keyContent.ToInt32());

        public override ActionResult GetGridViewResult(LiveGameManageQueryParam queryParam)
        {
            PagedResultModel<LiveGameManageModel> model = _liveGameManageReadService.Value.GetPagedModel(queryParam);

            return PartialView(model);
        }

        protected override IActionResult GetInsertView()
        {
            SetSelectListItems();

            return GetEditView(new BaseLiveGameManageParam()
            {
                LiveGameManageDataTypeValue = LiveGameManageDataType.GameCenter.Value,
                TabType = FrontsideMenuTypeSetting.Hot.Value
            });
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            LiveGameManage data = _liveGameManageReadService.Value.GetDetail(keyContent.ToInt32());
            var model = data.CastByJson<BaseLiveGameManageParam>();
            model.LiveGameManageDataTypeValue = LiveGameManageDataType.GetSingle(model).Value;

            SetSelectListItems(model.IsH5, model.IsFollow, model.IsCountdown, model.IsActive,
                model.LiveGameManageDataTypeValue, model.TabType);

            return GetEditView(model);
        }

        private IActionResult GetEditView(BaseLiveGameManageParam model)
        {
            return View("Edit", model);
        }

        private void SetSelectListItems(bool? isH5 = null, bool? isFollow = null, bool? isCountdown = null, bool? isActive = null,
            int liveGameManageDataType = 1, int tabType = 7)
        {
            List<JxBackendSelectListItem> isH5SelectListItems = MMSelectListItem.GetYesNoItems();
            isH5SelectListItems.SetSelected(isH5.ToNonNullString());
            ViewBag.IsH5SelectListItems = isH5SelectListItems;

            List<JxBackendSelectListItem> isFollowSelectListItems = MMSelectListItem.GetIsActiveJxBackendSelectListItemItems();
            isFollowSelectListItems.SetSelected(isFollow.ToNonNullString());
            ViewBag.IsFollowSelectListItems = isFollowSelectListItems;

            List<JxBackendSelectListItem> isCountdownSelectListItems = MMSelectListItem.GetYesNoItems();
            isCountdownSelectListItems.SetSelected(isCountdown.ToNonNullString());
            ViewBag.IsCountdownSelectListItems = isCountdownSelectListItems;

            SetIsActiveSelectListItems(needAll: false, isActive);

            List<JxBackendSelectListItem> dataTypeSelectListItems = LiveGameManageDataType.GetSelectListItems();
            dataTypeSelectListItems.SetSelected(liveGameManageDataType.ToString());
            ViewBag.LiveGameManageDataTypeListItems = dataTypeSelectListItems;

            SetLiveGameTabTypesSelectListItems(hasBlankOption: false, tabType);
        }

        private void SetIsActiveSelectListItems(bool needAll, bool? isActive = null)
        {
            List<JxBackendSelectListItem> isActiveSelectListItems = _boolSelectListItemsService.Value.GetActionSelectListItems();

            if (needAll)
            {
                isActiveSelectListItems.Insert(0, new JxBackendSelectListItem { Value = string.Empty, Text = CommonElement.All, Selected = true });
            }
            else
            {
                isActiveSelectListItems.SetSelected(isActive.ToNonNullString());
            }

            ViewBag.IsActiveSelectListItems = isActiveSelectListItems;
        }

        private void SetLiveGameTabTypesSelectListItems(bool hasBlankOption, int? tabType = null)
        {
            string defaultValue = string.Empty;
            string defaultDisplayText = string.Empty;

            if (hasBlankOption)
            {
                defaultDisplayText = CommonElement.All;
            }

            List<JxBackendSelectListItem> liveGameTabTypesSelectListItems =
                _frontsideMenuTypeService.Value.GetLiveGameTabTypesSelectListItems(hasBlankOption, defaultValue, defaultDisplayText);

            liveGameTabTypesSelectListItems.SetSelected(tabType.ToNonNullString());

            ViewBag.LiveGameTabTypesSelectListItems = liveGameTabTypesSelectListItems;
        }

        private static string Validate(BaseLiveGameManageParam model)
        {
            if (model.LiveGameManageDataTypeValue == LiveGameManageDataType.MiseLottery)
            {
                if (model.LotteryId == 0)
                {
                    return "请填写GameID";
                }

                if (!MMValidate.IsExistDecimalRange(model.FrameRatio.ToString(), 0, 1) ||
                    model.FrameRatio != model.FrameRatio.Floor(4))
                {
                    return "对话框高度比例请填写 0.0000 ~ 1.0000";
                }

                if (!MMValidate.IsExistIntRange(model.Style.ToString(), 1, 9999))
                {
                    return "样式请填写1~9999间正整数";
                }

                if (!MMValidate.IsExistIntRange(model.Duration.ToString(), 1, 9999))
                {
                    return "时长请填写1~9999间正整数";
                }
            }
            else
            {
                if (model.ProductCode.IsNullOrEmpty())
                {
                    return string.Format(MessageElement.FieldIsNotAllowEmpty, DisplayElement.ThirdPartyOwnership);
                }
            }

            return string.Empty;
        }

        private void SetDefaultValue(ILiveGameManageSetDefaultParam model)
        {
            _liveGameManageReadService.Value.SetDefaultValue(model);
        }
    }
}