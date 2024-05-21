using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IThirdPartyUserAccountRep : IBaseDbRepository<ThirdPartyUserAccount>
    {
        Dictionary<PlatformProduct, BaseTPGameUserInfo> GetAllTPGameUserInfoMap(int userId);

        List<ThirdPartyUserAccount> GetListByTPGameAccount(HashSet<string> tpGameAccounts);

        List<ThirdPartyUserAccount> GetListByTPGameAccount(PlatformProduct product, HashSet<string> tpGameAccounts);

        List<ThirdPartyUserAccount> GetListByUserId(int userId);

        List<ThirdPartyUserAccount> GetListByUserIdsAndThirdPartyType(string thirdPartyType, IEnumerable<int> userIds);

        ThirdPartyUserAccount GetSingleByUserId(int userId, PlatformProduct platformProduct);
    }
}