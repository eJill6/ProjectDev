using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWChangePasswordService
    {
        BaseReturnModel ChangePassword(ChangePasswordParam param);

        bool IsPasswordExpiredCheckWithCache();
    }
}