﻿using Microsoft.Extensions.Logging;
using MS.Core.Infrastructures.Exceptions;
using MS.Core.Models;
using MS.Core.Services;
using MS.Core.Utils;
using System.Runtime.CompilerServices;

namespace MS.Core.MM.Services.Bases
{
    public class MMBaseService : BaseService
    {
        public MMBaseService(ILogger logger) : base(logger)
        {
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
            catch (MSException mmex)
            {
                if (param == null)
                {
                    _logger.LogError(mmex, $"{methodName} {failLog} {mmex.Message}");
                }
                else
                {
                    _logger.LogError(mmex, $"{methodName} {failLog} {mmex.Message}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(mmex.ReturnCode);
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
            catch (MSException mmex)
            {
                _logger.LogError(mmex, $"{methodName} {failLog} {mmex.Message}");
                result.SetCode(mmex.ReturnCode);
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