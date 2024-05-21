namespace BackSideWeb.Models
{
    public class DateRange
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class DateRangePickerSetting : DateRange
    {
        public bool ShowDateInputBox { get; set; } = true;

        public string LabelText { get; set; }

        /// <summary>
        /// 執行帶有startDate, endDate傳入值的js函式名稱
        /// </summary>
        public string Callback { get; set; }
    }
}
