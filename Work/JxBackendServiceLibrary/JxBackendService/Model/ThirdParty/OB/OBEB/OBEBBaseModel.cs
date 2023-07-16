namespace JxBackendService.Model.ThirdParty.OB.OBEB
{
    public class OBEBBaseResponseModel
    {
        /// <summary> 状态码 </summary>
        public string code { get; set; }

        /// <summary> 错误消息 </summary>
        public string message { get; set; }

        public bool IsSuccess => code == OBEBResponseCode.Success;
    }

    public class OBEBBaseResponseWtihDataModel<T> : OBEBBaseResponseModel
    {
        public T data { get; set; }
    }
}