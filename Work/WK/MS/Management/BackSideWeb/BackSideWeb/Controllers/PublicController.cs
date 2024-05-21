using BackSideWeb.Controllers.Base;
using BackSideWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackSideWeb.Controllers
{
    public class PublicController : BaseController
    {
        public PublicController()
        {
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test()
        {
            throw new NotImplementedException();
        }
    }
}