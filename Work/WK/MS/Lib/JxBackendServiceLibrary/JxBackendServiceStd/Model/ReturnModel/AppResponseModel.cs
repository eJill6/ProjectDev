using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ReturnModel
{
    public class AppResponseModel<T> : AppResponseModel where T : class
    {
        public AppResponseModel()
        { }

        public AppResponseModel(BaseReturnModel baseReturnModel) : base(baseReturnModel)
        {
        }

        public AppResponseModel(BaseReturnDataModel<T> baseReturnDataModel) : base(baseReturnDataModel)
        {
            Data = baseReturnDataModel.DataModel;
        }

        public AppResponseModel(ReturnCode returnCode) : base(returnCode)
        {
        }

        public T Data { get; set; }
    }

    public class AppResponseModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public AppResponseModel()
        { }

        public AppResponseModel(string customiszedMessage)
        {
            if (!customiszedMessage.IsNullOrEmpty())
            {
                Success = false;
                Message = customiszedMessage;
            }
            else
            {
                Success = true;
            }
        }

        public AppResponseModel(BaseReturnModel baseReturnModel)
        {
            Success = baseReturnModel.IsSuccess;

            if (!Success)
            {
                Message = baseReturnModel.Message;
            }
        }

        public AppResponseModel(ReturnCode returnCode)
        {
            Success = returnCode.IsSuccess;

            if (!Success)
            {
                Message = returnCode.Name;
            }
        }
    }
}