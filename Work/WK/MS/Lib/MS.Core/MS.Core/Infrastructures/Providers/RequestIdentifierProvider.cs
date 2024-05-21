using Microsoft.AspNetCore.Http;

namespace MS.Core.Infrastructures.Providers
{
    public class RequestIdentifierProvider : IRequestIdentifierProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestIdentifierProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRequestId()
        {
            return _httpContextAccessor?.HttpContext?.TraceIdentifier ?? string.Empty;
        }
    }
}