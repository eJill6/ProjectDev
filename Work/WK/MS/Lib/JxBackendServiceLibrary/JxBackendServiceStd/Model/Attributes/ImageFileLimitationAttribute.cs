using JxBackendService.Common.Util;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JxBackendService.Model.Attributes
{
    public class ImageFileLimitationAttribute : ValidationAttribute
    {
        private readonly string[] _fileExtensionLimit = { ".jpg", ".png" };

        private readonly int _fileSizeLimit = 500 * 1024; // 500KB
        //private readonly int _fileSideLengthLimit = 440;

        public ImageFileLimitationAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) // 此attr不檢查非null，非null由 CustomizedRequiredAttribute 做檢查
            {
                return ValidationResult.Success;
            }

            IFormFile file = value as IFormFile;

            string displayName = ResolveDisplayName(validationContext);
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!_fileExtensionLimit.Contains(fileExtension))
            {
                return new ValidationResult(string.Format(
                    MessageElement.FileLimitedByExtension,
                    displayName,
                    string.Join("、", _fileExtensionLimit)));
            }

            var fileSize = file.Length;

            if (fileSize > _fileSizeLimit)
            {
                return new ValidationResult(string.Format(
                    MessageElement.FileLimitedBySize,
                    displayName,
                    _fileSizeLimit));
            }

            /*由於套件問題 長寬的驗證做到前端了*/
            //using (SKBitmap bitmap = SKBitmap.Decode(file.OpenReadStream()))
            //{
            //    if (bitmap.Width > _fileSideLengthLimit || bitmap.Height > _fileSideLengthLimit)
            //    {
            //        return new ValidationResult(string.Format(
            //            MessageElement.FileLimitedByArea,
            //            displayName,
            //            _fileSideLengthLimit,
            //            _fileSideLengthLimit));
            //    }
            //}

            return ValidationResult.Success;
        }

        private string ResolveDisplayName(ValidationContext validationContext)
        {
            string displayName = null;

            DisplayAttribute displayAttribute = validationContext.ObjectType
                .GetProperty(validationContext.MemberName)
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            if (displayAttribute != null)
            {
                displayName = GetNameByResourceInfo(displayAttribute.ResourceType, displayAttribute.Name);
            }

            return displayName ?? validationContext.MemberName;
        }

        private string GetNameByResourceInfo(Type resourceType, string resourcePropertyName)
        {
            if (resourceType == null || resourcePropertyName.IsNullOrEmpty())
            {
                return string.Empty;
            }

            PropertyInfo propertyInfo = resourceType.GetProperty(resourcePropertyName, BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo == null)
            {
                return string.Empty;
            }

            return propertyInfo.GetValue(null, null).ToNonNullString();
        }
    }
}