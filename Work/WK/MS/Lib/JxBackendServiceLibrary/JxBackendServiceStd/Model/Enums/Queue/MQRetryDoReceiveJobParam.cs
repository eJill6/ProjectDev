using System;

namespace JxBackendService.Model.Enums.Queue
{
    public class MQRetryDoReceiveJobParam
    {
        public TaskQueueName TaskQueueName { get; set; }

        public int RetryCount { get; set; }

        public Func<DoDequeueJobAfterReceivedParam, bool> DoJobAfterReceived { get; set; }

        public DoDequeueJobAfterReceivedParam DoDequeueJobAfterReceivedParam { get; set; }
    }
}