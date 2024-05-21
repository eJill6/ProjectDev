namespace JxBackendService.Model.Enums.ThirdParty.JDB
{
    public class JDBApiAction : BaseIntValueModel<JDBApiAction>
    {
        public int SearchRangeMinutes { get; private set; }

        public double MinSearchMinutesAgo { get; private set; }

        public double MaxSearchMinutesAgo { get; private set; }

        private JDBApiAction()
        { }

        /// <summary>
        /// 查询游戏详细交易信息
        /// </summary>
        public static readonly JDBApiAction QueryBetLogRecently = new JDBApiAction()
        {
            Value = 29,
            SearchRangeMinutes = 15,
            MinSearchMinutesAgo = 3,
            MaxSearchMinutesAgo = 120,
        };

        /// <summary>
        /// 查询历史游戏详细交易信息
        /// </summary>
        public static readonly JDBApiAction QueryBetLogHistory = new JDBApiAction()
        {
            Value = 64,
            SearchRangeMinutes = 5,
            MinSearchMinutesAgo = 120,
            MaxSearchMinutesAgo = 60 * 24 * 60, //60days
        };

        public static readonly JDBApiAction Transfer = new JDBApiAction() { Value = 19 };

        public static readonly JDBApiAction QueryOrder = new JDBApiAction() { Value = 55 };

        public static readonly JDBApiAction QueryUserScore = new JDBApiAction() { Value = 15 };

        public static readonly JDBApiAction CreateUser = new JDBApiAction() { Value = 12 };

        public static readonly JDBApiAction GetToken = new JDBApiAction() { Value = 11 };
    }
}