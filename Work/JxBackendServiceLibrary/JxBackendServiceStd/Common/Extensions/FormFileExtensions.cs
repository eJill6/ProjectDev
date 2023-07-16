using Microsoft.AspNetCore.Http;
using System.IO;

namespace JxBackendService.Common.Extensions
{
    public static class FormFileExtensions
    {
        public static byte[] ToBytes(this IFormFile formFile)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}