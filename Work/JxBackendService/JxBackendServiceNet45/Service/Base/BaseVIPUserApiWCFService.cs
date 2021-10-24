using System.Collections.Generic;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Interface.Service.VIP.Activity;
using JxBackendService.Interface.Service.VIP.Bonus;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.VIP;
using JxBackendService.Service.Base;
using JxBackendServiceNet45.Interface.Service.VIP;

namespace JxBackendServiceNet45.Service.Base
{
    public abstract class BaseVIPUserApiWCFService : BaseApplicationService, IVIPUserApiService
    {
        public BaseReturnModel ReceiveVIPBonus(int vipBonusTypeValue)
        {
            VIPBonusType vipBonusType = VIPBonusType.GetSingle(vipBonusTypeValue);

            var vipBonusTypeService = ResolveJxBackendService<IVIPBonusTypeService>(vipBonusType, DbConnectionTypes.Master);
            BaseReturnModel returnModel = vipBonusTypeService.ReceiveBonus();

            return returnModel;
        }

        public int GetVIPLevel(int userId)
        {
            var vipUserService = ResolveJxBackendService<IVIPUserService>(DbConnectionTypes.Slave);

            return vipUserService.GetUserCurrentLevel(userId);
        }

        public BaseReturnModel ApplyForMonthlyDesposit(int vipEventTypeValue)
        {
            VIPEventType vipEventType = VIPEventType.GetSingle(vipEventTypeValue);

            var vipUserEventDetailService = ResolveJxBackendService<IVIPUserEventDetailService>(vipEventType, DbConnectionTypes.Master);

            BaseReturnModel returnModel = vipUserEventDetailService.VIPUserApplyForActivity();

            return returnModel;
        }

        public PagedResultModel<VIPPointsChangeLogModel> GetVIPPointsChangeLogs(BaseScoreSearchParam param)
        {
            var vipUserChangeLogService = ResolveJxBackendService<IVIPUserChangeLogService>(DbConnectionTypes.Slave);

            BaseUserScoreSearchParam searchParam = param.CastByJson<BaseUserScoreSearchParam>();
            searchParam.UserID = EnvLoginUser.LoginUser.UserId;

            return vipUserChangeLogService.GetVIPPointsChangeLogs(searchParam);
        }
        
        public PagedResultModel<VIPFlowChangeLogModel> GetVIPFlowChangeLogs(BaseScoreSearchParam param)
        {
            var vipUserChangeLogService = ResolveJxBackendService<IVIPUserChangeLogService>(DbConnectionTypes.Slave);

            BaseUserScoreSearchParam searchParam = param.CastByJson<BaseUserScoreSearchParam>();
            searchParam.UserID = EnvLoginUser.LoginUser.UserId;

            return vipUserChangeLogService.GetVIPFlowChangeLogs(searchParam);
        }

        public PagedResultModel<VIPAgentAccountLogModel> GetVIPAgentScoreChangeLogs(BaseScoreSearchParam param)
        {
            var vipUserChangeLogService = ResolveJxBackendService<IVIPUserChangeLogService>(DbConnectionTypes.Slave);

            BaseUserScoreSearchParam searchParam = param.CastByJson<BaseUserScoreSearchParam>();
            searchParam.UserID = EnvLoginUser.LoginUser.UserId;

            return vipUserChangeLogService.GetVIPAgentScoreChangeLogs(searchParam);
        }

        public List<VIPLevelSetting> GetVIPSettings()
        {
            var vipSettingService = ResolveJxBackendService<IVIPSettingService>(DbConnectionTypes.Slave);

            return vipSettingService.GetAll();
        }
    }
}