using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.ViewModel;
using System;
using TencentCloud.Ssl.V20191205.Models;

namespace JxBackendService.Interface.Service.GlobalSystem
{
    public interface IErrorMsgUtilService
    {
        IdGenerator CreateErrorIdGenerator(Func<int> getGeneratorIdJob);

        void ErrorHandle(Exception exception, EnvironmentUser environmentUser, SendErrorMsgTypes sendErrorMsgType);

        void ErrorHandle(Exception exception, EnvironmentUser environmentUser, SendErrorMsgTypes sendErrorMsgType, long? errorId);
    }
}