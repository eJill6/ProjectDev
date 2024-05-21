namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABUserBaseModel
    {
        /// <summary> 玩家帐号 </summary>
        public string Player { get; set; }
    }

    public class ABBaseResponseModel
    {
        public bool IsSuccess => ResultCode == ABResponseCode.Success;

        public string ResultCode { get; set; }

        public string Message { get; set; }
    }

    public class ABBaseResponseWithDataModel<T> : ABBaseResponseModel
    {
        /// <summary> 返回的额外数据 </summary>
        public T Data { get; set; }
    }
}