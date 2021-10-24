using JxBackendService.Model.Entity;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service
{
    public class SmsService
    {
        public SmsService()
        {
        }

        public void InsertSysSMS(InsertSysSMS model, string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest("SMS/InsertSysSMS/", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(model);
            client.Execute(request);
        }

    }
}
 