namespace JxBackendService.Model.Enums.Queue
{
    public class DoDequeueJobAfterReceivedParam
    {
        public string ClientProvidedName { get; set; }

        public string Message { get; set; }
    }
}