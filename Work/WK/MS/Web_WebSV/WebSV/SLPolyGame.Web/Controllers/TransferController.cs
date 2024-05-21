using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty.MiseLive;
using SLPolyGame.Web.Controllers.Base;
using SLPolyGame.Web.Extensions;
using SLPolyGame.Web.Filters;
using SLPolyGame.Web.Models;
using System.Net.Http;
using System.Web.Http;

namespace SLPolyGame.Web.Controllers
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
            Request.SetRequestPropertyValue(RequestMessagePropertyKey.EnvironmentUser, currentUser);

            IInvocationUserParam invocationUserParam = new InvocationUserParam()
            {
                UserID = userId,
                CorrelationId = GetCorrelationId()
            };

            ITPGameTransferOutService tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(
                currentUser,
                DbConnectionTypes.Slave);

            BaseReturnModel baseReturnModel = tpGameTransferOutService.AllAmountByAllProducts(
                invocationUserParam,
                isOperateByBackSide: false);

            if (!baseReturnModel.IsSuccess)
            {
                return baseReturnModel.ToMiseLiveResponse();
            }

            BaseReturnModel updateResult = ClearLastAutoTransInfo(currentUser);

            return updateResult.ToMiseLiveResponse();
        }

        /// <summary>轉回上一次進入的第三方遊戲餘額</summary>
        [HttpGet]
        public MiseLiveResponse<MiseLiveTransferOutResult> TransferOutLastTPGameBalance(int userId)
        {
            EnvironmentUser currentUser = CreateEnvironmentUser(userId);
            Request.SetRequestPropertyValue(RequestMessagePropertyKey.EnvironmentUser, currentUser);
            string lastAutoTransProductCode = GetLastAutoTransProductCode(userId);

            if (lastAutoTransProductCode.IsNullOrEmpty())
            {
                return new MiseLiveResponse<MiseLiveTransferOutResult>()
                {
                    Success = true,
                    Data = new MiseLiveTransferOutResult()
                };
            }

            return TransferAllBySingleProductToMiseLive(currentUser, lastAutoTransProductCode);
        }

        private string GetLastAutoTransProductCode(int userId)
        {
            var userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvLoginUser, DbConnectionTypes.Slave);
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

        private MiseLiveResponse<MiseLiveTransferOutResult> TransferAllBySingleProductToMiseLive(EnvironmentUser currentUser, string productCode)
        {
            PlatformProduct platformProduct = PlatformProduct.GetSingle(productCode);
            string correlationId = GetCorrelationId();
            IInvocationParam invocationParam = new InvocationUserParam { CorrelationId = correlationId };

            ITPGameTransferOutService tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(
                currentUser,
                DbConnectionTypes.Slave);

            BaseReturnModel baseReturnModel = tpGameTransferOutService.AllAmountBySingleProduct(
                platformProduct,
                isSynchronizing: true,
                invocationParam,
                out decimal actuallyAmount,
                out string moneyId);

            if (!baseReturnModel.IsSuccess)
            {
                return baseReturnModel.ToMiseLiveResponse<MiseLiveTransferOutResult>();
            }

            BaseReturnModel updateResult = ClearLastAutoTransInfo(currentUser);

            if (!updateResult.IsSuccess)
            {
                return new MiseLiveResponse<MiseLiveTransferOutResult>()
                {
                    Success = false,
                    Error = updateResult.Message
                };
            }

            //轉回MiseLive
            var withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(currentUser, DbConnectionTypes.Master);
            BaseReturnModel withdrawResult = withdrawService.WithdrawToMiseLive(actuallyAmount);

            if (!withdrawResult.IsSuccess)
            {
                return withdrawResult.ToMiseLiveResponse<MiseLiveTransferOutResult>();
            }

            return new MiseLiveResponse<MiseLiveTransferOutResult>()
            {
                Success = true,
                Data = new MiseLiveTransferOutResult()
                {
                    Amount = actuallyAmount
                }
            };
        }

        /// <summary>清除上一家自動轉入ProductCode</summary>
        private BaseReturnModel ClearLastAutoTransInfo(EnvironmentUser currentUser)
        {
            var userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(currentUser, DbConnectionTypes.Master);

            return userInfoRelatedService.UpdateLastAutoTransInfo(currentUser.LoginUser.UserId, null);
        }

        private string GetCorrelationId()
        {
            string correlationId = ActionContext.ActionArguments["CorrelationId"] as string;

            if (correlationId.IsNullOrEmpty())
            {
                correlationId = Request.GetCorrelationId().ToString();
            }

            return correlationId;
        }
    }
}