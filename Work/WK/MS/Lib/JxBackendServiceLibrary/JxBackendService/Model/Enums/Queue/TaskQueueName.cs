namespace JxBackendService.Model.Enums.Queue
{
    public class TaskQueueName : BaseStringValueModel<TaskQueueName>
    {
        private TaskQueueName()
        { }

        public static readonly TaskQueueName TransferToMiseLive = new TaskQueueName() { Value = "TransferToMiseLive" };

        public static TaskQueueName TransferAllOut(PlatformProduct platformProduct)
            => new TaskQueueName() { Value = $"TransferAllOut:{platformProduct.Value}" };
    }
}