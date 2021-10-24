using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Repository.Security
{
    public interface IBlackAreaListRep : IBaseDbRepository<BlackAreaList>
    {
        ExistInBlackAreaListResult GetExistInBlackAreaList(JxIpInformation jxIpInformation, BlackIpType blackIpType);        
    }
}
