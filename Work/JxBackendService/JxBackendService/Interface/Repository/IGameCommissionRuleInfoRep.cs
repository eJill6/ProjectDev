using System.Collections.Generic;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;

namespace JxBackendService.Interface.Repository
{
    public interface IGameCommissionRuleInfoRep
    {
        List<GameCommissionRuleInfo> GetGameCommissionRuleInfos(int userId);

        BaseReturnModel SaveRuleInfo(List<SaveCommissionRuleInfo> saveCommissionRuleInfos);

        List<GameCommissionRuleInfo> GetGameCommissionRuleInfosByParentId(int parentId);

        string GetRuleInfoTableName();
    }
}