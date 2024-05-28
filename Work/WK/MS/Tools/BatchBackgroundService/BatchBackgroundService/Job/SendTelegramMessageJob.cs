using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.MessageQueue;
using System.Collections.Concurrent;
using System.Threading.RateLimiting;

namespace BatchService.Job
{
    public class SendTelegramMessageJob : BaseTaskJob, ITaskJob
    {
        private readonly int _workerCount = 3;

        private static readonly TokenBucketRateLimiterOptions s_sendTelegramMsgRateLimiterOption = new TokenBucketRateLimiterOptions()
        {
            QueueLimit = 1,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1),
            TokensPerPeriod = 18,
            TokenLimit = 18,
            AutoReplenishment = false
        };

        private static readonly ConcurrentDictionary<string, TokenBucketRateLimiter> s_tokenBucketRateLimiterMap = new ConcurrentDictionary<string, TokenBucketRateLimiter>();

        public SendTelegramMessageJob()
        {
        }

        protected override void DoWork(CancellationToken cancellationToken)
        {
            IMessageQueueService messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.BatchService).Value;

            for (int i = 1; i <= _workerCount; i++)
            {
                messageQueueService.StartNewDequeueJob(TaskQueueName.TelegramMessage, DoJobAfterReceived);
            }
        }

        private bool DoJobAfterReceived(string queueMessage)
        {
            TelegramQueueMessage telegramQueueMessage;

            try
            {
                telegramQueueMessage = queueMessage.Deserialize<TelegramQueueMessage>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"TelegramMessage DoJobAfterReceived:{queueMessage}");
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);

                return true;
            }

            int delaySeconds = 0;

            if (!s_tokenBucketRateLimiterMap.TryGetValue(telegramQueueMessage.telegramChatGroupValue, out TokenBucketRateLimiter s_sendTelegramMsgRateLimiter))
            {
                s_sendTelegramMsgRateLimiter = new TokenBucketRateLimiter(s_sendTelegramMsgRateLimiterOption);
                s_tokenBucketRateLimiterMap.TryAdd(telegramQueueMessage.telegramChatGroupValue, s_sendTelegramMsgRateLimiter);
            }

            while (true)
            {
                s_sendTelegramMsgRateLimiter.TryReplenish();
                RateLimitLease limit = s_sendTelegramMsgRateLimiter.AttemptAcquire();

                if (limit.IsAcquired)
                {
                    break;
                }

                delaySeconds++;
                Task.Delay(1000).Wait();
            }

            string message = null;

            if (delaySeconds > 0)
            {
                message = $"delay {delaySeconds}s ";
            }

            message += telegramQueueMessage.Message;

            TelegramUtil.SendMessage(
                new BaseSendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    Message = message,
                },
                TelegramChatGroup.GetSingle(telegramQueueMessage.telegramChatGroupValue));

            return true;
        }
    }
}