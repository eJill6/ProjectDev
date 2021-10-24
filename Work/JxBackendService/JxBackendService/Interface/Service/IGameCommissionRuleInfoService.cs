using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IGameCommissionRuleInfoService
    {
        List<GameCommissionRuleInfo> GetAllGameCommissionRuleInfos(int userId);
        
        List<GameCommissionRuleInfo> GetGameCommissionRuleInfos(CommissionGroupType commissionGroupType, int userId);

        BaseReturnModel CheckRuleInfoForPeriodCommission(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos);

        BaseReturnModel SaveRuleInfoForPeriodCommission(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos);

        BaseReturnModel CheckRuleInfoFixedContractForFrontUser(int userId, CommissionGroupType commissionGroupType, double commissionPercent);

        BaseReturnModel SaveRuleInfoFixedContractForFrontUser(int userId, CommissionGroupType commissionGroupType, double commissionPercent);

        List<string> GetAvailableRuleInfoTableName();
    }
}
