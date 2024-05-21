namespace JxBackendService.Model.BackSideWeb
{
    public class RecordCompareParam
    {
        public string Title { get; set; }

        public string OriginValue { get; set; }

        public string NewValue { get; set; }

        public bool IsVisibleCompareValue { get; set; } = true;

        /// <summary>
        /// 是否是日志标题
        /// </summary>
        public bool IsLogTitleValue { get; set; } = false;
    }
}