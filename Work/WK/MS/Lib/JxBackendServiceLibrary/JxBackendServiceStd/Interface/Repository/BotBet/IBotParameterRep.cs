using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Repository.BotBet
{
    public interface IBotParameterRep : IBaseDbRepository<SettingInfoContext>
    {
        PagedResultModel<SettingInfoContext> GetSettingInfoContext(BotBetParam param);
        SettingInfoContext GetSettingInfoContextDetail(int id, int type);
        bool CreateSettingInfoContext(BotParameterInput param);

        bool UpdateSettingInfoContext(BotParameterInput param);

        bool DeleteSettingInfoContext(string keyContent);

        PagedResultModel<AnchorInfoContext> GetAnchorInfoContext(AnchorInfoParam param);
        AnchorInfoContext GetAnchorInfoContextDetail(long id);
        bool IsExistAnchorInfoContext(long id);
        bool CreateAnchorInfoContext(AnchorInfoContext param);

        bool UpdateAnchorInfoContext(AnchorInfoContext param);

        bool DeleteAnchorInfoContext(string keyContent);
        bool IsLast(BotParameterInput param);
    }
}
