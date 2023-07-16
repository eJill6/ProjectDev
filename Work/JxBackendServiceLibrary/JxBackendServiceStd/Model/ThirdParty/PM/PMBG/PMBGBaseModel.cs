namespace JxBackendService.Model.ThirdParty.PM.PMBG
{
    public class PMBGBaseResponseModel
    {
        public int Code { get; set; }

        public string Msg { get; set; }
    }

    public class PMBGBaseResponseWtihDataModel<T> : PMBGBaseResponseModel
    {
        public T Data { get; set; }
    }
}