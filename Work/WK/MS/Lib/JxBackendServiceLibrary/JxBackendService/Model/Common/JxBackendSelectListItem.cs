using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Common
{

    public class JxBackendSelectListItem
    {
        public JxBackendSelectListItem() { }

        public JxBackendSelectListItem(string value, string text)
        {
            Value = value;
            Text = text;
        }

        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class JxBackendSelectListItem<T> : JxBackendSelectListItem
    {
        public T DataModel { get; set; }
    }

    public static class SelectListItemExtensions
    {
        public static void SetSelected(this List<JxBackendSelectListItem> selectListItems, List<string> selectedValues)
        {
            if (selectListItems == null)
            {
                return;
            }

            foreach (JxBackendSelectListItem item in selectListItems.Where(w => selectedValues.Contains(w.Value)))
            {
                item.Selected = true;
            }
        }

        public static void SetSelected(this List<JxBackendSelectListItem> selectListItems, string selectedValue)
        {
            selectListItems.SetSelected(new List<string>() { selectedValue });
        }

        public static void AddBlankOption(this List<JxBackendSelectListItem> selectListItems)
        {
            selectListItems.AddBlankOption(true);
        }

        public static List<JxBackendSelectListItem> AddBlankOption(this List<JxBackendSelectListItem> selectListItems, bool hasBlankOption)
        {
            return selectListItems.AddBlankOption(hasBlankOption, string.Empty, SelectItemElement.All);
        }

        public static List<JxBackendSelectListItem> AddBlankOption(this List<JxBackendSelectListItem> selectListItems, bool hasBlankOption, string defaultValue, string defaultDisplayText)
        {
            if (hasBlankOption)
            {
                selectListItems.Insert(0, CreateBlankOption(defaultValue, defaultDisplayText));
            }

            return selectListItems;
        }
        public static void RemoveBlankOption(this List<JxBackendSelectListItem> selectListItems)
        {
            selectListItems.RemoveBlankOption(string.Empty);
        }

        public static void RemoveBlankOption(this List<JxBackendSelectListItem> selectListItems, string defaultValue)
        {
            selectListItems.RemoveAll(r => r.Value == defaultValue);
        }


        private static JxBackendSelectListItem CreateBlankOption(string defaultValue, string defaultDispalyText)
        {
            return new JxBackendSelectListItem() { Text = defaultDispalyText, Value = defaultValue };
        }
    }
}
