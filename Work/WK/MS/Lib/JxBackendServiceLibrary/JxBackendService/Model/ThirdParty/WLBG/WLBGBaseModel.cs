namespace JxBackendService.Model.ThirdParty.WLBG
{
    public class WLBGBaseResponseModel
    {
        /// <summary> 状态码 </summary>
        public int Code { get; set; }

        /// <summary> 供调试的详细信息。仅失败时有此字段。 </summary>
        public string Msg { get; set; }

        public bool IsSuccess => Code == WLBGResponseCode.Success;
    }

    public class WLBGBaseResponseWithDataModel<T> : WLBGBaseResponseModel
    {
        /// <summary> 返回的额外数据 </summary>
        public T Data { get; set; }
    }
}