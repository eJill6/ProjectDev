using JxBackendService.Model.Entity.Util;

namespace JxBackendService.Interface.Repository.Util
{
    public interface IMachineGeneratorRep : IBaseDbRepository<MachineGenerator>
    {
        int GetOrCreateGeneratorId(string machineName, int maxGeneratorId);
    }
}