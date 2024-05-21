namespace JxBackendService.Model.Attributes.Security
{
    public class MobileApiSignAttribute : SignAttribute
    {
        public MobileApiSignAttribute() : base(isCamelCase: true, 0)
        { }

        public MobileApiSignAttribute(int sortNo) : base(isCamelCase: true, sortNo)
        {
        }
    }
}