using RestSharp;

namespace MS.Core.Helpers.RestRequestHelpers
{
    public class RestResponseModel<T>
    {
        public IRestResponse Response { get; set; } = null!;

        public T? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public RestRequestModel Request { get; set; } = null!;
    }
    public class RestRequestModel
    {
        public Uri? BaseUrl { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public string Method { get; set; }
    }
}
