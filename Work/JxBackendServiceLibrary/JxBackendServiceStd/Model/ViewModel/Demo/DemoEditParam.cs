using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.ViewModel.Demo
{
    public class DemoEditParam
    {
        public int No { get; set; }

        public int Sort { get; set; }

        public IFormFile DemoFile { get; set; }
    }
}