using System;

namespace JxBackendService.Model.ViewModel.Security
{
    public class InvalidCountViewModel
    {
        /// <summary>
        /// 記錄錯誤開始的統計時間
        /// </summary>        
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 錯誤次數
        /// </summary>        
        public int Count { get; set; }
    }    
}
