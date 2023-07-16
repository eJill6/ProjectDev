using Microsoft.AspNetCore.Http;

namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface IImageFileUpload
    {
        string FullImageUrl { get; }

        IFormFile ImageFile { get; set; }
    }
}