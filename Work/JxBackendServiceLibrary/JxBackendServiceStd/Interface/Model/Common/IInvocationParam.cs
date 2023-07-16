namespace JxBackendService.Interface.Model.Common
{
    public interface IInvocationParam
    {
        string CorrelationId { get; set; }
    }

    public interface IInvocationUserParam : IInvocationParam
    {
        int UserID { get; set; }
    }
}