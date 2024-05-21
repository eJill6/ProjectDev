using Microsoft.Extensions.Logging;
using MS.Core.Infrastructures.Exceptions;
using MS.Core.Models;
using MS.Core.Utils;
using System.Runtime.CompilerServices;

namespace MS.Core.Services
{
    public class BaseService
    {
        /// <inheritdoc cref="ILogger"/>
        protected readonly ILogger _logger = null;

        public BaseService(ILogger logger)
        {
            _logger = logger;
        }

        protected virtual async Task<ReturnModel> TryCatchProcedure<InputData, ReturnModel>(Func<InputData, Task<ReturnModel>> procedure, InputData param, string failLog = "fail", [CallerMemberName] string methodName = "") where ReturnModel : BaseReturnModel, new()
        {
            var result = new ReturnModel();
            try
            {
                if (procedure != null)
                {
                    return await procedure(param);
                }
            }
            catch (MSException msex)
            {
                if (param == null)
                {
                    _logger.LogError(msex, $"{methodName} {failLog} {msex.Message}");
                }
                else
                {
                    _logger.LogError(msex, $"{methodName} {failLog} {msex.Message}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(msex.ReturnCode);
            }
            catch (Exception ex)
            {
                if (param == null)
                {
                    _logger.LogError(ex, $"{methodName} {failLog}");
                }
                else
                {
                    _logger.LogError(ex, $"{methodName} {failLog}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(ReturnCode.SystemError);
            }
            return result;
        }

        protected virtual async Task<T> TryCatchProcedure<T>(Func<Task<T>> procedure, string failLog = "fail", [CallerMemberName] string methodName = "") where T : BaseReturnModel, new()
        {
            var result = new T();
            try
            {
                if (procedure != null)
                {
                    return await procedure();
                }
            }
            catch (MSException msex)
            {
                _logger.LogError(msex, $"{methodName} {failLog} {msex.Message}");
                result.SetCode(msex.ReturnCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{methodName} {failLog}");
                result.SetCode(ReturnCode.SystemError);
            }
            return result;
        }
    }
}
