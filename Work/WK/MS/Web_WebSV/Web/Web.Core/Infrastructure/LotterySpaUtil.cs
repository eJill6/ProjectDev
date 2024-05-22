using Microsoft.AspNetCore.Mvc;

namespace Web.Core.Infrastructure
{
    public static class LotterySpaUtil
    {
        public static JsonResult CreateJsonResult(string errorMessage)
            => CreateJsonResult(isSuccess: false, errorMessage, data: null);

        public static JsonResult CreateJsonResult(bool isSuccess, string errorMessage, object data)
        {
            var response = new
            {
                isSuccess,
                errorMessage,
                data
            };

            return new JsonResult(response);
        }
    }
}