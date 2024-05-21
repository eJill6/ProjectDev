using JxBackendService.Model.ReturnModel;
using System.Text.RegularExpressions;

namespace BackSideWeb.Helpers
{
    public class MMValidate
    {
        public static bool IsValidLength(string value, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length < minLength || value.Length > maxLength)
            {
                return false;
            }
            return true;
        }
        public static bool IsNumeric(string input)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(input);
        }
        public static bool IsChineseOrEnglishOrNumber(string input)
        {
            Regex regex = new Regex(@"^[\u4E00-\u9FA5A-Za-z0-9]+$");
            return regex.IsMatch(input);
        }
        public static bool IsCorrectStartDateToEndDate(DateTime? startDate, DateTime? endDate)
        {
            return startDate < endDate;
        }
        public static bool IsImageFileValid(IFormFile file)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }
        public static bool IsExistIntRange(string input, int min, int max)
        {
            if (int.TryParse(input, out int number))
            {
                if (number >= min && number <= max)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsExistDecimalRange(string input, decimal min, decimal max)
        {
            if (decimal.TryParse(input, out decimal number))
            {
                if (number >= min && number <= max)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
