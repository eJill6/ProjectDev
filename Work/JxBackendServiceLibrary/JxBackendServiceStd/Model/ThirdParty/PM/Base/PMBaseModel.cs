namespace JxBackendService.Model.ThirdParty.PM.Base
{
    public class PMBaseResponseModel
    {
        public int Code { get; set; }

        public string Msg { get; set; }
    }

    public class PMBaseResponseWtihDataModel<T> : PMBaseResponseModel
    {
        public T Data { get; set; }
    }
}