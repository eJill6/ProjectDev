using JxBackendService.Model.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IUserCommissionListRep
    {
        List<UserCommissionList> GetByProcessMonth(int processMonth);
    }
}
