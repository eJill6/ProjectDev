using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System.Linq;

namespace JxBackendService.Model.Attributes
{
    public class RequiredIfAttribute : RequiredAttribute
    {
        public string OtherPropertyName { get; set; }

        public object[] OtherPropertyValidValues { get; set; }

        public RequiredIfAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(OtherPropertyName);
            object otherPropertyValue = propertyInfo.GetValue(instance);

            if (!OtherPropertyValidValues.Any(a => a.Equals(otherPropertyValue)))
            {
                return ValidationResult.Success; //不檢查必填
            }

            if (value == null || (value is string && value.ToNonNullString().IsNullOrEmpty()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}