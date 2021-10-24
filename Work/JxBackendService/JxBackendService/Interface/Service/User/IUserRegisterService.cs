using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.User
{
    public interface IUserRegisterService
    {
        BaseReturnModel UrlRegisterUser(UserRegisterParam userRegisterParam);

        BaseReturnModel CheckVIPAgentQualify();

        BaseReturnModel AppliedForVIPAgent(string userPwdHash);
    }
}
