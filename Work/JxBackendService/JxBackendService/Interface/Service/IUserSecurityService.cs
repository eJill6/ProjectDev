using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IUserSecurityService
    {
        string CreateSecurityToken(int userId, TokenType tokenType, int stepId, string data);
        
        BaseReturnModel IsSecurityTokenValid(int userId, string accessToken, TokenType tokenType, int tokenStepId, int expiredSeconds, string data);
    }
}
