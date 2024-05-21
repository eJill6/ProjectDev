using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.Param.Filter;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Core.Filters;

namespace SLPolyGame.Web.Core.Controllers
{
    /// <summary>轉帳</summary>
    [ApiLogRequest(isLogToDB: true)]
    public class TransferController : BaseApiController
    {
        /// <summary>TransferController ctor</summary>
        public TransferController()
        {
        }

        /// <summary>轉回所有第三方遊戲餘額</summary>
        [HttpGet]
        public BaseMiseLiveResponse TransferOutAllTPGameBalance(int userId)
        {
            EnvironmentUser currentUser = CreateEnvironmentUser(userId);

            IInvocationUserParam invocationUserParam = new InvocationUserParam()
            {
                UserID = userId,
                CorrelationId = GetCorrelationId()
            };

            ITPGameTransferOutService tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(
                currentUser,
                DbConnectionTypes.Slave).Value;

            BaseReturnModel baseReturnModel = tpGameTransferOutService.AllAmountByAllProducts(invocationUserParam);
            baseReturnModel = ConvertTryTooOftenToSuccess(baseReturnModel, userId);

            return baseReturnModel.ToMiseLiveResponse();
        }

        /// <summary>嘗試過於頻繁時，為了不讓用戶端一直跳出錯誤訊息，寫 log 並反轉結果</summary>
        private BaseReturnModel ConvertTryTooOftenToSuccess(BaseReturnModel baseReturnModel, int userId)
        {
            if (baseReturnModel.Code == ReturnCode.TryTooOften)
            {
                LogUtilService.ForcedDebug($"TransferOutAllTPGameBalance userId={userId}, Message={baseReturnModel.Message}");
                baseReturnModel = new BaseReturnModel(ReturnCode.Success);
            }

            return baseReturnModel;
        }

        /// <summary>轉回上一次進入的第三方遊戲餘額</summary>
        [HttpGet]
        public BaseMiseLiveResponse TransferOutLastTPGameBalance(int userId)
        {
            EnvironmentUser currentUser = CreateEnvironmentUser(userId);
            string? lastAutoTransProductCode = GetLastAutoTransProductCode(userId);

            if (lastAutoTransProductCode.IsNullOrEmpty())
            {
                return new BaseMiseLiveResponse()
                {
                    Success = true,
                };
            }

            return TransferAllBySingleProductToMiseLive(currentUser, lastAutoTransProductCode!);
        }

        private string? GetLastAutoTransProductCode(int userId)
        {
            var userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvLoginUser, DbConnectionTypes.Slave).Value;
            UserInfoAdditional userInfoAdditional = userInfoRelatedService.GetUserInfoAdditional(userId);

            // 沒設定過資料，不做後續處理
            if (userInfoAdditional == null ||
                userInfoAdditional.UserTransferSettingJson == null)
            {
                return null;
            }

            // 最後自動轉帳ProductCode
            string lastAutoTransferProductCode = userInfoAdditional.GetUserTransferSetting().LastAutoTransProductCode.ToTrimString();

            return lastAutoTransferProductCode;
        }

        private BaseMiseLiveResponse TransferAllBySingleProductToMiseLive(EnvironmentUser currentUser, string productCode)
        {
            PlatformProduct platformProduct = PlatformProduct.GetSingle(productCode);
            string correlationId = GetCorrelationId();

            IInvocationUserParam invocationUserParam = new InvocationUserParam
            {
                UserID = currentUser.LoginUser.UserId,
                CorrelationId = correlationId
            };

            ITPGameTransferOutService tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(
                currentUser,
                DbConnectionTypes.Master).Value;

            BaseReturnModel baseReturnModel = tpGameTransferOutService.AllAmountBySingleProductQueue(
                invocationUserParam,
                platformProduct,
                routingSetting: null);

            baseReturnModel = ConvertTryTooOftenToSuccess(baseReturnModel, currentUser.LoginUser.UserId);

            return baseReturnModel.ToMiseLiveResponse();
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
    }
}