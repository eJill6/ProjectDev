using System.Web.Mvc;
using System.Net;
using JxBackendService.Model.ReturnModel;

namespace JxBackendServiceNF.Attributes
{
    public class ValidModelStateAttribute : BaseValidModelStateNFAttribute
    {
        public ValidModelStateAttribute()
        {
        }

        public ValidModelStateAttribute(HttpStatusCode httpStatusCode) : base(httpStatusCode)
        {
        }

        protected override ActionResult GetInvalidActionResult(string errorMessage)
        {
            return new JsonResult()
            {
                Data = new BaseReturnModel()
                {
                    IsSuccess = false,
                    Message = errorMessage
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}