namespace JxBackendService.Model.Enums.Finance
{
    public class MoneyInDealType : BaseValueModel<int, MoneyInDealType>
    {
        private MoneyInDealType()
        { }

        /// <summary>
        /// 已处理
        /// </summary>
        public static readonly MoneyInDealType Done = new MoneyInDealType()
        {
            Value = 1,
        };

        /// <summary>
        /// 正在处理
        /// </summary>
        public static readonly MoneyInDealType Processing = new MoneyInDealType()
        {
            Value = 2,
        };

        /// <summary>
        /// 处理失败
        /// </summary>
        public static readonly MoneyInDealType Fail = new MoneyInDealType()
        {
            Value = 3,
        };
    }
}