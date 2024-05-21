using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Models
{
    public class ImageFileUploadSetting
    {
        public IImageFileUpload ImageFileUploadModel { get; set; }

        public Dimension[] DimensionLimits { get; set; }

        public int SizeLimit { get; set; }

        public string SizeLimitKB => (SizeLimit / 1024m).ToAmountString(isRemoveZeroAfterPoint: true);

        public string[] AllowedExtensions { get; set; }

        public string FieldName { get; set; }

        public string CustomizedDimensionLimitsMessage { get; set; }
    }

    public class Dimension
    {
        public int Width { get; set; }

        public int Height { get; set; }
    }
}