using JxBackendService.Model.Entity.Game.Lottery;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game.Lottery
{
    public interface IPlayTypeRadioRep : IBaseDbRepository<PlayTypeRadio>
    {
        List<PlayTypeRadio> GetAll();
    }
}