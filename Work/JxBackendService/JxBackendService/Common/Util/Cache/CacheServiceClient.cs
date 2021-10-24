using JxBackendService.Model.Common;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace JxBackendService.Common.Util.Cache
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CacheServiceClient : System.ServiceModel.ClientBase<ICacheService>, ICacheService
    {

        public CacheServiceClient()
        {
        }

        public CacheServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public CacheServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public CacheServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public CacheServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public bool Save1(string key, string value, double expireSeconds, bool slideExpired)
        {
            return base.Channel.Save1(key, value, expireSeconds, slideExpired);
        }

        public bool Save2(string key, string value)
        {
            return base.Channel.Save2(key, value);
        }

        public string Get(string key)
        {
            return base.Channel.Get(key);
        }

        public bool Del(string key)
        {
            return base.Channel.Del(key);
        }
    }

    public class EndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            MessageInspector inspector = new MessageInspector();
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class MessageInspector : IClientMessageInspector
    {
        public static string MyCustomHeaderContent
        {
            get;
            set;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            request.Headers.Add(MessageHeader.CreateHeader("userName", string.Empty, SharedAppSettings.CacheServiceUserName));
            request.Headers.Add(MessageHeader.CreateHeader("password", string.Empty, SharedAppSettings.CacheServiceKey));
            return null;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "CS.ICacheService")]
    public interface ICacheService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICacheService/Save1", ReplyAction = "http://tempuri.org/ICacheService/Save1Response")]
        bool Save1(string key, string value, double expireSeconds, bool slideExpired);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICacheService/Save2", ReplyAction = "http://tempuri.org/ICacheService/Save2Response")]
        bool Save2(string key, string value);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICacheService/Get", ReplyAction = "http://tempuri.org/ICacheService/GetResponse")]
        string Get(string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICacheService/Del", ReplyAction = "http://tempuri.org/ICacheService/DelResponse")]
        bool Del(string key);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICacheServiceChannel : ICacheService, System.ServiceModel.IClientChannel
    {
    }    
}
