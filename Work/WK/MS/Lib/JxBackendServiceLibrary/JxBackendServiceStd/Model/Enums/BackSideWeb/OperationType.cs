using JxBackendService.Model.BackSideWeb;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums.BackSideWeb
{
    public class OperationType : BaseStringValueModel<OperationType>
    {
        public Type OperationContentModelType { get; private set; } = typeof(string);

        private OperationType()
        {
        }

        public static readonly OperationType DemoCRUD = new OperationType
        {
            Value = "DemoCRUD",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.DemoCRUD),
            Sort = 10,
        };

        public static readonly OperationType AdvertisingContent = new OperationType
        {
            Value = "AdvertisingContent",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.AdvertisingContent),
            Sort = 20,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType Options = new OperationType
        {
            Value = "Options",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.Options),
            Sort = 30,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType Banner = new OperationType
        {
            Value = "Banner",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.Banner),
            Sort = 40,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType PostWeight = new OperationType
        {
            Value = "PostWeight",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostWeight),
            Sort = 50,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType HomeAnnouncement = new OperationType
        {
            Value = "HomeAnnouncement",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.HomeAnnouncement),
            Sort = 60,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType MailSetting = new OperationType
        {
            Value = "MailSetting",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.MailSetting),
            Sort = 65,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType PublishPostRecord = new OperationType
        {
            Value = "PostRecord",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostRecord),
            Sort = 70,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType User = new OperationType
        {
            Value = "User",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.UserQuery),
            Sort = 80,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType EvaluateRecord = new OperationType
        {
            Value = "EvaluateRecord",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.EvaluateRecord),
            Sort = 90,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType ReportRecord = new OperationType
        {
            Value = "ReportRecord",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.ReportRecord),
            Sort = 100,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType UserIdentity = new OperationType
        {
            Value = "UserIdentity",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.UserIdentityQuery),
            Sort = 110,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType AdminIncome = new OperationType
        {
            Value = "AdminIncome",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.AdminIncome),
            Sort = 120,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType OfficialPublishPostRecord = new OperationType
        {
            Value = "OfficialPostRecord",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.OfficialPostRecord),
            Sort = 130,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType OfficialEvaluateRecord = new OperationType
        {
            Value = "OfficialEvaluateRecord",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.OfficialEvaluateRecord),
            Sort = 140,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType OfficialReportRecord = new OperationType
        {
            Value = "OfficialReportRecord",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.OfficialReportRecord),
            Sort = 150,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType StoreEditorReview = new OperationType
        {
            Value = "StoreEditorReview",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.StoreEditorReview),
            Sort = 160,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType StoreManage = new OperationType
        {
            Value = "StoreManage",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.StoreManage),
            Sort = 170,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType PageRedirect = new OperationType
        {
            Value = "PageRedirect",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PageRedirect),
            Sort = 170,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType PostContactUpdate = new OperationType
        {
            Value = "PostContactUpdate",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostContactUpdate),
            Sort = 180,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType GameCenterManage = new OperationType
        {
            Value = "GameCenterManage",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.GameCenterManage),
            Sort = 300,
        };

        public static readonly OperationType HotGameManage = new OperationType
        {
            Value = "HotGameManage",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.HotGameManage),
            Sort = 310,
        };

        public static readonly OperationType SlotGameManage = new OperationType
        {
            Value = "SlotGameManage",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.SlotGameManage),
            Sort = 320,
        };

        public static readonly OperationType RecycleBalance = new OperationType
        {
            Value = "RecycleBalance",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.RecycleBalance),
            Sort = 330,
        };

        public static readonly OperationType UserManagement = new OperationType
        {
            Value = "UserManagement",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.BWUserManagement),
            Sort = 340,
        };

        public static readonly OperationType RoleManagement = new OperationType
        {
            Value = "RoleManagement",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.BWRoleManagement),
            Sort = 350,
        };

        public static readonly OperationType LiveGameManage = new OperationType
        {
            Value = "LiveGameManage",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.LiveGameManage),
            Sort = 360,
        };

        public static readonly OperationType OperationOverview = new OperationType
        {
            Value = "OperationOverview",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.OperationOverview),
            Sort = 600,
        };

        public static readonly OperationType DailyRevenue = new OperationType
        {
            Value = "DailyRevenue",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.DailyRevenue),
            Sort = 610,
        };

        public static readonly OperationType MonthlyRevenue = new OperationType
        {
            Value = "MonthlyRevenue",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.MonthlyRevenue),
            Sort = 620,
        };

        public static readonly OperationType DailyUsers = new OperationType
        {
            Value = "DailyUsers",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.DailyUsers),
            Sort = 630,
        };

        public static readonly OperationType MonthlyUsers = new OperationType
        {
            Value = "MonthlyUsers",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.MonthlyUsers),
            Sort = 640,
        };

        public static readonly OperationType PostOverview = new OperationType
        {
            Value = "PostOverview",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostOverview),
            Sort = 650,
        };

        public static readonly OperationType PostDailyRevenue = new OperationType
        {
            Value = "PostDailyRevenue",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostDailyRevenue),
            Sort = 660,
        };

        public static readonly OperationType PostMonthlyRevenue = new OperationType
        {
            Value = "PostMonthlyRevenue",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostMonthlyRevenue),
            Sort = 670,
        };

        public static readonly OperationType PostDailyTrend = new OperationType
        {
            Value = "PostDailyTrend",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostDailyTrend),
            Sort = 680,
        };

        public static readonly OperationType PostMonthlyTrend = new OperationType
        {
            Value = "PostMonthlyTrend",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.PostMonthlyTrend),
            Sort = 690,
        };

        ///<summary>修改密码</summary>
        public static readonly OperationType ChangePassword = new OperationType
        {
            Value = "ChangePassword",
            ResourceType = typeof(BWOperationLogElement),
            ResourcePropertyName = nameof(BWOperationLogElement.ChangePassword),
            Sort = 1000,
        };

        /// <summary>
        /// 预约申请退款
        /// </summary>
        public static readonly OperationType AdminBookingRefund = new OperationType
        {
            Value = "AdminBookingRefund",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.AdminBookingRefund),
            Sort = 350,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType BetHistory = new OperationType
        {
            Value = "BetHistory",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.BetHistory),
            Sort = 700,
        };

        public static readonly OperationType LotteryHistory = new OperationType
        {
            Value = "LotteryHistory",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.LotteryHistory),
            Sort = 800,
        };

        public static readonly OperationType BotParameter = new OperationType
        {
            Value = "BotParameter",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.BotParameter),
            Sort = 1100,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };

        public static readonly OperationType LiveBot = new OperationType
        {
            Value = "LiveBot",
            ResourceType = typeof(PermissionElement),
            ResourcePropertyName = nameof(PermissionElement.LiveBot),
            Sort = 1110,
            OperationContentModelType = typeof(List<RecordCompareParam>),
        };
    }
}