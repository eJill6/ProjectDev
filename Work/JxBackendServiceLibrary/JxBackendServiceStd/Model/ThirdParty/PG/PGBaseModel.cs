namespace JxBackendService.Model.ThirdParty.PG
{
    public class PGBaseResponseModel<T>
    {
        public T data { get; set; }
        public PGErrorModel error { get; set; }
    }


    public class PGErrorModel
    {
        public string code { get; set; }
        public string message { get; set; }
    }

}
