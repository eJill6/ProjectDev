using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.DelayJob
{
    public abstract class BaseDelayJobService<T> : BaseService, IBaseDelayJobService<T> where T : class
    {
        private static readonly int s_delayQueueSize = 3600;

        private static int s_delayQueueCurrentIndex = 0;

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, object> s_typeLockMap = new ConcurrentDictionary<RuntimeTypeHandle, object>();
        
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, ConcurrentDictionary<int, List<DelayParamInfo<T>>>> s_delayQueueMap =
            new ConcurrentDictionary<RuntimeTypeHandle, ConcurrentDictionary<int, List<DelayParamInfo<T>>>>();

        public BaseDelayJobService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            GetOrInitTypeDelayQueue();
        }

        protected abstract void DoJob(T param);

        public void AddDelayJobParam(T param, int delaySeconds)
        {
            if (delaySeconds == 0)
            {
                DoJob(param);
            }
            else
            {
                int placeIndex = (s_delayQueueCurrentIndex + delaySeconds) % s_delayQueueSize;
                int delayCircleCount = Math.Floor((decimal)delaySeconds / s_delayQueueSize).ToInt32();

                ConcurrentDictionary<int, List<DelayParamInfo<T>>> delayQueue = GetOrInitTypeDelayQueue();
                delayQueue[placeIndex].Add(new DelayParamInfo<T>() { DelayCircleCount = delayCircleCount, Param = param });
            }
        }        

        private object GetTypeLock()
        {
            RuntimeTypeHandle typeHandle = GetType().TypeHandle;

            object typeLock = s_typeLockMap.GetOrAdd(
                typeHandle,
                (key) =>
                {
                    return new object();
                });

            return typeLock;
        }

        private ConcurrentDictionary<int, List<DelayParamInfo<T>>> GetOrInitTypeDelayQueue()
        {
            RuntimeTypeHandle typeHandle = GetType().TypeHandle;

            ConcurrentDictionary<int, List<DelayParamInfo<T>>> returnQueue = s_delayQueueMap.GetOrAdd(
                typeHandle,
                (key) =>
                {
                    var delayQueue = new ConcurrentDictionary<int, List<DelayParamInfo<T>>>();

                    for (int i = 0; i < s_delayQueueSize; i++)
                    {
                        delayQueue[i] = new List<DelayParamInfo<T>>();
                    }

                    Task.Run(() =>
                    {
                        while (true)
                        {
                            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
                            {
                                List<DelayParamInfo<T>> delayParamInfos = delayQueue[s_delayQueueCurrentIndex];

                                for (int i = delayParamInfos.Count - 1; i >= 0; i--)
                                {
                                    DelayParamInfo<T> delayParamInfo = delayParamInfos[i];

                                    if (delayParamInfo.DelayCircleCount > 0)
                                    {
                                        delayParamInfo.DelayCircleCount--;
                                    }
                                    else
                                    {
                                        Console.WriteLine(DateTime.Now);
                                        DoJob(delayParamInfo.Param);
                                        delayParamInfos.Remove(delayParamInfo);
                                    }
                                }

                                lock (GetTypeLock())
                                {
                                    s_delayQueueCurrentIndex++;

                                    if (s_delayQueueCurrentIndex >= s_delayQueueSize)
                                    {
                                        s_delayQueueCurrentIndex = 0;
                                    }
                                }

                                TaskUtil.DelayAndWait(1000);
                            });
                        }
                    });

                    return delayQueue;
                });

            return returnQueue;
        }


    }
}