namespace JxBackendService.Interface.Model.MessageQueue
{
    public interface IRoutingSetting
    {
        string RequestId { get; set; }

        string RoutingKey { get; set; }
    }
}