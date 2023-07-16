using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;

namespace JxBackendService.Service
{
    public class RecycleBalanceService : BaseBackSideService, IRecycleBalanceService
    {
        private static readonly PermissionKeyDetail s_permissionKey = PermissionKeyDetail.RecycleBalance;

        private readonly ITPGameTransferOutService _tpGameTransferOutService;

        public RecycleBalanceService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        public BaseReturnModel RecycleByAllProducts(IInvocationUserParam invocationUserParam, RoutingSetting routingSetting)
        {
            BaseReturnModel baseReturnModel = _tpGameTransferOutService.AllAmountByAllProductsWithMQToClient(
                    invocationUserParam,
                    routingSetting);

            CreateOperationLog(invocationUserParam, product: null);

            return baseReturnModel;
        }

        public BaseReturnModel RecycleBySingleProduct(IInvocationUserParam invocationUserParam, PlatformProduct product, RoutingSetting routingSetting)
        {
            BaseReturnModel baseReturnModel = _tpGameTransferOutService.AllAmountBySingleProductQueue(
                invocationUserParam,
                product,
                routingSetting);

            CreateOperationLog(invocationUserParam, product);

            return baseReturnModel;
        }

        private void CreateOperationLog(IInvocationUserParam invocationUserParam, PlatformProduct product)
        {
            string content = string.Empty;

            if (product == null)
            {
                content = string.Format(RecycleBalanceElement.RecycleBalanceByAllProducts,
                    invocationUserParam.UserID);
            }
            else
            {
                content = string.Format(RecycleBalanceElement.RecycleBalanceBySingleProduct,
                    invocationUserParam.UserID,
                    product.Name);
            }

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = s_permissionKey,
                UserID = invocationUserParam.UserID,
                Content = content,
                ReferenceKey = invocationUserParam.CorrelationId
            });
        }
    }
}