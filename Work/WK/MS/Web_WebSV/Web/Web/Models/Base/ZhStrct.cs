namespace Web.Models.Base
{
    public class ZhStrct
    {
        /// <summary>
        /// 倍数
        /// </summary>
        private long multiple;

        public long Multiple
        {
            get { return multiple; }
            set { multiple = value; }
        }
        /// <summary>
        /// 下注金额
        /// </summary>
        private double money;

        public double Money
        {
            get { return money; }
            set { money = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        private int num;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

    }
}

