using JxBackendService.Model.Common;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceNet45.Interface.Service.Finance
{
    public interface IUserFinanceReadService
    {
        List<ProSelectBankResult> GetUserAllBankCard(int userId);

        bool HasUserActiveUsdtAccount();
     
        List<JxBackendSelectListItem> GetBankTypeListItems(int? moneyInType);
    }

    public interface IUserFinanceService
    {
        BaseReturnModel TransferToChild(TransferToChildParam param);

        BaseReturnModel UpdateUSDTWallet(BlockChainInfoParam param);
    }
}
