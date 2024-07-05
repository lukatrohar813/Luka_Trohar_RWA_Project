﻿using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;


namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin")) return RedirectToAction("ListAdmin", "Project");
            if (User.IsInRole("User")) return RedirectToAction("Index", "Application");
            if (User.IsInRole("NonUser")) return RedirectToAction("List", "Project");
            return View();


          
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}