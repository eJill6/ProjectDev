using Microsoft.Extensions.Logging;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.Models;
using MS.Core.Utils;
using System.Runtime.CompilerServices;

namespace MS.Core.MM.Services.Bases
{
    public class BaseHttpRequestService : MMBaseService
    {
        private ILogger logger;

        protected IRequestIdentifierProvider Provider { get; }
        protected IDateTimeProvider DateTimeProvider { get; }
        public BaseHttpRequestService(IRequestIdentifierProvider provider, ILogger logger, IDateTimeProvider dateTimeProvider) : base(logger)
        {
            Provider = provider;
            DateTimeProvider = dateTimeProvider;
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
            catch (MMException mmex)
            {
                if (param == null)
                {
                    _logger.LogError(mmex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog} {mmex.Message}");
                }
                else
                {
                    _logger.LogError(mmex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog} {mmex.Message}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(mmex.ReturnCode);
                result.Message = mmex.Message ?? string.Empty;
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
            catch (MMException mmex)
            {
                _logger.LogError(mmex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog} {mmex.Message}");
                result.SetCode(mmex.ReturnCode);
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