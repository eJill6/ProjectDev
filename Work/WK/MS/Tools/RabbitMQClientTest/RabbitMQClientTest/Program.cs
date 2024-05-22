// See https://aka.ms/new-console-template for more information

using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.MessageQueue;
using JxBackendServiceN6.DependencyInjection;
using RabbitMQClientTest;

var containerBuilder = new ContainerBuilder();
string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
containerBuilder = DependencyUtilN6.RegisterHttpContextAccessor(containerBuilder);
DependencyUtilN6.RegisterConfiguration(containerBuilder);
DependencyUtil.SetContainer(containerBuilder.Build());

IConfigUtilService configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
List<RabbitMQSetting> rabbitMQSettings = configUtilService.Get<List<RabbitMQSetting>>("Internal.RabbitMQConnections");

foreach (RabbitMQSetting rabbitMQSetting in rabbitMQSettings)
{
    var rabbitMQClientManager = new TestRabbitMQClientManager(
        rabbitMQSetting.HostName,
        rabbitMQSetting.Port,
        rabbitMQSetting.UserName,
        rabbitMQSetting.Password,
        rabbitMQSetting.VirtualHost);

    rabbitMQClientManager.GetConnection();
    Console.WriteLine(new { rabbitMQClientManager.ClientProvidedName, rabbitMQClientManager.IsOpen }.ToJsonString());
}

Console.ReadLine();