namespace JxBackendService.Model.ThirdParty.PM.PMSL
{
    public class PMSLBaseResponseModel
    {
        public int Code { get; set; }
        public string Msg { get; set; }
    }

    public class PMSLBaseResponseWtihDataModel<T> : PMSLBaseResponseModel
    {
        public T Data { get; set; }
    }
}