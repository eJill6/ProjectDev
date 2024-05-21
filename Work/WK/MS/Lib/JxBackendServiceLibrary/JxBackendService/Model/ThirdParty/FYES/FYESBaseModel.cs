namespace JxBackendService.Model.ThirdParty.FYES
{
    public class FYESBaseModel
    {
        public int success { get; set; }

        public string msg { get; set; }

        public bool IsSuccess => success == FYESResponseSuccessCode.Success;
    }

    public class FYESBaseInfoModel<T> : FYESBaseModel
    {
        public T info { get; set; }
    }
}