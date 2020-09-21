using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Heavy.Web.Models;
using Microsoft.Extensions.Logging;
using Heavy.Web.Data;
using Heavy.Web.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Heavy.Web.Controllers
{
    //[LogResourceFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache memoryCache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            this._logger = logger;
            this.memoryCache = memoryCache;
        }


        
        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]//前进后退管用
        [ResponseCache(CacheProfileName = "Default")]  //使用startup里的配置
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
