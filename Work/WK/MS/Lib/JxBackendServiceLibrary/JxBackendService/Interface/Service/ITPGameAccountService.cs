using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameAccountReadService
    {
        BaseReturnDataModel<UserAccountSearchReault> GetByLocalAccount(int userId);

        BaseReturnDataModel<UserAccountSearchReault> GetByTPGameAccount(PlatformProduct searchProduct, string tpGameAccount);

        BaseReturnDataModel<string> GetTPGameAccountByLocalAccount(int userId, PlatformProduct searchProduct);

        string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId);

        string GetTPGameUserNameByRule(PlatformProduct platformProduct, int userId);

        Dictionary<string, int> GetUserIdsByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts);

        Dictionary<string, BaseBasicUserInfo> GetUsersByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts);

        List<UserProductScore> GetAllTPGameUserScores(SearchAllGameUserScoresParam param);

        ThirdPartyUserAccount GetThirdPartyUserAccount(int userId, PlatformProduct searchProduct);
    }

    public interface ITPGameAccountService
    {
        bool CheckTPGAccountExist(int userId, PlatformProduct searchProduct);

        BaseReturnDataModel<long> Create(int userId, PlatformProduct platformProduct, string tpGameAccount);
    }
}