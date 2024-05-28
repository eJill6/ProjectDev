using BatchService.Job;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using System;
using System.Configuration;

namespace BatchService.Model.Enum
{
    public class JobSetting : BaseStringValueModel<JobSetting>
    {
        public ScheduleJobTypes ScheduleJobType { get; private set; } = ScheduleJobTypes.Quartz;

        public Type JobType { get; private set; }

        public string TriggerName => $"Trigger{Value}";

        public string CronExpression { get; private set; }

        public bool IsEnabled { get; private set; } = true;

        public bool IsStartNow { get; set; } //方便單元測試,所以開放外部修改

        private JobSetting()
        { }

        public static JobSetting StoredProcedureErrorNotice = new JobSetting()
        {
            Value = nameof(StoredProcedureErrorNotice),
            CronExpression = CronExpressions.Every10Minutes,
            JobType = typeof(StoredProcedureErrorNoticeJob),
            IsStartNow = false
        };

        public static JobSetting RecheckTranferOrdersFromMiseLive = new JobSetting()
        {
            Value = nameof(RecheckTranferOrdersFromMiseLive),
            CronExpression = CronExpressions.EveryMinute,
            JobType = typeof(RecheckTranferOrdersFromMiseLiveJob),
            IsStartNow = false
        };

        public static JobSetting CrawlOBEBAnchorList = new JobSetting()
        {
            Value = nameof(CrawlOBEBAnchorList),
            CronExpression = CronExpressions.Every5Seconds,
            JobType = typeof(CrawlOBEBAnchorListJob)
        };

        public static JobSetting TransferToMiseLive = new JobSetting()
        {
            Value = nameof(TransferToMiseLive),
            JobType = typeof(TransferToMiseLiveJob),
            IsStartNow = true,
            ScheduleJobType = ScheduleJobTypes.QueueUserWorkItem,
        };

        public static JobSetting CheckIdleAvailableScores = new JobSetting()
        {
            Value = nameof(CheckIdleAvailableScores),
            CronExpression = CronExpressions.EveryMinute,
            JobType = typeof(CheckIdleAvailableScoresJob)
        };
    }

    public static class CronExpressions
    {
        /// <summary>每5秒1次</summary>
        public static readonly string Every5Seconds = "0/5 * * ? * * *";

        /// <summary>每分鐘1次</summary>
        public static readonly string EveryMinute = "0 0/1 * ? * * *";

        /// <summary>每10分鐘1次</summary>
        public static readonly string Every10Minutes = "0 0/10 * * * ?";

        /// <summary>每小時1次</summary>
        public static readonly string EveryHour = "0 0 0/1 ? * * *";

        /// <summary>每天1點</summary>
        public static readonly string EveryDayAt0100 = "0 0 1 ? * * *";

        /// <summary>每天2點</summary>
        public static readonly string EveryDayAt0200 = "0 0 2 ? * * *";

        /// <summary>每天2點10分</summary>
        public static readonly string EveryDayAt0210 = "0 10 2 ? * * *";

        /// <summary>每天3點</summary>
        public static readonly string EveryDayAt0300 = "0 0 3 ? * * *";

        /// <summary>每天4點</summary>
        public static readonly string EveryDayAt0400 = "0 0 4 ? * * *";

        /// <summary>每天8點</summary>
        public static readonly string EveryDayAt0800 = "0 0 8 ? * * *";

        /// <summary>每天0點15分</summary>
        public static readonly string EveryDayAt0015 = "0 15 0 ? * * *";

        /// <summary>每天0點30分開始,每兩小時一次</summary>
        public static readonly string EveryTwoHourAt0030 = "0 30 0/2 ? * * *";

        /// <summary>每月1號1點05分開始</summary>
        public static readonly string FirstOfEveryMonthAt0105 = "0 5 1 1 * ?";

        /// <summary>每月1號1點07分~59分</summary>
        public static readonly string FirstOfEveryMonthBetween0108To0159 = "0 7-59 1 1 * ?";

        /// <summary>每周禮拜一0點0分開始</summary>
        public static readonly string FirstOfEveryWeekAt0000 = "0 0 0 ? * MON *";
    }

    public enum ScheduleJobTypes
    {
        Quartz,

        QueueUserWorkItem
    }
}