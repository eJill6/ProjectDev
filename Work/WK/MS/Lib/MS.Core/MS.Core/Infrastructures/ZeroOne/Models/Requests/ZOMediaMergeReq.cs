namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOMediaUploadReq
    {
        public ZOMediaUploadReq()
        {
        }

        public string FileName { get; set; }

        public string FileNameExtension { get; set; }

        public byte[] FileBody { get; set; }
    }
}
