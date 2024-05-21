using Microsoft.AspNetCore.Http;

namespace JxBackendService.Model.ViewModel.Demo
{
    public class DemoEditParam
    {
        public int No { get; set; }

        public int Sort { get; set; }

        public IFormFile DemoFile { get; set; }
    }
}