using Microsoft.AspNetCore.Http;

namespace ControllerShareLib.Interfaces.Service
{
    public interface IByteArrayApiService
    {
        bool IsEncodingRequired(HttpRequest httpRequest);
        
        string EncBytesHeader { get; }
        
        Task DecodeToBodyAsync(HttpRequest request);
        
        Task EncodeResponseAsync(HttpResponse response, string json, IHeaderDictionary headers);
        
        Task EncodeResponseAsync(HttpResponse response, byte[] responseBytes, IHeaderDictionary headers);
        
        void DecodeEncodingPath(HttpRequest httpContextRequest);
        
        bool CheckAllowUnencryptedRequest(HttpRequest httpRequest);

        bool IsEncodingPathFromQueryString(HttpRequest request);
    }
}