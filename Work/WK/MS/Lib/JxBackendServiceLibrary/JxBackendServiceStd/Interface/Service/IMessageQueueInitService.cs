using JxBackendService.Service.MessageQueue;
using System.Collections.Concurrent;

namespace JxBackendService.Interface.Service
{
    public interface IBaseRabbitMqInitService
    {
        ConcurrentDictionary<string, RabbitMQClientManager> Init();
    }

    public interface IEndUserRabbitMqInitService : IBaseRabbitMqInitService
    {
    }

    public interface IInternalRabbitMqInitService : IBaseRabbitMqInitService
    { }
}