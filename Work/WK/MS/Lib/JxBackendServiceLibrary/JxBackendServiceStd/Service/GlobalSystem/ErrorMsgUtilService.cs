using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace JxBackendService.Service.GlobalSystem
{
    public class ErrorMsgUtilService : IErrorMsgUtilService
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        private static readonly object s_lock = new object();

        private static IdGenerator s_idGenerator;

        private static readonly TokenBucketRateLimiter s_sendTelegramRateLimiter = new TokenBucketRateLimiter(
            new TokenBucketRateLimiterOptions()
            {
                QueueLimit = 1,
                ReplenishmentPeriod = TimeSpan.FromSeconds(3),
                TokensPerPeriod = 1,
                TokenLimit = 3,
                AutoReplenishment = false
            });

        private static readonly TokenBucketRateLimiter s_enqueueRateLimiter = new TokenBucketRateLimiter(
            new TokenBucketRateLimiterOptions()
            {
                QueueLimit = 1,
                ReplenishmentPeriod = TimeSpan.FromSeconds(1),
                TokensPerPeriod = 10,
                TokenLimit = 20,
                AutoReplenishment = false
            });

        private static readonly ConcurrentQueue<TelegramQueueMessage> s_TelegramQueueMessageQueue = new ConcurrentQueue<TelegramQueueMessage>();

        private static readonly int s_maxQueueMessageCount = 100;

        static ErrorMsgUtilService()
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        s_sendTelegramRateLimiter.TryReplenish();
                        RateLimitLease limit = s_sendTelegramRateLimiter.AttemptAcquire();

                        if (!limit.IsAcquired || !s_TelegramQueueMessageQueue.TryDequeue(out TelegramQueueMessage telegramQueueMessage))
                        {
                            TaskUtil.DelayAndWait(1000);

                            continue;
                        }

                        var baseSendTelegramParam = new BaseSendTelegramParam()
                        {
                            ApiUrl = SharedAppSettings.TelegramApiUrl,
                            Message = telegramQueueMessage.Message
                        };

                        TelegramChatGroup telegramChatGroup = TelegramChatGroup.GetSingle(telegramQueueMessage.telegramChatGroupValue);
                        TelegramUtil.SendMessage(baseSendTelegramParam, telegramChatGroup);

                        if (!s_TelegramQueueMessageQueue.Any())
                        {
                            TaskUtil.DelayAndWait(5000);
                        }
                    }
                    catch (Exception ex)
                    {
                        logUtilService.Value.Error(ex);
                    }
                }
            });
        }

        public ErrorMsgUtilService()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public IdGenerator CreateErrorIdGenerator(Func<int> getGeneratorIdJob)
        {
            lock (s_lock)
            {
                if (s_idGenerator == null)
                {
                    int generatorId = getGeneratorIdJob.Invoke();

                    s_idGenerator = new IdGenerator(generatorId);
                }

                return s_idGenerator;
            }
        }

        public void ErrorHandle(Exception exception, EnvironmentUser environmentUser, SendErrorMsgTypes sendErrorMsgType)
            => ErrorHandle(exception, environmentUser, sendErrorMsgType, errorId: null);

        public void ErrorHandle(Exception exception, EnvironmentUser environmentUser, SendErrorMsgTypes sendErrorMsgType, long? errorId)
        {
            switch (sendErrorMsgType)
            {
                case SendErrorMsgTypes.Direct:

                    exception.ErrorHandle(environmentUser, isSendMessageToTelegram: true, errorId);

                    break;

                case SendErrorMsgTypes.Queue:

                    exception.ErrorHandle(
                        environmentUser,
                        () =>
                        {
                            s_enqueueRateLimiter.TryReplenish();
                            RateLimitLease limit = s_enqueueRateLimiter.AttemptAcquire();

                            if (!limit.IsAcquired || s_TelegramQueueMessageQueue.Count >= s_maxQueueMessageCount)
                            {
                                return;
                            }

                            var allErrorMsg = new StringBuilder();

                            if (errorId.HasValue)
                            {
                                allErrorMsg.Append($"ErrorID = {errorId}, ");
                            }

                            allErrorMsg.Append(exception.ToString());

                            TelegramQueueMessage telegramQueueMessage = TelegramUtil.ConvertToQueueMessage(new SendTelegramParam()
                            {
                                ApiUrl = SharedAppSettings.TelegramApiUrl,
                                EnvironmentUser = environmentUser,
                                Message = allErrorMsg.ToString()
                            });

                            try
                            {
                                s_TelegramQueueMessageQueue.Enqueue(telegramQueueMessage);
                            }
                            catch (Exception ex)
                            {
                                _logUtilService.Value.Error(ex);
                            }
                        },
                        errorId);

                    break;
            }
        }
    }
}