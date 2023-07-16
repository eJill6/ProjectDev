using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Attributes
{
    public class CustomizedRequiredAttribute : RequiredAttribute
    {
        public bool IsErrorMessageContainField { get; set; } = true;

        public CustomizedRequiredAttribute()
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (!IsErrorMessageContainField)
            {
                ErrorMessage = MessageElement.IsNotAllowEmpty;
            }
            else
            {
                ErrorMessage = string.Format(MessageElement.FieldIsNotAllowEmpty, name);
            }

            return base.FormatErrorMessage(name);
        }
    }
}