using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace ControllerShareLib.Interfaces.Service
{
    public interface IMiseWebTokenService
    {
        void AddHttpContextUser(HttpContext httpContext, BasicUserInfo basicUserInfo);

        string CreateToken(BasicUserInfo basicUserInfo);

        BasicUserInfo GetTokenModel();

        bool IsCacheTokenValid(BasicUserInfo basicUserInfo);
    }
}