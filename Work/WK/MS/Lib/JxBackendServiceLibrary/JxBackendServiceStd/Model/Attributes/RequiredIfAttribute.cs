using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Attribute;
using JxBackendService.Resource.Element;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JxBackendService.Model.Attributes
{
    public class RequiredIfAttribute : RequiredAttribute, IRequiredIfAttribute
    {
        public string OtherPropertyName { get; set; }

        public object[] OtherPropertyValidValues { get; set; }

        public RequiredIfAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext?.ObjectInstance;

            if(instance == null)
            {
                return ValidationResult.Success;
            }

            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(OtherPropertyName);
            object otherPropertyValue = propertyInfo.GetValue(instance);

            if (!OtherPropertyValidValues.Any(a => a.Equals(otherPropertyValue)))
            {
                return ValidationResult.Success; //不檢查必填
            }

            if (value == null || (value is string && (value as string).IsNullOrEmpty()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = string.Format(MessageElement.FieldIsNotAllowEmpty, name);

            return base.FormatErrorMessage(name);
        }
    }
}