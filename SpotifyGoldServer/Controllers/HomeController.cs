using Microsoft.AspNetCore.Mvc;
using SpotifyGoldServer.Controllers.API;
using SpotifyGoldServer.Models;
using System.Diagnostics;

namespace SpotifyGoldServer.Controllers {
    public class HomeController(ILogger<HomeController> logger): Controller {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index() {
            string ip = HttpContext.Connection.RemoteIpAddress!.ToString();
            ViewBag.IpPlusPort = ip;
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
