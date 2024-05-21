using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Attribute;
using JxBackendService.Interface.Service.Attribute;
using System.ComponentModel.DataAnnotations;

namespace JxBackendServiceNF.Attributes
{
    public class RequiredIfNFAttribute : RequiredAttribute, IRequiredIfAttribute
    {
        private readonly IRequiredIfService _requiredIfService;

        public string OtherPropertyName { get; set; }

        public object[] OtherPropertyValidValues { get; set; }

        public RequiredIfNFAttribute()
        {
            _requiredIfService = DependencyUtil.ResolveService<IRequiredIfService>();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isValid = _requiredIfService.IsValid(value, validationContext.ObjectInstance, OtherPropertyName, OtherPropertyValidValues);

            if (!isValid)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}