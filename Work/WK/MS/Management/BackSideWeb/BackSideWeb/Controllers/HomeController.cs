using BackSideWeb.Controllers.Base;
using BackSideWeb.Models;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BackSideWeb.Controllers
{
    public class HomeController : BaseAuthController
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            SetPageTitle("主页");

            return View();
        }

        public IActionResult NoPermission()
        {
            return View();
        }
    }
}