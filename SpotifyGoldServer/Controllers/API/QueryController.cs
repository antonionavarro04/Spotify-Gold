using Microsoft.AspNetCore.Mvc;

namespace SpotifyGoldServer.Controllers.API {

    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase {

        [HttpGet("{query}")]
        public async Task<IActionResult> Get(string query) {
            IActionResult result = StatusCode(403);

            

            return result;
        }
    }
}
