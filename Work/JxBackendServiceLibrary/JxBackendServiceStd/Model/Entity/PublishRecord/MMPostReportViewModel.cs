using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Entity.PublishRecord
{
    public class MMPostReportViewModel : MMPostReportModel
    {
        public string PostTypeDesc
        {
            get
            {
                return PostType == 1 ? "广场" : PostType == 2 ? "担保" : PostType == 3 ? "官方" : "体验";
            }
        }

        public string ExamineTimeDesc
        {
            get { return ExamineTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc
        {
            get
            {
                return Status == 1 ? "审核中" : Status == 2 ? "显示中" : "未通过";
            }
        }

        /// <summary>
        /// 投诉原因描述
        /// </summary>
        public string ReportTypeDesc
        {
            get
            {
                return ReportType == 0 ? "骗子" : ReportType == 1 ? "广告骚扰" : ReportType == 2 ? "货不对版" : "无效联系方式";
            }
        }
    }
}