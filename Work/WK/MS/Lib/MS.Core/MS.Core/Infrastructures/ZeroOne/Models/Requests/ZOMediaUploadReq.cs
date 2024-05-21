namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOMediaMergeReq
    {
        public ZOMediaMergeReq()
        {
        }

        public List<string> path_list { get; set; } = new List<string>();

        public string suffix { get; set; }
    }
}
