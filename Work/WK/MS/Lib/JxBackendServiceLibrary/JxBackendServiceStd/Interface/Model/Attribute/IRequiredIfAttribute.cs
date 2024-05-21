namespace JxBackendService.Interface.Model.Attribute
{
    public interface IRequiredIfAttribute
    {
        string OtherPropertyName { get; set; }

        object[] OtherPropertyValidValues { get; set; }
    }
}