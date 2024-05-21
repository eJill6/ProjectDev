using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MMModel.Models;
using MS.Core.Models;
using MS.Core.Utils;
using System.Runtime.CompilerServices;

namespace MMService.Controllers
{
    /// <summary>
    /// api base <see cref="Controller"/>
    /// </summary>
    [ApiController]
    [Route("[controller]/[Action]")]
    [Produces("application/json")]
    [Authorize]
    public abstract class ApiControllerBase : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger">Logger</param>
        public ApiControllerBase(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Api Success
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IActionResult ApiSuccessResult(object data)
        {
            return ApiResult(true, data, string.Empty);
        }

        /// <summary>
        /// Api Success
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IActionResult ApiResult<T>(MS.Core.Models.BaseReturnDataModel<T> data)
        {
            return ApiResult(data.IsSuccess, data.DataModel, data.Message, data.Code);
        }

        /// <summary>
        /// Api Success
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IActionResult ApiResult(MS.Core.Models.BaseReturnModel data)
        {
            var response = new ApiResponse()
            {
                IsSuccess = data.IsSuccess,
                Message = data.Message,
                Code = data.Code,
            };

            return Ok(response);
        }

        /// <summary>
        /// Api Fail
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected IActionResult ApiFailureResult(string errorMessage)
        {
            return ApiResult(false, new object(), errorMessage);
        }

        /// <summary>
        /// Result base
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private IActionResult ApiResult(bool isSuccess, object data, string message, string code = "")
        {
            var response = new ApiResponse<object>()
            {
                IsSuccess = isSuccess,
                Data = data,
                Message = message,
                Code = code
            };

            return Ok(response);
        }


        /// <summary>
        /// 用TryCatch包起來的程序
        /// </summary>
        /// <typeparam name="T">輸入參數</typeparam>
        /// <typeparam name="T1">返還參數</typeparam>
        /// <param name="procedure">要執行的程序</param>
        /// <param name="param">輸入的參數</param>
        /// <param name="failLog">失敗時需要寫入的訊息</param>
        /// <param name="methodName">方法名稱</param>
        /// <returns>非同步的結果</returns>
        protected async Task<T1> TryCatchProcedure<T, T1>(Func<T, Task<T1>> procedure,
            T param,
            string failLog = "fail",
            [CallerMemberName] string methodName = "") where T1 : MS.Core.Models.BaseReturnModel, new()
        {
            var result = new T1();
            try
            {
                if (procedure != null)
                {
                    return await procedure(param);
                }
            }
            catch (MMException ex)
            {
                _logger.LogError(ex, $"{methodName} {failLog}");
                result.SetCode(ex.ReturnCode);
                result.Message = ex.Message ?? string.Empty;
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
            }

            result.SetCode(MS.Core.Models.ReturnCode.SystemError);
            return result;
        }

        /// <summary>
        /// 用TryCatch包起來的程序
        /// </summary>
        /// <typeparam name="T1">返還參數</typeparam>
        /// <param name="procedure">要執行的程序</param>
        /// <param name="failLog">失敗時需要寫入的訊息</param>
        /// <param name="methodName">方法名稱</param>
        /// <returns>非同步的結果</returns>
        protected async Task<T1> TryCatchProcedure<T1>(Func<Task<T1>> procedure,
            string failLog = "fail",
            [CallerMemberName] string methodName = "") where T1 : MS.Core.Models.BaseReturnModel, new()
        {
            var result = new T1();
            try
            {
                if (procedure != null)
                {
                    return await procedure();
                }
            }
            catch (MMException ex)
            {
                _logger.LogError(ex, $"{methodName} {failLog}");
                result.SetCode(ex.ReturnCode);
                result.Message = ex.Message ?? string.Empty;
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