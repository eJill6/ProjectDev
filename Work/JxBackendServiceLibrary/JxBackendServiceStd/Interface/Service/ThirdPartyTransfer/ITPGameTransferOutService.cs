using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer
{
    public interface ITPGameTransferOutService
    {
        BaseReturnModel AllAmountByAllProducts(IInvocationUserParam invocationUserParam);

        BaseReturnModel AllAmountByAllProductsWithMQToClient(IInvocationUserParam invocationUserParam, RoutingSetting routingSetting);

        BaseReturnModel AllAmountBySingleProductQueue(IInvocationUserParam invocationUserParam, PlatformProduct product, RoutingSetting routingSetting);

        BaseReturnModel AllAmountBySingleProduct(PlatformProduct product, IInvocationParam invocationParam);

        BaseReturnModel AllAmountBySingleProduct(PlatformProduct product, IInvocationParam invocationParam, out decimal actuallyAmount, out string moneyId);

        BaseReturnModel SingleProduct(PlatformProduct product, string correlationId);

        Dictionary<string, BaseReturnDataModel<decimal>> UpdateTransferAllResult(int userId, PlatformProduct product, BaseReturnDataModel<decimal> transferResult);

        void RemoveTransferAllResult(int userId);

        bool ProcessTransferAllOutQueue(PlatformProduct product, TransferOutUserDetail transferOutUserDetail);
    }
}