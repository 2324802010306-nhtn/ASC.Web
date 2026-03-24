using ASC.Web.Configuration;
using ASC.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ASC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ASC.Web.Configuration.ApplicationSettings _appSettings;

        // ✅ Constructor duy nhất
        public HomeController(
            ILogger<HomeController> logger,
            IOptions<ASC.Web.Configuration.ApplicationSettings> options)
        {
            _logger = logger;
            _appSettings = options.Value;
        }

        public IActionResult Index()
        {
            ViewBag.AppName = _appSettings.ApplicationName;
            ViewBag.Version = _appSettings.Version;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}