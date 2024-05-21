namespace ControllerShareLib.Helpers.Cache
{
    [Serializable]
    public class CachedObj<T>
    {
        public DateTime ExpiredTime { get; set; }
        public double ExpiredSeconds { get; set; }
        public T Value { get; set; }
    }
}