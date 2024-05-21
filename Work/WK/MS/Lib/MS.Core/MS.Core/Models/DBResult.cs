namespace MS.Core.Models
{
    public class DBResult
    {
        public DBResult() { }

        public DBResult(ReturnCode code)
        {
            Code = code.Code;
        }

        /// <summary>
        /// 狀態碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Msg { get; set; } = string.Empty;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return GetReturnCode()?.IsSuccess ?? false;
            }
        }

        /// <summary>
        /// 取得對應的ReturnCode
        /// </summary>
        /// <returns></returns>
        public ReturnCode GetReturnCode()
        {
            return ReturnCode.GetDefault(Code);
        }
    }
}