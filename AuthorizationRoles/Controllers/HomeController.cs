﻿using Microsoft.AspNetCore.Mvc;

namespace Authentication.Roles.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}