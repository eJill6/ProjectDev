using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum ViewOfficialReportStatus
    {
        /// <summary>
        /// 可以回報
        /// </summary>
        CanReport,

        /// <summary>
        /// 未預約過
        /// </summary>
        NoAppointment,

        /// <summary>
        /// 已回報過
        /// </summary>
        HasReported,

        /// <summary>
        /// 預約超过72小时后不可投诉
        /// </summary>
        Overtime,

        /// <summary>
        /// 所有预约单已完成
        /// </summary>
        AllAppointmentsFinished
    }
}