using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.BackSideWeb.Permission
{
    public enum PermissionKeys
    {
        DemoSearch,

        DemoCRUD,

        ChangePassword,

        GameCenterManage,

        HotGameManage,

        SlotGameManage,

        TransferRecord,

        LiveGameManage,

        AdvertisingContent,

        Options,

        Banner,

        PostWeight,

        HomeAnnouncement,

        /// <summary> 用戶管理 </summary>
        UserManagement,

        /// <summary> 角色管理 </summary>
        RoleManagement,

        /// <summary> 操作日志 </summary>
        OperationLog,

        /// <summary> 会员管理 </summary>
        User,

        /// <summary> 会员卡消费记录 </summary>
        UserCard,

        MemberTransactionRec,

        PostRecord,

        EvaluateRecord,

        ReportRecord,

        /// <summary> 会员收付记录 </summary>
        IncomeExpense,

        /// <summary> 收益单记录 </summary>
        AdminIncome,

        /// <summary> 解锁单记录 </summary>
        AdminPostTransaction,

        /// <summary> 预约单记录 </summary>
        AdminBooking,

        /// <summary> 预约退款申请记录 </summary>
        AdminBookingRefund,

        /// <summary>
        /// 官方发表记录
        /// </summary>
        OfficialPostRecord,

        /// <summary>
        /// 官方评价记录
        /// </summary>
        OfficialEvaluateRecord,

        /// <summary>
        /// 官方投诉记录
        /// </summary>
        OfficialReportRecord,

        /// <summary> 会员身份认证 </summary>
        UserIdentity,

        /// <summary> 回收余额 </summary>
        RecycleBalance,

        /// <summary> 私信页设定 </summary>
        MailSetting,

        /// <summary> 營運總覽 </summary>
        OperationOverview,

        /// <summary> 日营收 </summary>
        DailyRevenue,

        /// <summary> 月營收 </summary>
        MonthlyRevenue,

        /// <summary> 日人數 </summary>
        DailyUsers,

        /// <summary> 月人數 </summary>
        MonthlyUsers,

        /// <summary> 帖子總覽 </summary>
        PostOverview,

        /// <summary> 帖子_日營收 </summary>
        PostDailyRevenue,

        /// <summary> 帖子_月營收 </summary>
        PostMonthlyRevenue,

        /// <summary> 帖子_日趨勢 </summary>
        PostDailyTrend,

        /// <summary> 帖子_月趨勢 </summary>
        PostMonthlyTrend,

        /// <summary> 秘色彩票投注 </summary>
        BetHistory,

        /// <summary> 开奖号码 </summary>
        LotteryHistory,

        /// <summary> 设置机器人参数 </summary>
        BotParameter,

        /// <summary> 设置直播间机器人 </summary>
        LiveBot,

        /// <summary> 店铺编辑审核 </summary>
        StoreEditorReview,

        /// <summary> 店铺管理 </summary>
        StoreManage,

        /// <summary> 秘色广告转导 </summary>
        PageRedirect,

        /// <summary> 批量修改联系 </summary>
        PostContactUpdate,
    }

    public class PermissionKeyDetail : BaseStringValueModel<PermissionKeyDetail>
    {
        public string MenuPath { get; private set; }

        public MenuType MenuType { get; private set; }

        public AuthorityTypeDetail[] AuthorityTypeDetails { get; private set; } = AuthoritySetting.Standard();

        /// <summary ref="https://fontawesome.com/v4/icons/"> Icon 樣式 </summary>
        public string FaIconClass { get; private set; } = "fa-angle-right";

        public OperationType OperationType { get; private set; }

        private PermissionKeyDetail(PermissionKeys permissionKey)
        {
            Value = permissionKey.ToString();
            ResourceType = typeof(PermissionElement);
        }

        ///<summary>示範頁</summary>
        public static readonly PermissionKeyDetail DemoSearch = new PermissionKeyDetail(PermissionKeys.DemoSearch)
        {
            ResourcePropertyName = nameof(PermissionElement.DemoSearch),
            MenuType = MenuType.Demo,
            MenuPath = "/Demo/Index",
            Sort = 1,
            AuthorityTypeDetails = AuthoritySetting.Read(),
        };

        ///<summary>示範頁CRUD</summary>
        public static readonly PermissionKeyDetail DemoCRUD = new PermissionKeyDetail(PermissionKeys.DemoCRUD)
        {
            ResourcePropertyName = nameof(PermissionElement.DemoCRUD),
            MenuType = MenuType.Demo,
            MenuPath = "/DemoCRUD/Index",
            Sort = 2,
            OperationType = OperationType.DemoCRUD,
        };

        ///<summary>游戏大厅管理</summary>
        public static readonly PermissionKeyDetail GameCenterManage = new PermissionKeyDetail(PermissionKeys.GameCenterManage)
        {
            ResourcePropertyName = nameof(PermissionElement.GameCenterManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/GameManage/Index",
            Sort = 100,
            FaIconClass = "fa-life-ring",
            AuthorityTypeDetails = AuthoritySetting.ReadEdit(),
            OperationType = OperationType.GameCenterManage,
        };

        ///<summary>热门游戏管理</summary>
        public static readonly PermissionKeyDetail HotGameManage = new PermissionKeyDetail(PermissionKeys.HotGameManage)
        {
            ResourcePropertyName = nameof(PermissionElement.HotGameManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/HotGameManage/Index",
            Sort = 200,
            FaIconClass = "fa-trophy",
            OperationType = OperationType.HotGameManage,
        };

        ///<summary>电子游戏管理</summary>
        public static readonly PermissionKeyDetail SlotGameManage = new PermissionKeyDetail(PermissionKeys.SlotGameManage)
        {
            ResourcePropertyName = nameof(PermissionElement.SlotGameManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/SlotGameManage/Index",
            Sort = 300,
            FaIconClass = "fa-gamepad",
            OperationType = OperationType.SlotGameManage,
        };

        ///<summary>电子游戏管理</summary>
        public static readonly PermissionKeyDetail TransferRecord = new PermissionKeyDetail(PermissionKeys.TransferRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.TransferRecord),
            MenuType = MenuType.ProductManage,
            MenuPath = "/TransferRecord/Index",
            Sort = 500,
            FaIconClass = "fa-refresh",
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
        };

        ///<summary>直播游戏选单管理</summary>
        public static readonly PermissionKeyDetail LiveGameManage = new PermissionKeyDetail(PermissionKeys.LiveGameManage)
        {
            ResourcePropertyName = nameof(PermissionElement.LiveGameManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/LiveGameManage/Index",
            Sort = 600,
            FaIconClass = "fa-gamepad",
            OperationType = OperationType.LiveGameManage,
        };

        ///<summary>宣传文字頁</summary>
        public static readonly PermissionKeyDetail AdvertisingContent = new PermissionKeyDetail(PermissionKeys.AdvertisingContent)
        {
            ResourcePropertyName = nameof(PermissionElement.AdvertisingContent),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/AdvertisingContent/Index",
            Sort = 1,
            OperationType = OperationType.AdvertisingContent,
        };

        ///<summary>发帖项目编辑</summary>
        public static readonly PermissionKeyDetail Options = new PermissionKeyDetail(PermissionKeys.Options)
        {
            ResourcePropertyName = nameof(PermissionElement.Options),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/Options/Index",
            Sort = 2,
            OperationType = OperationType.Options,
        };

        ///<summary>Banner頁</summary>
        public static readonly PermissionKeyDetail Banner = new PermissionKeyDetail(PermissionKeys.Banner)
        {
            ResourcePropertyName = nameof(PermissionElement.Banner),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/Banner/Index",
            Sort = 3,
            OperationType = OperationType.Banner,
        };

        ///<summary>首页帖子设定</summary>
        public static readonly PermissionKeyDetail PostWeight = new PermissionKeyDetail(PermissionKeys.PostWeight)
        {
            ResourcePropertyName = nameof(PermissionElement.PostWeight),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/PostWeight/Index",
            Sort = 4,
            OperationType = OperationType.PostWeight,
        };

        ///<summary>公告设定</summary>
        public static readonly PermissionKeyDetail HomeAnnouncement = new PermissionKeyDetail(PermissionKeys.HomeAnnouncement)
        {
            ResourcePropertyName = nameof(PermissionElement.HomeAnnouncement),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/HomeAnnouncement/Index",
            Sort = 5,
            OperationType = OperationType.HomeAnnouncement,
        };

        /////<summary>私信页设定</summary>
        //public static readonly PermissionKeyDetail MailSetting = new PermissionKeyDetail(PermissionKeys.MailSetting)
        //{
        //    ResourcePropertyName = nameof(PermissionElement.MailSetting),
        //    MenuType = MenuType.PublicitySettings,
        //    MenuPath = "/MailSetting/Index",
        //    Sort = 6,
        //    OperationType = OperationType.MailSetting,
        //};

        ///<summary>秘色广告转导</summary>
        public static readonly PermissionKeyDetail PageRedirect = new PermissionKeyDetail(PermissionKeys.PageRedirect)
        {
            ResourcePropertyName = nameof(PermissionElement.PageRedirect),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/PageRedirect/Index",
            Sort = 7,
            OperationType = OperationType.PageRedirect,
        };

        ///<summary>发表记录</summary>
        public static readonly PermissionKeyDetail PublishPostRecord = new PermissionKeyDetail(PermissionKeys.PostRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.PostRecord),
            MenuType = MenuType.PublishPostRecord,
            MenuPath = "/PublishRecord/Index",
            Sort = 1,
            OperationType = OperationType.PublishPostRecord,
        };

        ///// <summary>
        ///// 评价记录
        ///// </summary>
        //public static readonly PermissionKeyDetail EvaluateRecord = new PermissionKeyDetail(PermissionKeys.EvaluateRecord)
        //{
        //    ResourcePropertyName = nameof(PermissionElement.EvaluateRecord),
        //    MenuType = MenuType.PublishPostRecord,
        //    MenuPath = "/EvaluateRecord/Index",
        //    Sort = 2,
        //    OperationType = OperationType.EvaluateRecord,
        //};

        //ReportRecord
        /// <summary>
        /// 投诉记录
        /// </summary>
        public static readonly PermissionKeyDetail ReportRecord = new PermissionKeyDetail(PermissionKeys.ReportRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.ReportRecord),
            MenuType = MenuType.PublishPostRecord,
            MenuPath = "/ReportRecord/Index",
            Sort = 3,
            OperationType = OperationType.ReportRecord,
        };

        ///<summary>批量修改联系</summary>
        public static readonly PermissionKeyDetail PostContactUpdate = new PermissionKeyDetail(PermissionKeys.PostContactUpdate)
        {
            ResourcePropertyName = nameof(PermissionElement.PostContactUpdate),
            MenuType = MenuType.PublishPostRecord,
            MenuPath = "/PostContactUpdate/Index",
            Sort = 4,
            OperationType = OperationType.PostContactUpdate,
        };

        ///<summary>
        ///官方发帖记录
        ///</summary>
        public static readonly PermissionKeyDetail OfficialPublishPostRecord = new PermissionKeyDetail(PermissionKeys.OfficialPostRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.OfficialPostRecord),
            MenuType = MenuType.OfficialPublishPostRecord,
            MenuPath = "/OfficialPublishRecord/Index",
            Sort = 1,
            OperationType = OperationType.OfficialPublishPostRecord
        };

        ///// <summary>
        ///// 官方评价记录
        ///// </summary>
        //public static readonly PermissionKeyDetail OfficialEvaluateRecord = new PermissionKeyDetail(PermissionKeys.OfficialEvaluateRecord)
        //{
        //    ResourcePropertyName = nameof(PermissionElement.OfficialEvaluateRecord),
        //    MenuType = MenuType.OfficialPublishPostRecord,
        //    MenuPath = "/OfficialEvaluateRecord/Index",
        //    Sort = 2,
        //    OperationType = OperationType.OfficialEvaluateRecord
        //};

        /// <summary>
        /// 官方投诉记录
        /// </summary>
        public static readonly PermissionKeyDetail OfficialReportRecord = new PermissionKeyDetail(PermissionKeys.OfficialReportRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.OfficialReportRecord),
            MenuType = MenuType.OfficialPublishPostRecord,
            MenuPath = "/OfficialReportRecord/Index",
            Sort = 3,
            OperationType = OperationType.OfficialReportRecord
        };

        ///<summary>会员查询页</summary>
        public static readonly PermissionKeyDetail User = new PermissionKeyDetail(PermissionKeys.User)
        {
            ResourcePropertyName = nameof(PermissionElement.UserQuery),
            MenuType = MenuType.User,
            MenuPath = "/User/Index",
            Sort = 1,
            OperationType = OperationType.User,
        };

        /////<summary>会员卡消费记录</summary>
        //public static readonly PermissionKeyDetail UserCard = new PermissionKeyDetail(PermissionKeys.UserCard)
        //{
        //    ResourcePropertyName = nameof(PermissionElement.UserCardQuery),
        //    MenuType = MenuType.User,
        //    MenuPath = "/UserCard/Index",
        //    Sort = 2,
        //};

        ///<summary>会员收付记录</summary>
		public static readonly PermissionKeyDetail IncomeExpense = new PermissionKeyDetail(PermissionKeys.IncomeExpense)
        {
            ResourcePropertyName = nameof(PermissionElement.IncomeExpenseQuery),
            MenuType = MenuType.User,
            MenuPath = "/IncomeExpense/Index",
            Sort = 3,
            AuthorityTypeDetails = AuthoritySetting.All(),
        };

        ///<summary>会员身份认证</summary>
		public static readonly PermissionKeyDetail UserIdentity = new PermissionKeyDetail(PermissionKeys.UserIdentity)
        {
            ResourcePropertyName = nameof(PermissionElement.UserIdentityQuery),
            MenuType = MenuType.User,
            MenuPath = "/UserIdentity/Index",
            Sort = 4,
            OperationType = OperationType.UserIdentity,
        };

        ///<summary>店铺编辑审核</summary>
        public static readonly PermissionKeyDetail StoreEditorReview = new PermissionKeyDetail(PermissionKeys.StoreEditorReview)
        {
            ResourcePropertyName = nameof(PermissionElement.StoreEditorReview),
            MenuType = MenuType.User,
            MenuPath = "/StoreEditorReview/Index",
            Sort = 5,
            OperationType = OperationType.StoreEditorReview,
        };

        ///<summary>店铺管理</summary>
        public static readonly PermissionKeyDetail StoreManage = new PermissionKeyDetail(PermissionKeys.StoreManage)
        {
            ResourcePropertyName = nameof(PermissionElement.StoreManage),
            MenuType = MenuType.User,
            MenuPath = "/StoreManage/Index",
            Sort = 5,
            OperationType = OperationType.StoreManage,
        };

        ///<summary>角色管理</summary>
        public static readonly PermissionKeyDetail RoleManagement = new PermissionKeyDetail(PermissionKeys.RoleManagement)
        {
            ResourcePropertyName = nameof(PermissionElement.BWRoleManagement),
            MenuType = MenuType.SystemSetting,
            MenuPath = "/RoleManagement/Index",
            Sort = 200,
            FaIconClass = "fa-users",
            OperationType = OperationType.RoleManagement,
        };

        ///<summary>用戶管理</summary>
        public static readonly PermissionKeyDetail UserManagement = new PermissionKeyDetail(PermissionKeys.UserManagement)
        {
            ResourcePropertyName = nameof(PermissionElement.BWUserManagement),
            MenuType = MenuType.SystemSetting,
            MenuPath = "/UserManagement/Index",
            Sort = 100,
            FaIconClass = "fa-user-circle-o",
            OperationType = OperationType.UserManagement,
        };

        ///<summary>操作日志</summary>
        public static readonly PermissionKeyDetail OperationLog = new PermissionKeyDetail(PermissionKeys.OperationLog)
        {
            ResourcePropertyName = nameof(PermissionElement.BWOperationLog),
            MenuType = MenuType.SystemSetting,
            MenuPath = "/OperationLog/Index",
            Sort = 300,
            FaIconClass = "fa-pencil-square-o",
            AuthorityTypeDetails = AuthoritySetting.Read(),
        };

        ///<summary>收益单记录</summary>
        public static readonly PermissionKeyDetail AdminIncome = new PermissionKeyDetail(PermissionKeys.AdminIncome)
        {
            ResourcePropertyName = nameof(PermissionElement.AdminIncome),
            MenuType = MenuType.PaymentRecord,
            MenuPath = "/AdminIncome/Index",
            Sort = 1,
            FaIconClass = "fa-navicon",
            OperationType = OperationType.AdminIncome,
            AuthorityTypeDetails = AuthoritySetting.All(),
        };

        ///<summary>解锁单记录</summary>
        public static readonly PermissionKeyDetail AdminPostTransaction = new PermissionKeyDetail(PermissionKeys.AdminPostTransaction)
        {
            ResourcePropertyName = nameof(PermissionElement.AdminPostTransaction),
            MenuType = MenuType.PaymentRecord,
            MenuPath = "/AdminPostTransaction/Index",
            Sort = 2,
            FaIconClass = "fa-unlock-alt",
            AuthorityTypeDetails = AuthoritySetting.All(),
        };

        ///<summary>预约单记录</summary>
        public static readonly PermissionKeyDetail AdminBooking = new PermissionKeyDetail(PermissionKeys.AdminBooking)
        {
            ResourcePropertyName = nameof(PermissionElement.AdminBooking),
            MenuType = MenuType.PaymentRecord,
            MenuPath = "/AdminBooking/Index",
            Sort = 3,
            FaIconClass = "fa-navicon",
            AuthorityTypeDetails = AuthoritySetting.All(),
        };

        ///<summary>预约退款申请记录</summary>
        public static readonly PermissionKeyDetail AdminBookingRefund = new PermissionKeyDetail(PermissionKeys.AdminBookingRefund)
        {
            ResourcePropertyName = nameof(PermissionElement.AdminBookingRefund),
            MenuType = MenuType.PaymentRecord,
            MenuPath = "/AdminBookingRefund/Index",
            Sort = 4,
            FaIconClass = "fa-navicon",
            OperationType = OperationType.AdminBookingRefund
        };

        ///<summary>回收余额</summary>
        public static readonly PermissionKeyDetail RecycleBalance = new PermissionKeyDetail(PermissionKeys.RecycleBalance)
        {
            ResourcePropertyName = nameof(PermissionElement.RecycleBalance),
            MenuType = MenuType.ProductManage,
            MenuPath = "/RecycleBalance/Index",
            Sort = 400,
            FaIconClass = "fa-exchange",
            AuthorityTypeDetails = AuthoritySetting.ReadEdit(),
            OperationType = OperationType.RecycleBalance,
        };

        public static readonly PermissionKeyDetail OperationOverview = new PermissionKeyDetail(PermissionKeys.OperationOverview)
        {
            ResourcePropertyName = nameof(PermissionElement.OperationOverview),
            MenuType = MenuType.OperatingData,
            MenuPath = "/OperationOverview/Index",
            Sort = 600,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.OperationOverview,
        };

        public static readonly PermissionKeyDetail DailyRevenue = new PermissionKeyDetail(PermissionKeys.DailyRevenue)
        {
            ResourcePropertyName = nameof(PermissionElement.DailyRevenue),
            MenuType = MenuType.OperatingData,
            MenuPath = "/DailyRevenue/Index",
            Sort = 610,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.DailyRevenue,
        };

        public static readonly PermissionKeyDetail MonthlyRevenue = new PermissionKeyDetail(PermissionKeys.MonthlyRevenue)
        {
            ResourcePropertyName = nameof(PermissionElement.MonthlyRevenue),
            MenuType = MenuType.OperatingData,
            MenuPath = "/MonthlyRevenue/Index",
            Sort = 620,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.MonthlyRevenue,
        };

        public static readonly PermissionKeyDetail DailyUsers = new PermissionKeyDetail(PermissionKeys.DailyUsers)
        {
            ResourcePropertyName = nameof(PermissionElement.DailyUsers),
            MenuType = MenuType.OperatingData,
            MenuPath = "/DailyUsers/Index",
            Sort = 630,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.DailyUsers,
        };

        public static readonly PermissionKeyDetail MonthlyUsers = new PermissionKeyDetail(PermissionKeys.MonthlyUsers)
        {
            ResourcePropertyName = nameof(PermissionElement.MonthlyUsers),
            MenuType = MenuType.OperatingData,
            MenuPath = "/MonthlyUsers/Index",
            Sort = 640,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.MonthlyUsers,
        };

        public static readonly PermissionKeyDetail PostOverview = new PermissionKeyDetail(PermissionKeys.PostOverview)
        {
            ResourcePropertyName = nameof(PermissionElement.PostOverview),
            MenuType = MenuType.OperatingData,
            MenuPath = "/PostOverview/Index",
            Sort = 650,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.PostOverview,
        };

        public static readonly PermissionKeyDetail PostDailyRevenue = new PermissionKeyDetail(PermissionKeys.PostDailyRevenue)
        {
            ResourcePropertyName = nameof(PermissionElement.PostDailyRevenue),
            MenuType = MenuType.OperatingData,
            MenuPath = "/PostDailyRevenue/Index",
            Sort = 660,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.PostDailyRevenue,
        };

        public static readonly PermissionKeyDetail PostMonthlyRevenue = new PermissionKeyDetail(PermissionKeys.PostMonthlyRevenue)
        {
            ResourcePropertyName = nameof(PermissionElement.PostMonthlyRevenue),
            MenuType = MenuType.OperatingData,
            MenuPath = "/PostMonthlyRevenue/Index",
            Sort = 670,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.PostMonthlyRevenue,
        };

        public static readonly PermissionKeyDetail PostDailyTrend = new PermissionKeyDetail(PermissionKeys.PostDailyTrend)
        {
            ResourcePropertyName = nameof(PermissionElement.PostDailyTrend),
            MenuType = MenuType.OperatingData,
            MenuPath = "/PostDailyTrend/Index",
            Sort = 680,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.PostDailyTrend,
        };

        public static readonly PermissionKeyDetail PostMonthlyTrend = new PermissionKeyDetail(PermissionKeys.PostMonthlyTrend)
        {
            ResourcePropertyName = nameof(PermissionElement.PostMonthlyTrend),
            MenuType = MenuType.OperatingData,
            MenuPath = "/PostMonthlyTrend/Index",
            Sort = 690,
            AuthorityTypeDetails = AuthoritySetting.ReadExport(),
            OperationType = OperationType.PostMonthlyTrend,
        };

        ///<summary>秘色彩票投注</summary>
        public static readonly PermissionKeyDetail BetHistory = new PermissionKeyDetail(PermissionKeys.BetHistory)
        {
            ResourcePropertyName = nameof(PermissionElement.BetHistory),
            MenuType = MenuType.PlayBetRecord,
            MenuPath = "/BetHistory/Index",
            Sort = 1,
            OperationType = OperationType.BetHistory,
            AuthorityTypeDetails = AuthoritySetting.Read(),
        };

        ///<summary>开奖号码</summary>
        public static readonly PermissionKeyDetail LotteryHistory = new PermissionKeyDetail(PermissionKeys.LotteryHistory)
        {
            ResourcePropertyName = nameof(PermissionElement.LotteryHistory),
            MenuType = MenuType.Lottery,
            MenuPath = "/LotteryHistory/Index",
            Sort = 1,
            OperationType = OperationType.LotteryHistory,
            AuthorityTypeDetails = AuthoritySetting.Read(),
        };

        ///<summary>设置机器人参数</summary>
        public static readonly PermissionKeyDetail BotParameter = new PermissionKeyDetail(PermissionKeys.BotParameter)
        {
            ResourcePropertyName = nameof(PermissionElement.BotParameter),
            MenuType = MenuType.BetBot,
            MenuPath = "/BotParameter/Index",
            Sort = 1,
            OperationType = OperationType.BotParameter,
        };

        ///<summary>设置直播间机器人</summary>
        public static readonly PermissionKeyDetail LiveBot = new PermissionKeyDetail(PermissionKeys.LiveBot)
        {
            ResourcePropertyName = nameof(PermissionElement.LiveBot),
            MenuType = MenuType.BetBot,
            MenuPath = "/LiveBot/Index",
            Sort = 2,
            OperationType = OperationType.LiveBot,
        };

        public static PermissionKeyDetail GetSingle(PermissionKeys permissionKey)
        {
            return GetSingle(permissionKey.ToString());
        }
    }

    public class MenuType : BaseStringValueModel<MenuType>
    {
        private MenuType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public static MenuType ProductManage = new MenuType()
        {
            Value = "ProductManage",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_ProductManage),
            Sort = 800,
        };

        public static MenuType Demo = new MenuType()
        {
            Value = "Demo",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_Demo),
            Sort = 2,
        };

        /// <summary>
        /// 一级菜单-帖子记录
        /// </summary>
        public static MenuType PublishPostRecord = new MenuType()
        {
            Value = "PublishPostRecord",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_PostRecord),
            Sort = 3
        };

        /// <summary>
        /// 一级菜单-官方帖子管理
        /// </summary>
        public static MenuType OfficialPublishPostRecord = new MenuType()
        {
            Value = "OfficialPublishPostRecord",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_OfficialPostRecord),
            Sort = 4
        };

        //public static MenuType EvaluateRecord = new MenuType()
        //{
        //    Value = "EvaluateRecord",
        //    ResourcePropertyName = nameof(SelectItemElement.MenuType_PostRecord),
        //    Sort = 3
        //};

        //public static MenuType Member = new MenuType()
        //{
        //	Value = "Member",
        //	ResourcePropertyName = nameof(SelectItemElement.MenuType_Member),
        //	Sort = 3
        //};

        public static MenuType SystemSetting = new MenuType()
        {
            Value = "SystemSetting",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_SystemSetting),
            Sort = 900
        };

        public static MenuType PublicitySettings = new MenuType()
        {
            Value = "PublicitySettings",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_PublicitySettings),
            Sort = 1
        };

        public static MenuType User = new MenuType()
        {
            Value = "User",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_User),
            Sort = 5
        };

        public static MenuType PaymentRecord = new MenuType()
        {
            Value = "PaymentRecord",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_PaymentRecord),
            Sort = 6
        };

        public static MenuType OperatingData = new MenuType()
        {
            Value = "OperatingData",
            ResourcePropertyName = nameof(SelectItemElement.OperatingData),
            Sort = 0
        };

        public static MenuType PlayBetRecord = new MenuType()
        {
            Value = "PlayBetRecord",
            ResourcePropertyName = nameof(SelectItemElement.PlayBetRecord),
            Sort = 7
        };

        public static MenuType Lottery = new MenuType()
        {
            Value = "Lottery",
            ResourcePropertyName = nameof(SelectItemElement.Lottery),
            Sort = 8
        };

        public static MenuType BetBot = new MenuType()
        {
            Value = "BetBot",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_BetBot),
            Sort = 9
        };
    }
}