using JxMsgEntities;
using System;

namespace JxBackendService.Model.Enums.Queue
{
    public class TaskQueueName : BaseStringValueModel<TaskQueueName>
    {
        public BindExchangeInfo BindExchangeInfo { get; private set; }

        public bool IsDurable { get; private set; } = true;

        public bool IsAutoDelete { get; private set; }

        public MQBusinessTypes MQBusinessType { get; private set; } = MQBusinessTypes.Internal;

        private TaskQueueName()
        { }

        public static readonly TaskQueueName TransferToMiseLive = new TaskQueueName() { Value = "TransferToMiseLive" };

        public static readonly TaskQueueName AddChatMessage = new TaskQueueName() { Value = "MiseLive.AddChatMessage" };

        public static readonly TaskQueueName DeleteChatMessage = new TaskQueueName() { Value = "MiseLive.DeleteChatMessage" };

        public static readonly TaskQueueName SendSMS = new TaskQueueName() { Value = "MiseLive.SendSMS" };

        public static TaskQueueName TransferAllOut(PlatformProduct platformProduct)
            => new TaskQueueName() { Value = $"TransferAllOut:{platformProduct.Value}" };

        public static TaskQueueName UpdateTPGameUserScore(PlatformProduct platformProduct)
            => new TaskQueueName() { Value = $"UpdateTPGameUserScore:{platformProduct.Value}" };

        public static readonly TaskQueueName UnitTest = new TaskQueueName() { Value = "UnitTest" };

        public static readonly TaskQueueName DirectQueue = new TaskQueueName()
        {
            Value = "direct_queue",
            MQBusinessType = MQBusinessTypes.EndUser,
            BindExchangeInfo = new BindExchangeInfo()
            {
                Exchange = RQSettings.RQ_WCF_EXCHANGE,
                RoutingKey = RQSettings.RQ_WCF_ROUTKEY
            }
        };

        public static TaskQueueName RefreshLotteryFanout(JxApplication jxApplication)
        {
            return new TaskQueueName()
            {
                Value = $"RefreshLotteryFanout:{Environment.MachineName}:{jxApplication.Value}:{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                IsAutoDelete = true,
                IsDurable = false,
                MQBusinessType = MQBusinessTypes.EndUser,
                BindExchangeInfo = new BindExchangeInfo()
                {
                    Exchange = RQSettings.RQ_HECBET_REFRESHLOTTERY_FANOUT,
                    RoutingKey = string.Empty
                }
            };
        }
    }

    public enum MQBusinessTypes
    {
        Internal = 1,

        EndUser = 2,
    }

    public class BindExchangeInfo
    {
        public string Exchange { get; set; }

        public string RoutingKey { get; set; }
    }
}