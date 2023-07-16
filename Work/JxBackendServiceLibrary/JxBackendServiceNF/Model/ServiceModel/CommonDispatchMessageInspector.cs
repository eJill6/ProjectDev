using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace JxBackendServiceNF.Model.ServiceModel
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

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        public abstract object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext);
    }
}