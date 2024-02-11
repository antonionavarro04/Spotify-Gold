using Microsoft.AspNetCore.Mvc;

namespace SpotifyGoldServer.Controllers {
    public class APIController : Controller {
        public IActionResult Index() {
            return RedirectToAction("Index", "Home");
        }
    }
}
