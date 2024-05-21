using BackSideWeb.Models.Enums;
using JxBackendService.Attributes;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Setting;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.Setting;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Util.Export;
using JxBackendService.Model.ViewModel.Setting;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BackSideWeb.Controllers.Base
{
    public abstract class BaseSearchGridController<SearchRequestType> : BasePermissionController
    {
        private readonly Lazy<IRouteUtilService> _routeUtilService;

        private readonly Lazy<IExportUtilService> _exportUtilService;

        private readonly Lazy<IRefreshFrequencySettingService> _refreshFrequencySettingService;

        private readonly Lazy<IBWOperationLogService> _bwOperationLogService;

        protected string DefaultClientPopupServiceName => "readSingleRowService";

        protected IRouteUtilService RouteUtilService => _routeUtilService.Value;

        protected BaseSearchGridController()
        {
            PageApiUrlSetting = new PageApiUrlSetting();
            _routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();
            _exportUtilService = DependencyUtil.ResolveService<IExportUtilService>();
            _refreshFrequencySettingService = DependencyUtil.ResolveJxBackendService<IRefreshFrequencySettingService>(EnvLoginUser, DbConnectionTypes.Master);
            _bwOperationLogService = DependencyUtil.ResolveJxBackendService<IBWOperationLogService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        private string? SearchApiUrl => Url.Action(nameof(GetGridViewResult));

        private string? ExportApiUrl => HasExportPermission ? Url.Action(nameof(Export)) : string.Empty;

        private bool HasExportPermission => HasPermission(GetPermissionKey(), AuthorityTypes.Export);

        private string PageTitle => PermissionKeyDetail.GetSingle(GetPermissionKey()).Name;

        protected virtual string[] BaseJavaScripts => new string[]
        {
            "base/searchGrid/baseSearchService.js",
            "base/searchGrid/baseSearchGridService.min.js",
            "base/searchGrid/searchGridInitService.min.js",
        };

        protected virtual string[] PageJavaScripts => new string[]
        {
            "base/searchGrid/commonSearchService.min.js"
        };

        protected virtual string ClientServiceName => "commonSearchService";

        protected virtual string SearchButtonSelector => ".jqSearchBtn";

        protected virtual string FilterSelector => ".jqFilter";

        protected virtual string ResetButtonSelector => ".jqResetBtn";

        protected virtual bool IsRefreshFrequencySetting => false;

        protected virtual bool IsAutoSearchAfterPageLoaded => true;

        protected PageApiUrlSetting PageApiUrlSetting { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.DefaultLayout = LayoutType.SearchGrid.Value;
            ViewBag.BaseJavaScripts = BaseJavaScripts;
            ViewBag.PageJavaScripts = PageJavaScripts;
            ViewBag.SearchButtonSelector = SearchButtonSelector;
            ViewBag.FilterSelector = FilterSelector;
            ViewBag.ResetButtonSelector = ResetButtonSelector;
            ViewBag.ClientServiceName = ClientServiceName;
            ViewBag.IsAutoSearchAfterPageLoaded = IsAutoSearchAfterPageLoaded;
            ViewBag.PageApiUrlSetting = PageApiUrlSetting;
            ViewBag.IsRefreshFrequencySetting = IsRefreshFrequencySetting;

            PageApiUrlSetting.SearchApiUrl = SearchApiUrl;
            PageApiUrlSetting.ExportApiUrl = ExportApiUrl;
        }

        public virtual ActionResult Index()
        {
            SetPageTitleByPermission();

            return View();
        }

        public IActionResult Export(SearchRequestType queryParam)
        {
            ExportFullResultParam exportParam = new ExportFullResultParam { PageTitle = PageTitle };

            exportParam.PageAdditionalData.AddRange(SetExportAdditionalData(queryParam));

            if (queryParam is BasePagedParamsModel)
            {
                (queryParam as BasePagedParamsModel).HasMaxPageSize = false;
                (queryParam as BasePagedParamsModel).PageSize = exportParam.PageGrid.QueryResultLimitCount + 1;
            }

            ActionResult queryResult = GetGridViewResult(queryParam);
            exportParam.PageGrid = ConvertQueryResultToExportParam(queryResult);

            byte[] exportResult = _exportUtilService.Value.ExportFullPageResult(exportParam);

            return File(exportResult, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        protected virtual List<string> SetExportAdditionalData(SearchRequestType queryParam)
        {
            return new List<string>();
        }

        // 這個方法抽成virtual是為了方便若子類的
        // 1. GetGridViewResult的結果不是PartialViewResult
        // 2. model的類型不是PagedResultModel
        // 的話, 也可以自由覆寫
        protected virtual ExportQueryResultParam ConvertQueryResultToExportParam(ActionResult queryResult)
        {
            if (queryResult is not PartialViewResult viewResult
                || viewResult.Model == null
                || !_exportUtilService.Value.TryConvertPagedResultModelToExportParam(viewResult.Model, out ExportQueryResultParam exportParam))
            {
                throw new Exception("查詢結果的物件類型不符合預設類型, 請RD自行定義");
            }

            return exportParam;
        }

        [AjaxValidModelState]
        public abstract ActionResult GetGridViewResult(SearchRequestType requestParam);

        #region 頻率設置相關

        [HttpGet]
        public IActionResult RefreshFrequencySetting()
        {
            InitPopupEditView("refreshFrequencySettingEditService");
            SetPageTitle(DisplayElement.RefreshFrequencySetting);
            ViewBag.SubmitUrl = Url.Action(nameof(SaveRefreshFrequencySetting));
            RefreshFrequencySettingViewModel viewModel = GetRefreshFrequencySettingInfo();

            return View(viewModel);
        }

        [HttpGet]
        public JsonResult GetRefreshFrequencySetting()
        {
            RefreshFrequencySettingViewModel viewModel = GetRefreshFrequencySettingInfo();

            return Json(new { viewModel.IntervalSeconds });
        }

        [HttpPost]
        public IActionResult SaveRefreshFrequencySetting(int intervalSeconds)
        {
            var settingParam = new SaveRefreshFrequencySettingParam()
            {
                PermissionKey = PermissionKeyDetail.GetSingle(GetPermissionKey()).Value,
                IntervalSeconds = intervalSeconds,
            };

            BaseReturnModel returnModel = _refreshFrequencySettingService.Value.SaveRefreshFrequencySetting(settingParam);

            return Json(returnModel);
        }

        #endregion 頻率設置相關

        protected void SetPageTitleByPermission()
        {
            SetPageTitle(PageTitle);
        }

        protected void InitPopupReadView()
        {
            InitPopupReadView(DefaultClientPopupServiceName);
        }

        protected void InitPopupReadView(string clientPopupServiceName)
        {
            SetLayout(LayoutType.ReadSingleRow);
            ViewBag.ClientPopupWindowServiceName = clientPopupServiceName;
        }

        protected void InitPopupEditView(string clientPopupServiceName)
        {
            SetLayout(LayoutType.EditSingleRow);
            ViewBag.ClientPopupWindowServiceName = clientPopupServiceName;
        }

        private RefreshFrequencySettingViewModel GetRefreshFrequencySettingInfo()
        {
            string permissionKey = PermissionKeyDetail.GetSingle(GetPermissionKey()).Value;
            RefreshFrequencySettingViewModel viewModel = _refreshFrequencySettingService.Value.GetBWUserRefreshFrequencySettingInfo(permissionKey);

            return viewModel;
        }

        public string GetOperationCompareContent(List<RecordCompareParam> compareParams, ActTypes actTypes)
        {
            switch (actTypes)
            {
                case ActTypes.Insert:
                case ActTypes.Delete:

                    return GetInsertOperationCompareContent(compareParams);

                case ActTypes.Update:

                    return GetUpdateOperationCompareContent2(compareParams);
            }

            return string.Empty;
        }

        private string GetUpdateOperationCompareContent2(List<RecordCompareParam> compareParams)
        {
            var builders = new List<RecordCompareParam>();

            foreach (var item in compareParams)
            {
                if (item.OriginValue == item.NewValue && !item.IsLogTitleValue)
                {
                    continue;
                }

                if (item.IsLogTitleValue)
                {
                    builders.Add(new RecordCompareParam()
                    {
                        Title = item.Title,
                        OriginValue = item.OriginValue,
                        IsLogTitleValue = item.IsLogTitleValue
                    });
                    continue;
                }
                if (!item.IsVisibleCompareValue)
                {
                    builders.Add(new RecordCompareParam()
                    {
                        Title = item.Title
                    });
                    continue;
                }
                builders.Add(new RecordCompareParam()
                {
                    Title = item.Title,
                    OriginValue = item.OriginValue,
                    NewValue = item.NewValue
                });
            }
            return JsonConvert.SerializeObject(builders);
        }

        private string GetInsertOperationCompareContent(List<RecordCompareParam> compareParams)
            => GetOperationCompareContent(compareParams, (param) =>
            {
                if (!param.IsVisibleCompareValue)
                {
                    return null;
                }

                return $"{param.Title}: {param.NewValue}";
            });

        private string GetUpdateOperationCompareContent(List<RecordCompareParam> compareParams)
            => GetOperationCompareContent(compareParams, (param) =>
            {
                if (param.OriginValue == param.NewValue)
                {
                    return null;
                }

                if (!param.IsVisibleCompareValue)
                {
                    return param.Title;
                }

                return string.Format(BWOperationLogElement.CompareValueMessage,
                    param.Title, param.OriginValue, param.NewValue);
            });

        private string GetOperationCompareContent(List<RecordCompareParam> compareParams,
            Func<RecordCompareParam, string> compare)
        {
            var builders = new List<string>();

            foreach (RecordCompareParam param in compareParams)
            {
                string compareResult = compare(param);

                if (!string.IsNullOrEmpty(compareResult))
                {
                    builders.Add(compareResult);
                }
            }

            return string.Join(", ", builders);
        }

        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="permissionKey">所属模块</param>
        protected void CreateOperationLog(string content, PermissionKeyDetail permissionKey)
        {
            _bwOperationLogService.Value.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = permissionKey,
                Content = content
            });
        }
    }
}