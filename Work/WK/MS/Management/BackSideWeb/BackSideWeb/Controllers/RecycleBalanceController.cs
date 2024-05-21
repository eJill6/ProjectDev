using BackSideWeb.Controllers.Base;
using BackSideWeb.Filters;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.Param.Filter;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.RecycleBalance;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class RecycleBalanceController : BaseSearchGridController<QueryRecycleBalance>
    {
        private readonly Lazy<IUserInfoRelatedService> _userInfoRelatedService;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private readonly Lazy<ITPGameAccountReadService> _tpGameAccountReadService;

        private readonly Lazy<IMiseLiveApiService> _miseLiveApiService;

        private readonly Lazy<IRecycleBalanceService> _recycleBalanceService;

        protected override string[] PageJavaScripts => new string[]
        {
            "business/recycleBalance/recycleBalanceService.min.js",
        };

        protected override string[] BaseJavaScripts
        {
            get
            {
                List<string> baseJavaScripts = base.BaseJavaScripts.ToList();
                baseJavaScripts.Add("base/crud/baseReturnModel.min.js");

                return baseJavaScripts.ToArray();
            }
        }

        protected override string ClientServiceName => "recycleBalanceService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.RecycleBalance;

        protected override bool IsAutoSearchAfterPageLoaded => false;

        public RecycleBalanceController()
        {
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Master);
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _miseLiveApiService = DependencyUtil.ResolveJxBackendService<IMiseLiveApiService>(EnvLoginUser, DbConnectionTypes.Slave);
            _recycleBalanceService = DependencyUtil.ResolveJxBackendService<IRecycleBalanceService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        public override ActionResult Index()
        {
            base.Index();

            List<JxBackendSelectListItem> accountTypeSelectListItems = AccountType.GetSelectListItems();
            ViewBag.AccountTypeSelectListItems = accountTypeSelectListItems;

            return View(new QueryRecycleBalance());
        }

        public override ActionResult GetGridViewResult(QueryRecycleBalance requestParam)
        {
            int userId = ConvertToUserId(requestParam);
            var miseAndTPGameBalance = new MiseAndTPGameBalance();

            if (userId == 0)
            {
                return PartialView(miseAndTPGameBalance);
            }

            miseAndTPGameBalance.UserID = userId.ToString();

            MiseLiveResponse<MiseLiveBalance> balanceResponse = _miseLiveApiService
                .Value
                .GetUserBalance(new MiseLiveUserBalanceRequestParam() { UserId = userId });

            if (balanceResponse.Success)
            {
                miseAndTPGameBalance.MiseLiveBalance = balanceResponse.Data;
            }

            BaseReturnDataModel<UserAccountSearchResult> baseReturnDataModel = _tpGameAccountReadService.Value.GetByLocalAccount(userId);

            if (baseReturnDataModel.IsSuccess)
            {
                miseAndTPGameBalance.UserAccountSearchResult = baseReturnDataModel.DataModel;
            }

            return PartialView(miseAndTPGameBalance);
        }

        [HttpPost]
        [ApiLogRequest(isLogToDB: true)]
        public JsonResult RecycleBalance(int userId, string productCode, string routingKey, string requestId)
        {
            IInvocationUserParam invocationUserParam = new InvocationUserParam()
            {
                UserID = userId,
                CorrelationId = GetCorrelationId()
            };

            BaseReturnModel baseReturnModel;
            PlatformProduct product = _platformProductService.Value.GetSingle(productCode);

            var routingSetting = new RoutingSetting()
            {
                RoutingKey = routingKey,
                RequestId = requestId
            };

            if (product == null)
            {
                baseReturnModel = _recycleBalanceService.Value.RecycleByAllProducts(invocationUserParam, routingSetting);
            }
            else
            {
                baseReturnModel = _recycleBalanceService.Value.RecycleBySingleProduct(
                    invocationUserParam,
                    product,
                    routingSetting);
            }

            return Json(baseReturnModel);
        }

        [HttpGet]
        public string? GetMiseLiveBalance(int userId)
        {
            MiseLiveResponse<MiseLiveBalance> balanceResponse = _miseLiveApiService
                .Value
                .GetUserBalance(new MiseLiveUserBalanceRequestParam() { UserId = userId });

            if (balanceResponse.Success)
            {
                return balanceResponse.Data.Balance.ToCurrency();
            }

            return null;
        }

        [HttpGet]
        public string? GetTPGameBalance(int userId, string productCode)
        {
            PlatformProduct product = _platformProductService.Value.GetSingle(productCode);
            var tpGameUserInfoService = DependencyUtil.ResolveJxBackendService<ITPGameUserInfoService>(
                product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DbConnectionTypes.Slave).Value;

            BaseTPGameUserInfo tpGameUserInfo = tpGameUserInfoService.GetTPGameUserInfo(userId);

            if (tpGameUserInfo != null)
            {
                return tpGameUserInfo.AvailableScores.ToCurrency();
            }

            return null;
        }

        private string GetCorrelationId()
        {
            var apiLogRequestHttpContextItem = HttpContext.GetItemValue<ApiLogRequestHttpContextItem>(HttpContextItemKey.ApiLogRequestItem);

            if (apiLogRequestHttpContextItem == null)
            {
                return null;
            }

            return apiLogRequestHttpContextItem.CorrelationId;
        }

        private int ConvertToUserId(QueryRecycleBalance requestParam)
        {
            if (requestParam.AccountType == AccountType.MSUserAccountID)
            {
                if (int.TryParse(requestParam.AccountID, out int userId))
                {
                    return userId;
                }
            }
            else if (requestParam.AccountType == AccountType.TPGameAccountID)
            {
                Dictionary<PlatformProduct, BaseBasicUserInfo> accountMap = _tpGameAccountReadService
                    .Value
                    .GetUsersByTPGameAccount(requestParam.AccountID);

                if (accountMap.Any())
                {
                    return accountMap.First().Value.UserId;
                }
            }

            return 0;
        }
    }
}