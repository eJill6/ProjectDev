using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IThirdPartyUserAccountRep : IBaseDbRepository<ThirdPartyUserAccount>
    {
        List<ThirdPartyUserAccount> GetListByTPGameAccount(HashSet<string> tpGameAccounts);
        List<ThirdPartyUserAccount> GetListByTPGameAccount(PlatformProduct product, HashSet<string> tpGameAccounts);
        List<ThirdPartyUserAccount> GetListByUserId(int userId);
        ThirdPartyUserAccount GetSingleByUserId(int userId, PlatformProduct platformProduct);
    }
}
