﻿using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MiseLive.Response;

namespace JxBackendService.Model.ReturnModel
{
    public class BaseReturnModel
    {
        public BaseReturnModel()
        { }

        public BaseReturnModel(ReturnCode returnCode)
        {
            SetReturnCode(returnCode);
        }

        public BaseReturnModel(SuccessMessage successMessage)
        {
            SetReturnCode(successMessage.SuccessCode);

            if (!successMessage.Text.IsNullOrEmpty())
            {
                Message = successMessage.Text;
            }
        }

        public BaseReturnModel(ReturnCode returnCode, params string[] messageArgs) : this(returnCode)
        {
            Message = string.Format(returnCode.Name, messageArgs);
        }

        public BaseReturnModel(string customiszedMessage)
        {
            if (!customiszedMessage.IsNullOrEmpty())
            {
                Code = ReturnCode.CustomizedMessage.Value;
                Message = customiszedMessage;
            }
            else
            {
                Code = ReturnCode.Success.Value;
            }
        }

        /// <summary>
        /// 是否成功(方便前端判斷,不把判斷CODE內容寫死在前端)
        /// </summary>
        public bool IsSuccess
        { get { return ReturnCode.GetSingle(Code).IsSuccess; } set {/*為了WCF參考產生程式碼*/ } }

        public string Code { get; set; } = ReturnCode.Success.Value;

        public string Message { get; set; } = ReturnCode.Success.Name;

        public void SetReturnCode(ReturnCode returnCode)
        {
            Code = returnCode.Value;
            Message = returnCode.Name;
        }
    }

    public class BaseReturnDataModel<T> : BaseReturnModel
    {
        public BaseReturnDataModel()
        { }

        public BaseReturnDataModel(ReturnCode returnCode) : base(returnCode)
        {
        }

        public BaseReturnDataModel(string customiszedMessage) : base(customiszedMessage)
        {
        }

        public BaseReturnDataModel(ReturnCode returnCode, T dataModel) : base(returnCode)
        {
            DataModel = dataModel;
        }

        public BaseReturnDataModel(ReturnCode returnCode, params string[] messageArgs) : base(returnCode)
        {
            Message = string.Format(returnCode.Name, messageArgs);
        }

        public BaseReturnDataModel(SuccessMessage successMessage) : this(successMessage, default)
        {
        }

        public BaseReturnDataModel(SuccessMessage successMessage, T dataModel) : base(successMessage)
        {
            DataModel = dataModel;
        }

        public BaseReturnDataModel(string customiszedMessage, T dataModel) : base(customiszedMessage)
        {
            DataModel = dataModel;
        }

        public T DataModel { get; set; }
    }

    public static class BaseReturnDataModelExtensions
    {
        public static BaseReturnDataModel<T> CastByCodeAndMessage<T>(this BaseReturnModel source)
        {
            return new BaseReturnDataModel<T>
            {
                Code = source.Code,
                Message = source.Message
            };
        }

        public static BaseMiseLiveResponse ToMiseLiveResponse(this BaseReturnModel baseReturnModel)
        {
            return new BaseMiseLiveResponse()
            {
                Success = baseReturnModel.IsSuccess,
                Error = !baseReturnModel.IsSuccess ? baseReturnModel.Message : null
            };
        }

        public static MiseLiveResponse<T> ToMiseLiveResponse<T>(this BaseReturnModel baseReturnModel) where T : class
        {
            string error = null;

            if (!baseReturnModel.IsSuccess)
            {
                error = baseReturnModel.Message;
            }

            return new MiseLiveResponse<T>()
            {
                Success = baseReturnModel.IsSuccess,
                Error = error
            };
        }
    }
}