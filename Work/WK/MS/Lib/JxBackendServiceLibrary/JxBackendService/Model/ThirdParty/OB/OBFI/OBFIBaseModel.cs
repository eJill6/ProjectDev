namespace JxBackendService.Model.ThirdParty.OB.OBFI
{

    public class OBFIBaseResponseModel
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

    public class OBFIBaseResponseWtihDataModel<T> : OBFIBaseResponseModel
    {
        public T data { get; set; }
    }
}
