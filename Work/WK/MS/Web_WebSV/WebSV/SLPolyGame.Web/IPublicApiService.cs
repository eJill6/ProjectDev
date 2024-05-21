using SLPolyGame.Web.Model;
using System.ServiceModel;

namespace SLPolyGame.Web
{
    [ServiceContract]
    public interface IPublicApiService
    {
        [OperationContract]
        string GetRemoteCacheForOldService(string key);

        [OperationContract]
        void RemoveRemoteCacheForOldService(string key);

        [OperationContract]
        void SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam);

        [OperationContract]
        bool IsAllowExecutingActionWithLock(string key, double cacheSeconds);

        [OperationContract]
        bool IsAllowExecutingAction(string key, double cacheSeconds);
    }
}