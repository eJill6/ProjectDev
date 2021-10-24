using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceNet45.Model.ViewModel.Security
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
