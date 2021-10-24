using JxBackendService.Model.Entity.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IUserCommissionInfoRep
    {
        List<UserCommissionInfo> GetByProcessMonth(int processMonth);
    }
}
