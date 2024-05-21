namespace MS.Core.Models
{
    public class Pro_ErrorLogs : BaseDBModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ErrorLogID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ErrorTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrorNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ErrorSeverity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ErrorState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorProcedure { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ErrorLine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
