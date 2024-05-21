using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface IGameTypeManageService
    {
        BaseReturnModel Update(List<GameCenterUpdateParam> updateParams);
    }
}