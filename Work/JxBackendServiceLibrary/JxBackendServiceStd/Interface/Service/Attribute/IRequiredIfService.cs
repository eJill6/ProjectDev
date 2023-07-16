namespace JxBackendService.Interface.Service.Attribute
{
    public interface IRequiredIfService
    {
        bool IsValid(object value, object instance, string otherPropertyName, object[] otherPropertyValidValues);
    }
}