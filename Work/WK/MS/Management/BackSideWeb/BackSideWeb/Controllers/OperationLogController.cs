using BackSideWeb.Controllers.Base;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ViewModel.SystemSetting;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;

namespace BackSideWeb.Controllers
{
    public class OperationLogController : BaseSearchGridController<QueryBWOperationLogParam>
    {
        private readonly Lazy<IBWOperationLogReadService> _bwOperationLogReadService;

        private readonly Lazy<IOperationTypeService> _operationTypeService;

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.OperationLog;

        public OperationLogController()
        {
            _bwOperationLogReadService = DependencyUtil.ResolveJxBackendService<IBWOperationLogReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _operationTypeService = DependencyUtil.ResolveService<IOperationTypeService>();
        }

        protected override string ClientServiceName => "operationLogSearchService";

        protected override string[] PageJavaScripts => new string[]
        {
            "business/operationLog/operationLogSearchService.min.js"
        };

        public override ActionResult GetGridViewResult(QueryBWOperationLogParam requestParam)
        {
            PagedResultModel<OperationLogViewModel> model = _bwOperationLogReadService.Value.GetPagedBWOperationLogs(requestParam);

            return PartialView(model);
        }

        public override ActionResult Index()
        {
            base.Index();

            List<JxBackendSelectListItem> operationTypeSelectListItems = _operationTypeService.Value.GetSelectListItems(
                hasBlankOption: true, defaultValue: null, defaultDisplayText: CommonElement.All);

            ViewBag.OperationTypeSelectListItems = operationTypeSelectListItems;

            return View(new QueryBWOperationLogParam());
        }

        /// <summary>
        /// 根据OperationID查询数据
        /// </summary>
        /// <param name="keyContent"></param>
        /// <returns></returns>
        public IActionResult Detail(int keyContent)
        {
            InitPopupReadView();

            SetPageTitle("日志详细");

            OperationLogViewModel model = _bwOperationLogReadService.Value.GetOperationLogById(keyContent);

            return View(model);
        }
    }
}