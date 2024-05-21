using JxBackendService.Common.Util.Cache;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;

namespace JxBackendService.Interface.Service.MessageQueue
{
    public interface IBaseDelayJobService<T>
    {
        void AddDelayJobParam(T param, int delaySeconds);
    }

    public interface IMessageQueueDelayJobService : IBaseDelayJobService<MessageQueueParam>
    {
    }

    public interface ICacheDelayJobService : IBaseDelayJobService<DelaySetCacheParam>
    {
        void AddDeleteDelayJobParam(CacheKey cacheKey, int delaySeconds);
    }

    public interface IMQRetryDoReceiveDelayJobService : IBaseDelayJobService<MQRetryDoReceiveJobParam>
    {
        void AddDelayJobParam(MQRetryDoReceiveJobParam param);
    }
}