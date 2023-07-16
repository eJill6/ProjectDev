using RabbitMQ.Client;

namespace JxBackendService.Model.Enums.Queue
{
    public class TaskQueueChannel
    {
        public IModel Channel { get; set; }

        public IBasicProperties BasicProperties { get; set; }
    }

    public static class RabbitMQExtensions
    {
        public static void QueueDeclare(this IModel channel, TaskQueueName taskQueueName)
        {
            channel.QueueDeclare(taskQueueName.Value, durable: true, exclusive: false, autoDelete: false, null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        public static IBasicProperties CreateBasicPropertiesForTaskQueue(this IModel channel)
        {
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.Persistent = true;

            return basicProperties;
        }
    }
}