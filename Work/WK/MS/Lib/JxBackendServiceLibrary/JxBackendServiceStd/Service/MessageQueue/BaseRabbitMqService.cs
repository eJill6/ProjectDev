using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.MessageQueue.Init;
using JxMsgEntities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JxBackendService.Service.MessageQueue
{
    [SingleInstance]
    public abstract class BaseRabbitMqService : IBaseMessageQueueService
    {
        private static readonly int s_retryDequeueWaitSeconds = 5;

        private static readonly Lazy<ILogUtilService> s_logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        private int _executeRabbitmqManagerIndex = 0;

        private readonly Lazy<IMQRetryDoReceiveDelayJobService> _mqRetryDoReceiveDelayJobService;

        private readonly List<RabbitMQClientManager> _rabbitMQClientManagers;

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        protected static IEnvironmentService EnvironmentService => s_environmentService.Value;

        protected static ILogUtilService LogUtilService => s_logUtilService.Value;

        protected BaseRabbitMqService()
        {
            _mqRetryDoReceiveDelayJobService = DependencyUtil.ResolveJxBackendService<IMQRetryDoReceiveDelayJobService>(
               CreateEnvUser(),
               DbConnectionTypes.Slave);

            _rabbitMQClientManagers = GetRabbitMQClientManagerMap().Values.ToList();
        }

        protected RabbitMQClientManager GetRabbitMQClientManagerInOrder()
        {
            return _semaphoreSlim.DoJob(() =>
            {
                RabbitMQClientManager executeRabbitmqManager = _rabbitMQClientManagers[_executeRabbitmqManagerIndex];
                _executeRabbitmqManagerIndex++;
                _executeRabbitmqManagerIndex %= _rabbitMQClientManagers.Count;

                return executeRabbitmqManager;
            });
        }

        protected abstract ConcurrentDictionary<string, RabbitMQClientManager> GetRabbitMQClientManagerMap();

        protected BaseReturnModel Enqueue<T>(TaskQueueName taskQueueName, T model, decimal? taskExpirationMillisecond = null)
        {
            var environmentUser = CreateEnvUser();
            SendResult sendResult = null;

            for (int i = 1; i <= _rabbitMQClientManagers.Count; i++)
            {
                sendResult = ErrorMsgUtil.DoWorkWithErrorHandle(
                    environmentUser,
                    () =>
                    {
                        RabbitMQClientManager rabbitMQClientManager = GetRabbitMQClientManagerInOrder();

                        if (!rabbitMQClientManager.IsOpen)
                        {
                            return null;
                        }

                        return rabbitMQClientManager.Enqueue(taskQueueName.Value, model, taskExpirationMillisecond);
                    });

                if (sendResult != null && sendResult.IsSuccess)
                {
                    break;
                }
            }

            return ConvertToBaseReturnModel(sendResult);
        }

        private BaseReturnModel ConvertToBaseReturnModel(SendResult sendResult)
        {
            if (sendResult == null)
            {
                return new BaseReturnModel("No Send Result");
            }

            if (sendResult.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel(sendResult.Message);
        }

        public ICollection<string> GetClientProvidedNames()
        {
            return GetRabbitMQClientManagerMap().Keys;
        }

        public void StartNewDequeueJob(TaskQueueName taskQueueName, Func<DoDequeueJobAfterReceivedParam, bool> doJobAfterReceived)
            => StartNewDequeueJob(queryClientProvidedName: null, taskQueueName, doJobAfterReceived);

        public void StartNewDequeueJob(string queryClientProvidedName, TaskQueueName taskQueueName, Func<DoDequeueJobAfterReceivedParam, bool> doJobAfterReceived)
        {
            ConcurrentDictionary<string, RabbitMQClientManager> rabbitMQClientManagerMap = GetRabbitMQClientManagerMap();
            List<string> clientProvidedNames = rabbitMQClientManagerMap.Keys.ToList();

            if (!queryClientProvidedName.IsNullOrEmpty())
            {
                clientProvidedNames = clientProvidedNames.Where(w => w == queryClientProvidedName).ToList();
            }

            foreach (string clientProvidedName in clientProvidedNames)
            {
                if (!rabbitMQClientManagerMap.TryGetValue(clientProvidedName, out RabbitMQClientManager rabbitMQClientManager))
                {
                    continue;
                }

                IConnection connection = rabbitMQClientManager.GetConnection();
                IModel channel = connection.CreateModel();
                EventingBasicConsumer eventingBasicConsumer = InitEventingBasicConsumer(clientProvidedName, taskQueueName, doJobAfterReceived, channel);

                connection.ConnectionShutdown += (object sender, ShutdownEventArgs e) =>
                {
                    IConnection conn = sender as IConnection;
                    ConnectionShutdown(conn.ClientProvidedName, e, eventingBasicConsumer, taskQueueName, doJobAfterReceived);
                };
            }
        }

        protected static void QueueDeclare(IModel channel, TaskQueueName taskQueueName)
        {
            BaseRabbitMqInitService.QueueDeclare(channel, taskQueueName);
        }

        private EventingBasicConsumer InitEventingBasicConsumer(
            string clientProvidedName,
            TaskQueueName taskQueueName,
            Func<DoDequeueJobAfterReceivedParam, bool> doJobAfterReceived,
            IModel channel)
        {
            if (taskQueueName.IsAutoDelete)
            {
                QueueDeclare(channel, taskQueueName);
            }

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: true);
            var eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received += (object sender, BasicDeliverEventArgs basicDeliverEventArgs) =>
            {
                var mqRetryDoReceiveJobParam = new MQRetryDoReceiveJobParam()
                {
                    TaskQueueName = taskQueueName,
                    DoJobAfterReceived = doJobAfterReceived,
                    DoDequeueJobAfterReceivedParam = new DoDequeueJobAfterReceivedParam()
                    {
                        ClientProvidedName = clientProvidedName                        
                    }
                };

                try
                {
                    ReadOnlyMemory<byte> body = basicDeliverEventArgs.Body;
                    mqRetryDoReceiveJobParam.DoDequeueJobAfterReceivedParam.Message = Encoding.UTF8.GetString(body.ToArray());

                    bool isSuccess = doJobAfterReceived.Invoke(mqRetryDoReceiveJobParam.DoDequeueJobAfterReceivedParam);

                    if (!isSuccess)
                    {
                        _mqRetryDoReceiveDelayJobService.Value.AddDelayJobParam(mqRetryDoReceiveJobParam);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgUtil.ErrorHandle(ex, CreateEnvUser());
                    string message = mqRetryDoReceiveJobParam.DoDequeueJobAfterReceivedParam.Message;

                    if (!message.IsNullOrEmpty())
                    {
                        s_logUtilService.Value.Error($"doJobAfterReceived({taskQueueName.Value}) Error by message = {message}");
                        _mqRetryDoReceiveDelayJobService.Value.AddDelayJobParam(mqRetryDoReceiveJobParam);
                    }
                }
                finally
                {
                    channel.BasicAck(basicDeliverEventArgs.DeliveryTag, multiple: false);
                }
            };

            channel.BasicConsume(taskQueueName.Value, autoAck: false, eventingBasicConsumer);

            return eventingBasicConsumer;
        }

        private void ConnectionShutdown(string clientProvidedName, ShutdownEventArgs e, EventingBasicConsumer eventingBasicConsumer, TaskQueueName taskQueueName,
            Func<DoDequeueJobAfterReceivedParam, bool> doJobAfterReceived)
        {
            s_logUtilService.Value.Error(e.ToString());
            eventingBasicConsumer = null;

            ConcurrentDictionary<string, RabbitMQClientManager> rabbitMQClientManagerMap = GetRabbitMQClientManagerMap();

            while (true)
            {
                TaskUtil.DelayAndWait(millisecondsDelay: s_retryDequeueWaitSeconds * 1000);

                try
                {
                    if (!rabbitMQClientManagerMap.TryGetValue(clientProvidedName, out RabbitMQClientManager rabbitMQClientManager))
                    {
                        continue;
                    }

                    IConnection connection = rabbitMQClientManager.GetConnection();
                    IModel channel = connection.CreateModel();
                    eventingBasicConsumer = InitEventingBasicConsumer(clientProvidedName, taskQueueName, doJobAfterReceived, channel);

                    connection.ConnectionShutdown += (object sender, ShutdownEventArgs shutdownEventArgs) =>
                    {
                        IConnection conn = sender as IConnection;
                        ConnectionShutdown(conn.ClientProvidedName, shutdownEventArgs, eventingBasicConsumer, taskQueueName, doJobAfterReceived);
                    };

                    break;
                }
                catch (Exception ex)
                {
                    s_logUtilService.Value.Error(ex.ToString());
                }
            }
        }

        protected EnvironmentUser CreateEnvUser() => new EnvironmentUser
        {
            Application = s_environmentService.Value.Application,
            LoginUser = new BasicUserInfo { }
        };
    }
}