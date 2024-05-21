using System.Reflection;

namespace MS.Core.Models
{
    public class BaseReturnModel
    {
        public string Code { get; protected set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess
        {
            get
            {
                return ReturnCode.GetDefault(Code).IsSuccess;
            }
        }

        public ReturnCode ReturnCode => ReturnCode.GetDefault(Code);

        public BaseReturnModel()
        { }

        public BaseReturnModel(ReturnCode returnCode)
        {
            SetCode(returnCode);
        }

        public BaseReturnModel(ReturnCode returnCode ,string message)
        {
            SetCode(returnCode);
            Message = message;
        }

        public BaseReturnModel(BaseReturnModel model)
        {
            SetModel(model);
            Message = model.Message;
        }

        public void SetCode(ReturnCode returnCode)
        {
            Code = returnCode.Code;
            Message = returnCode.Message;
        }

        public void SetModel(BaseReturnModel model)
        {
            Code = model.Code;
        }
    }

    public class BaseReturnDataModel<T> : BaseReturnModel
    {
        public BaseReturnDataModel()
        { }

        public BaseReturnDataModel(BaseReturnModel model) : base(model)
        {

        }
        public BaseReturnDataModel(ReturnCode returnCode) : base(returnCode)
        {
        }

        public T DataModel { get; set; }
    }
}
