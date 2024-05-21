namespace JxBackendService.Model.Param.SMS.ThirdParty
{
    public class TercentCloudSetting
    {
        public string AppID { get; set; }

        public string AppKey { get; set; }

        public string SecretId { get; set; }

        public string SecretKey { get; set; }

        public string Endpoint { get; set; }

        public string SignName { get; set; }

        public ValidateCodeTemplate ValidateCodeTemplate { get; set; }
    }
}