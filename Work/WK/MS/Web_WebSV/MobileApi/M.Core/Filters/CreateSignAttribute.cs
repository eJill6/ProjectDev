using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interfaces.Model.Mobile;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using M.Core.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Org.BouncyCastle.Math;
using System.Net;

namespace M.Core.Filters
{
    /// <summary> 產生簽名方便測試 </summary>
    public class CreateSignAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            EnvironmentCode environmentCode = DependencyUtil.ResolveService<IAppSettingService>().Value.GetEnvironmentCode();

            if (!environmentCode.IsTestingEnvironment)
            {
                throw new PlatformNotSupportedException();
            }

            if (!actionExecutingContext.ModelState.IsValid)
            {
                return;
            }

            KeyValuePair<string, object> keyValuePair = actionExecutingContext.ActionArguments.SingleOrDefault(w => w.Value is IMobileApiSignParam);
            IMobileApiSignParam? param = keyValuePair.Value as IMobileApiSignParam;

            var publicKeyService = DependencyUtil.ResolveService<IPublicKeyService>();
            BaseReturnDataModel<BigInteger> roomKeyResult = publicKeyService.Value.GetRoomKey(param.Coordinate);
            param.Key = roomKeyResult.DataModel.ToNonNullString();

            string sign = ValidSignUtil.CreateSign(param);
            param.Sign = sign;

            actionExecutingContext.Result = new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = param.ToJsonString(isCamelCaseNaming: true),
                ContentType = HttpWebRequestContentType.Json.Value
            };
        }
    }
}