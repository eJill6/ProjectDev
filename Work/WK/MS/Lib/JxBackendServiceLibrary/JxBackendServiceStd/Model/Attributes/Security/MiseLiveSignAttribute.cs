namespace JxBackendService.Model.Attributes.Security
{
    public class MiseLiveSignAttribute : SignAttribute
    {
        public MiseLiveSignAttribute() : base(isCamelCase: true, 0)
        { }

        public MiseLiveSignAttribute(int sortNo) : base(isCamelCase: true, sortNo)
        {
        }
    }
}