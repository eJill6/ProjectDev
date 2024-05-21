using JxBackendService.Model.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace BackSideWeb.Helpers
{
    public static class EnumHelper
    {
        public static List<SelectListItem> GetEnumAll<TEnum>()
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enum type.");
            }

            var enumValues = Enum.GetValues(enumType);
            var selectListItems = new List<SelectListItem>();

            selectListItems.Add(new SelectListItem
            {
                Text = "全部",
                Value = null,
                Selected = true
            });

            foreach (var enumValue in enumValues)
            {
                var descriptionAttribute = enumType
               .GetMember(enumValue.ToString())
               .FirstOrDefault()
               ?.GetCustomAttribute<DescriptionAttribute>();

                string description = descriptionAttribute?.Description ?? enumValue.ToString();
                int numericValue = (int)enumValue;

                selectListItems.Add(new SelectListItem
                {
                    Text = description,
                    Value = numericValue.ToString()
                });
            }

            return selectListItems;
        }

        public static List<SelectListItem> GetEnum<TEnum>()
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enum type.");
            }

            var enumValues = Enum.GetValues(enumType);
            var selectListItems = new List<SelectListItem>();

            foreach (var enumValue in enumValues)
            {
                var descriptionAttribute = enumType
                    .GetMember(enumValue.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>();

                string description = descriptionAttribute?.Description ?? enumValue.ToString();

                selectListItems.Add(new SelectListItem
                {
                    Text = description,
                    Value = enumValue.ToString()
                });
            }
            return selectListItems;
        }

        public static List<JxBackendSelectListItem> GetEnumAllJxBackendSelectListItem<TEnum>()
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enum type.");
            }

            var enumValues = Enum.GetValues(enumType);
            var selectListItems = new List<JxBackendSelectListItem>();

            selectListItems.Add(new JxBackendSelectListItem
            {
                Text = "全部",
                Value = null,
                Selected = true
            });

            foreach (var enumValue in enumValues)
            {
                var descriptionAttribute = enumType
               .GetMember(enumValue.ToString())
               .FirstOrDefault()
               ?.GetCustomAttribute<DescriptionAttribute>();

                string description = descriptionAttribute?.Description ?? enumValue.ToString();
                int numericValue = (int)enumValue;

                selectListItems.Add(new JxBackendSelectListItem
                {
                    Text = description,
                    Value = numericValue.ToString()
                });
            }
            return selectListItems;
        }

        public static List<JxBackendSelectListItem> GetEnumJxBackendSelectListItem<TEnum>()
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enum type.");
            }

            var enumValues = Enum.GetValues(enumType);
            var selectListItems = new List<JxBackendSelectListItem>();

            foreach (var enumValue in enumValues)
            {
                var descriptionAttribute = enumType
                    .GetMember(enumValue.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>();

                string description = descriptionAttribute?.Description ?? enumValue.ToString();

                selectListItems.Add(new JxBackendSelectListItem
                {
                    Text = description,
                    Value = enumValue.ToString()
                });
            }
            return selectListItems;
        }
    }
}