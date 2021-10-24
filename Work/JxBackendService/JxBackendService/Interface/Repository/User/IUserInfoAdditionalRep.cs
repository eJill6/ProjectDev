using JxBackendService.Model.Entity.User;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository.User
{
    public interface IUserInfoAdditionalRep : IBaseDbRepository<UserInfoAdditional>
    {
        SPReturnModel SaveUserTransferChildStatus(int userId, string loginUserName, bool isLowMoneyIn, bool? isAllowSetTransferByParent);
    }
}
