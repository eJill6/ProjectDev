using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Enums.CTL
{
    public class WalletTypeCTLService : BaseValueModelService<int, WalletType>, IWalletTypeService
    {
        protected override List<WalletType> CreateAllList()
        {
            return base.CreateAllList().Where(w => w == WalletType.Center).ToList();
        }
    }
}
