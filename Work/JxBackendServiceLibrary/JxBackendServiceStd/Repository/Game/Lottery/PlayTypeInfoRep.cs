using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;

namespace JxBackendService.Repository.Game.Lottery
{
    public class PlayTypeInfoRep : BaseDbRepository<PlayTypeInfo>, IPlayTypeInfoRep
    {
        public PlayTypeInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<PlayTypeInfo> GetAll() => GetAll(InlodbType.Inlodb);
    }
}