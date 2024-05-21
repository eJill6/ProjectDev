namespace SLPolyGame.Web.Model
{
    public class SetRemoteCacheParam
    {
        public string Key { get; set; }

        public int CacheSeconds { get; set; }

        public bool IsSlidingExpiration { get; set; }

        public string Value { get; set; }
    }
}