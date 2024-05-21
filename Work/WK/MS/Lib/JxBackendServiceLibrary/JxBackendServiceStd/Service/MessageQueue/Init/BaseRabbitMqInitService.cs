using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JxBackendService.Service.MessageQueue.Init
{
    public abstract class BaseRabbitMqInitService : IBaseRabbitMqInitService
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
        
        private static readonly Lazy<ILogUtilService> s_logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

        private readonly Lazy<IAppSettingService> _appSettingService;

        protected abstract List<RabbitMQSetting> GetRabbitMQSettings();

        protected abstract void InitExchanges(RabbitMQClientManager rabbitMQClientManager);

        protected abstract void InitQueues(RabbitMQClientManager rabbitMQClientManager);

        protected IAppSettingService AppSettingService => _appSettingService.Value;

        protected IEnvironmentService EnvironmentService => s_environmentService.Value;

        protected BaseRabbitMqInitService()
        {
            _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(
                s_environmentService.Value.Application,
                SharedAppSettings.PlatformMerchant);
        }

        public ConcurrentDictionary<string, RabbitMQClientManager> Init()
        {
            List<RabbitMQSetting> rabbitMQSettings = GetRabbitMQSettings();
            var rabbitMQClientManagerMap = new ConcurrentDictionary<string, RabbitMQClientManager>();

            foreach (RabbitMQSetting rabbitMQSetting in rabbitMQSettings)
            {                
                var rabbitMQClientManager = new RabbitMQClientManager(
                    rabbitMQSetting.HostName,
                    rabbitMQSetting.Port,
                    rabbitMQSetting.UserName,
                    rabbitMQSetting.Password,
                    rabbitMQSetting.VirtualHost);

                rabbitMQClientManagerMap.TryAdd(rabbitMQClientManager.ClientProvidedName, rabbitMQClientManager);

                try
                {
                    InitExchanges(rabbitMQClientManager);
                    InitQueues(rabbitMQClientManager);
                }
                catch(Exception ex)
                {
                    s_logUtilService.Value.Error(ex);
                }
            }

            return rabbitMQClientManagerMap;
        }

        public static void QueueDeclare(IModel channel, TaskQueueName taskQueueName)
        {
            channel.QueueDeclare(taskQueueName);

            if (taskQueueName.BindExchangeInfo != null)
            {
                channel.QueueBind(taskQueueName.Value, taskQueueName.BindExchangeInfo.Exchange, taskQueueName.BindExchangeInfo.RoutingKey);
            }
        }
    }
}