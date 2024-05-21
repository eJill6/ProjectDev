using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace JxBackendService.Model.ServiceModel
{
    public abstract class CommonDispatchMessageInspector : IDispatchMessageInspector
    {
        private static readonly Lazy<IOperationContextService> _operationContextService = new Lazy<IOperationContextService>(
            () => DependencyUtil.ResolveService<IOperationContextService>());

        protected abstract Exception GetUnauthorizedException();

        protected abstract Exception CreateForbiddenException();

        protected abstract JxApplication Application { get; }

        //public static readonly List<string> AuthRequiredActions = new List<string>();

        private static readonly ConcurrentDictionary<string, object> _authRequiredActions = new ConcurrentDictionary<string, object>();

        public void AddOperation(OperationDescription operation)
        {
            _authRequiredActions.TryAdd(operation.Messages[0].Action, null);
        }

        public CommonDispatchMessageInspector(OperationDescription operation)
        {
            AddOperation(operation);
        }

        //public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        //{
        //    string action = request.Headers.Action;

        //    // 由MOperationBehavior動態生成忽略驗證的方法列表, 取代以往直接hard code方法名字的做法
        //    if (!_authRequiredActions.ContainsKey(action))
        //    {
        //        return null;
        //    }

        //    IOperationContextService operationContextService = _operationContextService.Value;
        //    string userKey = null;

        //    try
        //    {
        //        userKey = operationContextService.GetUserKey();
        //    }
        //    catch
        //    {
        //        //ignore;
        //    }

        //    if (string.IsNullOrEmpty(userKey))
        //    {
        //        string msg = $"回传消息头参数缺失（内容为空），用户被强制下线，Key={userKey}，Action={action}";
        //        LogUtil.Error(msg);
        //        throw GetUnauthorizedException();
        //    }

        //    BaseReturnDataModel<BaseUserInfoToken> returnDataModel = BaseApplicationService.GetUserByUserKey(
        //        Application,
        //        userKey,
        //        () => $"Key={userKey}，Action={action}");

        //    if (!returnDataModel.IsSuccess)
        //    {
        //        throw CreateForbiddenException();
        //    }

        //    BaseUserInfoToken userInfoToken = returnDataModel.DataModel;

        //    if (userInfoToken.UserId == 0)
        //    {
        //        LogUtil.Error($"user資訊不完整, user={userInfoToken.ToJsonString()}");
        //        throw CreateForbiddenException();
        //    }

        //    return null;
        //}

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        public abstract object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext);
    }
}