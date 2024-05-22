using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Interfaces.Model.Mobile;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using M.Core.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Org.BouncyCastle.Math;
using System.Net;
using System.Text;

namespace M.Core.Filters
{
    public class ValidateSignAttribute : ActionFilterAttribute
    {
        private readonly Lazy<IPublicKeyService> _publicKeyService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public ValidateSignAttribute()
        {
            _publicKeyService = DependencyUtil.ResolveService<IPublicKeyService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            try
            {
                base.OnActionExecuting(actionExecutingContext);

                if (!actionExecutingContext.ModelState.IsValid)
                {
                    return;
                }

                if (actionExecutingContext.ActionArguments == null)
                {
                    actionExecutingContext.Result = LogAndCreateFailResponse($"ActionArguments = null", actionExecutingContext);

                    return;
                }

                KeyValuePair<string, object> keyValuePair = actionExecutingContext.ActionArguments
                    .SingleOrDefault(w => w.Value is IMobileApiSignParam);

                var param = keyValuePair.Value as IMobileApiSignParam;

                BaseReturnDataModel<BigInteger> roomKeyResult = _publicKeyService.Value.GetRoomKey(param.Coordinate);

                if (!roomKeyResult.IsSuccess)
                {
                    actionExecutingContext.Result = LogAndCreateFailResponse($"取得UserKey失敗", actionExecutingContext);

                    return;
                }

                param.Key = roomKeyResult.DataModel.ToNonNullString();
                bool isValid = ValidSignUtil.IsSignValid(param, param.Sign);

                if (!isValid)
                {
                    actionExecutingContext.Result = LogAndCreateFailResponse("驗證簽章不通過", actionExecutingContext);

                    return;
                }

                return;
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, _environmentUser);

                actionExecutingContext.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = new AppResponseModel(ex.Message).ToJsonString(isCamelCaseNaming: true),
                    ContentType = HttpWebRequestContentType.Json.Value
                };

                return;
            }
        }

        private ContentResult LogAndCreateFailResponse(string failReason, ActionExecutingContext actionExecutingContext)
        {
            string logErrorContent = $"ValidateSign 驗證簽章失敗, 失敗原因: {failReason}";

            HttpRequest request = actionExecutingContext.HttpContext.Request;
            string url = request.ToUri().ToString();
            logErrorContent += $", RequestUrl = '{url}'";

            if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                Stream inputStream = request.Body;

                using (var streamReader = new StreamReader(inputStream, Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: true,
                    bufferSize: 1024,
                    leaveOpen: true))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                    string postBody = streamReader.ReadToEnd();

                    //讀取完畢要將讀取位置還原到起始點
                    inputStream.Seek(0, SeekOrigin.Begin);

                    logErrorContent += $", Param = '{postBody.Replace("\r", string.Empty).Replace("\n", string.Empty)}'";
                }
            }

            _logUtilService.Value.Error(logErrorContent);

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Content = new AppResponseModel(ReturnCode.ValidateSignFailed).ToJsonString(isCamelCaseNaming: true),
                ContentType = HttpWebRequestContentType.Json.Value
            };
        }

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = s_environmentService.Value.Application,
            LoginUser = new BasicUserInfo()
        };
    }
}