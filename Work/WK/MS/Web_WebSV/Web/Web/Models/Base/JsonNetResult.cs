using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Models.Base
{
    public class JsonNetResult : JsonResult
    {
        private const string _dateFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public bool IsFlashReqeuest { get; set; }

        public JsonNetResult()
        {

        }

        public JsonNetResult(object data)
        {
            this.Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (IsFlashReqeuest)
            {
                base.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                base.ExecuteResult(context);
                return;
            }

            var response = context.HttpContext.Response;

            if (!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var isoConvert = new IsoDateTimeConverter();

                isoConvert.DateTimeFormat = _dateFormat;
                response.Write(JsonConvert.SerializeObject(Data, isoConvert));
            }
        }
    }
}