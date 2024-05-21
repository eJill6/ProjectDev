namespace JxBackendService.Interface.Service.Attribute
{
    public interface IActionFilterService
    {
        void ActionExecuting<T>(T actionExecutingContext);

        void ActionExecuted<T>(T actionExecutedContext);
    }
}