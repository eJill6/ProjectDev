using JxBackendService.Model.Entity.Game.Lottery;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game.Lottery
{
    public interface IPlayTypeInfoRep : IBaseDbRepository<PlayTypeInfo>
    {
        List<PlayTypeInfo> GetAll();
    }
}