using Microsoft.AspNetCore.Http;

namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface IImageFileUpload
    {
        string AesFullImageUrl { get; }

        IFormFile ImageFile { get; set; }
    }
}