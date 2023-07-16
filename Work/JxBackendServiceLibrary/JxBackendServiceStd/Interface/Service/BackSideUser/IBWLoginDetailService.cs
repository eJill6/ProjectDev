using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Authenticator;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Authenticator;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWLoginDetailService
    {
        BaseReturnDataModel<long> InsertLoginDetail(BWLoginResultParam param);
    }
}