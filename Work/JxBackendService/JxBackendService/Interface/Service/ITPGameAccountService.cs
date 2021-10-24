using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameAccountReadService
    {
        BaseReturnDataModel<UserAccountSearchReault> GetByLocalAccount(string userName);

        BaseReturnDataModel<UserAccountSearchReault> GetByTPGameAccount(PlatformProduct searchProduct, string tpGameAccount);

        BaseReturnDataModel<string> GetTPGameAccountByLocalAccount(int userId, PlatformProduct searchProduct);

        string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId, string userName);

        string GetTPGameUserNameByRule(PlatformProduct platformProduct, int userId);

        Dictionary<string, int> GetUserIdsByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts);

        List<UserProductScore> GetAllTPGameUserScores(int userId, bool isForcedRefresh);
    }

    public interface ITPGameAccountService
    {
        bool CheckTPGAccountExist(int userId, PlatformProduct searchProduct);

        BaseReturnDataModel<long> Create(int userId, string userName, PlatformProduct platformProduct, string tpGameAccount);
    }
}
