namespace JxBackendService.Interface.Service.Attribute
{
    public interface ILogActionExecutingTimeService : IActionFilterService
    {
        void Init(double? warningMilliseconds);
    }
}