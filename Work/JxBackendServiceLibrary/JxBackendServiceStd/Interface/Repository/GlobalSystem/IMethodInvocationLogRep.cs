using JxBackendService.Model.Entity.GlobalSystem;

namespace JxBackendService.Interface.Repository.GlobalSystem
{
    public interface IMethodInvocationLogRep : IBaseDbRepository<MethodInvocationLog>
    {
        string CreateSEQID();
    }
}