using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Attributes
{
    public class CustomizedMaxLengthAttribute : MaxLengthAttribute
    {
        private readonly int _maxLength;

        public CustomizedMaxLengthAttribute(int maxLength) : base(maxLength)
        {
            _maxLength = maxLength;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = string.Format(MessageElement.FieldExceedsMaxLength, name, _maxLength);

            return base.FormatErrorMessage(name);
        }
    }
}