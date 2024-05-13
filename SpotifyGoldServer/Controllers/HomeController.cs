using Microsoft.AspNetCore.Mvc;
using SpotifyGoldServer.Controllers.API;
using SpotifyGoldServer.Models;
using System.Diagnostics;

namespace SpotifyGoldServer.Controllers {
    public class HomeController(): Controller {
        public IActionResult Index() {
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
