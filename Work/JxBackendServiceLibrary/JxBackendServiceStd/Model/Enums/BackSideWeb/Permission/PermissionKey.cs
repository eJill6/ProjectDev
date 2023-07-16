using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.BackSideWeb.Permission
{
    public enum PermissionKeys
    {
        DemoSearch,

        DemoCRUD,

        GameCenterManage,

        HotGameManage,

        SlotGameManage,

        TransferRecord,

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

        /// <summary> 收益但记录 </summary>
        AdminIncome,

        /// <summary> 解锁单记录 </summary>
        AdminPostTransaction,

        /// <summary>
        /// 官方发表记录
        /// </summary>
        OfficialPostRecord,

       /// <summary>
       /// 官方评价记录
       /// </summary>
        OfficialEvaluateRecord,

       /// <summary>
       /// 投诉记录
       /// </summary>
        OfficialReportRecord,

        /// <summary> 会员身份认证 </summary>
        UserIdentity,

        /// <summary> 回收余额 </summary>
        RecycleBalance,
    }

    public class PermissionKeyDetail : BaseStringValueModel<PermissionKeyDetail>
    {
        public string MenuPath { get; private set; }

        public MenuType MenuType { get; private set; }

        public AuthorityTypeDetail[] AuthorityTypeDetails { get; private set; } = AuthorityTypeDetail.GetAll().ToArray();

        /// <summary ref="https://fontawesome.com/v4/icons/"> Icon 樣式 </summary>
        public string FaIconClass { get; private set; } = "fa-angle-right";

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
        };

        ///<summary>游戏大厅管理</summary>
        public static readonly PermissionKeyDetail GameCenterManage = new PermissionKeyDetail(PermissionKeys.GameCenterManage)
        {
            ResourcePropertyName = nameof(PermissionElement.GameCenterManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/GameManage/Index",
            Sort = 100,
            FaIconClass = "fa-life-ring",
            AuthorityTypeDetails = AuthoritySetting.ReadEdit()
        };

        ///<summary>热门游戏管理</summary>
        public static readonly PermissionKeyDetail HotGameManage = new PermissionKeyDetail(PermissionKeys.HotGameManage)
        {
            ResourcePropertyName = nameof(PermissionElement.HotGameManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/HotGameManage/Index",
            Sort = 200,
            FaIconClass = "fa-trophy",
        };

        ///<summary>电子游戏管理</summary>
        public static readonly PermissionKeyDetail SlotGameManage = new PermissionKeyDetail(PermissionKeys.SlotGameManage)
        {
            ResourcePropertyName = nameof(PermissionElement.SlotGameManage),
            MenuType = MenuType.ProductManage,
            MenuPath = "/SlotGameManage/Index",
            Sort = 300,
            FaIconClass = "fa-gamepad",
        };

        ///<summary>电子游戏管理</summary>
        public static readonly PermissionKeyDetail TransferRecord = new PermissionKeyDetail(PermissionKeys.TransferRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.TransferRecord),
            MenuType = MenuType.ProductManage,
            MenuPath = "/TransferRecord/Index",
            Sort = 500,
            FaIconClass = "fa-refresh",
            AuthorityTypeDetails = AuthoritySetting.Read(),
        };

        ///<summary>宣传文字頁</summary>
        public static readonly PermissionKeyDetail AdvertisingContent = new PermissionKeyDetail(PermissionKeys.AdvertisingContent)
        {
            ResourcePropertyName = nameof(PermissionElement.AdvertisingContent),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/AdvertisingContent/Index",
            Sort = 1,
        };

        ///<summary>项目编辑頁</summary>
        public static readonly PermissionKeyDetail Options = new PermissionKeyDetail(PermissionKeys.Options)
        {
            ResourcePropertyName = nameof(PermissionElement.Options),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/Options/Index",
            Sort = 2,
        };

        ///<summary>发表记录</summary>
        public static readonly PermissionKeyDetail PublishPostRecord = new PermissionKeyDetail(PermissionKeys.PostRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.PostRecord),
            MenuType = MenuType.PublishPostRecord,
            MenuPath = "/PublishRecord/Index",
            Sort = 1,
        };

        /// <summary>
        /// 评价记录
        /// </summary>
        public static readonly PermissionKeyDetail EvaluateRecord = new PermissionKeyDetail(PermissionKeys.EvaluateRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.EvaluateRecord),
            MenuType = MenuType.PublishPostRecord,
            MenuPath = "/EvaluateRecord/Index",
            Sort = 2,
        };

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
        };
        /// <summary>
        /// 官方发表记录
        /// </summary>
        public static readonly PermissionKeyDetail OfficialPublishPostRecord = new PermissionKeyDetail(PermissionKeys.OfficialPostRecord)
        {
            ResourcePropertyName = nameof(PermissionElement.OfficialPublishPostRecord),
            MenuType = MenuType.OfficialPublishPostRecord,
            MenuPath = "/OfficialPublishRecord/Index",
            Sort = 1,
        };

        ///<summary>会员查询页</summary>
        public static readonly PermissionKeyDetail User = new PermissionKeyDetail(PermissionKeys.User)
		{
			ResourcePropertyName = nameof(PermissionElement.UserQuery),
			MenuType = MenuType.User,
			MenuPath = "/User/Index",
			Sort = 1,
		};

        ///<summary>会员卡消费记录</summary>
        public static readonly PermissionKeyDetail UserCard = new PermissionKeyDetail(PermissionKeys.UserCard)
        {
            ResourcePropertyName = nameof(PermissionElement.UserCardQuery),
            MenuType = MenuType.User,
            MenuPath = "/UserCard/Index",
            Sort = 2,
        };

        ///<summary>会员收付记录</summary>
		public static readonly PermissionKeyDetail IncomeExpense = new PermissionKeyDetail(PermissionKeys.IncomeExpense)
        {
            ResourcePropertyName = nameof(PermissionElement.IncomeExpenseQuery),
            MenuType = MenuType.User,
            MenuPath = "/IncomeExpense/Index",
            Sort = 3,
        };

        ///<summary>会员身份认证</summary>
		public static readonly PermissionKeyDetail UserIdentity = new PermissionKeyDetail(PermissionKeys.UserIdentity)
        {
            ResourcePropertyName = nameof(PermissionElement.UserIdentityQuery),
            MenuType = MenuType.User,
            MenuPath = "/UserIdentity/Index",
            Sort = 4,
        };

        //請勿改動
        ///<summary>Banner頁</summary>
        public static readonly PermissionKeyDetail Banner = new PermissionKeyDetail(PermissionKeys.Banner)
        {
            ResourcePropertyName = nameof(PermissionElement.Banner),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/Banner/Index",
            Sort = 3,
        };

		///<summary>首页帖子设定</summary>
		public static readonly PermissionKeyDetail PostWeight = new PermissionKeyDetail(PermissionKeys.PostWeight)
		{
			ResourcePropertyName = nameof(PermissionElement.PostWeight),
			MenuType = MenuType.PublicitySettings,
			MenuPath = "/PostWeight/Index",
			Sort = 4,
		};

        ///<summary>首页公告设定</summary>
        public static readonly PermissionKeyDetail HomeAnnouncement = new PermissionKeyDetail(PermissionKeys.HomeAnnouncement)
        {
            ResourcePropertyName = nameof(PermissionElement.HomeAnnouncement),
            MenuType = MenuType.PublicitySettings,
            MenuPath = "/HomeAnnouncement/Index",
            Sort = 5,
        };

        ///<summary>角色管理</summary>
        public static readonly PermissionKeyDetail RoleManagement = new PermissionKeyDetail(PermissionKeys.RoleManagement)
        {
            ResourcePropertyName = nameof(PermissionElement.BWRoleManagement),
            MenuType = MenuType.SystemSetting,
            MenuPath = "/RoleManagement/Index",
            Sort = 200,
            FaIconClass = "fa-users",
        };

        ///<summary>用戶管理</summary>
        public static readonly PermissionKeyDetail UserManagement = new PermissionKeyDetail(PermissionKeys.UserManagement)
        {
            ResourcePropertyName = nameof(PermissionElement.BWUserManagement),
            MenuType = MenuType.SystemSetting,
            MenuPath = "/UserManagement/Index",
            Sort = 100,
            FaIconClass = "fa-user-circle-o",
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
            Sort = 4,
            FaIconClass = "fa-navicon",
        };

        ///<summary>解锁单记录</summary>
        public static readonly PermissionKeyDetail AdminPostTransaction = new PermissionKeyDetail(PermissionKeys.AdminPostTransaction)
        {
            ResourcePropertyName = nameof(PermissionElement.AdminPostTransaction),
            MenuType = MenuType.PaymentRecord,
            MenuPath = "/AdminPostTransaction/Index",
            Sort = 5,
            FaIconClass = "fa-unlock-alt",
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
            Sort = 3
        };

        public static MenuType PaymentRecord = new MenuType()
        {
            Value = "PaymentRecord",
            ResourcePropertyName = nameof(SelectItemElement.MenuType_PaymentRecord),
            Sort = 6
        };
    }
}