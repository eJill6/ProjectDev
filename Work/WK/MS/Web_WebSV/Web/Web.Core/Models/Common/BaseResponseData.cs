using JxBackendService.Model.Enums;

namespace Web.Models.Common
{
    public class BaseResponseData
    {
        public bool success { get; set; }

        public string message { get; set; }
    }

    public class ResponseData<T> : BaseResponseData where T : new()
    {
        public ResponseData()
        {
            success = true;
            data = new T();
        }

        public T data { get; set; }
    }

    public class ResponseData : ResponseData<object>
    {
    }

    public class ResponseValue<T>
    {
        public ResponseValue()
        { }

        public ResponseValue(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }

    public class ResponseCodeData<T> : ResponseData<T> where T : new()
    {
        public ResponseCodeData()
        { }

        public ResponseCodeData(ReturnCode returnCode)
        {
            success = returnCode.IsSuccess;
            code = returnCode.Value;
            message = returnCode.Name;
        }

        public string code { get; set; }
    }

    public class ResponseCodeData : ResponseCodeData<object>
    {
        public ResponseCodeData()
        { }

        public ResponseCodeData(ReturnCode returnCode) : base(returnCode)
        {
        }
    }
}