using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IColourEggService
    {
        BaseReturnDataModel<int> CheckReopened(int userId, string ip, bool isInsertLog = false);
    }
}
