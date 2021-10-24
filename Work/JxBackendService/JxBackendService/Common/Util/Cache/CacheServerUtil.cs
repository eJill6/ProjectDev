using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace JxBackendService.Common.Util.Cache
{
    //public static class CacheServerUtil
    //{
    //    private static readonly int _defaultCacheSeconds = 60 * 60;
    //    private static readonly Lazy<CacheServiceClient> _cacheServiceClient = new Lazy<CacheServiceClient>(() =>
    //    {
    //        string cacheAddress = SharedAppSettings.CacheAddress;

    //        if (cacheAddress.IsNullOrEmpty())
    //        {
    //            return null;

    //        }

    //        string fullCacheServiceUrl = $"http://{cacheAddress}/Service";
    //        System.Net.ServicePointManager.Expect100Continue = false;

    //        EndpointAddress address = new EndpointAddress(fullCacheServiceUrl);
    //        BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
    //        binding.MaxBufferSize = int.MaxValue;
    //        binding.MaxReceivedMessageSize = int.MaxValue;
    //        XmlDictionaryReaderQuotas quo = new XmlDictionaryReaderQuotas();
    //        binding.ReaderQuotas = quo;
    //        binding.ReaderQuotas.MaxArrayLength = 655360;
    //        binding.ReaderQuotas.MaxBytesPerRead = 10 * 1024 * 1024;
    //        binding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024;
    //        binding.OpenTimeout = TimeSpan.FromSeconds(3);
    //        binding.SendTimeout = TimeSpan.FromSeconds(5);
    //        binding.ReceiveTimeout = TimeSpan.FromSeconds(5);
    //        binding.CloseTimeout = TimeSpan.FromSeconds(3);
    //        binding.ReceiveTimeout = TimeSpan.MaxValue;
    //        binding.TransferMode = TransferMode.Streamed;
    //        var client = new CacheServiceClient(binding, address);
    //        EndpointBehavior endpointBehavior = new EndpointBehavior();
    //        client.Endpoint.Behaviors.Add(endpointBehavior);
    //        return client;
    //    });

    //    public static T GetCache<T>(string key, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration,
    //        bool isDeserializeByCacheServerDataModel, Func<T> getCacheData) where T : class
    //    {
    //        T cacheObject = default(T);
    //        string cacheJsonString = null;

    //        //強制更新就不用去遠端取, 直接拿最新的資料回寫
    //        if (!isForceRefresh)
    //        {
    //            cacheJsonString = _cacheServiceClient.Value.Get(key);
    //        }

    //        CacheServerDataModel<T> cacheServerDataModel = null;

    //        if (!cacheJsonString.IsNullOrEmpty())
    //        {
    //            //AMD遠程快取的slide功能沒辦法關閉, 只要呼叫get就會自動延長, 會導致資料沒辦法定期更新
    //            //故要先轉為物件判斷是否過期

    //            if (isDeserializeByCacheServerDataModel)
    //            {
    //                cacheServerDataModel = cacheJsonString.Deserialize<CacheServerDataModel<T>>();
    //            }
    //            else
    //            {
    //                cacheObject = cacheJsonString as T;

    //                if (cacheObject != null)
    //                {
    //                    cacheServerDataModel = new CacheServerDataModel<T>(cacheObject, cacheSeconds, isSlidingExpiration);
    //                }
    //            }

    //            if (cacheServerDataModel != null && cacheServerDataModel.ExpiredTime < DateTime.Now)
    //            {
    //                cacheJsonString = null;
    //                cacheServerDataModel = null;
    //                RemoveCache(key);                    
    //            }
    //        }

    //        if (cacheServerDataModel == null)
    //        {
    //            if (getCacheData != null)
    //            {
    //                cacheObject = getCacheData.Invoke();
    //            }

    //            if (cacheObject != null)
    //            {
    //                SetCache(key, cacheObject, cacheSeconds, isSlidingExpiration, isDeserializeByCacheServerDataModel);
    //            }
    //        }
    //        else
    //        {
    //            //延長            
    //            if (cacheServerDataModel.IsSlidingExpiration)
    //            {
    //                SetCache(key, cacheServerDataModel.Data, cacheServerDataModel.CacheSeconds, cacheServerDataModel.IsSlidingExpiration, isDeserializeByCacheServerDataModel);
    //            }

    //            cacheObject = cacheServerDataModel.Data;
    //        }

    //        return cacheObject;
    //    }

    //    public static void SetCache<TItem>(string key, TItem value, int cacheSeconds, bool isSlidingExpiration, bool isSerializeByCacheServerDataModel)
    //    {
    //        if (cacheSeconds == 0)
    //        {
    //            cacheSeconds = _defaultCacheSeconds;
    //        }

    //        string data;

    //        if (isSerializeByCacheServerDataModel)
    //        {
    //            data = new CacheServerDataModel<TItem>(value, cacheSeconds, isSlidingExpiration).ToJsonString(ignoreNull: true);
    //        }
    //        else
    //        {
    //            data = value.ToString();
    //        }

    //        _cacheServiceClient.Value.Save1(key, data, cacheSeconds, true);
    //    }

    //    public static void RemoveCache(string key)
    //    {
    //        //舊架構的cache server del有時候會失敗或延遲,有時候還會把資料複寫回去, 目前測試Save2是最穩定可以更新快取資料的方式
    //        _cacheServiceClient.Value.Save2(key, string.Empty);             
    //    }
    //}
}
