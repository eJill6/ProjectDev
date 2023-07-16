using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameAccountReadService
    {
        BaseReturnDataModel<UserAccountSearchResult> GetByLocalAccount(int userId);

        BaseReturnDataModel<UserAccountSearchResult> GetByTPGameAccount(PlatformProduct searchProduct, string tpGameAccount);

        CreateRemoteAccountParam GetTPGameAccountByLocalAccount(int userId, PlatformProduct searchProduct, out bool isDbExists);

        string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId);

        string GetTPGameUserNameByRule(PlatformProduct platformProduct, int userId);

        Dictionary<string, int> GetUserIdsByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts);

        Dictionary<string, BaseBasicUserInfo> GetUsersByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts);

        List<UserProductScore> GetAllTPGameUserScores(SearchAllGameUserScoresParam param);

        ThirdPartyUserAccount GetThirdPartyUserAccount(int userId, PlatformProduct searchProduct);

        Dictionary<PlatformProduct, BaseBasicUserInfo> GetUsersByTPGameAccount(string tpGameAccount);
    }

    public interface ITPGameAccountService
    {
        bool CheckTPGAccountExist(int userId, PlatformProduct searchProduct);

        BaseReturnDataModel<long> Create(int userId, PlatformProduct platformProduct, string tpGameAccount, string tpGamePassword);

        BaseReturnModel UpdatePassword(int userId, PlatformProduct platformProduct, string tpGamePassword);
    }
}