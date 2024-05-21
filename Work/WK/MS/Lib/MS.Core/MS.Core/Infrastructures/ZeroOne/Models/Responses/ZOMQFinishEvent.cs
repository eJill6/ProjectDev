namespace MS.Core.Infrastructures.ZeroOne.Models.Responses
{
    public class ZOMQFinishEvent
    {
        public long id { get; set; }
        public string orgin_path { get; set; }
        public string converted_path { get; set; }
        public string model { get; set; }
        public string operation { get; set; }
    }
}
