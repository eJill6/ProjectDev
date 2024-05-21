using Microsoft.Extensions.Logging;
using MS.Core.Infrastructures.Providers;
using MS.Core.Models;
using MS.Core.Utils;
using System.Runtime.CompilerServices;

namespace MS.Core.Services
{
    public class BaseHttpRequestService : BaseService
    {
        protected IRequestIdentifierProvider Provider { get; }

        public BaseHttpRequestService(IRequestIdentifierProvider provider, ILogger logger) : base(logger)
        {
            Provider = provider;
        }

        protected override async Task<ReturnModel> TryCatchProcedure<InputData, ReturnModel>(Func<InputData, Task<ReturnModel>> procedure, InputData param, string failLog = "fail", [CallerMemberName] string methodName = "")
        {
            var result = new ReturnModel();
            try
            {
                if (procedure != null)
                {
                    return await procedure(param);
                }
            }
            catch (Exception ex)
            {
                if (param == null)
                {
                    _logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog}");
                }
                else
                {
                    _logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(ReturnCode.SystemError);
            }
            return result;
        }

        protected override async Task<T> TryCatchProcedure<T>(Func<Task<T>> procedure, string failLog = "fail", [CallerMemberName] string methodName = "")
        {
            var result = new T();
            try
            {
                if (procedure != null)
                {
                    return await procedure();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog}");
            }
            result.SetCode(ReturnCode.SystemError);
            return result;
        }
    }
}
