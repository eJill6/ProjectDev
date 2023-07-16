namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 注單類型
    /// </summary>
    public class WagerType : BaseIntValueModel<WagerType>
    {
        private WagerType()
        { }

        /// <summary>單一</summary>
        public static WagerType Single = new WagerType()
        {
            Value = 1,
        };

        /// <summary>混合(串關)</summary>
        public static WagerType Combo = new WagerType()
        {
            Value = 2
        };
    }
}