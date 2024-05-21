using Castle.Core.Internal;
using BackSideWeb.Models.Enums;
using JxBackendService.Attributes;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BackSideWeb.Controllers.Base
{
    public abstract class BaseCRUDController<SearchRequestType, InsertModelType, UpdateModelType> : BaseSearchGridController<SearchRequestType>
    {
        private readonly Lazy<IBWOperationLogService> _bwOperationLogService;
        // private readonly IExportUtilService _exportUtilService;

        public BaseCRUDController()
        {
            // _exportUtilService = DependencyUtil.ResolveService<IExportUtilService>();
            _bwOperationLogService = DependencyUtil.ResolveJxBackendService<IBWOperationLogService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        private string PageTitle => PermissionKeyDetail.GetSingle(GetPermissionKey()).Name;

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

        private static readonly HashSet<string> s_defaultActionsByEditPermission = new HashSet<string>()
        {
            nameof(InsertView),
            nameof(Insert),
            nameof(UpdateView),
            nameof(Update),
        };

        protected virtual string ClientEditSingleRowServiceName => "editSingleRowService";

        protected override string[] BaseJavaScripts
        {
            get
            {
                List<string> baseJavaScripts = base.BaseJavaScripts.ToList();
                baseJavaScripts.Add("base/crud/baseReturnModel.min.js");
                baseJavaScripts.Add("base/crud/baseCRUDService.js");

                return baseJavaScripts.ToArray();
            }
        }

        protected override string[] PageJavaScripts => new string[]
        {
            "base/crud/commonCRUDService.min.js"
        };

        protected override string ClientServiceName => "commonCRUDService";

        protected abstract IActionResult GetInsertView();

        protected abstract IActionResult GetUpdateView(string keyContent);

        protected abstract BaseReturnModel DoInsert(InsertModelType insertModel);

        protected abstract BaseReturnModel DoUpdate(UpdateModelType updateModel);

        protected abstract BaseReturnModel DoDelete(string keyContent);

        #region View Action

        [HttpGet]
        public IActionResult InsertView()
        {
            SetPageActType(ActTypes.Insert);
            InitPopupEditView(ClientEditSingleRowServiceName);
            SetPageTitle("新增资料");
            ViewBag.SubmitUrl = GetInsertActionUrl();

            return GetInsertView();
        }

        [HttpGet]
        public IActionResult UpdateView(string keyContent)
        {
            SetPageActType(ActTypes.Update);
            InitPopupEditView(ClientEditSingleRowServiceName);
            SetPageTitle("编辑资料");
            ViewBag.SubmitUrl = GetUpdateActionUrl();

            IActionResult updateViewResult = GetUpdateView(keyContent);

            if (updateViewResult is ViewResult viewResult && viewResult.Model == null)
            {
                return NoMatchKey();
            }

            return updateViewResult;
        }

        #endregion View Action

        #region CRUD Api Url

        protected virtual string? GetInsertViewUrl()
        {
            return Url.Action(nameof(InsertView));
        }

        protected virtual string? GetUpdateViewUrl()
        {
            return Url.Action(nameof(UpdateView));
        }

        protected virtual string? GetInsertActionUrl()
        {
            return Url.Action(nameof(Insert));
        }

        protected virtual string? GetUpdateActionUrl()
        {
            return Url.Action(nameof(Update));
        }

        protected virtual string? GetDeleteActionUrl()
        {
            return Url.Action(nameof(Delete));
        }

        #endregion CRUD Api Url

        #region CRUD Api Action

        [HttpPost]
        [AjaxValidModelState]
        public virtual IActionResult Insert(InsertModelType insertModel)
        {
            BaseReturnModel baseReturnModel = DoInsert(insertModel);

            return Json(baseReturnModel);
        }

        [HttpPost]
        [AjaxValidModelState]
        public virtual IActionResult Update(UpdateModelType updateModel)
        {
            BaseReturnModel baseReturnModel = DoUpdate(updateModel);

            return Json(baseReturnModel);
        }

        [HttpPost]
        [AjaxValidModelState]
        public virtual IActionResult Delete(string keyContent)
        {
            BaseReturnModel baseReturnModel = DoDelete(keyContent);

            return Json(baseReturnModel);
        }

        #endregion CRUD Api Action

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string actionName = RouteUtilService.GetActionName();

            //由於父類無法單獨掛權限改在這邊統一判斷,Read由更上層判斷,這邊只判斷Edit與Delete
            bool hasEditPermission = HasPermission(GetPermissionKey(), AuthorityTypes.Edit);

            if (hasEditPermission)
            {
                PageApiUrlSetting.InsertViewUrl = GetInsertViewUrl();
                PageApiUrlSetting.UpdateViewUrl = GetUpdateViewUrl();
                PageApiUrlSetting.InsertApiUrl = GetInsertActionUrl();
                PageApiUrlSetting.UpdateApiUrl = GetUpdateActionUrl();
            }
            else if (s_defaultActionsByEditPermission.Contains(actionName))
            {
                SetNoPermissionResult(filterContext);

                return;
            }

            bool hasDeletePermission = HasPermission(GetPermissionKey(), AuthorityTypes.Delete);

            if (hasDeletePermission)
            {
                PageApiUrlSetting.DeleteApiUrl = GetDeleteActionUrl();
            }
            else if (actionName == nameof(Delete))
            {
                SetNoPermissionResult(filterContext);

                return;
            }
        }

        public IActionResult NoMatchKey()
        {
            SetLayout(LayoutType.Base);

            return View(nameof(NoMatchKey));
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

                if (!compareResult.IsNullOrEmpty())
                {
                    builders.Add(compareResult);
                }
            }

            return string.Join(", ", builders);
        }
    }
}