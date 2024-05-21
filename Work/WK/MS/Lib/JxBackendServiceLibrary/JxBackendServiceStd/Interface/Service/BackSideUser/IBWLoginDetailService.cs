using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWLoginDetailService
    {
        BaseReturnDataModel<long> InsertLoginDetail(BWLoginResultParam param);
    }
}