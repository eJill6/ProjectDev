using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Attributes
{
    public class CustomizedRangeAttribute : RangeAttribute
    {
        private readonly double _minimum;
        private readonly double _maximum;

        public CustomizedRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = string.Format(ErrorMessageTemplate, name, _minimum, _maximum);

            return base.FormatErrorMessage(name);
        }

        protected virtual string ErrorMessageTemplate => MessageElement.FieldOutOfRange;
    }

    public class CustomizedPositiveIntegerRangeAttribute : CustomizedRangeAttribute
    {
        public CustomizedPositiveIntegerRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
        }

        protected override string ErrorMessageTemplate => MessageElement.PositiveIntegerFieldOutOfRange;
    }
}