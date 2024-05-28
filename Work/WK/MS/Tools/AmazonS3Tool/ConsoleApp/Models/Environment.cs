namespace ClassLibrary
{
    public enum Environments
    {
        DEV,

        SIT,

        UAT,

        LIVE
    }

    public class EnvironmentDomain
    {
        public const string DEV = "https://aws-ms-dev.rtykf.com";

        public const string SIT = "https://aws-ms-sit.rtykf.com";

        public const string UAT = "https://aws-ms-uat.rtykf.com";

        public const string LIVE = "https://aws-ms.rtykf.com";
    }
}