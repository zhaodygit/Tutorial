﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Heavy.Web.Models;
using Microsoft.Extensions.Logging;
using Heavy.Web.Data;
using Heavy.Web.Filters;

namespace Heavy.Web.Controllers
{
    [LogResourceFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
        }

        
        public IActionResult Index()
        {
            _logger.LogInformation(MyLogEventIds.HomePage,"Vsisiting Home Index...");
            //throw new Exception("some happend!");
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

        public IActionResult MyError()
        {
            return View();
        }
    }
}
