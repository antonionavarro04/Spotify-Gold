using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SpotifyGoldServer.Controllers.API {
    [Route("api/[controller]")]
    [ApiController]
    public class FileController: ControllerBase {

        [HttpGet]
        public IActionResult Get() {
            return StatusCode((int) HttpStatusCode.NotImplemented);
        }
    }
}
